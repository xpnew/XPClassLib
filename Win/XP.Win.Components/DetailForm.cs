using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XP.Win.Components
{
    public partial class DetailForm : Form
    {

        public int MaxSecond { get; set; }


        public string Title { get; set; }
        private int _CurrentStep;

        public bool IsAutoClose { get; set; }


        public DetailForm(string content):this(content,false)
        {
        }

        public DetailForm(string tit, string cot) : this(tit,cot,false)
        {
        }

        public DetailForm(string content, bool flag):this(null,content,flag)
        {
        }

        public DetailForm(string tit,string content,bool flag)
        {
            Title = tit;
            MaxSecond = 20;
            IsAutoClose = flag;
            InitializeComponent();
            txtContent.Text = content;
            if (!String.IsNullOrEmpty(Title))
            {
                this.Text = "【" +Title +  "】" + "-查看详细";
            }

            if (IsAutoClose)
            {
                StartAutoClose();
                timer1.Enabled = true;
            }
            else
            {
                timer1.Enabled = false;
            }
        }


        public void StartAutoClose()
        {
            CB_AutoClose.Visible = true;
            _CurrentStep = MaxSecond + 1;
        }

        private void _AutoCloseTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(txtContent.Text);
            }
            catch (Exception ex)
            {
                label1.Text = "复制失败。" + ex.Message;
            }

            label1.Visible = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CB_AutoClose_CheckedChanged(object sender, EventArgs e)
        {
            if (CB_AutoClose.Checked)
            {
                timer1.Start();
            }
            else
            {
                timer1.Stop();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (0 == _CurrentStep)
            {
                Close();
            }

            if (IsAutoClose && _CurrentStep > 0)
            {
                _CurrentStep--;
                CB_AutoClose.Text = "自动关闭(" + _CurrentStep + ")";
            }

        }
    }
}
