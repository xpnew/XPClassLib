
namespace XP.Util.Win
{
    partial class BaseTestForm
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
            this.WinStatusStrip = new System.Windows.Forms.StatusStrip();
            this.WinStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.WinStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // WinStatusStrip
            // 
            this.WinStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.WinStatusLabel});
            this.WinStatusStrip.Location = new System.Drawing.Point(0, 472);
            this.WinStatusStrip.Name = "WinStatusStrip";
            this.WinStatusStrip.Size = new System.Drawing.Size(800, 22);
            this.WinStatusStrip.TabIndex = 0;
            this.WinStatusStrip.Text = "OK!";
            // 
            // WinStatusLabel
            // 
            this.WinStatusLabel.Name = "WinStatusLabel";
            this.WinStatusLabel.Size = new System.Drawing.Size(25, 17);
            this.WinStatusLabel.Text = "Ok";
            // 
            // BaseTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 494);
            this.Controls.Add(this.WinStatusStrip);
            this.Name = "BaseTestForm";
            this.Text = "BaseTestForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BaseTestForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BaseTestForm_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BaseTestForm_KeyPress);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.BaseTestForm_PreviewKeyDown);
            this.WinStatusStrip.ResumeLayout(false);
            this.WinStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip WinStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel WinStatusLabel;
    }
}