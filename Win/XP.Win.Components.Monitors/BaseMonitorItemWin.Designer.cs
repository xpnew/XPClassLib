namespace XP.Win.Components.Monitors
{
    partial class BaseMonitorItemWin
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
            this.rb_MainText = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.bt_Min = new System.Windows.Forms.Button();
            this.bt_Max = new System.Windows.Forms.Button();
            this.bt_Clean = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rb_MainText
            // 
            this.rb_MainText.BackColor = System.Drawing.SystemColors.MenuText;
            this.rb_MainText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rb_MainText.ForeColor = System.Drawing.SystemColors.Info;
            this.rb_MainText.Location = new System.Drawing.Point(3, 43);
            this.rb_MainText.Name = "rb_MainText";
            this.rb_MainText.Size = new System.Drawing.Size(296, 242);
            this.rb_MainText.TabIndex = 0;
            this.rb_MainText.Text = "";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.rb_MainText, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(302, 288);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.bt_Clean);
            this.panel1.Controls.Add(this.bt_Max);
            this.panel1.Controls.Add(this.bt_Min);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(296, 34);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(4, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "消息显示";
            // 
            // bt_Min
            // 
            this.bt_Min.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_Min.ForeColor = System.Drawing.SystemColors.ControlText;
            this.bt_Min.Location = new System.Drawing.Point(214, 5);
            this.bt_Min.Name = "bt_Min";
            this.bt_Min.Size = new System.Drawing.Size(29, 23);
            this.bt_Min.TabIndex = 1;
            this.bt_Min.Text = "__";
            this.bt_Min.UseVisualStyleBackColor = true;
            this.bt_Min.Click += new System.EventHandler(this.bt_Min_Click);
            // 
            // bt_Max
            // 
            this.bt_Max.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_Max.ForeColor = System.Drawing.SystemColors.ControlText;
            this.bt_Max.Location = new System.Drawing.Point(253, 5);
            this.bt_Max.Name = "bt_Max";
            this.bt_Max.Size = new System.Drawing.Size(29, 23);
            this.bt_Max.TabIndex = 2;
            this.bt_Max.Text = "□";
            this.bt_Max.UseVisualStyleBackColor = true;
            this.bt_Max.Click += new System.EventHandler(this.bt_Max_Click);
            // 
            // bt_Clean
            // 
            this.bt_Clean.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_Clean.ForeColor = System.Drawing.SystemColors.ControlText;
            this.bt_Clean.Location = new System.Drawing.Point(156, 6);
            this.bt_Clean.Name = "bt_Clean";
            this.bt_Clean.Size = new System.Drawing.Size(55, 23);
            this.bt_Clean.TabIndex = 3;
            this.bt_Clean.Text = "清除";
            this.bt_Clean.UseVisualStyleBackColor = true;
            // 
            // BaseMonitorItemWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(302, 288);
            this.Controls.Add(this.tableLayoutPanel1);
            this.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "BaseMonitorItemWin";
            this.Text = "BaseMonitorItemWin";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rb_MainText;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bt_Max;
        private System.Windows.Forms.Button bt_Min;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bt_Clean;
    }
}