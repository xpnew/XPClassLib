using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace XP.Sys.Progress
{
    public class ProcessUtil
    {

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(
            string lpClassName,
            string lpWindowName
           );
        //已知窗口标题"abc",怎么得到窗口句柄?
        //IntPtr hWnd = FindWindow(null, "abc");


        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        //        使用实例:    ShowWindow(myPtr, 0);

        //0    关闭窗口

        //1    正常大小显示窗口

        //2    最小化窗口

        //3    最大化窗口
        //————————————————
        //版权声明：本文为CSDN博主「晴雨阳-_-!!!」的原创文章，遵循CC 4.0 BY-SA版权协议，转载请附上原文出处链接及本声明。
        //原文链接：https://blog.csdn.net/qq_40433102/article/details/84967483

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);


//        示例：                 

//                   InPtr awin = GetForegroundWindow();    //获取当前窗口

//        RECT rect = new RECT();
//        GetWindowRect(awin, ref rect);
//        int width = rect.Right - rect.Left;                        //窗口的宽度
//        int height = rect.Bottom - rect.Top;                   //窗口的高度
//————————————————
//版权声明：本文为CSDN博主「晴雨阳-_-!!!」的原创文章，遵循CC 4.0 BY-SA版权协议，转载请附上原文出处链接及本声明。
//原文链接：https://blog.csdn.net/qq_40433102/article/details/84967483


    }


    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;                             //最左坐标
        public int Top;                             //最上坐标
        public int Right;                           //最右坐标
        public int Bottom;                        //最下坐标
    }
}
