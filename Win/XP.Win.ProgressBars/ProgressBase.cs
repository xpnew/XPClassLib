using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XP.Win.ProgressBars
{
    public partial class ProgressBase : Form
    {
        public ProgressBase()
        {
            InitializeComponent();
            Pic_Loading02.BackColor = Color.Transparent;
        }


        public  Action<object, EventArgs> OnCannel { get; set; }


        //工作完成后执行的事件
        public void OnProcessCompleted(object sender, EventArgs e)
        {
            this.Close();
        }

        //工作中执行进度更新
        public void OnProgressChanged(object sender, MuiltiProgressEventArgs e)
        {
            if (-1 == e.ProgressPercentage)
            {
               // Bar.Visible = false;
            }
            else
            {
                Bar.Visible = true;
                Bar.Value = e.ProgressPercentage;
            }
            TextContent.Text =e.TextNotice;
        }

        //工作中执行进度更新
        public void OnWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (-1 == e.ProgressPercentage)
            {
                Pic_Loading02.Visible = true;
                Bar.Visible = false;
            }
            else
            {
                Pic_Loading02.Visible = false;
                Bar.Visible = true;
                Bar.Value = e.ProgressPercentage;
            }
            TextContent.Text = e.UserState.ToString();

            this.ResumeLayout();
        }

        private void bt_Cannel_Click(object sender, EventArgs e)
        {
            OnCannel?.Invoke(sender,e);
            this.Close();
        }
    }
}
