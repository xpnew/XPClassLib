using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace XP.Util.Http.Down
{
    public class PicDownTaskItem : XP.Comm.Http.Tasks.HttpTaskItem
    {
        //单个图片文件最大重试次数
        private readonly int _SinglePicDownTimesMax = 3;

        public string WebUrl { get; set; }

        public string PhyFile { get; set; }

        public string PhyDir { get; set; }


        public Size? MiniSize { get; set; }

        public override void Work()
        {
            base.Work();
            PhyDir = System.IO.Path.GetDirectoryName(PhyFile);

            PhyDir = PhyDir.TrimEnd('\\');
            PhyDir += "\\";

            if (!CheckDir(PhyDir))
            {
                LogErr("检查目录【" + PhyDir + "】失败，目录不存在并且无法创建。");
                return;
            }

            if (DownWebFile(WebUrl, PhyFile, _SinglePicDownTimesMax))
            {
                return ;
            }

        }


        public PicDownTaskItem(string web, string phy)
        {
            WebUrl = web;
            PhyFile = phy;
        }

        /// <summary>
        /// 从指定地址下载图片到本地
        /// </summary>
        /// <remarks>
        /// 2017年9月2日 新增，下载的文件先保存到临时的名字，下载成功再改名到正式的名字。
        /// 如果原先的文件已经存在，那么会先删除原来的文件。
        /// </remarks>
        /// <param name="webUrl"></param>
        /// <param name="phyPath"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        protected bool DownWebFile(string webUrl, string phyPath, int step)
        {

            if (0 == step)
            {
                LogErr("下载文件【" + webUrl + "】失败，重试次数用完。");
                return false;
            }
            step--;


            string DownTempFileName = GetRndTmpName(PhyDir);

            //获取远程文件的数据流
            int bufferSize = 2048;
            byte[] bytes = new byte[bufferSize];


            try
            {
                if (webUrl.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | (SecurityProtocolType)12288;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | (SecurityProtocolType)12288;


                    ServicePointManager.ServerCertificateValidationCallback =
                        new RemoteCertificateValidationCallback(checkValidationResult);
                }

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(webUrl);

                // 核心步骤：设置 User-Agent 模拟真实浏览器
                //request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36";

                //request.Accept = "image/webp,image/apng,image/svg+xml,image/*,*/*;q=0.8"; // 告诉服务器你能接收图片格式
                //request.Referer = "https://jrants.com/2024051007405.html/"; // 极其重要！伪造来源，填该图片所在的网页地址
                //request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8");
                //request.Headers.Add("Accept-Encoding", "gzip, deflate, br"); // 部分服务器会校验压缩格式

                request.Method = "GET";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36";
                request.Accept = "image/webp,image/apng,image/*,*/*;q=0.8";
                //request.KeepAlive = false; // 尝试关闭长连接，避免连接池复用导致的问题
                request.Timeout = 10000; // 设置合理的超时时间

                // 不使用代理
                //request.Proxy = null;
                // 或者使用默认系统代理
                 request.Proxy = WebRequest.DefaultWebProxy;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.IO.Stream stream = response.GetResponseStream();
                DownTempFileName =  DownTempNameBuilder.Self.GetName(PhyDir, "downtemp",1);
                FileStream fs = new FileStream(DownTempFileName, FileMode.Create);


                int length = stream.Read(bytes, 0, bufferSize);

                while (length > 0)
                {
                    fs.Write(bytes, 0, length);
                    length = stream.Read(bytes, 0, bufferSize);
                }
                stream.Close();
                fs.Close();
                response.Close();
                try
                {
                    if (System.IO.File.Exists(phyPath))
                    {
                        System.IO.File.Delete(phyPath);
                    }
                    System.IO.File.Move(DownTempFileName, phyPath);
                }
                catch (Exception ex)
                {
                    Log(" 下载失败" + ex.Message + "这是第" + (_SinglePicDownTimesMax - step).ToString() + "次");
                    return false;
                }
                Log("下载文件【" + webUrl + "】成功，这是第" + (_SinglePicDownTimesMax - step).ToString() + "次。");

                XP.Util.Loger.LogHelper.Instance.Debuglog("下载文件【" + webUrl + "】成功，这是第" + (_SinglePicDownTimesMax - step).ToString() + "次。", "_ReadyFile.txt");
                return true;
            }
            catch (System.Net.WebException webEx)
            {
                XP.Loger.Error("请求地址： " + webUrl + "出现错误。", webEx);
                var sta = webEx.Status;
                if (sta == WebExceptionStatus.ProtocolError)
                {
                    //Console.WriteLine("Status Code : {0}", ((HttpWebResponse)webEx.Response).StatusCode);
                    //Console.WriteLine("Status Description : {0}", ((HttpWebResponse)webEx.Response).StatusDescription);

                    LogErr("下载文件【" + webUrl + "】失败，这是第" + (_SinglePicDownTimesMax - step).ToString() + "次。返回服务器状态：" + ((HttpWebResponse)webEx.Response).StatusCode + " 说明： " + ((HttpWebResponse)webEx.Response).StatusDescription);

                    return false;
                }
                else
                {
                    return DownWebFile(webUrl, phyPath, step);

                }
            }
            catch (Exception ex)
            {
                XP.Loger.Error("请求地址： " + webUrl + "出现错误。", ex);
                LogErr("下载文件【" + webUrl + "】失败，这是第" + (_SinglePicDownTimesMax - step).ToString() + "次。异常信息：" + ex);

                if (System.IO.File.Exists(DownTempFileName))
                {
                    System.IO.File.Delete(DownTempFileName);
                }
                return DownWebFile(webUrl, phyPath, step);
            }
        }


        protected string GetRndTmpName(string dir)
        {
            string rnd = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            string DownTempFileName = dir + rnd + ".downtemp";

            //if (System.IO.File.Exists(DownTempFileName))
            //{
            //   int SleepRnd =   GeneralTool.Random(5, 120);


            //    Thread.Sleep(SleepRnd);

            //    return GetRndTmpName(dir);
            //}

            return DownTempFileName;

        }

        public bool CheckPic(string phyFile)
        {


            return true;
        }

        /// <summary>
        /// 检查文件夹路径，如果不存在则重新创建
        /// </summary>
        /// <param name="physicalPath"></param>
        /// <returns></returns>
        public bool CheckDir(string physicalDir)
        {
            if (System.IO.Directory.Exists(physicalDir))
            {
                return true;
            }
            else
            {
                try
                {
                    System.IO.Directory.CreateDirectory(physicalDir);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }


        private static bool checkValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }


    }
}
