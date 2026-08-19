using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CRYPTONIX
{
    public partial class Form1 : Form
    {
        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref int PhyAddrLen);

        private bool _stopRequested = false;
        private bool _isScanning = false;
        private string currentMode = "Main";
        private List<int> customPortsList = new List<int>();
        private string _modeLabelPrefix = "Режим: ";
        private string _currentThemeKey = "Light";
        private Dictionary<string, string> _themeDisplayNames;

        private const int RangeMainStart = 1;
        private const int RangeMainEnd = 1023;
        private const int RangeUserStart = 1024;
        private const int RangeUserEnd = 49151;
        private const int RangePrivateStart = 49152;
        private const int RangePrivateEnd = 65535;
        private const int RangeAllStart = 1;
        private const int RangeAllEnd = 65535;

        private const int PingTimeout = 2000;
        private const int PortConnectTimeout = 600;
        private const int MaxConcurrentPorts = 50;
        private const int BannerTimeout = 500;

        private string _firstIpInRange = "";
        private long _totalHosts = 0;
        private CancellationTokenSource _cts;

        public Form1()
        {
            InitializeComponent();
            SetupListView();
            currentMode = "Main";
            UpdateModeLabel();

            cmbPortMode.SelectedIndex = 0;
            cmbLanguage.Items.Clear();
            cmbLanguage.Items.AddRange(new object[] { "Русский", "English" });
            cmbLanguage.SelectedIndex = 0;

            ApplyLanguage(cmbLanguage.SelectedItem.ToString());

            _currentThemeKey = "Light";
            int idx = FindThemeIndexByKey(_currentThemeKey);
            if (idx >= 0) cmbTheme.SelectedIndex = idx;
            else cmbTheme.SelectedIndex = 0;
            ApplyTheme(_currentThemeKey);

            ConfigureButton(btnScan, Color.FromArgb(0, 200, 83));    // зелёный
            ConfigureButton(btnStop, Color.FromArgb(255, 82, 82));   // красный
            ConfigureButton(btnExport, Color.FromArgb(0, 150, 200)); // синий

            ShowTab(panelScan);
        }

        private void ConfigureButton(Button btn, Color hoverColor)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = Color.Transparent;
            btn.UseVisualStyleBackColor = false;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = hoverColor;
            btn.FlatAppearance.MouseDownBackColor = hoverColor;
        }

        private void SetupListView()
        {
            if (lstResult == null) return;
            lstResult.View = View.Details;
            lstResult.FullRowSelect = true;
            lstResult.GridLines = true;
            lstResult.CheckBoxes = true;
            if (imageList1 != null)
                lstResult.SmallImageList = imageList1;
            lstResult.Columns.Clear();
            lstResult.Columns.Add("IP Address", 140);
            lstResult.Columns.Add("Name", 150);
            lstResult.Columns.Add("MAC Address", 170);
            lstResult.Columns.Add("Status", 100);
            lstResult.Columns.Add("Ping Time (ms)", 100);
            lstResult.Columns.Add("OS", 160);
            lstResult.Columns.Add("Open Ports", -2);

            lstResult.ItemChecked += (s, e) => {
                if (e.Item.Checked)
                {
                    string ip = e.Item.SubItems[0].Text.Trim();
                    if (!string.IsNullOrEmpty(ip))
                    {
                        try
                        {
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = "http://" + ip,
                                UseShellExecute = true
                            });
                        }
                        catch { }
                    }
                }
            };
        }

        private void UpdateModeLabel()
        {
            if (lblMode != null)
                lblMode.Text = _modeLabelPrefix + GetModeName(currentMode);
        }

        private int FindThemeIndexByKey(string key)
        {
            if (_themeDisplayNames == null) return -1;
            if (!_themeDisplayNames.ContainsKey(key)) return -1;
            string displayName = _themeDisplayNames[key];
            for (int i = 0; i < cmbTheme.Items.Count; i++)
                if (cmbTheme.Items[i].ToString() == displayName)
                    return i;
            return -1;
        }

        private async void btnScan_Click(object sender, EventArgs e)
        {
            if (_isScanning) return;
            if (txtMyIp == null || lstResult == null)
            {
                MessageBox.Show("Ошибка инициализации формы.", "Критическая ошибка");
                return;
            }

            string input = txtMyIp.Text.Trim();
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show("Введи IP или диапазон.", "Пустое поле");
                return;
            }

            long startIpLong = 0, endIpLong = 0;
            _firstIpInRange = "";

            try
            {
                if (input.Contains("/"))
                {
                    string[] parts = input.Split('/');
                    string ipBase = parts[0].Trim();
                    int mask = int.Parse(parts[1].Trim());
                    if (mask < 8 || mask > 30) throw new Exception("Маска от 8 до 30");
                    uint ip = IpToUint(IPAddress.Parse(ipBase));
                    uint maskBits = 0xFFFFFFFF << (32 - mask);
                    uint network = ip & maskBits;
                    uint broadcast = network | ~maskBits;
                    startIpLong = network + 1;
                    endIpLong = broadcast - 1;
                    _firstIpInRange = LongToIp(startIpLong);
                }
                else if (input.Contains("-"))
                {
                    string[] parts = input.Split('-');
                    string ipStartStr = parts[0].Trim();
                    string ipEndStr = parts[1].Trim();
                    if (!ipEndStr.Contains("."))
                    {
                        string[] firstOctets = ipStartStr.Split('.');
                        ipEndStr = firstOctets[0] + "." + firstOctets[1] + "." + firstOctets[2] + "." + ipEndStr;
                    }
                    if (!IPAddress.TryParse(ipStartStr, out IPAddress ipStart) || !IPAddress.TryParse(ipEndStr, out IPAddress ipEnd))
                    {
                        MessageBox.Show("Неверный формат диапазона.", "Ошибка");
                        return;
                    }
                    startIpLong = IpToLong(ipStart);
                    endIpLong = IpToLong(ipEnd);
                    _firstIpInRange = ipStartStr;
                    if (startIpLong > endIpLong) { long tmp = startIpLong; startIpLong = endIpLong; endIpLong = tmp; }
                }
                else
                {
                    if (!IPAddress.TryParse(input, out IPAddress myIp))
                    {
                        MessageBox.Show("Введи корректный IP.", "Неправильный IP");
                        return;
                    }
                    startIpLong = IpToLong(myIp);
                    endIpLong = startIpLong;
                    _firstIpInRange = input;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка парсинга: " + ex.Message);
                return;
            }

            if (startIpLong > endIpLong)
            {
                MessageBox.Show("Начальный IP больше конечного.", "Ошибка диапазона");
                return;
            }

            _totalHosts = endIpLong - startIpLong + 1;
            if (_totalHosts <= 0)
            {
                MessageBox.Show("Диапазон пуст.", "Ошибка");
                return;
            }

            lstResult.Items.Clear();

            btnScan.Enabled = false;
            btnStop.Enabled = true;
            UpdateModeLabel();
            _stopRequested = false;
            _isScanning = true;
            _cts = new CancellationTokenSource();

            try
            {
                PortRangeInfo range = GetPortRangeByMode(currentMode);
                if (currentMode == "All" && range.StartPort == 0)
                {
                    btnScan.Enabled = true;
                    btnStop.Enabled = false;
                    _isScanning = false;
                    return;
                }
                if (currentMode == "Custom")
                {
                    customPortsList = ParseCustomPorts(txtCustomPorts.Text);
                    if (customPortsList.Count == 0)
                    {
                        MessageBox.Show("Список портов пуст. Введите порты через запятую, например: 80,443,22", "Ошибка");
                        btnScan.Enabled = true;
                        btnStop.Enabled = false;
                        _isScanning = false;
                        return;
                    }
                    await ScanRangeAsync(startIpLong, endIpLong, customPortsList, _cts.Token);
                }
                else
                {
                    await ScanRangeAsync(startIpLong, endIpLong, range.StartPort, range.EndPort, _cts.Token);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            finally
            {
                btnScan.Enabled = true;
                btnStop.Enabled = false;
                _isScanning = false;
                _cts?.Dispose();
                _cts = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            MessageBox.Show("Сканирование завершено.", "Готово");
        }

        private List<int> ParseCustomPorts(string text)
        {
            var result = new List<int>();
            if (string.IsNullOrWhiteSpace(text)) return result;
            string[] parts = text.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string part in parts)
            {
                if (int.TryParse(part.Trim(), out int port) && port >= 1 && port <= 65535)
                {
                    if (!result.Contains(port))
                        result.Add(port);
                }
            }
            result.Sort();
            return result;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _stopRequested = true;
            _cts?.Cancel();
        }

        private void txtMyIp_Click(object sender, EventArgs e)
        {
            txtMyIp.Clear();
        }

        private long IpToLong(IPAddress ip) { byte[] b = ip.GetAddressBytes(); return ((long)b[0] << 24) | ((long)b[1] << 16) | ((long)b[2] << 8) | b[3]; }
        private uint IpToUint(IPAddress ip) { byte[] b = ip.GetAddressBytes(); return (uint)((b[0] << 24) | (b[1] << 16) | (b[2] << 8) | b[3]); }
        private string LongToIp(long ipLong) { return IPAddress.Parse(((uint)ipLong).ToString()).ToString(); }

        private PortRangeInfo GetPortRangeByMode(string mode)
        {
            PortRangeInfo info = new PortRangeInfo();
            if (mode == "Private")
            {
                info.StartPort = RangePrivateStart;
                info.EndPort = RangePrivateEnd;
                info.ModeName = "Приватные порты";
            }
            else if (mode == "User")
            {
                info.StartPort = RangeUserStart;
                info.EndPort = RangeUserEnd;
                info.ModeName = "Пользовательские порты";
            }
            else if (mode == "All")
            {
                if (MessageBox.Show("ВНИМАНИЕ! Сканирование всех портов займёт много времени.\nПродолжить?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    info.StartPort = 0;
                    info.EndPort = 0;
                    info.ModeName = "";
                    return info;
                }
                info.StartPort = RangeAllStart;
                info.EndPort = RangeAllEnd;
                info.ModeName = "Все порты";
            }
            else if (mode == "Custom")
            {
                info.StartPort = -1;
                info.EndPort = -1;
                info.ModeName = "Свои порты";
            }
            else
            {
                info.StartPort = RangeMainStart;
                info.EndPort = RangeMainEnd;
                info.ModeName = "Основные порты";
            }
            return info;
        }

        private string GetModeName(string mode)
        {
            if (mode == "Main") return "Основные порты (1–1023)";
            if (mode == "User") return "Пользовательские порты (1024–49151)";
            if (mode == "Private") return "Приватные порты (49152–65535)";
            if (mode == "All") return "Все порты (1–65535)";
            if (mode == "Custom") return "Свои порты";
            return "Основные порты";
        }

        private void cmbPortMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPortMode.SelectedIndex == -1) return;
            string selected = cmbPortMode.SelectedItem.ToString();
            if (selected.StartsWith("Основные") || selected == "Main (1-1023)")
                currentMode = "Main";
            else if (selected.StartsWith("Пользовательские") || selected == "User (1024-49151)")
                currentMode = "User";
            else if (selected.StartsWith("Приватные") || selected == "Private (49152-65535)")
                currentMode = "Private";
            else if (selected.StartsWith("Все") || selected == "All (1-65535)")
                currentMode = "All";
            else if (selected == "Свои" || selected == "Custom")
                currentMode = "Custom";

            txtCustomPorts.Visible = (currentMode == "Custom");

            UpdateModeLabel();
        }

        private void cmbTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTheme.SelectedIndex == -1) return;
            string selectedDisplay = cmbTheme.SelectedItem.ToString();
            string key = _themeDisplayNames.FirstOrDefault(x => x.Value == selectedDisplay).Key;
            if (!string.IsNullOrEmpty(key))
            {
                _currentThemeKey = key;
                ApplyTheme(key);
            }
        }

        private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLanguage.SelectedIndex == -1) return;
            string savedThemeKey = _currentThemeKey;
            ApplyLanguage(cmbLanguage.SelectedItem.ToString());
            int idx = FindThemeIndexByKey(savedThemeKey);
            if (idx >= 0)
                cmbTheme.SelectedIndex = idx;
            else
                cmbTheme.SelectedIndex = 0;
            ApplyTheme(_currentThemeKey);
        }

        private void ApplyTheme(string themeKey)
        {
            Color back, fore, panelBack, listBack, txtBack, btnBack, btnFore, panelLeftBack;

            switch (themeKey)
            {
                case "Light":
                    back = Color.White; fore = Color.Black; panelBack = Color.WhiteSmoke; listBack = Color.White; txtBack = Color.White; btnBack = Color.WhiteSmoke; btnFore = Color.Black; panelLeftBack = Color.LightGray;
                    break;
                case "Dark":
                    back = Color.FromArgb(44, 44, 44); fore = Color.White; panelBack = Color.FromArgb(44, 44, 44); listBack = Color.FromArgb(50, 50, 50); txtBack = Color.FromArgb(60, 60, 60); btnBack = Color.Transparent; btnFore = Color.White; panelLeftBack = Color.FromArgb(30, 30, 30);
                    break;
                case "Yellow":
                    back = Color.FromArgb(255, 230, 100); fore = Color.Black; panelBack = Color.FromArgb(255, 240, 150); listBack = Color.FromArgb(255, 235, 120); txtBack = Color.FromArgb(255, 235, 120); btnBack = Color.FromArgb(255, 235, 120); btnFore = Color.Black; panelLeftBack = Color.FromArgb(230, 200, 50);
                    break;
                case "LightBlue": 
                    back = Color.FromArgb(173, 216, 230); fore = Color.Black; panelBack = Color.FromArgb(200, 230, 240); listBack = Color.FromArgb(190, 225, 235); txtBack = Color.FromArgb(190, 225, 235); btnBack = Color.FromArgb(190, 225, 235); btnFore = Color.Black; panelLeftBack = Color.FromArgb(130, 180, 200);
                    break;
                case "Purple":
                    back = Color.FromArgb(50, 0, 80); fore = Color.White; panelBack = Color.FromArgb(70, 20, 100); listBack = Color.FromArgb(60, 10, 90); txtBack = Color.FromArgb(70, 20, 100); btnBack = Color.FromArgb(70, 20, 100); btnFore = Color.White; panelLeftBack = Color.FromArgb(40, 0, 60);
                    break;
                case "Pastel":
                    back = Color.FromArgb(255, 228, 225); fore = Color.Black; panelBack = Color.FromArgb(255, 240, 235); listBack = Color.FromArgb(255, 235, 230); txtBack = Color.FromArgb(255, 235, 230); btnBack = Color.FromArgb(255, 235, 230); btnFore = Color.Black; panelLeftBack = Color.FromArgb(230, 200, 190);
                    break;
                case "Mint":
                    back = Color.FromArgb(200, 255, 200); fore = Color.Black; panelBack = Color.FromArgb(220, 255, 220); listBack = Color.FromArgb(210, 250, 210); txtBack = Color.FromArgb(210, 250, 210); btnBack = Color.FromArgb(210, 250, 210); btnFore = Color.Black; panelLeftBack = Color.FromArgb(160, 220, 160);
                    break;
                case "Lavender":
                    back = Color.FromArgb(230, 200, 255); fore = Color.Black; panelBack = Color.FromArgb(240, 220, 255); listBack = Color.FromArgb(235, 210, 255); txtBack = Color.FromArgb(235, 210, 255); btnBack = Color.FromArgb(235, 210, 255); btnFore = Color.Black; panelLeftBack = Color.FromArgb(200, 170, 220);
                    break;
                case "System": 
                    back = Color.FromArgb(200, 200, 200); fore = Color.Black; panelBack = Color.FromArgb(220, 220, 220); listBack = Color.FromArgb(210, 210, 210); txtBack = Color.FromArgb(210, 210, 210); btnBack = Color.FromArgb(210, 210, 210); btnFore = Color.Black; panelLeftBack = Color.FromArgb(160, 160, 160);
                    break;
                default:
                    back = Color.FromArgb(44, 44, 44); fore = Color.White; panelBack = Color.FromArgb(44, 44, 44); listBack = Color.FromArgb(50, 50, 50); txtBack = Color.FromArgb(60, 60, 60); btnBack = Color.Transparent; btnFore = Color.White; panelLeftBack = Color.FromArgb(30, 30, 30);
                    break;
            }

            this.BackColor = back;
            this.ForeColor = fore;
            panelLeft.BackColor = panelLeftBack;
            panelContent.BackColor = panelBack;
            panelScan.BackColor = panelBack;
            panelSettings.BackColor = panelBack;
            panelAbout.BackColor = panelBack;
            lstResult.BackColor = listBack;
            lstResult.ForeColor = fore;
            foreach (ListViewItem item in lstResult.Items)
            {
                item.ForeColor = fore;
            }
            txtMyIp.BackColor = txtBack;
            txtMyIp.ForeColor = fore;
            txtCustomPorts.BackColor = txtBack;
            txtCustomPorts.ForeColor = fore;
            btnTabScan.BackColor = btnBack;
            btnTabScan.ForeColor = btnFore;
            btnTabSettings.BackColor = btnBack;
            btnTabSettings.ForeColor = btnFore;
            btnTabAbout.BackColor = btnBack;
            btnTabAbout.ForeColor = btnFore;
            foreach (Button btn in new Button[] { btnScan, btnStop, btnExport })
            {
                btn.ForeColor = fore;
            }
            lblMode.ForeColor = fore;
            lblPorts.ForeColor = fore;
            lblTheme.ForeColor = fore;
            lblLanguage.ForeColor = fore;
            lblAbout.ForeColor = fore;
            cmbPortMode.BackColor = txtBack;
            cmbPortMode.ForeColor = fore;
            cmbTheme.BackColor = txtBack;
            cmbTheme.ForeColor = fore;
            cmbLanguage.BackColor = txtBack;
            cmbLanguage.ForeColor = fore;
            progressBar1.BackColor = txtBack;
        }

        private void ApplyLanguage(string lang)
        {
            if (lang == "English")
            {
                btnTabScan.Text = "📡 Scan";
                btnTabSettings.Text = "⚙ Settings";
                btnTabAbout.Text = "ℹ Program";
                btnScan.Text = "Scan";
                btnStop.Text = "Stop";
                btnExport.Text = "Export";
                lblPorts.Text = "Ports:";
                cmbPortMode.Items.Clear();
                cmbPortMode.Items.AddRange(new object[] { "Main (1-1023)", "User (1024-49151)", "Private (49152-65535)", "All (1-65535)", "Custom" });
                lblTheme.Text = "Theme:";
                _themeDisplayNames = new Dictionary<string, string>
                {
                    ["Light"] = "Light",
                    ["System"] = "Gray",
                    ["Yellow"] = "Yellow",
                    ["LightBlue"] = "Sea",
                    ["Pastel"] = "Pastel",
                    ["Mint"] = "Mint",
                    ["Lavender"] = "Lavender",
                    ["Purple"] = "Purple"
                };
                cmbTheme.Items.Clear();
                foreach (var pair in _themeDisplayNames)
                    cmbTheme.Items.Add(pair.Value);

                lblLanguage.Text = "Language:";
                lblAbout.Text = "CRYPTONIX v1.1.7 patch b33\n\nDeveloped by: wexyami (CRYPTONIX XLL)\nLicense: MIT (ACTIVE)\n\nIP scanner with OS detection and open ports.\nUsed for fast local network analysis.";
                this.Text = "CRYPTONIX";
                _modeLabelPrefix = "Mode: ";

                if (lstResult.Columns.Count >= 7)
                {
                    lstResult.Columns[0].Text = "IP Address";
                    lstResult.Columns[1].Text = "Name";
                    lstResult.Columns[2].Text = "MAC Address";
                    lstResult.Columns[3].Text = "Status";
                    lstResult.Columns[4].Text = "Ping Time (ms)";
                    lstResult.Columns[5].Text = "OS";
                    lstResult.Columns[6].Text = "Open Ports";
                }
                cmbPortMode.SelectedIndex = 0;
            }
            else 
            {
                btnTabScan.Text = "📡 Сканирование";
                btnTabSettings.Text = "⚙ Настройки";
                btnTabAbout.Text = "ℹ Программа";
                btnScan.Text = "Сканировать";
                btnStop.Text = "Стоп";
                btnExport.Text = "Выгрузить";
                lblPorts.Text = "Диапазон портов:";
                cmbPortMode.Items.Clear();
                cmbPortMode.Items.AddRange(new object[] { "Основные (1-1023)", "Пользовательские (1024-49151)", "Приватные (49152-65535)", "Все (1-65535)", "Свои" });
                lblTheme.Text = "Тема:";
                _themeDisplayNames = new Dictionary<string, string>
                {
                    ["Light"] = "Светлая",
                    ["System"] = "Серая",
                    ["Yellow"] = "Жёлтая",
                    ["LightBlue"] = "Морская",
                    ["Pastel"] = "Пастельная",
                    ["Mint"] = "Мятная",
                    ["Lavender"] = "Лавандовая",
                    ["Purple"] = "Фиолетовая"
                };
                cmbTheme.Items.Clear();
                foreach (var pair in _themeDisplayNames)
                    cmbTheme.Items.Add(pair.Value);

                lblLanguage.Text = "Язык:";
                lblAbout.Text = "CRYPTONIX v1.1.7 patch b33\n\nРазработано: wexyami (CRYPTONIX XLL)\nЛицензия: MIT (АКТИВНА)\n\nСканер IP-адресов с определением ОС и открытых портов.\nИспользуется для быстрого анализа локальной сети.";
                this.Text = "CRYPTONIX";
                _modeLabelPrefix = "Режим: ";

                if (lstResult.Columns.Count >= 7)
                {
                    lstResult.Columns[0].Text = "IP Address";
                    lstResult.Columns[1].Text = "Имя";
                    lstResult.Columns[2].Text = "MAC-адрес";
                    lstResult.Columns[3].Text = "Статус";
                    lstResult.Columns[4].Text = "Время пинга (мс)";
                    lstResult.Columns[5].Text = "ОС";
                    lstResult.Columns[6].Text = "Открытые порты";
                }
                cmbPortMode.SelectedIndex = 0;
            }

            UpdateModeLabel();
        }

        private void ShowTab(Panel panel)
        {
            panelScan.Visible = false;
            panelSettings.Visible = false;
            panelAbout.Visible = false;
            panel.Visible = true;
            panel.BringToFront();

            btnTabScan.BackColor = Color.Transparent;
            btnTabSettings.BackColor = Color.Transparent;
            btnTabAbout.BackColor = Color.Transparent;
            if (panel == panelScan) btnTabScan.BackColor = Color.White;
            else if (panel == panelSettings) btnTabSettings.BackColor = Color.White;
            else if (panel == panelAbout) btnTabAbout.BackColor = Color.White;

            if (panel == panelScan) { btnTabScan.ForeColor = Color.Black; btnTabSettings.ForeColor = Color.White; btnTabAbout.ForeColor = Color.White; }
            else if (panel == panelSettings) { btnTabSettings.ForeColor = Color.Black; btnTabScan.ForeColor = Color.White; btnTabAbout.ForeColor = Color.White; }
            else if (panel == panelAbout) { btnTabAbout.ForeColor = Color.Black; btnTabScan.ForeColor = Color.White; btnTabSettings.ForeColor = Color.White; }
        }

        private void BtnTabScan_Click(object sender, EventArgs e) { ShowTab(panelScan); }
        private void BtnTabSettings_Click(object sender, EventArgs e) { ShowTab(panelSettings); }
        private void BtnTabAbout_Click(object sender, EventArgs e) { ShowTab(panelAbout); }

        private async Task ScanRangeAsync(long startIp, long endIp, int portStart, int portEnd, CancellationToken token)
        {
            var tasks = new List<Task>();
            int addedCount = 0;
            long processed = 0;

            for (long i = startIp; i <= endIp; i++)
            {
                if (token.IsCancellationRequested || _stopRequested) break;
                string ipString = LongToIp(i);

                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        using (var ping = new Ping())
                        {
                            PingReply reply = await ping.SendPingAsync(ipString, PingTimeout);
                            long current = Interlocked.Increment(ref processed);

                            if (reply.Status == IPStatus.Success)
                            {
                                int ttl = reply.Options?.Ttl ?? 0;
                                string mac = GetMacAddressViaARP(ipString);
                                string name = await GetHostNameAsync(ipString);
                                string openPorts = await CheckPortsInRangeAsync(ipString, portStart, portEnd, token);
                                string os = DetectOS(ipString, openPorts, ttl);

                                bool hasOpenPorts = (openPorts != "None");
                                int imageIndex = hasOpenPorts ? 1 : 0;
                                string pingDisplay = reply.RoundtripTime.ToString();

                                lstResult.BeginInvoke((MethodInvoker)(() =>
                                {
                                    AddResultItem(ipString, name, mac, "OK", pingDisplay, os, openPorts, imageIndex);
                                    if (++addedCount % 5 == 0)
                                        lstResult.Refresh();
                                }));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Ошибка при сканировании {ipString}: {ex.Message}");
                    }
                }, token));

                if (tasks.Count >= 100)
                {
                    await Task.WhenAny(tasks);
                    tasks.RemoveAll(t => t.IsCompleted);
                }
            }

            await Task.WhenAll(tasks);
            lstResult.Invoke((MethodInvoker)(() => lstResult.Refresh()));
        }

        private async Task ScanRangeAsync(long startIp, long endIp, List<int> customPorts, CancellationToken token)
        {
            var tasks = new List<Task>();
            int addedCount = 0;
            long processed = 0;

            for (long i = startIp; i <= endIp; i++)
            {
                if (token.IsCancellationRequested || _stopRequested) break;
                string ipString = LongToIp(i);

                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        using (var ping = new Ping())
                        {
                            PingReply reply = await ping.SendPingAsync(ipString, PingTimeout);
                            long current = Interlocked.Increment(ref processed);

                            if (reply.Status == IPStatus.Success)
                            {
                                int ttl = reply.Options?.Ttl ?? 0;
                                string mac = GetMacAddressViaARP(ipString);
                                string name = await GetHostNameAsync(ipString);
                                string openPorts = await CheckPortsListAsync(ipString, customPorts, token);
                                string os = DetectOS(ipString, openPorts, ttl);

                                bool hasOpenPorts = (openPorts != "None");
                                int imageIndex = hasOpenPorts ? 1 : 0;
                                string pingDisplay = reply.RoundtripTime.ToString();

                                lstResult.BeginInvoke((MethodInvoker)(() =>
                                {
                                    AddResultItem(ipString, name, mac, "OK", pingDisplay, os, openPorts, imageIndex);
                                    if (++addedCount % 5 == 0)
                                        lstResult.Refresh();
                                }));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Ошибка при сканировании {ipString}: {ex.Message}");
                    }
                }, token));

                if (tasks.Count >= 100)
                {
                    await Task.WhenAny(tasks);
                    tasks.RemoveAll(t => t.IsCompleted);
                }
            }

            await Task.WhenAll(tasks);
            lstResult.Invoke((MethodInvoker)(() => lstResult.Refresh()));
        }

        private string GetMacAddressViaARP(string ip)
        {
            try
            {
                IPAddress ipAddr = IPAddress.Parse(ip);
                byte[] macAddr = new byte[6];
                int len = macAddr.Length;
                int destIp = BitConverter.ToInt32(ipAddr.GetAddressBytes(), 0);
                int result = SendARP(destIp, 0, macAddr, ref len);
                if (result == 0 && len == 6)
                    return string.Join(":", macAddr.Select(b => b.ToString("X2")));
            }
            catch { }
            return "Unknown";
        }

        private async Task<string> GetHostNameAsync(string ip)
        {
            return await Task.Run(() =>
            {
                try
                {
                    IPAddress address = IPAddress.Parse(ip);
                    IPHostEntry hostEntry = Dns.GetHostEntry(address);
                    return hostEntry.HostName;
                }
                catch
                {
                    return ip;
                }
            });
        }

        private async Task<string> CheckPortsInRangeAsync(string ip, int startPort, int endPort, CancellationToken token)
        {
            var foundPorts = new List<int>();
            var semaphore = new SemaphoreSlim(MaxConcurrentPorts);
            var tasks = new List<Task>();
            for (int port = startPort; port <= endPort; port++)
            {
                if (token.IsCancellationRequested || _stopRequested) break;
                tasks.Add(CheckPortWithSemaphore(ip, port, semaphore, foundPorts, token));
            }
            await Task.WhenAll(tasks);
            return foundPorts.Count > 0 ? string.Join(", ", foundPorts.OrderBy(p => p)) : "None";
        }

        private async Task<string> CheckPortsListAsync(string ip, List<int> ports, CancellationToken token)
        {
            var foundPorts = new List<int>();
            var semaphore = new SemaphoreSlim(MaxConcurrentPorts);
            var tasks = new List<Task>();
            foreach (int port in ports)
            {
                if (token.IsCancellationRequested || _stopRequested) break;
                tasks.Add(CheckPortWithSemaphore(ip, port, semaphore, foundPorts, token));
            }
            await Task.WhenAll(tasks);
            return foundPorts.Count > 0 ? string.Join(", ", foundPorts.OrderBy(p => p)) : "None";
        }

        private async Task CheckPortWithSemaphore(string ip, int port, SemaphoreSlim semaphore, List<int> foundPorts, CancellationToken token)
        {
            await semaphore.WaitAsync(token);
            try
            {
                if (token.IsCancellationRequested || _stopRequested) return;
                bool isOpen = await IsPortOpenAsync(ip, port, token);
                if (isOpen) { lock (foundPorts) foundPorts.Add(port); }
            }
            finally { semaphore.Release(); }
        }

        private async Task<bool> IsPortOpenAsync(string ip, int port, CancellationToken token)
        {
            using (var client = new TcpClient())
            {
                var connectTask = client.ConnectAsync(ip, port);
                if (await Task.WhenAny(connectTask, Task.Delay(PortConnectTimeout, token)) == connectTask)
                {
                    if (client.Connected) { client.Close(); return true; }
                }
                return false;
            }
        }

        private string DetectOS(string ip, string openPorts, int ttl)
        {
            string os = "Unknown";
            if (ttl == 64) os = "Linux / macOS / Unix";
            else if (ttl == 128) os = "Windows";
            else if (ttl == 255) os = "Cisco / FreeBSD";
            else if (ttl == 60) os = "Linux (low TTL)";
            else if (ttl == 32) os = "Windows (low TTL)";

            bool hasSMB = openPorts.Contains("445") || openPorts.Contains("139") || openPorts.Contains("135");
            bool hasRDP = openPorts.Contains("3389");
            bool hasSSH = openPorts.Contains("22");
            bool hasHTTP = openPorts.Contains("80") || openPorts.Contains("443") || openPorts.Contains("8080");
            bool hasFTP = openPorts.Contains("21");
            bool hasSMTP = openPorts.Contains("25") || openPorts.Contains("587");

            if ((hasSMB || hasRDP) && ttl == 128)
            {
                os = "Windows";
                string banner = "";
                if (hasSMB) banner = GetBanner(ip, 445);
                if (banner.Contains("Windows 10")) os = "Windows 10";
                else if (banner.Contains("Windows 11")) os = "Windows 11";
                else if (banner.Contains("Windows Server 2019")) os = "Windows Server 2019";
                else if (banner.Contains("Windows Server 2016")) os = "Windows Server 2016";
                else if (banner.Contains("Windows 7")) os = "Windows 7";
                else if (banner.Contains("Windows 8")) os = "Windows 8";
                if (os == "Windows" && hasRDP)
                {
                    string rdpBanner = GetBanner(ip, 3389);
                    if (rdpBanner.Contains("6.1")) os = "Windows 7";
                    else if (rdpBanner.Contains("6.2")) os = "Windows 8";
                    else if (rdpBanner.Contains("10.0")) os = "Windows 10/11";
                }
                if (hasHTTP)
                {
                    string header = GetHttpHeader(ip, openPorts.Contains("443") ? 443 : 80);
                    if (header.Contains("Microsoft-IIS/10.0")) os = "Windows Server 2019/2022";
                    else if (header.Contains("Microsoft-IIS/8.5")) os = "Windows Server 2012 R2";
                    else if (header.Contains("Microsoft-IIS/7.5")) os = "Windows Server 2008 R2 / Windows 7";
                }
            }
            else if (hasSSH && (ttl == 64 || ttl == 60))
            {
                os = "Linux / Unix";
                string sshBanner = GetBanner(ip, 22);
                if (sshBanner.Contains("Ubuntu")) os = "Ubuntu " + ExtractVersion(sshBanner);
                else if (sshBanner.Contains("Debian")) os = "Debian " + ExtractVersion(sshBanner);
                else if (sshBanner.Contains("Fedora")) os = "Fedora";
                else if (sshBanner.Contains("CentOS")) os = "CentOS";
                else if (sshBanner.Contains("Red Hat")) os = "RHEL";
                else if (sshBanner.Contains("OpenSSH") && !os.Contains("Ubuntu") && !os.Contains("Debian"))
                    os = "Linux (OpenSSH)";
                if (hasHTTP)
                {
                    string header = GetHttpHeader(ip, openPorts.Contains("443") ? 443 : 80);
                    if (header.Contains("nginx") && header.Contains("Ubuntu")) os = "Ubuntu with nginx";
                    else if (header.Contains("nginx") && header.Contains("Debian")) os = "Debian with nginx";
                    else if (header.Contains("Apache") && header.Contains("Ubuntu")) os = "Ubuntu with Apache";
                    else if (header.Contains("Apache") && header.Contains("Debian")) os = "Debian with Apache";
                    else if (header.Contains("Apache") && header.Contains("CentOS")) os = "CentOS with Apache";
                }
            }
            else if (ttl == 64 && (openPorts.Contains("88") || openPorts.Contains("548")))
            {
                os = "macOS";
                if (hasHTTP) { string header = GetHttpHeader(ip, 80); if (header.Contains("Apache") && header.Contains("macOS")) os = "macOS with Apache"; }
            }
            else if (ttl == 64 && hasSSH && !hasSMB && !hasRDP)
            {
                string banner = GetBanner(ip, 22);
                if (banner.Contains("FreeBSD")) os = "FreeBSD";
            }
            else if (ttl == 255 && (openPorts.Contains("23") || openPorts.Contains("22")))
            {
                os = "Cisco IOS";
            }

            if (os == "Unknown" && hasHTTP)
            {
                string header = GetHttpHeader(ip, openPorts.Contains("443") ? 443 : 80);
                if (header.Contains("Microsoft-IIS")) os = "Windows (IIS)";
                else if (header.Contains("nginx")) os = "Linux (nginx)";
                else if (header.Contains("Apache")) os = "Linux (Apache)";
                else if (header.Contains("lighttpd")) os = "Linux (lighttpd)";
            }
            if (os == "Unknown" && hasFTP)
            {
                string banner = GetBanner(ip, 21);
                if (banner.Contains("Windows")) os = "Windows FTP";
                else if (banner.Contains("vsftpd")) os = "Linux (vsftpd)";
                else if (banner.Contains("ProFTPD")) os = "Linux (ProFTPD)";
            }
            if (os == "Unknown" && hasSMTP)
            {
                string banner = GetBanner(ip, openPorts.Contains("25") ? 25 : 587);
                if (banner.Contains("Exchange")) os = "Windows Exchange";
                else if (banner.Contains("Postfix")) os = "Linux (Postfix)";
                else if (banner.Contains("Sendmail")) os = "Linux/Unix (Sendmail)";
            }
            return os;
        }

        private string GetBanner(string ip, int port)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    var task = client.ConnectAsync(ip, port);
                    if (task.Wait(BannerTimeout))
                    {
                        using (var stream = client.GetStream())
                        {
                            stream.ReadTimeout = BannerTimeout;
                            byte[] buffer = new byte[256];
                            int bytes = stream.Read(buffer, 0, buffer.Length);
                            if (bytes > 0) return Encoding.ASCII.GetString(buffer, 0, bytes).Trim();
                        }
                    }
                }
            }
            catch { }
            return "";
        }

        private string GetHttpHeader(string ip, int port)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    var task = client.ConnectAsync(ip, port);
                    if (task.Wait(BannerTimeout))
                    {
                        using (var stream = client.GetStream())
                        {
                            string request = "HEAD / HTTP/1.0\r\nHost: " + ip + "\r\nConnection: close\r\n\r\n";
                            byte[] data = Encoding.ASCII.GetBytes(request);
                            stream.Write(data, 0, data.Length);
                            stream.ReadTimeout = BannerTimeout;
                            byte[] buffer = new byte[1024];
                            int bytes = stream.Read(buffer, 0, buffer.Length);
                            if (bytes > 0) return Encoding.ASCII.GetString(buffer, 0, bytes);
                        }
                    }
                }
            }
            catch { }
            return "";
        }

        private string ExtractVersion(string banner)
        {
            var match = System.Text.RegularExpressions.Regex.Match(banner, @"\d+\.\d+(\.\d+)?");
            return match.Success ? match.Value : "";
        }

        private void AddResultItem(string ip, string name, string mac, string status, string pingTime, string os, string openPorts, int imageIndex)
        {
            if (lstResult == null) return;
            if (string.IsNullOrEmpty(name) || name == "Unknown")
                name = ip;

            var item = new ListViewItem(ip, imageIndex);
            item.SubItems.Add(name);
            item.SubItems.Add(mac);
            item.SubItems.Add(status);
            item.SubItems.Add(pingTime);
            item.SubItems.Add(os);
            item.SubItems.Add(openPorts);
            item.ForeColor = lstResult.ForeColor;
            item.Font = new Font(item.Font, FontStyle.Bold);
            item.Checked = false;
            lstResult.Items.Add(item);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (lstResult.Items.Count == 0)
            {
                MessageBox.Show("Нет данных для выгрузки. Сначала выполните сканирование.", "Информация");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Текстовые файлы (*.txt)|*.txt";
                sfd.DefaultExt = "txt";
                sfd.FileName = $"CRYPTONIX_scan_{DateTime.Now:yyyyMMdd_HHmmss}";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(sfd.FileName, false, Encoding.UTF8))
                        {
                            List<string> headers = new List<string>();
                            foreach (ColumnHeader col in lstResult.Columns)
                                headers.Add(col.Text);
                            sw.WriteLine(string.Join("\t", headers));

                            foreach (ListViewItem item in lstResult.Items)
                            {
                                List<string> row = new List<string>();
                                for (int i = 0; i < item.SubItems.Count; i++)
                                    row.Add(item.SubItems[i].Text);
                                sw.WriteLine(string.Join("\t", row));
                            }
                        }
                        System.Diagnostics.Process.Start("notepad.exe", sfd.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка");
                    }
                }
            }
        }

        private void btnOpenBrowser_Click(object sender, EventArgs e)
        {
            if (lstResult.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выделите IP-адрес в списке.", "Нет выбора");
                return;
            }
            ListViewItem item = lstResult.SelectedItems[0];
            if (item != null && item.SubItems.Count > 0)
            {
                string ip = item.SubItems[0].Text.Trim();
                if (!string.IsNullOrEmpty(ip))
                {
                    try
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = "http://" + ip,
                            UseShellExecute = true
                        });
                    }
                    catch { MessageBox.Show("Не удалось открыть браузер.", "Ошибка"); }
                }
            }
        }

        private class PortRangeInfo { public int StartPort; public int EndPort; public string ModeName; }
    }
}
