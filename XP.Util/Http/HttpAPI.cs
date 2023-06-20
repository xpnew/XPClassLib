using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace XP.Util.Http
{
    /// <summary>
    /// API接口通信
    /// </summary>
    public class HttpAPI
    {


        public string ContentType { get; set; }

        public Dictionary<string, string> HeaderDict { get; set; }

        /// <summary>  成功标志 </summary>
        public int MaxSendStep { get; set; }

        /// <summary>
        /// URL路径根节点列表
        /// </summary>
        /// <remarks>
        /// 它的目标是为了支持针对以下两种API地址能够轮流访问：
        /// http://a.com/b/c/123/456/
        /// http://xxxx.com/yyy/zzz/123/456/
        /// 然后上面的路径当中公共的部分【/123/456/】提炼出来放入【Path】。当然，【/123/456/】这样写是为了表达方便，个人习惯是把【/】留给上一级/上一层，所以实际上【Path】保存的是【123/456/】
        /// UrlRootList的内容是：
        /// http://a.com/b/c/
        /// http://xxxx.com/yyy/zzz/
        /// </remarks>
        public List<string> UrlRootList { get; set; }
        /// <summary>
        /// 请求的地址
        /// </summary>
        public string Path { get; set; }

        public string FullPath { get; set; }


        /// <summary>  需要发送的数据（一般是由SendJson转换过来） </summary>
        public byte[] SendData { get; set; }

        /// <summary> 准备发送的Json </summary>
        public string SendJson { get; set; }

        public string RecieveJson { get; set; }
        /// <summary>
        /// 接收回来的文本
        /// </summary>
        public string ResponseText { get; set; }

        public string RequestMethod { get; set; }

        /// <summary> 发送错误内容</summary>
        public string PostErrorMsg { get; set; }


        /// <summary>
        /// 每个地址请求的最大次数，默认为3
        /// </summary>
        private int _CurrentStep { get; set; }
        /// 轮流向主机发送请求用
        private int _HostIndex { get; set; }


        /// <summary>  结束标志（不管成不成功） </summary>
        public bool IsFinished { get; set; }
        /// <summary>  成功标志 </summary>
        public bool IsSuccess { get; set; }

        public string ProxyHost { get; set; }


        public string ProxyUsername { get; set; }
        public string ProxyPassword { get; set; }

        protected HttpWebRequest req;


        public HttpAPI(List<string> roots, string path, string sendJosn)
        {
            UrlRootList = roots;
            Path = path;
            SendJson = sendJosn;
            _Init();
        }

        public HttpAPI(string root, string path, string sendJson) : this(new List<string>() { root }, path, sendJson)
        {

        }

        protected virtual void _Init()
        {
            MaxSendStep = 3;

            SendData = Encoding.UTF8.GetBytes(SendJson);
            _HostIndex = -1;
            ContentType = "application/json";
            RequestMethod = "POST";
        }


        protected void _InitRequest()
        {
            req = (HttpWebRequest)WebRequest.Create(FullPath);
            if (!String.IsNullOrEmpty(ProxyHost))
            {
                req.Proxy = CreatProxy(ProxyHost, ProxyUsername, ProxyPassword);
                req.ContinueTimeout = 200 * 1000;
                req.Timeout = 200 * 1000;
            }

            req.ContentType = ContentType;   //application/octet-stream
            if (null != HeaderDict && 0 != HeaderDict.Count)
            {
                foreach (string key in HeaderDict.Keys)
                {
                    req.Headers.Set(key, HeaderDict[key]);
                }
            }
            req.Method = RequestMethod;
            //if ("POST" == RequestMethod.ToUpper())
            //{
            //    req.ContentLength = SendData.Length;
            //    using (Stream reqStream = req.GetRequestStream())
            //    {
            //        reqStream.Write(SendData, 0, SendData.Length);
            //        reqStream.Close();
            //    }
            //}

            try
            {
                if ("POST" == RequestMethod.ToUpper())
                {
                    req.ContentLength = SendData.Length;
                    using (Stream reqStream = req.GetRequestStream())
                    {
                        reqStream.Write(SendData, 0, SendData.Length);
                        reqStream.Close();
                    }
                }

            }
            catch (System.Net.WebException ex)
            {
                x.Say("网络异常： " + ex);
                System.Threading.Thread.Sleep(100);
                if (_HostIndex >= (UrlRootList.Count - 1) && _CurrentStep <= 0)
                {
                    return;
                }

            }
            catch (Exception ex)
            {
                x.Say("Post异常： " + ex);
            }
        }

        /// <summary>
        /// 切换到下一个主机地址
        /// </summary>
        protected void NextHost()
        {
            _CurrentStep = MaxSendStep;
            _HostIndex++;

            FullPath = UrlRootList[_HostIndex] + Path;
            if (null != req)
            {
                req.Abort();
            }
            x.Say("准备发送的地址：");
            x.Say(FullPath);


            _InitRequest();

        }

        protected void NextRequest()
        {
            _CurrentStep--;
            x.Say("这是第：" + (MaxSendStep - _CurrentStep) + "次请求");

        }

        #region 同步请求

        protected bool Post()
        {
            HttpWebResponse resp = null;
            Stream resStream = null;
            try
            {
                resp = req.GetResponse() as HttpWebResponse;
                resStream = resp.GetResponseStream();
                //获取响应内容  
                using (StreamReader reader = new StreamReader(resStream, Encoding.UTF8))
                {
                    ResponseText = reader.ReadToEnd();
                }

                return true;
            }
            catch (System.Net.WebException ex)
            {
                x.Say("网络异常： " + ex);
                return false;
            }
            catch (Exception ex)
            {
                x.Say("Post异常： " + ex);
                return false;
            }
            finally
            {
                if (resp != null)
                {
                    resp.Close();
                }
                if (resStream != null)
                {
                    resStream.Close();
                }
                if (req != null)
                {
                    //如果不能正确断开Reqeust，就不能释放相关的资源
                    //参考：
                    //https://blog.csdn.net/weixin_33828101/article/details/85588689
                    //https://www.cnblogs.com/Fooo/archive/2008/10/31/1323400.html
                    //https://blog.csdn.net/shenmafuyunnan/article/details/48542365
                    req.Abort();
                }
            }

        }

        /// <summary>
        /// 下一次请求（所有的资源地址上）
        /// </summary>
        protected void Next()
        {
            if (Post())
            {
                IsFinished = true;
                IsSuccess = true;
            }
            else
            {
                NextRequest();
                if (_HostIndex >= (UrlRootList.Count - 1) && _CurrentStep <= 0)
                {
                    PostErrorMsg = "最大重试次数已经用尽";
                    IsSuccess = false;
                    IsFinished = true;
                    return;
                }
                if (_CurrentStep <= 0)
                {
                    NextHost();
                }
                Next();
            }
        }


        /// <summary>
        /// 开始向api地址发送信息
        /// </summary>
        public void Send()
        {
            NextHost();

            Next();
        }

        #endregion

        #region 异步请求

        //protected async Task<bool> PostAsync()
        //{
        //    HttpWebResponse resp = null;
        //    Stream resStream = null;
        //    try
        //    {
        //        resp = await req.GetResponseAsync() as HttpWebResponse;
        //        resStream = resp.GetResponseStream();
        //        //获取响应内容  
        //        using (StreamReader reader = new StreamReader(resStream, Encoding.UTF8))
        //        {
        //            ResponseText = reader.ReadToEnd();
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //    finally
        //    {
        //        if (resp != null)
        //        {
        //            resp.Close();
        //        }
        //        if (resStream != null)
        //        {
        //            resStream.Close();
        //        }
        //        if (req != null)
        //        {
        //            //如果不能正确断开Reqeust，就不能释放相关的资源
        //            //参考：
        //            //https://blog.csdn.net/weixin_33828101/article/details/85588689
        //            //https://www.cnblogs.com/Fooo/archive/2008/10/31/1323400.html
        //            //https://blog.csdn.net/shenmafuyunnan/article/details/48542365
        //            req.Abort();
        //        }
        //    }
        //}

        ///// <summary>
        ///// 下一次请求（所有的资源地址上）
        ///// </summary>
        //protected async void NextAsync()
        //{
        //    bool Ready = await PostAsync();
        //    if (Ready)
        //    {
        //        IsFinished = true;
        //        IsSuccess = true;
        //    }
        //    else
        //    {
        //        NextRequest();
        //        if (_HostIndex == UrlRootList.Count && _CurrentStep <= 0)
        //        {
        //            PostErrorMsg += "最大重试次数已经用尽";
        //            IsSuccess = false;
        //            IsFinished = true;
        //            return;
        //        }
        //        if (_CurrentStep <= 0)
        //        {
        //            NextHost();
        //        }
        //        NextAsync();
        //    }
        //}


        ///// <summary>
        ///// 开始向api地址发送信息
        ///// </summary>
        //public async Task SendAsync()
        //{
        //    NextHost();

        //    NextAsync();
        //}


        #endregion



        /// <summary>
        /// 修改系统的默认最大网络连接，防止连接api或者其它的WebRequest太多造成阻塞
        /// </summary>
        /// <remarks>
        /// ▲▲▲注意!!!
        /// 这个选项是为了解决连接太多的问题，
        /// 如果是资源没有正确释放的问题请给Reqeust.Abort()
        /// 具体请见Post()PostAsync()方法，以及NextHost()
        /// 参考：
        /// https://blog.csdn.net/silence1214/article/details/6649937
        /// https://blog.csdn.net/shenmafuyunnan/article/details/48542365
        /// </remarks>
        /// <param name="max">指定新的连接上限</param>
        public static void SetLimit(int max = 50)
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = max;
        }

        /// <summary>
        /// 创建代理对象
        /// </summary>
        /// <param name="str_IP">HTTP代理地址(默认是80端口)</param>
        /// <param name="str_UserName">用户名(匿名则用户名和密码为空)</param>
        /// <param name="str_Pwd">密码</param>
        /// <returns></returns>
        private WebProxy CreatProxy(string str_IP, string str_UserName, string str_Pwd)
        {
            str_IP = str_IP.ToUpper().IndexOf("HTTP://") > -1 ? str_IP : "http://" + str_IP;
            WebProxy myProxy = new WebProxy();
            myProxy.Address = new Uri(str_IP);
            myProxy.Credentials = new NetworkCredential(str_UserName, str_Pwd);
            return myProxy;
        }
    }
}
