using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XP.Comm.Enums;
using XP.Comm.Msgs;

namespace XP.Win.Components.Monitors
{
    public partial class BaseMonitorItemWin : Form
    {
        public BaseMonitorItemWin()
        {
            InitializeComponent();
        }

        private Font _TextFont = new Font("宋体", 10);
        private Font _CatptionFont = new Font("微软雅黑,黑体", 10);

       private Color _CapColor =  Color.White;
       private Color _TextColor = Color.White;

        // RichTextBox  文本使用多种颜色：
        //https://blog.csdn.net/anlog/article/details/137699575
        // RichTextBox  一行内使用多种不同的颜色
        //https://www.cnblogs.com/Jamesblog/p/16943708.html



        public List<LevelLogMsg> MsgLst = new List<LevelLogMsg>();
        /// <summary>
        /// 获取已设置无法关闭窗口创建参数。就是这里
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                int CS_NOCLOSE = 0x200;
                CreateParams parameters = base.CreateParams;
                parameters.ClassStyle |= CS_NOCLOSE;

                return parameters;
            }
        }



        public void Add(string msg)
        {
            LevelLogMsg levelLogMsg = new LevelLogMsg() { 
                Title = msg,
                Level= MsgLevel.Info,
            
            };
            Add(levelLogMsg);
        }
        public void Add(LevelLogMsg msg)
        {
            MsgLst.Add(msg);

            SendMsg2Rb(msg);
        }

        /// <summary>
        /// 可能涉及到线程调用，使用线程安全的 Invoke方法
        /// </summary>
        /// <param name="msg"></param>
        protected void SendMsg2Rb(LevelLogMsg msg)
        {
            if (rb_MainText.InvokeRequired)
            {
                Action SafeWrite = () =>
                {
                    SendMsg2Rb(msg);
                };
                rb_MainText.Invoke(SafeWrite);
            }
            else
            {
                AppentText(msg);
            }
        }



        protected void AppentText(LevelLogMsg msg)
        {
            string CapText = String.Empty;


            switch (msg.Level)
            {

                case Util.DebugLevel.Debug:
                    CapText = "调试";
                    _CapColor = Color.Green;
                    break;
                case Util.DebugLevel.Info:
                    CapText = "信息";
                    _CapColor = Color.Blue;
                    break;
                case Util.DebugLevel.Warn:
                    CapText = "警告";
                    _CapColor = Color.Yellow;
                    break;
                case Util.DebugLevel.Error:
                    CapText = "错误";
                    _CapColor = Color.Red;
                    break;
                case Util.DebugLevel.Execption:
                    CapText = "异常";
                    _CapColor = Color.Red;
                    break;


                default:
                    CapText = "其它";
                    _CapColor = Color.WhiteSmoke;
                    break;
            }

            rb_MainText.AppendText(CapText, _CapColor,_CatptionFont);
            rb_MainText.AppendText(msg.Title, _TextColor, _TextFont, true);

        }

        private void bt_Min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void bt_Max_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
    }
}
