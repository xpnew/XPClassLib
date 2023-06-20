using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XP.Sys.Progress
{
    public class ControlesFinder
    {

        public IntPtr Handle { get; set; }
        public Process WorkProcess { get; set; }




        public List<Control> AllSubList { get; set; } = new List<Control>();
        public List<Label> Result { get; set; } = new List<Label>();



        public async Task StartFind()
        {
            var c = Control.FromHandle(Handle);

            var arr = WorkProcess.Modules;


            var p1 = WorkProcess.Handle;
            var p2 = WorkProcess.MainWindowHandle;


            var p3 = ProcessUtil.FindWindow(null, "TeamViewer密码自动发送");

          

            //x.Say($"p3 == p2?? ==>> {(p3 == p2.ToInt32())}");

            x.Say("Process.Handle" + p1);
            x.Say("Process.MainWindowHandle" + p2);


            //var img1 = Capturer.GetWindowCapture(p1);

            //img1.Save("D:\\素材\\snapimg_" + XP.Util.GeneralTool.GetRndChars(12) + ".png");


            var img2 = Capturer.GetWindowCapture(p2);
            PicUtil.Color2Gray(img2);


            img2.Save("D:\\素材\\snapimg\\snapimg_" + XP.Util.GeneralTool.GetRndChars(12) + ".gif", System.Drawing.Imaging.ImageFormat.Gif);


            GetWin("TeamViewer");
            GetWin("TeamViewer密码自动发送");

            //await FindC(c);
            //await Filter();

        }
        /// <summary>
        /// 通过另一组代码获取窗口大小
        /// </summary>
        protected void GetWin(string name)
        {
            var p3 = ProcessUtil.FindWindow(null, name);

            RECT rect = new RECT();

            var flag =  ProcessUtil.GetWindowRect(p3, ref rect);

            x.Say($"获取 {name} 结果： {flag}， 获取到的宽度：{rect.Right - rect.Left} 高度：{rect.Bottom -  rect.Top}");




        }






        protected async Task FindC(Control c)
        {
            foreach (Control sub in c.Controls)
            {
                AllSubList.Add(sub);
                await FindC(sub);
            }
        }

        public async Task Filter()
        {
            foreach (var c in AllSubList)
            {
                if (c.GetType() == typeof(Label))
                {
                    Result.Add(c as Label);
                }
            }
        }


    }
}
