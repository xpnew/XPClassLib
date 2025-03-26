using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XP.Win.Components
{
    public class LogListBox : ListBox
    {

        /// <summary>
        /// 获取或设置一系列键值对，指示对条目中包含关键字采用不同的画刷。
        /// 键为关键字，值为画刷，可使用System.Drawing.Brushes下的标准画刷。
        /// </summary>
        [Description("指定对条目中包含关键字采用不同的画刷。")]
        public Dictionary<string, Brush> BrushDic { get; set; }

        /// <summary>
        /// 获取或设置一个bool值，该值指示是否在双击条目时弹出详细信息对话框。
        /// </summary>
        [Description("是否在双击条目时弹出详细信息对话框。")]
        public bool ShowDetailOnDoubleClick { get; set; }

        /// <summary>
        /// 当双击条目时执行。
        /// </summary>
        [Description("当双击条目时执行。")]
        public event EventHandler<ItemEventArgs> ItemDoubleClick;

        /// <summary>
        /// 当选中条目时发生。
        /// </summary>
        [Description("当选中条目时发生。")]
        public event EventHandler<ItemEventArgs> ItemSelected;


        /// <summary>
        /// 最大条目数量，默认为100。
        /// </summary>
        [Description("最大条目数量，默认为100。")]

        public int ItemsMaxLength { get; set; } = 100;


        public LogListBox()
        {
            this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ShowDetailOnDoubleClick = true;

            ContextMenuStrip cms = new ContextMenuStrip();
            ToolStripItem mnuClear = new ToolStripMenuItem();
            mnuClear.Text = "清除(&C)";
            mnuClear.Click += new EventHandler(mnuClear_Click);
            cms.Items.Add(mnuClear);
            this.ContextMenuStrip = cms;
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            if (this.SelectedItem != null)
            {
                WinLogDetails log = this.SelectedItem as WinLogDetails;
                object content = log == null ? this.SelectedItem : log.Title;

                if (ItemSelected != null)
                {
                    ItemSelected(this, new ItemEventArgs(content));
                }
            }
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);

            if (this.SelectedItem != null)
            {
                bool IsLogDetails = false;
                string Title = String.Empty;
                String Content = String.Empty;

                if (this.SelectedItem is WinLogDetails)
                {
                    IsLogDetails = true;
                    WinLogDetails log = this.SelectedItem as WinLogDetails;
                    Title = log.Title;
                    Content = log.Body;
                    if (String.IsNullOrEmpty(Content))
                    {
                        Content = Title;
                    }
                }
                else
                {
                    Content = this.SelectedItem.ToString();
                }

           
                //object content = log == null ? this.SelectedItem : log.Title;



                if (ItemDoubleClick != null)
                {
                    ItemDoubleClick(this, new ItemEventArgs(Content));
                }

                if (ShowDetailOnDoubleClick)
                {
                    if (IsLogDetails)
                    {
                        DetailForm frmDetail = new DetailForm(Title, Content);
                        frmDetail.Show();
                    }
                    else
                    {
                        DetailForm frmDetail = new DetailForm(Content);
                        frmDetail.Show();
                    }

                }
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);

            if (DesignMode == false)
            {
                if (e.Index < 0) return;

                string s = this.Items[e.Index].ToString();
                e.DrawBackground();

                bool hasBrushes = false;
                //根据预定义内容决定笔刷（文字颜色），没用上。
                if (BrushDic != null)
                {
                    foreach (var item in BrushDic)
                    {
                        if (s.Contains(item.Key))
                        {
                            e.Graphics.DrawString(s, e.Font, item.Value, e.Bounds);
                            hasBrushes = true;
                            break;
                        }
                    }
                }

                if (!hasBrushes)
                {
                    var item = this.Items[e.Index];

                    if (item is WinLogDetails)
                    {
                        var log = item as WinLogDetails;
                        SolidBrush FrontBrush = new SolidBrush(log.FontColor);
                        SolidBrush backBrush = new SolidBrush(log.BgColor);
                        Graphics g = e.Graphics;//获取Graphics对象。 

                        bool IsLight = 1 == e.Index % 2;

                        if (IsLight)
                        {
                            Color LightBg = Color.FromArgb(80, log.BgColor);
                            backBrush = new SolidBrush(LightBg);
                        }
                        //DarkBg.B = DarkBg.B / 2;


                        Rectangle LogBounds = e.Bounds;
                        //绘制背景
                        g.FillRectangle(backBrush, e.Bounds);
                        //e.Graphics.DrawString(s, e.Font, Brushes.Black, e.Bounds);
                        //先写时间：
                        if (log.CreateTime.HasValue)
                        {
                            string DateTimeStr = log.CreateTime.Value.ToString("yyyyMMdd.HHmmss");
                            TextRenderer.DrawText(g, DateTimeStr, this.Font, e.Bounds, log.FontColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);

                            //测算日期的宽度
                            StringFormat sf = new StringFormat(StringFormat.GenericTypographic);
                            SizeF size = g.MeasureString(DateTimeStr, this.Font, 1000, sf);
                            //文字右边留出日期的空白
                            LogBounds.Width -= (int)size.Width + 20;
                        }
                        //日志标题
                        TextRenderer.DrawText(g, log.Title, this.Font, LogBounds, log.FontColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
                    }
                    else
                    {
                        e.Graphics.DrawString(s, e.Font, Brushes.Black, e.Bounds);
                    }
                }
                e.DrawFocusRectangle();
            }
        }

        void mnuClear_Click(object sender, EventArgs e)
        {
            this.Items.Clear();
        }

        public void AddLog(string format, params object[] args)
        {
            string log = string.Format(format, args);
            AddLog(log);
        }
        public void AddLog(string log)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(AddLog), log);
                return;
            }
            while (this.Items.Count > ItemsMaxLength)
            {
                this.Items.RemoveAt(0);
            }

            bool isScrollBottom = this.TopIndex == this.Items.Count - (int)(this.Height / this.ItemHeight);
            this.Items.Add(new WinLogDetails(log));
            if (isScrollBottom)
            {
                NTUser32.SendMessage(this.Handle, NTUser32.WM_VSCROLL, new IntPtr(NTUser32.SB_BOTTOM), IntPtr.Zero);
            }
        }
        public void AddLog(string log, string body = null)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<string, string>(AddLog), log, body);
                return;
            }
            while (this.Items.Count > ItemsMaxLength)
            {
                this.Items.RemoveAt(0);
            }

            bool isScrollBottom = this.TopIndex == this.Items.Count - (int)(this.Height / this.ItemHeight);
            this.Items.Add(new WinLogDetails(log) { Title = log, Body = body });
            if (isScrollBottom)
            {

            }

            if (this.IsDisposed) return;
            NTUser32.SendMessage(this.Handle, NTUser32.WM_VSCROLL, new IntPtr(NTUser32.SB_BOTTOM), IntPtr.Zero);
        }

        //public async  Task  InvokeLog(string log)
        //{


        //}


        public void AddLog(WinLogDetails log)
        {

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<WinLogDetails>(AddLog), log);
                return;
            }
            while (this.Items.Count > 100)
            {
                this.Items.RemoveAt(0);
            }

            bool isScrollBottom = this.TopIndex == this.Items.Count - (int)(this.Height / this.ItemHeight);
            this.Items.Add(log);
            if (isScrollBottom)
            {
                //
            }
            NTUser32.SendMessage(this.Handle, NTUser32.WM_VSCROLL, new IntPtr(NTUser32.SB_BOTTOM), IntPtr.Zero);
        }
    }



    public class ItemEventArgs : EventArgs
    {
        public ItemEventArgs(object item)
        {
            this.Item = item;
        }

        public object Item { get; set; }
    }

    //public class Log
    //{
    //    public Log(string content)
    //    {
    //        Content = content;
    //    }

    //    public string Content { get; set; }

    //    public override string ToString()
    //    {
    //        if (Content.Length > 100)
    //            return Content.Substring(0, 97) + "...";
    //        return Content;
    //    }
    //}
}
