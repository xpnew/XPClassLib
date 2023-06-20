using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XP.IO.KeyAndMouse
{

    /// <summary>
    /// https://blog.csdn.net/qq_37477609/article/details/79524410
    /// </summary>
    public class HookKeyBoard
    {
        private enum HookType
        {
            WH_CALLWNDPROC = 4, //安装在系统将消息发送到目标窗口过程之前监视邮件的挂钩过程。
            WH_CALLWNDPROCRET = 12, //安装在目标窗口过程处理完邮件后监视消息的挂钩过程。
            WH_CBT = 5, //安装接收对 CBT 应用程序有用的通知的挂钩过程。
            WH_DEBUG = 9, //安装用于调试其他挂钩过程的挂钩过程。
            WH_FOREGROUNDIDLE = 11, //安装将在应用程序的前景线程即将变为空闲时调用的挂钩过程。此挂钩对于在空闲时间执行低优先级任务非常有用。
            WH_GETMESSAGE = 3, //安装用于监视张贴到消息队列的邮件的挂钩过程。
            WH_HARDWARE = 8, //安装一个挂接过程, 用于张贴以前由 WH_JOURNALRECORD 挂钩过程记录的消息。
            WH_JOURNALPLAYBACK = 1, //安装一个挂接过程, 用于张贴以前由 WH_JOURNALRECORD 挂钩过程记录的消息。
            WH_JOURNALRECORD = 0,//安装用于记录张贴到系统消息队列中的输入消息的挂钩过程。
            WH_KEYBOARD = 2,//安装监视击键消息的挂钩过程。
            WH_MOUSE = 7,//安装监视鼠标消息的挂钩过程。
            WH_MSGFILTER = (-1), //安装一个钩子过程, 用于监视对话框、消息框、菜单或滚动条中由于输入事件而生成的消息。
            WH_SHELL = 10,//安装接收对 shell 应用程序有用的通知的挂钩过程。
            WH_SYSMSGFILTER = 6,//安装一个钩子过程, 用于监视对话框、消息框、菜单或滚动条中由于输入事件而生成的消息。挂钩过程监视与调用线程在同一桌面上的所有应用程序的这些消息。
            WH_KEYBOARD_LL = 13,//安装用于监视低级键盘输入事件的挂钩过程。
            WH_MOUSE_LL = 14,//安装用于监视低级别鼠标输入事件的挂钩过程。
        }

        private const int WM_KEYUP = 0X101;
        private const int WM_SYSKEYUP = 0X105;

        private delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);

        private int KeyHook;

        private int MouseHook;

        public event KeyEventHandler keyeventhandler;

        [StructLayout(LayoutKind.Sequential)]
        private class KeyBoardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        //设置钩子 
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);// 钩子类型  回调函数地址  实例句柄 线程ID

        //卸载钩子 
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern bool UnhookWindowsHookEx(int idHook); //要移除的钩子的句柄。此参数是由以前对 SetWindowsHookEx 的调用获取的挂钩句柄。

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string name); //获取进程句柄

        [DllImport("user32", EntryPoint = "GetMessage")]
        public static extern int GetMessage(  //调用线程的消息队列里取得一个消息并将其放于指定的结构.此函数可取得与指定窗口联系的消息和由PostThreadMessage寄送的线程消息
                        out tagMSG lpMsg,//指向MSG结构的指针，该结构从线程的消息队列里接收消息信息。
                        IntPtr hwnd,//取得其消息的窗口的句柄。当其值取NULL时，GetMessage为任何属于调用线程的窗口检索消息，线程消息通过PostThreadMessage寄送给调用线程。
                        int wMsgFilterMin,//指定被检索的最小消息值的整数。
                        int wMsgFilterMax//指定被检索的最大消息值的整数。
        );
        [DllImport("user32", EntryPoint = "DispatchMessage")]
        public static extern int DispatchMessage(ref tagMSG lpMsg); //函数将键盘消息转化

        [DllImport("user32", EntryPoint = "TranslateMessage")]
        public static extern int TranslateMessage(ref tagMSG lpMsg); //函数将消息传给窗体函数去处理
        public struct tagMSG
        {
            public int hwnd;
            public uint message;
            public int wParam;
            public long lParam;
            public uint time;
            public int pt;
        }


        public void InstallHook(Form form)
        {
            if (KeyHook.Equals(0))
            {
                HookProc hp = new HookProc(KeyMouseHookProc);
                KeyHook = SetWindowsHookEx((int)HookType.WH_KEYBOARD_LL,
                          hp,
                        GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName), 0);
                MouseHook = SetWindowsHookEx((int)HookType.WH_MOUSE_LL, hp,
                    GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName), 0);
            }
            if (KeyHook.Equals(0) && MouseHook.Equals(0))
            {
                Hook_Clear();
                throw new Exception("安装钩子失败");
            }
        }

        /// <summary>
        /// 自己想要的信息处理 
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private int KeyMouseHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            if (keyeventhandler != null && nCode >= 0)
            {
                KeyBoardHookStruct kbh = (KeyBoardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyBoardHookStruct));
                if ((wParam == WM_KEYUP || wParam == WM_SYSKEYUP))
                {
                    Keys keyData = (Keys)kbh.vkCode;
                    KeyEventArgs e = new KeyEventArgs(keyData);
                    keyeventhandler(this, e);
                }
            }
            return 1;
        }

        //取消钩子事件 
        public void Hook_Clear()
        {
            bool retKeyboard = true;
            if (!KeyHook.Equals(0) && !MouseHook.Equals(0))
            {
                retKeyboard = UnhookWindowsHookEx(KeyHook);
                if (retKeyboard) retKeyboard = UnhookWindowsHookEx(MouseHook);
            }
            //如果去掉钩子失败. 
            if (!retKeyboard) throw new Exception("UnhookWindowsHookEx failed.");
        }

        public void ShowMoues()
        {
            ShowCursor(true);
        }
        public void HideMoues()
        {
            ShowCursor(false);
        }



        [DllImport("user32", EntryPoint = "ShowCursor")]
        public extern static bool ShowCursor(bool show);
    }
}
