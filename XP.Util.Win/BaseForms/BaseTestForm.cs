using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XP.Util.Win
{
    public partial class BaseTestForm : BaseForm
    {


        #region 基类和初始化

        public BaseTestForm()
        {
            InitializeComponent();

            //_Init();
        }

        protected virtual void _Init()
        {
            //_InitBaseTestForm();
            //_tm.Elapsed += _tm_Elapsed;
            //_tm.Interval = 1000;

        }

        protected void _InitBaseTestForm()
        {

            //this.WinStatusStrip = new System.Windows.Forms.StatusStrip();
            //this.WinStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            //this.WinStatusStrip.SuspendLayout();
            //this.SuspendLayout();


            ////动态宽度
            //this.WinStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            //this.WinStatusLabel});
            //// 
            //// WinStatusLabel
            //// 
            //this.WinStatusLabel.Name = "WinStatusLabel";
            //this.WinStatusLabel.Size = new System.Drawing.Size(25, 17);
            //this.WinStatusLabel.Text = "Ok";


            //var winsize = this.ClientSize;
            //this.WinStatusStrip.Location = new System.Drawing.Point(0, winsize.Height -22);
            //this.WinStatusStrip.Name = "WinStatusLabel";
            //this.WinStatusStrip.Size = new System.Drawing.Size(winsize.Width, 22);
            //this.WinStatusStrip.TabIndex = 0;
            //this.WinStatusStrip.Text = "OK!";

            //this.Controls.Add(this.WinStatusStrip);

            //this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BaseTestForm_KeyDown);

            //this.ResumeLayout(false);
            //this.PerformLayout();
            _AddTexboxKeyDown();

        }
        protected void _AddTexboxKeyDown()
        {
            _AddTexboxKeyDown(this.Controls);
        }
        private void _AddTexboxKeyDown(Control.ControlCollection cls)
        {
            foreach (Control ctl in cls)
            {
                if (ctl is TextBox)
                {
                    var tb = ctl as TextBox;
                    tb.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BaseTestForm_KeyDown);
                }else if( ctl is Button)
                {
                    ctl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BaseTestForm_KeyDown);
                }
                else
                {
                    if (ctl.HasChildren)
                    {
                        _AddTexboxKeyDown(ctl.Controls);
                    }
                }
            }
        }

        #endregion

        #region 公共方法

        protected void SayStatus(string txt)
        {
            this.WinStatusLabel.Text = txt;
        }





        #endregion
        #region 为测试窗口添加默认的键盘关闭操作

        //System.Timers.Timer _tm = new System.Timers.Timer();

        private void _tm_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            x.Say("TestWin计时器工作中。。。");
        }


        private void BaseTestForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e..CompareTo(Keys.Control) == 0 && e.KeyCode == Keys.Delete)
            //{
            //    //处理逻辑
            //}

        }

        private void BaseTestForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers.CompareTo(Keys.Control) == 0 && e.KeyCode == Keys.Delete)
            {
                //处理逻辑
            }
        }

        protected void BaseTestForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers.CompareTo(Keys.Control) == 0 && e.KeyCode == Keys.E)
            {
                //处理逻辑
                this.Close();

            }

            if (e.Modifiers.CompareTo(Keys.Control) == 0 && e.KeyCode == Keys.Q)
            {
                //_tm.Start();
                FormResult = BaseForms.FormResultDef.CloseAll;
                this.DialogResult = DialogResult.Abort;

                //Task.Factory.StartNew(() => {
                //    //Task.Delay(60 * 1000);
                //    System.Threading.Thread.Sleep(10 * 1000);
                //    x.Say("装备关闭  计数器。");
                //    if(null != _tm)
                //    {
                //        _tm.Stop();
                //        _tm = null;
                //    }
                //});

                //处理逻辑
                this.Close();
            }
        }

        #endregion

        private void BaseTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseEvent?.Invoke(this, new BaseForms.FormCloseEventArgs(this.FormResult));
        }
    }
}
