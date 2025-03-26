namespace XP.Comm.Task.Visual4Win
{
    partial class TextShowForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.Lab_Title = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bt_Help = new System.Windows.Forms.Button();
            this.bt_Mini = new System.Windows.Forms.Button();
            this.bt_Close = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tb_MainText = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(757, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel2.Controls.Add(this.Lab_Title, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(751, 34);
            this.tableLayoutPanel2.TabIndex = 0;
            this.tableLayoutPanel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tableLayoutPanel2_MouseDown);
            this.tableLayoutPanel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tableLayoutPanel2_MouseMove);
            this.tableLayoutPanel2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tableLayoutPanel2_MouseUp);
            // 
            // Lab_Title
            // 
            this.Lab_Title.AutoSize = true;
            this.Lab_Title.Dock = System.Windows.Forms.DockStyle.Left;
            this.Lab_Title.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lab_Title.Location = new System.Drawing.Point(3, 0);
            this.Lab_Title.Name = "Lab_Title";
            this.Lab_Title.Size = new System.Drawing.Size(83, 34);
            this.Lab_Title.TabIndex = 0;
            this.Lab_Title.Text = "文本显示窗体";
            this.Lab_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bt_Help);
            this.panel1.Controls.Add(this.bt_Mini);
            this.panel1.Controls.Add(this.bt_Close);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(528, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(220, 28);
            this.panel1.TabIndex = 1;
            // 
            // bt_Help
            // 
            this.bt_Help.Dock = System.Windows.Forms.DockStyle.Right;
            this.bt_Help.Location = new System.Drawing.Point(103, 0);
            this.bt_Help.Name = "bt_Help";
            this.bt_Help.Size = new System.Drawing.Size(39, 28);
            this.bt_Help.TabIndex = 5;
            this.bt_Help.Text = "?";
            this.bt_Help.UseVisualStyleBackColor = true;
            this.bt_Help.Click += new System.EventHandler(this.bt_Help_Click);
            // 
            // bt_Mini
            // 
            this.bt_Mini.Dock = System.Windows.Forms.DockStyle.Right;
            this.bt_Mini.Location = new System.Drawing.Point(142, 0);
            this.bt_Mini.Name = "bt_Mini";
            this.bt_Mini.Size = new System.Drawing.Size(39, 28);
            this.bt_Mini.TabIndex = 4;
            this.bt_Mini.Text = "-";
            this.bt_Mini.UseVisualStyleBackColor = true;
            this.bt_Mini.Click += new System.EventHandler(this.bt_Mini_Click);
            // 
            // bt_Close
            // 
            this.bt_Close.Dock = System.Windows.Forms.DockStyle.Right;
            this.bt_Close.Location = new System.Drawing.Point(181, 0);
            this.bt_Close.Name = "bt_Close";
            this.bt_Close.Size = new System.Drawing.Size(39, 28);
            this.bt_Close.TabIndex = 3;
            this.bt_Close.Text = "X";
            this.bt_Close.UseVisualStyleBackColor = true;
            this.bt_Close.Click += new System.EventHandler(this.bt_Close_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 43);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(751, 404);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.tb_MainText);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(751, 404);
            this.panel3.TabIndex = 0;
            // 
            // tb_MainText
            // 
            this.tb_MainText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_MainText.Location = new System.Drawing.Point(0, 0);
            this.tb_MainText.Multiline = true;
            this.tb_MainText.Name = "tb_MainText";
            this.tb_MainText.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.tb_MainText.Size = new System.Drawing.Size(751, 404);
            this.tb_MainText.TabIndex = 0;
            // 
            // TextShowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "TextShowForm";
            this.Text = "TextShowForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label Lab_Title;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bt_Help;
        private System.Windows.Forms.Button bt_Mini;
        private System.Windows.Forms.Button bt_Close;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox tb_MainText;
    }
}