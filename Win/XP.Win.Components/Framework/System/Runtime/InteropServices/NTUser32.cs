using System.Text;

namespace System.Runtime.InteropServices
{
    /// <summary>
    /// user32.dll的封装。
    /// </summary>
    public class NTUser32
    {
        #region << mouse event >>
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, UIntPtr extraInfo);

        [Flags]
        public enum MouseEventFlag : uint
        {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            XDown = 0x0080,
            XUp = 0x0100,
            Wheel = 0x0800,
            VirtualDesk = 0x4000,
            Absolute = 0x8000
        }
        #endregion

        #region << Send/PostMessage >>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        #endregion

        #region << EnumWindows >>
        /// <summary>
        /// 遍历窗口的回调函数。继续时返回True，否则返回False。
        /// </summary>
        public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
        #endregion

        #region << Get/SetWindowLong >>
        public const int GWL_STYLE = -16;
        public const int WS_CHILD = 0x40000000;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        #endregion

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool ShowWindow(IntPtr hWnd, short State);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public extern static IntPtr SetParent(IntPtr hChild, IntPtr hParent);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter,
                    int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public const int HWND_TOP = 0x0;
        public const int WM_COMMAND = 0x0112;
        public const int WM_QT_PAINT = 0xC2DC;
        public const int WM_PAINT = 0x000F;
        public const int WM_SIZE = 0x0005;
        public const int SWP_FRAMECHANGED = 0x0020;
        public const int WM_COPYDATA = 0x004A;

        public const int WM_CLOSE = 0x0010;     //关闭窗体。窗体进入Closing，允许窗体自己控制
        public const int WM_DESTROY = 0x0002;   //关闭程序。窗体和进程直接销毁了
        public const int WM_QUIT = 0x0012;      //关闭消息循环。Send时窗体销毁了，进程还在

        public const int WM_VSCROLL = 0x0115;   //垂直滚动条消息
        public const int SB_LINEDOWN = 1;       //向下滚动一行
        public const int SB_PAGEDOWN = 3;       //向下滚动一页
        public const int SB_BOTTOM = 7;         //滚动到最底部

        public const int WM_USER = 0x0400;

        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            public uint dwData;
            public int cbData;
            public IntPtr lpData;
        }
    }
}
