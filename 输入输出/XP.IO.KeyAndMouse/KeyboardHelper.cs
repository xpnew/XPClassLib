using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace XP.IO.KeyAndMouse
{


    /// <summary>
    /// 键盘帮助类
    /// </summary>
    /// <remarks>
    /// 参考：https://blog.csdn.net/weixin_33924770/article/details/86009043
    /// https://www.cnblogs.com/smallfa/p/5741351.html
    /// https://blog.csdn.net/fjsd155/article/details/79495242
    /// https://www.cnblogs.com/soundcode/p/14031332.html
    /// 
    /// 
    /// 解决字对不码的的问题：
    /// https://www.crifan.com/convert_char_letter_to_key_keycode_in_csharp/comment-page-1/
    /// </remarks>
    public class KeyboardHelper
    {



        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);



        public static void KeyPress(byte keyName)
        {
            x.Say("keyname: " + keyName);

            KeyboardHelper.keybd_event(keyName, 0, 0, 0);
            KeyboardHelper.keybd_event(keyName, 0, 2, 0);

        }

        public static void SendKeys(string input)
        {
            var charsss = input.ToCharArray();

            foreach (var cr in charsss)
            {
              
                System.Windows.Forms.SendKeys.SendWait("{"+cr+"}");
                System.Threading.Thread.Sleep(100);
            }
        }
        public static void SendNativeKeys(string input)
        {
            System.Windows.Forms.SendKeys.SendWait(input);
           
        }



        public static void SendText(string input)
        {

            var charsss = input.ToCharArray();

            foreach (var cr in charsss)
            {

             
                x.Say("source: " + cr);


                KeyPress((byte)cr); 
                System.Threading.Thread.Sleep(100);

            }

        }

    }
}
