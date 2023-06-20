namespace XP.Win.ProgressBars
{
    partial class ProgressBase
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressBase));
            this.Bar = new System.Windows.Forms.ProgressBar();
            this.TextContent = new System.Windows.Forms.Label();
            this.bt_Cannel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Pic_Loading02 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Loading02)).BeginInit();
            this.SuspendLayout();
            // 
            // Bar
            // 
            this.Bar.Location = new System.Drawing.Point(108, 161);
            this.Bar.Name = "Bar";
            this.Bar.Size = new System.Drawing.Size(392, 23);
            this.Bar.TabIndex = 0;
            // 
            // TextContent
            // 
            this.TextContent.AutoEllipsis = true;
            this.TextContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextContent.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.TextContent.Location = new System.Drawing.Point(0, 0);
            this.TextContent.Name = "TextContent";
            this.TextContent.Size = new System.Drawing.Size(509, 72);
            this.TextContent.TabIndex = 1;
            this.TextContent.Text = "0";
            // 
            // bt_Cannel
            // 
            this.bt_Cannel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bt_Cannel.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.bt_Cannel.Location = new System.Drawing.Point(242, 241);
            this.bt_Cannel.Name = "bt_Cannel";
            this.bt_Cannel.Size = new System.Drawing.Size(75, 23);
            this.bt_Cannel.TabIndex = 2;
            this.bt_Cannel.Text = "中止(&C)";
            this.bt_Cannel.UseVisualStyleBackColor = true;
            this.bt_Cannel.Click += new System.EventHandler(this.bt_Cannel_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.TextContent);
            this.panel1.Location = new System.Drawing.Point(65, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(509, 72);
            this.panel1.TabIndex = 3;
            // 
            // Pic_Loading02
            // 
            this.Pic_Loading02.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Pic_Loading02.Image = ((System.Drawing.Image)(resources.GetObject("Pic_Loading02.Image")));
            this.Pic_Loading02.Location = new System.Drawing.Point(203, 73);
            this.Pic_Loading02.Name = "Pic_Loading02";
            this.Pic_Loading02.Size = new System.Drawing.Size(162, 162);
            this.Pic_Loading02.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.Pic_Loading02.TabIndex = 4;
            this.Pic_Loading02.TabStop = false;
            // 
            // ProgressBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InfoText;
            this.ClientSize = new System.Drawing.Size(636, 293);
            this.Controls.Add(this.Pic_Loading02);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.bt_Cannel);
            this.Controls.Add(this.Bar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ProgressBase";
            this.ShowInTaskbar = false;
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Pic_Loading02)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar Bar;
        private System.Windows.Forms.Label TextContent;
        private System.Windows.Forms.Button bt_Cannel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox Pic_Loading02;
    }
}

