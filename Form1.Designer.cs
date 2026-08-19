namespace CRYPTONIX
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) { components.Dispose(); } base.Dispose(disposing); }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panelLeft = new System.Windows.Forms.Panel();
            this.tableLayoutLeft = new System.Windows.Forms.TableLayoutPanel();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnTabAbout = new System.Windows.Forms.Button();
            this.btnTabSettings = new System.Windows.Forms.Button();
            this.btnTabScan = new System.Windows.Forms.Button();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelScan = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.txtMyIp = new System.Windows.Forms.TextBox();
            this.btnScan = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.lblMode = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lstResult = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panelSettings = new System.Windows.Forms.Panel();
            this.txtCustomPorts = new System.Windows.Forms.TextBox();
            this.lblPorts = new System.Windows.Forms.Label();
            this.cmbPortMode = new System.Windows.Forms.ComboBox();
            this.lblTheme = new System.Windows.Forms.Label();
            this.cmbTheme = new System.Windows.Forms.ComboBox();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.panelAbout = new System.Windows.Forms.Panel();
            this.lblAbout = new System.Windows.Forms.Label();
            this.panelLeft.SuspendLayout();
            this.tableLayoutLeft.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.panelScan.SuspendLayout();
            this.panelSettings.SuspendLayout();
            this.panelAbout.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.panelLeft.Controls.Add(this.tableLayoutLeft);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Padding = new System.Windows.Forms.Padding(5);
            this.panelLeft.Size = new System.Drawing.Size(160, 607);
            this.panelLeft.TabIndex = 1;
            // 
            // tableLayoutLeft
            // 
            this.tableLayoutLeft.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutLeft.ColumnCount = 1;
            this.tableLayoutLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutLeft.Controls.Add(this.panelButtons, 0, 1);
            this.tableLayoutLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutLeft.Location = new System.Drawing.Point(5, 5);
            this.tableLayoutLeft.Name = "tableLayoutLeft";
            this.tableLayoutLeft.RowCount = 3;
            this.tableLayoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutLeft.Size = new System.Drawing.Size(150, 597);
            this.tableLayoutLeft.TabIndex = 0;
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.Color.Transparent;
            this.panelButtons.Controls.Add(this.btnTabAbout);
            this.panelButtons.Controls.Add(this.btnTabSettings);
            this.panelButtons.Controls.Add(this.btnTabScan);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelButtons.Location = new System.Drawing.Point(3, 200);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(144, 196);
            this.panelButtons.TabIndex = 0;
            // 
            // btnTabAbout
            // 
            this.btnTabAbout.BackColor = System.Drawing.Color.Transparent;
            this.btnTabAbout.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTabAbout.FlatAppearance.BorderSize = 0;
            this.btnTabAbout.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.btnTabAbout.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnTabAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTabAbout.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnTabAbout.ForeColor = System.Drawing.Color.White;
            this.btnTabAbout.Location = new System.Drawing.Point(0, 100);
            this.btnTabAbout.Name = "btnTabAbout";
            this.btnTabAbout.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnTabAbout.Size = new System.Drawing.Size(144, 50);
            this.btnTabAbout.TabIndex = 0;
            this.btnTabAbout.Text = "ℹ О программе";
            this.btnTabAbout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTabAbout.UseVisualStyleBackColor = false;
            this.btnTabAbout.Click += new System.EventHandler(this.BtnTabAbout_Click);
            // 
            // btnTabSettings
            // 
            this.btnTabSettings.BackColor = System.Drawing.Color.Transparent;
            this.btnTabSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTabSettings.FlatAppearance.BorderSize = 0;
            this.btnTabSettings.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.btnTabSettings.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnTabSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTabSettings.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnTabSettings.ForeColor = System.Drawing.Color.White;
            this.btnTabSettings.Location = new System.Drawing.Point(0, 50);
            this.btnTabSettings.Name = "btnTabSettings";
            this.btnTabSettings.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnTabSettings.Size = new System.Drawing.Size(144, 50);
            this.btnTabSettings.TabIndex = 1;
            this.btnTabSettings.Text = "⚙ Настройки";
            this.btnTabSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTabSettings.UseVisualStyleBackColor = false;
            this.btnTabSettings.Click += new System.EventHandler(this.BtnTabSettings_Click);
            // 
            // btnTabScan
            // 
            this.btnTabScan.BackColor = System.Drawing.Color.Transparent;
            this.btnTabScan.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTabScan.FlatAppearance.BorderSize = 0;
            this.btnTabScan.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.btnTabScan.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnTabScan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTabScan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnTabScan.ForeColor = System.Drawing.Color.White;
            this.btnTabScan.Location = new System.Drawing.Point(0, 0);
            this.btnTabScan.Name = "btnTabScan";
            this.btnTabScan.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnTabScan.Size = new System.Drawing.Size(144, 50);
            this.btnTabScan.TabIndex = 2;
            this.btnTabScan.Text = "📡 Сканирование";
            this.btnTabScan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTabScan.UseVisualStyleBackColor = false;
            this.btnTabScan.Click += new System.EventHandler(this.BtnTabScan_Click);
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.panelContent.Controls.Add(this.panelScan);
            this.panelContent.Controls.Add(this.panelSettings);
            this.panelContent.Controls.Add(this.panelAbout);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(160, 0);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(20);
            this.panelContent.Size = new System.Drawing.Size(1364, 607);
            this.panelContent.TabIndex = 0;
            // 
            // panelScan
            // 
            this.panelScan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.panelScan.Controls.Add(this.btnExport);
            this.panelScan.Controls.Add(this.txtMyIp);
            this.panelScan.Controls.Add(this.btnScan);
            this.panelScan.Controls.Add(this.btnStop);
            this.panelScan.Controls.Add(this.lblMode);
            this.panelScan.Controls.Add(this.progressBar1);
            this.panelScan.Controls.Add(this.lstResult);
            this.panelScan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelScan.Location = new System.Drawing.Point(20, 20);
            this.panelScan.Name = "panelScan";
            this.panelScan.Size = new System.Drawing.Size(1324, 567);
            this.panelScan.TabIndex = 0;
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(200)))));
            this.btnExport.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(200)))));
            this.btnExport.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(180)))), ((int)(((byte)(230)))));
            this.btnExport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(200)))));
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(512, 0);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(120, 30);
            this.btnExport.TabIndex = 6;
            this.btnExport.Text = "Выгрузить";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // txtMyIp
            // 
            this.txtMyIp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.txtMyIp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMyIp.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMyIp.ForeColor = System.Drawing.Color.White;
            this.txtMyIp.Location = new System.Drawing.Point(0, 0);
            this.txtMyIp.Name = "txtMyIp";
            this.txtMyIp.Size = new System.Drawing.Size(250, 25);
            this.txtMyIp.TabIndex = 0;
            this.txtMyIp.Text = "192.168.1.1/24";
            this.txtMyIp.Click += new System.EventHandler(this.txtMyIp_Click);
            // 
            // btnScan
            // 
            this.btnScan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(83)))));
            this.btnScan.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(83)))));
            this.btnScan.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(230)))), ((int)(((byte)(118)))));
            this.btnScan.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(83)))));
            this.btnScan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnScan.ForeColor = System.Drawing.Color.White;
            this.btnScan.Location = new System.Drawing.Point(260, 0);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(120, 30);
            this.btnScan.TabIndex = 1;
            this.btnScan.Text = "Сканировать";
            this.btnScan.UseVisualStyleBackColor = false;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(82)))), ((int)(((byte)(82)))));
            this.btnStop.Enabled = false;
            this.btnStop.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(82)))), ((int)(((byte)(82)))));
            this.btnStop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(82)))), ((int)(((byte)(82)))));
            this.btnStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.Location = new System.Drawing.Point(386, 0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(120, 30);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Стоп";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMode.ForeColor = System.Drawing.Color.LightGray;
            this.lblMode.Location = new System.Drawing.Point(1026, 10);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(146, 15);
            this.lblMode.TabIndex = 3;
            this.lblMode.Text = "Режим: Основные порты";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.progressBar1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(83)))));
            this.progressBar1.Location = new System.Drawing.Point(0, 40);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1321, 15);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 4;
            // 
            // lstResult
            // 
            this.lstResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lstResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstResult.CheckBoxes = true;
            this.lstResult.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lstResult.ForeColor = System.Drawing.Color.White;
            this.lstResult.FullRowSelect = true;
            this.lstResult.GridLines = true;
            this.lstResult.HideSelection = false;
            this.lstResult.Location = new System.Drawing.Point(0, 61);
            this.lstResult.Name = "lstResult";
            this.lstResult.Size = new System.Drawing.Size(1321, 574);
            this.lstResult.SmallImageList = this.imageList1;
            this.lstResult.TabIndex = 5;
            this.lstResult.UseCompatibleStateImageBehavior = false;
            this.lstResult.View = System.Windows.Forms.View.Details;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // panelSettings
            // 
            this.panelSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.panelSettings.Controls.Add(this.txtCustomPorts);
            this.panelSettings.Controls.Add(this.lblPorts);
            this.panelSettings.Controls.Add(this.cmbPortMode);
            this.panelSettings.Controls.Add(this.lblTheme);
            this.panelSettings.Controls.Add(this.cmbTheme);
            this.panelSettings.Controls.Add(this.lblLanguage);
            this.panelSettings.Controls.Add(this.cmbLanguage);
            this.panelSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSettings.Location = new System.Drawing.Point(20, 20);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.Size = new System.Drawing.Size(1324, 567);
            this.panelSettings.TabIndex = 1;
            this.panelSettings.Visible = false;
            // 
            // txtCustomPorts
            // 
            this.txtCustomPorts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustomPorts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.txtCustomPorts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCustomPorts.ForeColor = System.Drawing.Color.White;
            this.txtCustomPorts.Location = new System.Drawing.Point(180, 55);
            this.txtCustomPorts.Name = "txtCustomPorts";
            this.txtCustomPorts.Size = new System.Drawing.Size(1120, 20);
            this.txtCustomPorts.TabIndex = 2;
            this.txtCustomPorts.Text = "80,443,22";
            this.txtCustomPorts.Visible = false;
            // 
            // lblPorts
            // 
            this.lblPorts.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPorts.ForeColor = System.Drawing.Color.White;
            this.lblPorts.Location = new System.Drawing.Point(20, 20);
            this.lblPorts.Name = "lblPorts";
            this.lblPorts.Size = new System.Drawing.Size(150, 25);
            this.lblPorts.TabIndex = 0;
            this.lblPorts.Text = "Диапазон портов:";
            // 
            // cmbPortMode
            // 
            this.cmbPortMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.cmbPortMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPortMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbPortMode.ForeColor = System.Drawing.Color.White;
            this.cmbPortMode.Items.AddRange(new object[] {
            "Основные (1-1023)",
            "Пользовательские (1024-49151)",
            "Приватные (49152-65535)",
            "Все (1-65535)",
            "Свои"});
            this.cmbPortMode.Location = new System.Drawing.Point(180, 20);
            this.cmbPortMode.Name = "cmbPortMode";
            this.cmbPortMode.Size = new System.Drawing.Size(200, 21);
            this.cmbPortMode.TabIndex = 1;
            this.cmbPortMode.SelectedIndexChanged += new System.EventHandler(this.cmbPortMode_SelectedIndexChanged);
            // 
            // lblTheme
            // 
            this.lblTheme.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTheme.ForeColor = System.Drawing.Color.White;
            this.lblTheme.Location = new System.Drawing.Point(20, 100);
            this.lblTheme.Name = "lblTheme";
            this.lblTheme.Size = new System.Drawing.Size(150, 25);
            this.lblTheme.TabIndex = 3;
            this.lblTheme.Text = "Тема:";
            // 
            // cmbTheme
            // 
            this.cmbTheme.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.cmbTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbTheme.ForeColor = System.Drawing.Color.White;
            this.cmbTheme.Location = new System.Drawing.Point(180, 100);
            this.cmbTheme.Name = "cmbTheme";
            this.cmbTheme.Size = new System.Drawing.Size(200, 21);
            this.cmbTheme.TabIndex = 4;
            this.cmbTheme.SelectedIndexChanged += new System.EventHandler(this.cmbTheme_SelectedIndexChanged);
            // 
            // lblLanguage
            // 
            this.lblLanguage.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLanguage.ForeColor = System.Drawing.Color.White;
            this.lblLanguage.Location = new System.Drawing.Point(20, 140);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Size = new System.Drawing.Size(150, 25);
            this.lblLanguage.TabIndex = 5;
            this.lblLanguage.Text = "Язык:";
            // 
            // cmbLanguage
            // 
            this.cmbLanguage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLanguage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbLanguage.ForeColor = System.Drawing.Color.White;
            this.cmbLanguage.Items.AddRange(new object[] {
            "Русский",
            "English"});
            this.cmbLanguage.Location = new System.Drawing.Point(180, 140);
            this.cmbLanguage.Name = "cmbLanguage";
            this.cmbLanguage.Size = new System.Drawing.Size(200, 21);
            this.cmbLanguage.TabIndex = 6;
            this.cmbLanguage.SelectedIndexChanged += new System.EventHandler(this.cmbLanguage_SelectedIndexChanged);
            // 
            // panelAbout
            // 
            this.panelAbout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.panelAbout.Controls.Add(this.lblAbout);
            this.panelAbout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAbout.Location = new System.Drawing.Point(20, 20);
            this.panelAbout.Name = "panelAbout";
            this.panelAbout.Size = new System.Drawing.Size(1324, 567);
            this.panelAbout.TabIndex = 2;
            this.panelAbout.Visible = false;
            // 
            // lblAbout
            // 
            this.lblAbout.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblAbout.ForeColor = System.Drawing.Color.White;
            this.lblAbout.Location = new System.Drawing.Point(20, 20);
            this.lblAbout.Name = "lblAbout";
            this.lblAbout.Size = new System.Drawing.Size(400, 200);
            this.lblAbout.TabIndex = 0;
            this.lblAbout.Text = resources.GetString("lblAbout.Text");
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.ClientSize = new System.Drawing.Size(1524, 607);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelLeft);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "Form1";
            this.Text = "CRYPTONIX";
            this.panelLeft.ResumeLayout(false);
            this.tableLayoutLeft.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.panelScan.ResumeLayout(false);
            this.panelScan.PerformLayout();
            this.panelSettings.ResumeLayout(false);
            this.panelSettings.PerformLayout();
            this.panelAbout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.TableLayoutPanel tableLayoutLeft;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnTabScan;
        private System.Windows.Forms.Button btnTabSettings;
        private System.Windows.Forms.Button btnTabAbout;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Panel panelScan;
        private System.Windows.Forms.Panel panelSettings;
        private System.Windows.Forms.Panel panelAbout;
        private System.Windows.Forms.TextBox txtMyIp;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ListView lstResult;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label lblPorts;
        private System.Windows.Forms.ComboBox cmbPortMode;
        private System.Windows.Forms.TextBox txtCustomPorts;
        private System.Windows.Forms.Label lblTheme;
        private System.Windows.Forms.ComboBox cmbTheme;
        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.ComboBox cmbLanguage;
        private System.Windows.Forms.Label lblAbout;
        private System.Windows.Forms.Button btnExport;
    }
}
