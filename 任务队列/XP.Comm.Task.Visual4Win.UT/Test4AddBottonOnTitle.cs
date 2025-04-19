using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Drawing.Drawing2D;

using System.Collections;


using System.Runtime.InteropServices;

using System.Diagnostics;

namespace XP.Comm.Task.Visual4Win.UT
{
    public partial class Test4AddBottonOnTitle : Form
    {
        public Test4AddBottonOnTitle()
        {
            InitializeComponent();
        }

        /// <summary>

        /// 必需的设计器变量。

        /// </summary>

        private System.ComponentModel.Container components = null;





        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>

        protected override void Dispose(bool disposing)

        {

            if (disposing)

            {

                if (components != null)

                {

                    components.Dispose();

                }

            }

            base.Dispose(disposing);

        }

        //#region Windows 窗体设计器生成的代码

        ///// <summary>

        ///// 设计器支持所需的方法 - 不要使用代码编辑器修改

        ///// 此方法的内容。

        ///// </summary>

        private void InitializeComponent()

        {

            //

            // Form1

            //

            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);

            this.ClientSize = new System.Drawing.Size(292, 266);

            this.Name = "Form1";

            this.Text = "测试代码：窗体标题增加按钮";

            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);

        }

        //#endregion

        /// <summary>

        /// 应用程序的主入口点。

        /// </summary>

        //[STAThread]

        //static void Main()

        //{

        //    Application.Run(new Form1());

        //}

        [DllImport("User32.dll")]

        private static extern IntPtr GetWindowDC(IntPtr hwnd);

        [DllImport("User32.dll")]

        private static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("Kernel32.dll")]

        private static extern int GetLastError();



        //标题栏按钮的矩形区域。

        Rectangle m_rect = new Rectangle(205, 6, 40, 40);

        protected override void WndProc(ref Message m)

        {

            base.WndProc(ref m);

            switch (m.Msg)

            {

                case 0x86://WM_NCACTIVATE

                    goto case 0x85;

                case 0x85://WM_NCPAINT

                    {

                        IntPtr hDC = GetWindowDC(m.HWnd);

                        //把DC转换为.NET的Graphics就可以很方便地使用Framework提供的绘图功能了

                        Graphics gs = Graphics.FromHdc(hDC);

                        gs.FillRectangle(new LinearGradientBrush(m_rect, Color.Red, Color.Purple, LinearGradientMode.BackwardDiagonal), m_rect);

                        StringFormat strFmt = new StringFormat();

                        strFmt.Alignment = StringAlignment.Center;

                        strFmt.LineAlignment = StringAlignment.Center;

                        gs.DrawString("★√★", this.Font, Brushes.BlanchedAlmond, m_rect, strFmt);

                        gs.Dispose();

                        //释放GDI资源

                        ReleaseDC(m.HWnd, hDC);

                        break;

                    }

                case 0xA1://WM_NCLBUTTONDOWN

                    {

                        Point mousePoint = new Point((int)m.LParam);

                        mousePoint.Offset(-this.Left, -this.Top);

                        if (m_rect.Contains(mousePoint))

                        {

                            MessageBox.Show("hello");

                        }

                        break;

                    }

            }

        }

        //在窗口大小改变时及时更新按钮的区域。

        private void Form1_SizeChanged(object sender, System.EventArgs e)

        {

            m_rect.X = this.Bounds.Width - 95;

            m_rect.Y = 6;

            m_rect.Width = 40;
            m_rect.Height = 80;

        }
    }
}
