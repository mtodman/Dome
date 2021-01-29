namespace ASCOM.MattsDome
{
    partial class SetupDialogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupDialogForm));
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.frmComm = new System.Windows.Forms.GroupBox();
            this.cmbComm = new System.Windows.Forms.ComboBox();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numUDLoggingLevel = new System.Windows.Forms.NumericUpDown();
            this.chkLCDBacklight = new System.Windows.Forms.CheckBox();
            this.chkLogToFile = new System.Windows.Forms.CheckBox();
            this.chkLoggingWindow = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtHomeSensorPos = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFindHomeTimeout = new System.Windows.Forms.TextBox();
            this.txtTicksPerRev = new System.Windows.Forms.TextBox();
            this.txtParkPos = new System.Windows.Forms.TextBox();
            this.cmdTest = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnPark = new System.Windows.Forms.Button();
            this.btnHome = new System.Windows.Forms.Button();
            this.btnCCW = new System.Windows.Forms.Button();
            this.btnCW = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtAzimuth = new System.Windows.Forms.TextBox();
            this.btnGetAzimuth = new System.Windows.Forms.Button();
            this.btnSetAzimuth = new System.Windows.Forms.Button();
            this.txtSetAzimuth = new System.Windows.Forms.TextBox();
            this.btnAbortSlew = new System.Windows.Forms.Button();
            this.btnSlewToTarget = new System.Windows.Forms.Button();
            this.txtTargetAzimuth = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.frmComm.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUDLoggingLevel)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(12, 522);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(59, 24);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.CmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(142, 522);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(59, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.CmdCancel_Click);
            // 
            // picASCOM
            // 
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.MattsDome.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(244, 26);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            // 
            // frmComm
            // 
            this.frmComm.Controls.Add(this.cmbComm);
            this.frmComm.Location = new System.Drawing.Point(12, 9);
            this.frmComm.Name = "frmComm";
            this.frmComm.Size = new System.Drawing.Size(177, 73);
            this.frmComm.TabIndex = 56;
            this.frmComm.TabStop = false;
            this.frmComm.Text = "Comm Port";
            // 
            // cmbComm
            // 
            this.cmbComm.FormattingEnabled = true;
            this.cmbComm.Location = new System.Drawing.Point(7, 28);
            this.cmbComm.Name = "cmbComm";
            this.cmbComm.Size = new System.Drawing.Size(121, 21);
            this.cmbComm.TabIndex = 0;
            this.cmbComm.Text = "Choose a Com Port";
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.label1);
            this.GroupBox2.Controls.Add(this.numUDLoggingLevel);
            this.GroupBox2.Controls.Add(this.chkLCDBacklight);
            this.GroupBox2.Controls.Add(this.chkLogToFile);
            this.GroupBox2.Controls.Add(this.chkLoggingWindow);
            this.GroupBox2.Controls.Add(this.label7);
            this.GroupBox2.Controls.Add(this.txtHomeSensorPos);
            this.GroupBox2.Controls.Add(this.label6);
            this.GroupBox2.Controls.Add(this.label5);
            this.GroupBox2.Controls.Add(this.label4);
            this.GroupBox2.Controls.Add(this.txtFindHomeTimeout);
            this.GroupBox2.Controls.Add(this.txtTicksPerRev);
            this.GroupBox2.Controls.Add(this.txtParkPos);
            this.GroupBox2.Location = new System.Drawing.Point(10, 88);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(287, 232);
            this.GroupBox2.TabIndex = 58;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Misc Settings";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 180);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Logging Level";
            // 
            // numUDLoggingLevel
            // 
            this.numUDLoggingLevel.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::ASCOM.MattsDome.Properties.Settings.Default, "LogLevel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numUDLoggingLevel.Location = new System.Drawing.Point(8, 178);
            this.numUDLoggingLevel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numUDLoggingLevel.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numUDLoggingLevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUDLoggingLevel.Name = "numUDLoggingLevel";
            this.numUDLoggingLevel.Size = new System.Drawing.Size(28, 20);
            this.numUDLoggingLevel.TabIndex = 18;
            this.numUDLoggingLevel.Value = global::ASCOM.MattsDome.Properties.Settings.Default.LogLevel;
            // 
            // chkLCDBacklight
            // 
            this.chkLCDBacklight.AutoSize = true;
            this.chkLCDBacklight.Checked = global::ASCOM.MattsDome.Properties.Settings.Default.EnableLCDBacklight;
            this.chkLCDBacklight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLCDBacklight.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.MattsDome.Properties.Settings.Default, "EnableLCDBacklight", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkLCDBacklight.Location = new System.Drawing.Point(9, 202);
            this.chkLCDBacklight.Name = "chkLCDBacklight";
            this.chkLCDBacklight.Size = new System.Drawing.Size(130, 17);
            this.chkLCDBacklight.TabIndex = 17;
            this.chkLCDBacklight.Text = "Enable LCD Backlight";
            this.chkLCDBacklight.UseVisualStyleBackColor = true;
            this.chkLCDBacklight.CheckedChanged += new System.EventHandler(this.chkLCDBacklight_CheckedChanged);
            // 
            // chkLogToFile
            // 
            this.chkLogToFile.AutoSize = true;
            this.chkLogToFile.Location = new System.Drawing.Point(9, 156);
            this.chkLogToFile.Name = "chkLogToFile";
            this.chkLogToFile.Size = new System.Drawing.Size(79, 17);
            this.chkLogToFile.TabIndex = 16;
            this.chkLogToFile.Text = "Log To File";
            this.chkLogToFile.UseVisualStyleBackColor = true;
            // 
            // chkLoggingWindow
            // 
            this.chkLoggingWindow.AutoSize = true;
            this.chkLoggingWindow.Location = new System.Drawing.Point(9, 133);
            this.chkLoggingWindow.Name = "chkLoggingWindow";
            this.chkLoggingWindow.Size = new System.Drawing.Size(142, 17);
            this.chkLoggingWindow.TabIndex = 15;
            this.chkLoggingWindow.Text = "Enable Logging Window";
            this.chkLoggingWindow.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 28);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(111, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Home Sensor Position";
            // 
            // txtHomeSensorPos
            // 
            this.txtHomeSensorPos.Location = new System.Drawing.Point(152, 25);
            this.txtHomeSensorPos.Name = "txtHomeSensorPos";
            this.txtHomeSensorPos.Size = new System.Drawing.Size(71, 20);
            this.txtHomeSensorPos.TabIndex = 12;
            this.txtHomeSensorPos.Leave += new System.EventHandler(this.txtHomeSensorPos_Leave);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 53);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Park Position";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(136, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Ticks per Dome Revolution";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(126, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Find Home timeout (secs)";
            // 
            // txtFindHomeTimeout
            // 
            this.txtFindHomeTimeout.Location = new System.Drawing.Point(152, 103);
            this.txtFindHomeTimeout.Name = "txtFindHomeTimeout";
            this.txtFindHomeTimeout.Size = new System.Drawing.Size(71, 20);
            this.txtFindHomeTimeout.TabIndex = 5;
            // 
            // txtTicksPerRev
            // 
            this.txtTicksPerRev.Location = new System.Drawing.Point(152, 77);
            this.txtTicksPerRev.Name = "txtTicksPerRev";
            this.txtTicksPerRev.Size = new System.Drawing.Size(71, 20);
            this.txtTicksPerRev.TabIndex = 4;
            this.txtTicksPerRev.Leave += new System.EventHandler(this.txtTicksPerRev_Leave);
            // 
            // txtParkPos
            // 
            this.txtParkPos.Location = new System.Drawing.Point(152, 51);
            this.txtParkPos.Name = "txtParkPos";
            this.txtParkPos.Size = new System.Drawing.Size(71, 20);
            this.txtParkPos.TabIndex = 3;
            this.txtParkPos.Leave += new System.EventHandler(this.txtParkPos_Leave);
            // 
            // cmdTest
            // 
            this.cmdTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdTest.Location = new System.Drawing.Point(77, 522);
            this.cmdTest.Name = "cmdTest";
            this.cmdTest.Size = new System.Drawing.Size(59, 24);
            this.cmdTest.TabIndex = 59;
            this.cmdTest.Text = "&Test Link";
            this.cmdTest.UseVisualStyleBackColor = true;
            this.cmdTest.Click += new System.EventHandler(this.CmdTest_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnPark);
            this.groupBox1.Controls.Add(this.btnHome);
            this.groupBox1.Controls.Add(this.btnCCW);
            this.groupBox1.Controls.Add(this.btnCW);
            this.groupBox1.Location = new System.Drawing.Point(12, 336);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(287, 90);
            this.groupBox1.TabIndex = 60;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Controls";
            // 
            // btnPark
            // 
            this.btnPark.Image = ((System.Drawing.Image)(resources.GetObject("btnPark.Image")));
            this.btnPark.Location = new System.Drawing.Point(148, 19);
            this.btnPark.Name = "btnPark";
            this.btnPark.Size = new System.Drawing.Size(65, 65);
            this.btnPark.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnPark, "Send Dome to the Park Position");
            this.btnPark.UseVisualStyleBackColor = true;
            this.btnPark.Click += new System.EventHandler(this.BtnPark_Click);
            // 
            // btnHome
            // 
            this.btnHome.Image = ((System.Drawing.Image)(resources.GetObject("btnHome.Image")));
            this.btnHome.Location = new System.Drawing.Point(77, 19);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(65, 65);
            this.btnHome.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btnHome, "Send Dome to the Home (sensor) Position");
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.BtnHome_Click);
            // 
            // btnCCW
            // 
            this.btnCCW.Image = ((System.Drawing.Image)(resources.GetObject("btnCCW.Image")));
            this.btnCCW.Location = new System.Drawing.Point(6, 19);
            this.btnCCW.Name = "btnCCW";
            this.btnCCW.Size = new System.Drawing.Size(65, 65);
            this.btnCCW.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btnCCW, "Rotate Dome Counter Clockwise");
            this.btnCCW.UseVisualStyleBackColor = true;
            this.btnCCW.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnCCW_MouseDown);
            this.btnCCW.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnCCW_MouseUp);
            // 
            // btnCW
            // 
            this.btnCW.Image = ((System.Drawing.Image)(resources.GetObject("btnCW.Image")));
            this.btnCW.Location = new System.Drawing.Point(219, 19);
            this.btnCW.Name = "btnCW";
            this.btnCW.Size = new System.Drawing.Size(61, 65);
            this.btnCW.TabIndex = 0;
            this.toolTip1.SetToolTip(this.btnCW, "Rotate Dome Clockwise");
            this.btnCW.UseVisualStyleBackColor = true;
            this.btnCW.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnCW_MouseDown);
            this.btnCW.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnCW_MouseUp);
            // 
            // txtAzimuth
            // 
            this.txtAzimuth.Enabled = false;
            this.txtAzimuth.Location = new System.Drawing.Point(113, 433);
            this.txtAzimuth.Name = "txtAzimuth";
            this.txtAzimuth.Size = new System.Drawing.Size(100, 20);
            this.txtAzimuth.TabIndex = 62;
            // 
            // btnGetAzimuth
            // 
            this.btnGetAzimuth.Location = new System.Drawing.Point(12, 431);
            this.btnGetAzimuth.Name = "btnGetAzimuth";
            this.btnGetAzimuth.Size = new System.Drawing.Size(86, 23);
            this.btnGetAzimuth.TabIndex = 63;
            this.btnGetAzimuth.Text = "Get Azimuth";
            this.btnGetAzimuth.UseVisualStyleBackColor = true;
            this.btnGetAzimuth.Click += new System.EventHandler(this.btnGetAzimuth_Click);
            // 
            // btnSetAzimuth
            // 
            this.btnSetAzimuth.Location = new System.Drawing.Point(12, 459);
            this.btnSetAzimuth.Name = "btnSetAzimuth";
            this.btnSetAzimuth.Size = new System.Drawing.Size(86, 23);
            this.btnSetAzimuth.TabIndex = 4;
            this.btnSetAzimuth.Text = "Set Azimuth";
            this.btnSetAzimuth.UseVisualStyleBackColor = true;
            this.btnSetAzimuth.Click += new System.EventHandler(this.btnSetAzimuth_Click);
            // 
            // txtSetAzimuth
            // 
            this.txtSetAzimuth.Location = new System.Drawing.Point(113, 462);
            this.txtSetAzimuth.Name = "txtSetAzimuth";
            this.txtSetAzimuth.Size = new System.Drawing.Size(100, 20);
            this.txtSetAzimuth.TabIndex = 3;
            // 
            // btnAbortSlew
            // 
            this.btnAbortSlew.Location = new System.Drawing.Point(224, 489);
            this.btnAbortSlew.Name = "btnAbortSlew";
            this.btnAbortSlew.Size = new System.Drawing.Size(75, 23);
            this.btnAbortSlew.TabIndex = 2;
            this.btnAbortSlew.Text = "Abort Slew";
            this.btnAbortSlew.UseVisualStyleBackColor = true;
            this.btnAbortSlew.Click += new System.EventHandler(this.btnSlewToTargetCCW_Click);
            // 
            // btnSlewToTarget
            // 
            this.btnSlewToTarget.Location = new System.Drawing.Point(12, 488);
            this.btnSlewToTarget.Name = "btnSlewToTarget";
            this.btnSlewToTarget.Size = new System.Drawing.Size(86, 24);
            this.btnSlewToTarget.TabIndex = 1;
            this.btnSlewToTarget.Text = "Slew to Target";
            this.btnSlewToTarget.UseVisualStyleBackColor = true;
            this.btnSlewToTarget.Click += new System.EventHandler(this.btnSlewToTargetCW_Click);
            // 
            // txtTargetAzimuth
            // 
            this.txtTargetAzimuth.Location = new System.Drawing.Point(113, 492);
            this.txtTargetAzimuth.Name = "txtTargetAzimuth";
            this.txtTargetAzimuth.Size = new System.Drawing.Size(100, 20);
            this.txtTargetAzimuth.TabIndex = 0;
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 558);
            this.Controls.Add(this.btnAbortSlew);
            this.Controls.Add(this.txtSetAzimuth);
            this.Controls.Add(this.txtTargetAzimuth);
            this.Controls.Add(this.btnSlewToTarget);
            this.Controls.Add(this.btnSetAzimuth);
            this.Controls.Add(this.btnGetAzimuth);
            this.Controls.Add(this.txtAzimuth);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmdTest);
            this.Controls.Add(this.GroupBox2);
            this.Controls.Add(this.frmComm);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MattsDome Setup v0.6.0";
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.frmComm.ResumeLayout(false);
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUDLoggingLevel)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        public System.Windows.Forms.GroupBox frmComm;
        public System.Windows.Forms.ComboBox cmbComm;
        internal System.Windows.Forms.GroupBox GroupBox2;
        private System.Windows.Forms.Label label7;
        internal System.Windows.Forms.TextBox txtHomeSensorPos;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        internal System.Windows.Forms.TextBox txtFindHomeTimeout;
        internal System.Windows.Forms.TextBox txtTicksPerRev;
        internal System.Windows.Forms.TextBox txtParkPos;
        public System.Windows.Forms.Button cmdTest;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCW;
        private System.Windows.Forms.Button btnCCW;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button btnPark;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox txtAzimuth;
        private System.Windows.Forms.Button btnGetAzimuth;
        private System.Windows.Forms.Button btnAbortSlew;
        private System.Windows.Forms.Button btnSlewToTarget;
        private System.Windows.Forms.TextBox txtTargetAzimuth;
        private System.Windows.Forms.Button btnSetAzimuth;
        private System.Windows.Forms.TextBox txtSetAzimuth;
        private System.Windows.Forms.CheckBox chkLogToFile;
        private System.Windows.Forms.CheckBox chkLoggingWindow;
        private System.Windows.Forms.CheckBox chkLCDBacklight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numUDLoggingLevel;
    }
}