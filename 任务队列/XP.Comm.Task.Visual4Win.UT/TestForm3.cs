using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;

namespace XP.Comm.Task.Visual4Win.UT
{

    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
    public struct _RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    public class ThemeForm : Form
    {
        #region private structs

        struct _NonClientSizeInfo
        {
            public Size CaptionButtonSize;
            public Size BorderSize;
            public int CaptionHeight;
            public Rectangle CaptionRect;
            public Rectangle Rect;
            public Rectangle ClientRect;
            public int Width;
            public int Height;
        };

        #endregion

        #region constants

        const int WM_NCACTIVATE = 0x86;
        const int WM_NCPAINT = 0x85;
        const int WM_NCLBUTTONDOWN = 0xA1;
        const int WM_NCRBUTTONDOWN = 0x00A4;
        const int WM_NCRBUTTONUP = 0x00A5;
        const int WM_NCMOUSEMOVE = 0x00A0;
        const int WM_NCLBUTTONUP = 0x00A2;
        const int WM_NCCALCSIZE = 0x0083;
        const int WM_NCMOUSEHOVER = 0x02A0;
        const int WM_NCMOUSELEAVE = 0x02A2;
        const int WM_NCHITTEST = 0x0084;
        const int WM_NCCREATE = 0x0081;
        //const int WM_RBUTTONUP = 0x0205;

        const int WM_LBUTTONDOWN = 0x0201;
        const int WM_CAPTURECHANGED = 0x0215;
        const int WM_LBUTTONUP = 0x0202;
        const int WM_SETCURSOR = 0x0020;
        const int WM_CLOSE = 0x0010;
        const int WM_SYSCOMMAND = 0x0112;
        const int WM_MOUSEMOVE = 0x0200;
        const int WM_SIZE = 0x0005;
        const int WM_SIZING = 0x0214;
        const int WM_GETMINMAXINFO = 0x0024;
        const int WM_ENTERSIZEMOVE = 0x0231;
        const int WM_WINDOWPOSCHANGING = 0x0046;


        // FOR WM_SIZING MSG WPARAM
        const int WMSZ_BOTTOM = 6;
        const int WMSZ_BOTTOMLEFT = 7;
        const int WMSZ_BOTTOMRIGHT = 8;
        const int WMSZ_LEFT = 1;
        const int WMSZ_RIGHT = 2;
        const int WMSZ_TOP = 3;
        const int WMSZ_TOPLEFT = 4;
        const int WMSZ_TOPRIGHT = 5;

        // left mouse button is down.
        const int MK_LBUTTON = 0x0001;

        const int SC_CLOSE = 0xF060;
        const int SC_MAXIMIZE = 0xF030;
        const int SC_MINIMIZE = 0xF020;
        const int SC_RESTORE = 0xF120;
        const int SC_CONTEXTHELP = 0xF180;

        const int HTCAPTION = 2;
        const int HTCLOSE = 20;
        const int HTHELP = 21;
        const int HTMAXBUTTON = 9;
        const int HTMINBUTTON = 8;
        const int HTTOP = 12;

        const int SM_CYBORDER = 6;
        const int SM_CXBORDER = 5;
        const int SM_CYCAPTION = 4;

        const int CS_DropSHADOW = 0x20000;
        const int GCL_STYLE = (-26);

        #endregion

        #region windows api

        [DllImport("User32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hwnd);
        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hwnd, ref _RECT rect);
        [DllImport("User32.dll")]
        private static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);

        #endregion

        #region default constructor

        public ThemeForm()
        {
            Text = "ThemeForm1";
            CloseButtonImage = Properties.Resources.pic01;
            CloseButtonHoverImage = Properties.Resources.pic02;
            CloseButtonPressDownImage = Properties.Resources.pic02;

            MaximumButtonImage = Properties.Resources.pic03;
            MaximumButtonHoverImage = Properties.Resources.pic04;
            MaximumButtonPressDownImage = Properties.Resources.pic04;

            MaximumNormalButtonImage = Properties.Resources.pic03;
            MaximumNormalButtonHoverImage = Properties.Resources.pic04;
            MaximumNormalButtonPressDownImage = Properties.Resources.pic04;

            MinimumButtonImage = Properties.Resources.pic03;
            MinimumButtonHoverImage = Properties.Resources.pic04;
            MinimumButtonPressDownImage = Properties.Resources.pic04;

            HelpButtonImage = Properties.Resources.Log1;
            HelpButtonHoverImage = Properties.Resources.log2;
            HelpButtonPressDownImage = Properties.Resources.log2;
            CaptionColor = Brushes.White;
            CaptionBackgroundColor = Color.DimGray;

            SetClassLong(this.Handle, GCL_STYLE, GetClassLong(this.Handle, GCL_STYLE) | CS_DropSHADOW); //API函数加载，实现窗体边框阴影效果
        }

        #endregion

        [DefaultValue("")]
        [Browsable(true)]
        [Category("ControlBox")]
        public virtual ContextMenuStrip CaptionContextMenu { get; set; }

        protected virtual void OnCaptionContextMenu(int x, int y)
        {
            if (this.CaptionContextMenu != null)
                this.CaptionContextMenu.Show(x, y);
        }

        #region properties

        [Category("ControlBox")]
        [Description("Close button image in control box.")]
        [DisplayName("CloseButtonImage")]
        [DesignOnly(true)]
        public Image CloseButtonImage { get; set; }

        [Category("ControlBox")]
        [Description("Close button image pressed down in control box.")]
        [DisplayName("CloseButtonPressDownImage")]
        [DesignOnly(true)]
        public Image CloseButtonPressDownImage { get; set; }

        [Category("ControlBox")]
        [Description("Close button image hover in control box.")]
        [DisplayName("CloseButtonHoverImage")]
        [DesignOnly(true)]
        public Image CloseButtonHoverImage { get; set; }

        [Category("ControlBox")]
        [Description("Maximum button image in control box.")]
        [DisplayName("MaximumButtonImage")]
        [DesignOnly(true)]
        public Image MaximumButtonImage { get; set; }

        [Category("ControlBox")]
        [Description("Maximum button hover image in control box.")]
        [DisplayName("MaximumButtonHoverImage")]
        [DesignOnly(true)]
        public Image MaximumButtonHoverImage { get; set; }

        [Category("ControlBox")]
        [Description("Maximum button pressed down image in control box.")]
        [DisplayName("MaximumButtonPressDownImage")]
        [DesignOnly(true)]
        public Image MaximumButtonPressDownImage { get; set; }

        [Category("ControlBox")]
        [Description("Maximum Normal button image in control box.")]
        [DisplayName("MaximumNormalButtonImage")]
        [DesignOnly(true)]
        public Image MaximumNormalButtonImage { get; set; }

        [Category("ControlBox")]
        [Description("Maximum Normal button hover image in control box.")]
        [DisplayName("MaximumNormalButtonHoverImage")]
        [DesignOnly(true)]
        public Image MaximumNormalButtonHoverImage { get; set; }

        [Category("ControlBox")]
        [Description("Maximum Normal button pressed down image in control box.")]
        [DisplayName("MaximumNormalButtonPressDownImage")]
        [DesignOnly(true)]
        public Image MaximumNormalButtonPressDownImage { get; set; }

        [Category("ControlBox")]
        [Description("Minimum button image in control box.")]
        [DisplayName("MinimumButtonImage")]
        [DesignOnly(true)]
        public Image MinimumButtonImage { get; set; }

        [Category("ControlBox")]
        [Description("Minimum button hover image in control box.")]
        [DisplayName("MinimumButtonHoverImage")]
        [DesignOnly(true)]
        public Image MinimumButtonHoverImage { get; set; }

        [Category("ControlBox")]
        [Description("Minimum button pressed down image in control box.")]
        [DisplayName("MinimumButtonPressDownImage")]
        [DesignOnly(true)]
        public Image MinimumButtonPressDownImage { get; set; }

        [Category("ControlBox")]
        [Description("Help button image in control box.")]
        [DisplayName("HelpButtonImage")]
        [DesignOnly(true)]
        public Image HelpButtonImage { get; set; }

        [Category("ControlBox")]
        [Description("Help button hover image in control box.")]
        [DisplayName("HelpButtonHoverImage")]
        [DesignOnly(true)]
        public Image HelpButtonHoverImage { get; set; }

        [Category("ControlBox")]
        [Description("Help button pressed down image in control box.")]
        [DisplayName("HelpButtonPressDownImage")]
        [DesignOnly(true)]
        public Image HelpButtonPressDownImage { get; set; }

        [Category("CaptionColor")]
        [Description("The color of caption.")]
        [DisplayName("CaptionColor")]
        [DesignOnly(true)]
        public Brush CaptionColor { get; set; }

        [Category("CaptionColor")]
        [Description("The color of caption.")]
        [DisplayName("CaptionBackgroundColor")]
        [DefaultValue(typeof(Color), "Black")]
        [DesignOnly(true)]
        public Color CaptionBackgroundColor { get; set; }

        #endregion

        #region help methods

        private _NonClientSizeInfo GetNonClientInfo(IntPtr hwnd)
        {
            _NonClientSizeInfo info = new _NonClientSizeInfo();
            info.CaptionButtonSize = SystemInformation.CaptionButtonSize;
            info.CaptionHeight = SystemInformation.CaptionHeight;

            switch (this.FormBorderStyle)
            {
                case System.Windows.Forms.FormBorderStyle.Fixed3D:
                    info.BorderSize = SystemInformation.FixedFrameBorderSize;
                    break;
                case System.Windows.Forms.FormBorderStyle.FixedDialog:
                    info.BorderSize = SystemInformation.FixedFrameBorderSize;
                    break;
                case System.Windows.Forms.FormBorderStyle.FixedSingle:
                    info.BorderSize = SystemInformation.FixedFrameBorderSize;
                    break;
                case System.Windows.Forms.FormBorderStyle.FixedToolWindow:
                    info.BorderSize = SystemInformation.FixedFrameBorderSize;
                    info.CaptionButtonSize = SystemInformation.ToolWindowCaptionButtonSize;
                    info.CaptionHeight = SystemInformation.ToolWindowCaptionHeight;
                    break;
                case System.Windows.Forms.FormBorderStyle.Sizable:
                    info.BorderSize = SystemInformation.FrameBorderSize;
                    break;
                case System.Windows.Forms.FormBorderStyle.SizableToolWindow:
                    info.CaptionButtonSize = SystemInformation.ToolWindowCaptionButtonSize;
                    info.BorderSize = SystemInformation.FrameBorderSize;
                    info.CaptionHeight = SystemInformation.ToolWindowCaptionHeight;
                    break;
                default:
                    info.BorderSize = SystemInformation.BorderSize;
                    break;
            }

            _RECT areatRect = new _RECT();
            GetWindowRect(hwnd, ref areatRect);

            int width = areatRect.right - areatRect.left;
            int height = areatRect.bottom - areatRect.top;

            info.Width = width;
            info.Height = height;

            Point xy = new Point(areatRect.left, areatRect.top);
            xy.Offset(-areatRect.left, -areatRect.top);

            info.CaptionRect = new Rectangle(xy.X, xy.Y + info.BorderSize.Height, width, info.CaptionHeight);
            info.Rect = new Rectangle(xy.X, xy.Y, width, height);
            info.ClientRect = new Rectangle(xy.X + info.BorderSize.Width,
                xy.Y + info.CaptionHeight + info.BorderSize.Height,
                width - info.BorderSize.Width * 2,
                height - info.CaptionHeight - info.BorderSize.Height * 2);

            return info;
        }

        private void DrawTitle(Graphics g, _NonClientSizeInfo ncInfo, bool active)
        {
            int titleX;

            if (this.ShowIcon &&
                this.FormBorderStyle != System.Windows.Forms.FormBorderStyle.FixedToolWindow &&
                this.FormBorderStyle != System.Windows.Forms.FormBorderStyle.SizableToolWindow)
            {
                Size iconSize = SystemInformation.SmallIconSize;
                g.DrawIcon(this.Icon, new Rectangle(new Point(ncInfo.BorderSize.Width, ncInfo.BorderSize.Height + (ncInfo.CaptionHeight - iconSize.Height) / 2), iconSize));
                titleX = ncInfo.BorderSize.Width + iconSize.Width + ncInfo.BorderSize.Width;
            }
            else
            {
                titleX = ncInfo.BorderSize.Width;
            }

            SizeF captionTitleSize = g.MeasureString(this.Text, SystemFonts.CaptionFont);
            g.DrawString(this.Text, SystemFonts.CaptionFont, CaptionColor,
                    new RectangleF(titleX,
                        (ncInfo.BorderSize.Height + ncInfo.CaptionHeight - captionTitleSize.Height) / 2,
                        ncInfo.CaptionRect.Width - ncInfo.BorderSize.Width * 2 - SystemInformation.MinimumWindowSize.Width,
                        ncInfo.CaptionRect.Height), StringFormat.GenericTypographic);
        }

        private void DrawBorder(Graphics g, _NonClientSizeInfo ncInfo, Brush background, bool active)
        {
            Rectangle borderTop = new Rectangle(ncInfo.Rect.Left,
                    ncInfo.Rect.Top,
                    ncInfo.Rect.Left + ncInfo.Rect.Width,
                    ncInfo.Rect.Top + ncInfo.BorderSize.Height);
            Rectangle borderLeft = new Rectangle(
                    new Point(ncInfo.Rect.Location.X, ncInfo.Rect.Location.Y + ncInfo.BorderSize.Height),
                    new Size(ncInfo.BorderSize.Width, ncInfo.ClientRect.Height + ncInfo.CaptionHeight + ncInfo.BorderSize.Height));
            Rectangle borderRight = new Rectangle(ncInfo.Rect.Left + ncInfo.Rect.Width - ncInfo.BorderSize.Width,
                    ncInfo.Rect.Top + ncInfo.BorderSize.Height,
                    ncInfo.BorderSize.Width,
                    ncInfo.ClientRect.Height + ncInfo.CaptionHeight + ncInfo.BorderSize.Height);
            Rectangle borderBottom = new Rectangle(ncInfo.Rect.Left + ncInfo.BorderSize.Width,
                    ncInfo.Rect.Top + ncInfo.Rect.Height - ncInfo.BorderSize.Height,
                    ncInfo.Rect.Width - ncInfo.BorderSize.Width * 2,
                    ncInfo.Rect.Height);

            //Rectangle leftbottom = new Rectangle(new Point(ncInfo.Rect.Location.X, ncInfo.Rect.Height - ncInfo.BorderSize.Width * 2),
            //    new Size(ncInfo.BorderSize.Width * 2, ncInfo.BorderSize.Width * 2));

            //g.FillPie(Brushes.Red, leftbottom, 90, 180);
            //g.FillRectangle(Brushes.Red, leftbottom);
            // top border
            g.FillRectangle(background, borderTop);
            // left border
            g.FillRectangle(background, borderLeft);
            // right border
            g.FillRectangle(background, borderRight);
            // bottom border
            g.FillRectangle(background, borderBottom);
        }

        private void DrawCaption(IntPtr hwnd, bool active)
        {
            IntPtr dc;
            Graphics g;
            Size iconSize;
            _NonClientSizeInfo ncInfo;
            Brush backgroundColor = new SolidBrush(CaptionBackgroundColor);
            Brush foregroundColor = CaptionColor;

            iconSize = SystemInformation.SmallIconSize;

            dc = GetWindowDC(hwnd);
            ncInfo = GetNonClientInfo(hwnd);
            g = Graphics.FromHdc(dc);

            g.FillRectangle(backgroundColor, ncInfo.CaptionRect);

            DrawBorder(g, ncInfo, backgroundColor, active);
            DrawTitle(g, ncInfo, active);
            DrawControlBox(g, ncInfo, backgroundColor, this.ControlBox, this.MaximizeBox, this.MinimizeBox, this.HelpButton);

            g.Dispose();
            ReleaseDC(hwnd, dc);
        }

        private void DrawControlBox(Graphics g, _NonClientSizeInfo info, Brush background, bool closeBtn, bool maxBtn, bool minBtn, bool helpBtn)
        {
            if (this.ControlBox)
            {
                int closeBtnPosX = info.CaptionRect.Left + info.CaptionRect.Width - info.BorderSize.Width - info.CaptionButtonSize.Width;
                int maxBtnPosX = closeBtnPosX - info.CaptionButtonSize.Width;
                int minBtnPosX = maxBtnPosX - info.CaptionButtonSize.Width;
                int btnPosY = info.BorderSize.Height + (info.CaptionHeight - info.CaptionButtonSize.Height) / 2;

                Rectangle btnRect = new Rectangle(new Point(closeBtnPosX, btnPosY), info.CaptionButtonSize);
                Rectangle maxRect = new Rectangle(new Point(maxBtnPosX, btnPosY), info.CaptionButtonSize);
                Rectangle minRect = new Rectangle(new Point(minBtnPosX, btnPosY), info.CaptionButtonSize);

                Brush backgroundColor = new SolidBrush(CaptionBackgroundColor);

                g.FillRectangle(backgroundColor, btnRect);
                g.FillRectangle(backgroundColor, maxRect);
                g.FillRectangle(backgroundColor, minRect);

                g.DrawImage(CloseButtonImage, btnRect);

                if (this.MaximizeBox || this.MinimizeBox)
                {
                    if (this.FormBorderStyle != System.Windows.Forms.FormBorderStyle.FixedToolWindow &&
                        this.FormBorderStyle != System.Windows.Forms.FormBorderStyle.SizableToolWindow)
                    {
                        if (this.WindowState == FormWindowState.Maximized)
                        {
                            g.DrawImage(MaximumNormalButtonImage, maxRect);
                        }
                        else
                        {
                            g.DrawImage(MaximumButtonImage, maxRect);
                        }
                        g.DrawImage(MinimumButtonImage, minRect);
                    }
                }
                else if (this.HelpButton)
                {
                    if (this.FormBorderStyle != System.Windows.Forms.FormBorderStyle.FixedToolWindow &&
                        this.FormBorderStyle != System.Windows.Forms.FormBorderStyle.SizableToolWindow)
                    {
                        g.DrawImage(HelpButtonImage, maxRect);
                    }
                }
            }
        }

        #endregion

        #region Major method WndProc

        private int LOBYTE(long p) { return (int)(p & 0x0000FFFF); }
        private int HIBYTE(long p) { return (int)(p >> 16); }

        protected override void WndProc(ref Message m)
        {
            if (this.FormBorderStyle != System.Windows.Forms.FormBorderStyle.None)
            {
                switch (m.Msg)
                {
                    case WM_NCPAINT:
                        DrawCaption(m.HWnd, Form.ActiveForm == this);
                        return;
                    case WM_NCACTIVATE:
                        DrawCaption(m.HWnd, m.WParam.ToInt32() > 0);
                        return;
                    case WM_NCRBUTTONDOWN:
                        {
                            int posX, posY;
                            int wp = m.WParam.ToInt32();
                            long lp = m.LParam.ToInt64();
                            posX = LOBYTE(lp);
                            posY = HIBYTE(lp);

                            if (wp == HTCAPTION)
                            {
                                Point pt = this.PointToClient(new Point(posX, posY));
                                if (this.CaptionContextMenu != null)
                                {
                                    this.CaptionContextMenu.Show(posX, posY);
                                    return;
                                }
                            }
                            break;
                        }
                    case WM_SETCURSOR:
                        if (this.ControlBox)
                        {
                            int posX, posY;
                            int wp = m.WParam.ToInt32();
                            long lp = m.LParam.ToInt64();
                            posX = LOBYTE(lp);
                            posY = HIBYTE(lp);

                            Brush backgroundColor = new SolidBrush(CaptionBackgroundColor);
                            _NonClientSizeInfo ncInfo = GetNonClientInfo(m.HWnd);
                            IntPtr dc = GetWindowDC(m.HWnd);

                            Graphics g = Graphics.FromHdc(dc);
                            int closeBtnPosX = ncInfo.CaptionRect.Left + ncInfo.CaptionRect.Width - ncInfo.BorderSize.Width - ncInfo.CaptionButtonSize.Width;
                            int maxBtnPosX, minBtnPosX;
                            maxBtnPosX = closeBtnPosX - ncInfo.CaptionButtonSize.Width;
                            minBtnPosX = maxBtnPosX - ncInfo.CaptionButtonSize.Width;

                            int btnPosY = ncInfo.BorderSize.Height + (ncInfo.CaptionHeight - ncInfo.CaptionButtonSize.Height) / 2;

                            Rectangle btnRect = new Rectangle(new Point(closeBtnPosX, btnPosY), ncInfo.CaptionButtonSize);
                            Rectangle maxRect = new Rectangle(new Point(maxBtnPosX, btnPosY), ncInfo.CaptionButtonSize);
                            Rectangle minRect = new Rectangle(new Point(minBtnPosX, btnPosY), ncInfo.CaptionButtonSize);

                            g.FillRectangle(backgroundColor, btnRect);
                            g.FillRectangle(backgroundColor, maxRect);
                            g.FillRectangle(backgroundColor, minRect);

                            if (posX != HTCLOSE)
                            {
                                g.DrawImage(CloseButtonImage, btnRect);
                            }
                            else if (MouseButtons != System.Windows.Forms.MouseButtons.Left)
                            {
                                g.DrawImage(CloseButtonHoverImage, btnRect);
                            }
                            else
                            {
                                g.DrawImage(CloseButtonPressDownImage, btnRect);
                            }

                            if (this.MaximizeBox || this.MinimizeBox)
                            {
                                if (this.FormBorderStyle != System.Windows.Forms.FormBorderStyle.FixedToolWindow &&
                                    this.FormBorderStyle != System.Windows.Forms.FormBorderStyle.SizableToolWindow)
                                {
                                    if (this.WindowState == FormWindowState.Maximized)
                                    {
                                        if (this.MaximizeBox)
                                        {
                                            if (posX != HTMAXBUTTON)
                                            {
                                                g.DrawImage(MaximumNormalButtonImage, maxRect);
                                            }
                                            else if (MouseButtons != System.Windows.Forms.MouseButtons.Left)
                                            {
                                                g.DrawImage(MaximumNormalButtonHoverImage, maxRect);
                                            }
                                            else
                                            {
                                                g.DrawImage(MaximumNormalButtonPressDownImage, maxRect);
                                            }
                                        }
                                        else
                                        {
                                            g.DrawImage(MaximumNormalButtonImage, maxRect);
                                        }
                                    }
                                    else
                                    {
                                        if (this.MaximizeBox)
                                        {
                                            if (posX != HTMAXBUTTON)
                                            {
                                                g.DrawImage(MaximumButtonImage, maxRect);
                                            }
                                            else if (MouseButtons != System.Windows.Forms.MouseButtons.Left)
                                            {
                                                g.DrawImage(MaximumButtonHoverImage, maxRect);
                                            }
                                            else
                                            {
                                                g.DrawImage(MaximumButtonPressDownImage, maxRect);
                                            }
                                        }
                                        else
                                        {
                                            g.DrawImage(MaximumButtonImage, maxRect);
                                        }
                                    }

                                    if (this.MinimizeBox)
                                    {
                                        if (posX != HTMINBUTTON)
                                        {
                                            g.DrawImage(MinimumButtonImage, minRect);
                                        }
                                        else if (MouseButtons != System.Windows.Forms.MouseButtons.Left)
                                        {
                                            g.DrawImage(MinimumButtonHoverImage, minRect);
                                        }
                                        else
                                        {
                                            g.DrawImage(MinimumButtonPressDownImage, minRect);
                                        }
                                    }
                                    else
                                    {
                                        g.DrawImage(MinimumButtonImage, minRect);
                                    }
                                }
                            }
                            else if (this.HelpButton)
                            {
                                if (this.FormBorderStyle != System.Windows.Forms.FormBorderStyle.FixedToolWindow &&
                                    this.FormBorderStyle != System.Windows.Forms.FormBorderStyle.SizableToolWindow)
                                {
                                    if (posX != HTHELP)
                                    {
                                        g.DrawImage(HelpButtonImage, maxRect);
                                    }
                                    else if (MouseButtons != System.Windows.Forms.MouseButtons.Left)
                                    {
                                        g.DrawImage(HelpButtonHoverImage, maxRect);
                                    }
                                    else
                                    {
                                        g.DrawImage(HelpButtonPressDownImage, maxRect);
                                    }
                                }
                            }

                            g.Dispose();
                            ReleaseDC(m.HWnd, dc);
                        }
                        break;
                    case WM_NCLBUTTONUP:
                        {
                            int wp = m.WParam.ToInt32();
                            switch (wp)
                            {
                                case HTCLOSE:
                                    m.Msg = WM_SYSCOMMAND;
                                    m.WParam = new IntPtr(SC_CLOSE);
                                    break;
                                case HTMAXBUTTON:
                                    if (this.MaximizeBox)
                                    {
                                        m.Msg = WM_SYSCOMMAND;
                                        if (this.WindowState == FormWindowState.Maximized)
                                        {
                                            m.WParam = new IntPtr(SC_RESTORE);
                                        }
                                        else
                                        {
                                            m.WParam = new IntPtr(SC_MAXIMIZE);
                                        }
                                    }
                                    break;
                                case HTMINBUTTON:
                                    if (this.MinimizeBox)
                                    {
                                        m.Msg = WM_SYSCOMMAND;
                                        m.WParam = new IntPtr(SC_MINIMIZE);
                                    }
                                    break;
                                case HTHELP:
                                    m.Msg = WM_SYSCOMMAND;
                                    m.WParam = new IntPtr(SC_CONTEXTHELP);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        }

                    case WM_NCLBUTTONDOWN:
                        if (this.ControlBox)
                        {
                            bool ret = false;
                            int posX, posY;
                            int wp = m.WParam.ToInt32();
                            long lp = m.LParam.ToInt64();
                            posX = LOBYTE(lp);
                            posY = HIBYTE(lp);

                            _NonClientSizeInfo ncInfo = GetNonClientInfo(m.HWnd);
                            IntPtr dc = GetWindowDC(m.HWnd);
                            Brush backgroundColor = new SolidBrush(CaptionBackgroundColor);

                            Graphics g = Graphics.FromHdc(dc);
                            int closeBtnPosX = ncInfo.CaptionRect.Left + ncInfo.CaptionRect.Width - ncInfo.BorderSize.Width - ncInfo.CaptionButtonSize.Width;
                            int maxBtnPosX, minBtnPosX;
                            int btnPosY = ncInfo.BorderSize.Height + (ncInfo.CaptionHeight - ncInfo.CaptionButtonSize.Height) / 2;
                            maxBtnPosX = closeBtnPosX - ncInfo.CaptionButtonSize.Width;
                            minBtnPosX = maxBtnPosX - ncInfo.CaptionButtonSize.Width;

                            Rectangle btnRect = new Rectangle(new Point(closeBtnPosX, btnPosY), ncInfo.CaptionButtonSize);
                            Rectangle maxRect = new Rectangle(new Point(maxBtnPosX, btnPosY), ncInfo.CaptionButtonSize);
                            Rectangle minRect = new Rectangle(new Point(minBtnPosX, btnPosY), ncInfo.CaptionButtonSize);

                            g.FillRectangle(backgroundColor, btnRect);
                            g.FillRectangle(backgroundColor, maxRect);
                            g.FillRectangle(backgroundColor, minRect);

                            if (wp == HTCLOSE)
                            {
                                g.DrawImage(CloseButtonPressDownImage, btnRect);
                                ret = true;
                            }
                            else
                            {
                                g.DrawImage(CloseButtonImage, btnRect);
                            }

                            if (this.MaximizeBox || this.MinimizeBox)
                            {
                                if (this.FormBorderStyle != System.Windows.Forms.FormBorderStyle.SizableToolWindow &&
                                    this.FormBorderStyle != System.Windows.Forms.FormBorderStyle.FixedToolWindow)
                                {
                                    if (this.WindowState == FormWindowState.Maximized)
                                    {
                                        if (wp == HTMAXBUTTON && this.MaximizeBox)
                                        {
                                            minBtnPosX = maxBtnPosX - ncInfo.CaptionButtonSize.Width;
                                            g.DrawImage(MaximumNormalButtonPressDownImage, maxRect);
                                            ret = true;
                                        }
                                        else
                                        {
                                            g.DrawImage(MaximumNormalButtonImage, maxRect);
                                        }
                                    }
                                    else
                                    {
                                        if (wp == HTMAXBUTTON && this.MaximizeBox)
                                        {
                                            minBtnPosX = maxBtnPosX - ncInfo.CaptionButtonSize.Width;
                                            g.DrawImage(MaximumButtonPressDownImage, maxRect);
                                            ret = true;
                                        }
                                        else
                                        {
                                            g.DrawImage(MaximumButtonImage, maxRect);
                                        }
                                    }
                                    if (wp == HTMINBUTTON && this.MinimizeBox)
                                    {
                                        g.DrawImage(MinimumButtonPressDownImage, minRect);
                                        ret = true;
                                    }
                                    else
                                    {
                                        g.DrawImage(MinimumButtonImage, minRect);
                                    }
                                }
                            }
                            else if (this.HelpButton)
                            {
                                if (this.FormBorderStyle != System.Windows.Forms.FormBorderStyle.FixedToolWindow &&
                                    this.FormBorderStyle != System.Windows.Forms.FormBorderStyle.SizableToolWindow)
                                {
                                    if (wp == HTHELP)
                                    {
                                        g.DrawImage(HelpButtonPressDownImage, maxRect);
                                        ret = true;
                                    }
                                    else
                                    {
                                        g.DrawImage(HelpButtonImage, maxRect);
                                    }
                                }
                            }

                            g.Dispose();
                            ReleaseDC(m.HWnd, dc);

                            if (ret)
                                return;
                        }
                        break;
                }
            }

            base.WndProc(ref m);
        }

        #endregion
    }
}