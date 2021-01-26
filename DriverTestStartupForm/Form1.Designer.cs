namespace ASCOM.DriverStartupTestForm
{
    partial class Form1
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
            this.buttonChoose = new System.Windows.Forms.Button();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.labelDriverId = new System.Windows.Forms.Label();
            this.btnGetAz = new System.Windows.Forms.Button();
            this.btnSetAz = new System.Windows.Forms.Button();
            this.btnGotoAz = new System.Windows.Forms.Button();
            this.txtGetAz = new System.Windows.Forms.TextBox();
            this.txtSetAz = new System.Windows.Forms.TextBox();
            this.txtGotoAz = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonChoose
            // 
            this.buttonChoose.Location = new System.Drawing.Point(309, 10);
            this.buttonChoose.Name = "buttonChoose";
            this.buttonChoose.Size = new System.Drawing.Size(72, 23);
            this.buttonChoose.TabIndex = 0;
            this.buttonChoose.Text = "Choose";
            this.buttonChoose.UseVisualStyleBackColor = true;
            this.buttonChoose.Click += new System.EventHandler(this.ButtonChoose_Click);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(309, 39);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(72, 23);
            this.buttonConnect.TabIndex = 1;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.ButtonConnect_Click);
            // 
            // labelDriverId
            // 
            this.labelDriverId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelDriverId.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ASCOM.DriverStartupTestForm.Properties.Settings.Default, "DriverId", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.labelDriverId.Location = new System.Drawing.Point(12, 40);
            this.labelDriverId.Name = "labelDriverId";
            this.labelDriverId.Size = new System.Drawing.Size(291, 21);
            this.labelDriverId.TabIndex = 2;
            this.labelDriverId.Text = global::ASCOM.DriverStartupTestForm.Properties.Settings.Default.DriverId;
            this.labelDriverId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnGetAz
            // 
            this.btnGetAz.Location = new System.Drawing.Point(13, 89);
            this.btnGetAz.Name = "btnGetAz";
            this.btnGetAz.Size = new System.Drawing.Size(75, 23);
            this.btnGetAz.TabIndex = 3;
            this.btnGetAz.Text = "Get Az";
            this.btnGetAz.UseVisualStyleBackColor = true;
            this.btnGetAz.Click += new System.EventHandler(this.btnGetAz_Click);
            // 
            // btnSetAz
            // 
            this.btnSetAz.Location = new System.Drawing.Point(13, 119);
            this.btnSetAz.Name = "btnSetAz";
            this.btnSetAz.Size = new System.Drawing.Size(75, 23);
            this.btnSetAz.TabIndex = 4;
            this.btnSetAz.Text = "Set Az";
            this.btnSetAz.UseVisualStyleBackColor = true;
            this.btnSetAz.Click += new System.EventHandler(this.btnSetAz_Click);
            // 
            // btnGotoAz
            // 
            this.btnGotoAz.Location = new System.Drawing.Point(13, 149);
            this.btnGotoAz.Name = "btnGotoAz";
            this.btnGotoAz.Size = new System.Drawing.Size(75, 23);
            this.btnGotoAz.TabIndex = 5;
            this.btnGotoAz.Text = "Goto Az";
            this.btnGotoAz.UseVisualStyleBackColor = true;
            this.btnGotoAz.Click += new System.EventHandler(this.btnGotoAz_Click);
            // 
            // txtGetAz
            // 
            this.txtGetAz.Enabled = false;
            this.txtGetAz.Location = new System.Drawing.Point(113, 91);
            this.txtGetAz.Name = "txtGetAz";
            this.txtGetAz.Size = new System.Drawing.Size(100, 20);
            this.txtGetAz.TabIndex = 6;
            // 
            // txtSetAz
            // 
            this.txtSetAz.Location = new System.Drawing.Point(113, 121);
            this.txtSetAz.Name = "txtSetAz";
            this.txtSetAz.Size = new System.Drawing.Size(100, 20);
            this.txtSetAz.TabIndex = 7;
            // 
            // txtGotoAz
            // 
            this.txtGotoAz.Location = new System.Drawing.Point(113, 151);
            this.txtGotoAz.Name = "txtGotoAz";
            this.txtGotoAz.Size = new System.Drawing.Size(100, 20);
            this.txtGotoAz.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 262);
            this.Controls.Add(this.txtGotoAz);
            this.Controls.Add(this.txtSetAz);
            this.Controls.Add(this.txtGetAz);
            this.Controls.Add(this.btnGotoAz);
            this.Controls.Add(this.btnSetAz);
            this.Controls.Add(this.btnGetAz);
            this.Controls.Add(this.labelDriverId);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.buttonChoose);
            this.Name = "Form1";
            this.Text = "TEMPLATEDEVICETYPE Test";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonChoose;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Label labelDriverId;
        private System.Windows.Forms.Button btnGetAz;
        private System.Windows.Forms.Button btnSetAz;
        private System.Windows.Forms.Button btnGotoAz;
        private System.Windows.Forms.TextBox txtGetAz;
        private System.Windows.Forms.TextBox txtSetAz;
        private System.Windows.Forms.TextBox txtGotoAz;
    }
}

