using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using XP.Comm.Img;

namespace XP.IO.KeyAndMouse
{

    /// <summary>
    /// 鼠标辅助类
    /// </summary>
    /// <remarks>
    /// 参考：
    /// https://www.cnblogs.com/falcon-fei/p/11396740.html
    /// https://www.cnblogs.com/mq0036/p/11382223.html
    /// </remarks>
    public class MouseHelper
    {
        /// <summary>
        /// 综合鼠标移动和按钮点击。该方法包含鼠标左右键、滚轮、移动及点击操作。
        /// </summary>
        /// <remarks>
        /// 为了符合一般用户习惯，建议结合SetCursorPos使用，先将光标移动到指定位置，将坐标全部置0.
        ///
        /// </remarks>
        /// <param name="dwFlags">标志位集，指定点击按钮和鼠标动作的多种情况。一共9种。</param>
        /// <param name="dx">座标值，可以是相对坐标，也可以是绝对坐标。</param>
        /// <param name="dy"></param>
        /// <param name="dwData">如果dwFlags为MOUSEEVENTF_WHEEL，则dwData指定鼠标轮移动的数量。如果dwFlagsS不是MOUSEEVENTF_WHEEL，则dWData应为零。</param>
        /// <param name="dwExtraInfo">指定与鼠标事件相关的附加32位值。应用程序调用函数GetMessageExtraInfo来获得此附加信息</param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("user32")]
        public static extern int mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        //移动鼠标 
        public const int MOUSEEVENTF_MOVE = 0x0001;
        //模拟鼠标左键按下 
        public const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        //模拟鼠标左键抬起 
        public const int MOUSEEVENTF_LEFTUP = 0x0004;
        //模拟鼠标右键按下 
        public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        //模拟鼠标右键抬起 
        public const int MOUSEEVENTF_RIGHTUP = 0x0010;
        //模拟鼠标中键按下 
        public const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        //模拟鼠标中键抬起 
        public const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        //标示是否采用绝对坐标 
        public const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        /// <summary>
        /// 把光标移到屏幕的指定位置。（ps：是整个屏幕的坐标,相对于屏幕左上角的绝对位置）
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns>
        /// 如果成功，返回非0值
        /// 如果失败，返回值是0
        /// </returns>
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);


        /// <summary>
        /// 左健单击
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void LeftClick(int x, int y)
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, x, y, 0, 0);
        }

        /// <summary>
        /// 左健单击
        /// </summary>
        public static void LeftClick()
        {
            mouse_event( MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public static void PressAndDrag(int x, int y, int MoveX, int MoveY, bool enableDealy = false, int dealySecond =0)
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN , x, y, 0, 0);

            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, MoveX, MoveY, 0, 0);

            mouse_event( MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

        }



        /// <summary>
        /// 参考
        /// https://www.cnblogs.com/wuqianling/p/5958138.html
        /// </summary>
        /// <returns></returns>
        public static ScreenPoint GetScreenPoint()
        {
            POINT p = new POINT();
            GetCursorPos(out p);
            IntPtr hdc = GetDC(new IntPtr(0));//取到设备场景(0就是全屏的设备场景) 
            int c = GetPixel(hdc, p);//取指定点颜色 
            int r = (c & 0xFF);//转换R 
            int g = (c & 0xFF00) / 256;//转换G 
            int b = (c & 0xFF0000) / 65536;//转换B 
            var color = Color.FromArgb(r, g, b);

            return new ScreenPoint() { PointColor = color, X = p.X, Y = p.Y };
        }

        #region 鼠标颜色

        [DllImport("user32.dll")]//取设备场景 
        private static extern IntPtr GetDC(IntPtr hwnd);//返回设备场景句柄 
        [DllImport("gdi32.dll")]//取指定点颜色 
        private static extern int GetPixel(IntPtr hdc, POINT p);
        #endregion


        #region 光标位置

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public XP.XPoint GetPoint()
            {
                return new XPoint() {X = this.X, Y = this.Y};
            }
        }

        public static XPoint GetMousePoint()
        {
            POINT p = new POINT();
            GetCursorPos(out p);
            return new XPoint() { X = p.X, Y = p.Y };
        }
        #endregion
    }
}
