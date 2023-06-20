namespace XP.Win.Components
{
    partial class DetailForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.CB_AutoClose = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CB_AutoClose);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnCopy);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 171);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(473, 43);
            this.panel1.TabIndex = 1;
            // 
            // CB_AutoClose
            // 
            this.CB_AutoClose.AutoSize = true;
            this.CB_AutoClose.Checked = true;
            this.CB_AutoClose.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_AutoClose.Location = new System.Drawing.Point(162, 18);
            this.CB_AutoClose.Name = "CB_AutoClose";
            this.CB_AutoClose.Size = new System.Drawing.Size(72, 16);
            this.CB_AutoClose.TabIndex = 3;
            this.CB_AutoClose.Text = "自动关闭";
            this.CB_AutoClose.UseVisualStyleBackColor = true;
            this.CB_AutoClose.Visible = false;
            this.CB_AutoClose.CheckedChanged += new System.EventHandler(this.CB_AutoClose_CheckedChanged);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(360, 14);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "关闭(&C)";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(264, 14);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(75, 23);
            this.btnCopy.TabIndex = 1;
            this.btnCopy.Text = "复制";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "成功复制到剪切板";
            this.label1.Visible = false;
            // 
            // txtContent
            // 
            this.txtContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContent.Location = new System.Drawing.Point(0, 0);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.ReadOnly = true;
            this.txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtContent.Size = new System.Drawing.Size(473, 171);
            this.txtContent.TabIndex = 2;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // DetailForm
            // 
            this.AcceptButton = this.btnCopy;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(473, 214);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.panel1);
            this.Name = "DetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "查看详细";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtContent;
        private System.Windows.Forms.CheckBox CB_AutoClose;
        private System.Windows.Forms.Timer timer1;
    }
}