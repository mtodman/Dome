namespace ASCOM.MattsDome
{
    partial class logWindow
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
            this.txtLogWindow = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtLogWindow
            // 
            this.txtLogWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLogWindow.Location = new System.Drawing.Point(7, 10);
            this.txtLogWindow.Multiline = true;
            this.txtLogWindow.Name = "txtLogWindow";
            this.txtLogWindow.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLogWindow.Size = new System.Drawing.Size(552, 392);
            this.txtLogWindow.TabIndex = 0;
            // 
            // logWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 414);
            this.Controls.Add(this.txtLogWindow);
            this.Name = "logWindow";
            this.Text = "logWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox txtLogWindow;

    }
}