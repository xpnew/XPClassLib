namespace XP.Win.WaitWin
{
    partial class WaitInfoGUI
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
            this.ucWave1 = new XP.Win.Components.UIControls.UCWave();
            this.Label_Message = new System.Windows.Forms.Label();
            this.bt_Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ucWave1
            // 
            this.ucWave1.Location = new System.Drawing.Point(19, 69);
            this.ucWave1.Name = "ucWave1";
            this.ucWave1.Size = new System.Drawing.Size(443, 51);
            this.ucWave1.TabIndex = 0;
            this.ucWave1.Text = "ucWave1";
            this.ucWave1.WaveColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(119)))), ((int)(((byte)(232)))));
            this.ucWave1.WaveHeight = 30;
            this.ucWave1.WaveSleep = 50;
            this.ucWave1.WaveWidth = 200;
            // 
            // Label_Message
            // 
            this.Label_Message.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Label_Message.BackColor = System.Drawing.Color.Transparent;
            this.Label_Message.Location = new System.Drawing.Point(17, 10);
            this.Label_Message.Name = "Label_Message";
            this.Label_Message.Size = new System.Drawing.Size(440, 36);
            this.Label_Message.TabIndex = 1;
            this.Label_Message.Text = "Plase wait ....";
            this.Label_Message.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bt_Cancel
            // 
            this.bt_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Cancel.Location = new System.Drawing.Point(203, 128);
            this.bt_Cancel.Name = "bt_Cancel";
            this.bt_Cancel.Size = new System.Drawing.Size(75, 23);
            this.bt_Cancel.TabIndex = 2;
            this.bt_Cancel.Text = "取消(&C)";
            this.bt_Cancel.UseVisualStyleBackColor = true;
            this.bt_Cancel.Click += new System.EventHandler(this.bt_Cancel_Click);
            // 
            // WaitInfoGUI
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(480, 163);
            this.Controls.Add(this.bt_Cancel);
            this.Controls.Add(this.Label_Message);
            this.Controls.Add(this.ucWave1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "WaitInfoGUI";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WaitInfoGUI";
            this.ResumeLayout(false);

        }

        #endregion

        private Components.UIControls.UCWave ucWave1;
        public System.Windows.Forms.Label Label_Message;
        private System.Windows.Forms.Button bt_Cancel;
    }
}