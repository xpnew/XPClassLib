using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using XP.Comm.Task.TaskShow;
using XP.Util.Threadings;
using XP.Util.Win;

namespace XP.Comm.Task.Visual4Win
{
    public partial class TextShowForm : BaseForm, ITaskItemShower
    {
        public TextShowForm()
        {
            InitializeComponent();
            _Timerr.Elapsed += _Timerr_Elapsed;
            _Timerr.Interval = 10 * 1000;
            AsyncTask.BuildBGAsync(() =>
            {

                tb_MainText.Text += "添加一行文本";
                Thread.Sleep(1000);

                //tb_MainText.Invoke(new Action(() =>
                //{
                //    tb_MainText.Text += Environment.NewLine + "\n稍后再添加一行文本";
                //}));
                Add("稍后再添加一行文本");
                Add("20秒后将开始定时添加文本测试");

                _Timerr.Start();


            });
        }



        #region 文本管理 
        private void _Timerr_Elapsed(object sender, ElapsedEventArgs e)
        {
            Add("test txt (by Timer)");
        }

        private System.Timers.Timer _Timerr = new System.Timers.Timer();

        public int MaxLine { get; set; } = 5;

        public long TotalLine { get; set; } = 0;

        public List<LineItem> Items { get; set; } = new List<LineItem>();


        public class LineItem
        {
            public long LineNum { get; set; }
            public string LineText { get; set; }
            public DateTime LineTime { get; set; } = DateTime.Now;

            public override string ToString()
            {
                return $"[{LineNum} {LineTime.ToString("yyyyMMdd.HHmmss")} ] {LineText} ";
            }
        }
        public void Add(string item)
        {
            TotalLine++;
            Items.Add(new LineItem() { LineNum = TotalLine, LineText = item });
            if (MaxLine < Items.Count)
                Items.RemoveAt(0);

            PushText();

        }

        private void PushText()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                sb.AppendLine(Items[i].ToString());
            }
            
            if (this.IsHandleCreated)
            {
                tb_MainText.Invoke(new Action(() =>
                {
                    tb_MainText.Text = sb.ToString();
                }));

            }
        }
        #endregion
        #region 窗体控制
        private bool moving = false;
        private Point oldMousePosition;


        private void bt_Mini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void bt_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bt_Help_Click(object sender, EventArgs e)
        {
            Alert("这是一个专门用来滚动显示信息的窗口，\n最多只显示");
        }
        #endregion

        private void tableLayoutPanel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                return;
            }
            //Titlepanel.Cursor = Cursors.NoMove2D;
            oldMousePosition = e.Location;
            moving = true;
        }

        private void tableLayoutPanel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && moving)
            {
                Point newPosition = new Point(e.Location.X - oldMousePosition.X, e.Location.Y - oldMousePosition.Y);
                this.Location += new Size(newPosition);
            }
        }

        private void tableLayoutPanel2_MouseUp(object sender, MouseEventArgs e)
        {
            moving = false;
        }
    }
}
