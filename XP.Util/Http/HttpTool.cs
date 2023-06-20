using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace XP.Util.Http
{
    /// <summary>
    /// 简单的http工具
    /// </summary>
    public static class HttpTool
    {


        #region  Post Josn，发送和返回的都是json

        /// <summary>
        /// POST方式的HTTP请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="sendJson">数据</param>
        /// <param name="responseData">输出数据</param>
        /// <returns></returns>
        public static bool PostJson(string url, string sendJson, ref string returnJson)
        {
            HttpWebRequest request = null;
            try
            {
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                        new RemoteCertificateValidationCallback(checkValidationResult);
                }
                request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json; charset=UTF-8";   //application/octet-stream


                if (!String.IsNullOrEmpty(sendJson))
                {
                    using (Stream postStream = request.GetRequestStream())
                    {
                        byte[] byteArray = Encoding.GetEncoding("UTF-8").GetBytes(sendJson);
                        postStream.Write(byteArray, 0, byteArray.Length);
                    }
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        returnJson = reader.ReadToEnd();
                        reader.Close();
                        reader.Dispose();
                    }
                    response.Close();
                    response.Dispose();
                }
                request.Abort();
                return true;
            }
            catch (WebException ex)
            {
                if(ex.Status == WebExceptionStatus.ConnectFailure)
                {
                    x.Say("数据发送的时候出现了异常： 连接失败" );

                    return false;
                }

                string ErrorPage;
                //获取响应内容  
                using (HttpWebResponse response = (HttpWebResponse)ex.Response)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        ErrorPage = reader.ReadToEnd();
                        reader.Close();
                        reader.Dispose();
                    }
                    response.Close();
                    response.Dispose();
                }
                x.Say("数据发送的时候出现了异常： " + ErrorPage);
                XP.Loger.Error("数据发送的时候出现了异常： " + ErrorPage);
                return false;
            }
            catch (Exception ex)
            {
                XP.Loger.Error("网络请求失败，地址【" + url + "】，错误详细：" + ex);
                if (request != null)
                {
                    request.Abort();
                }
                return false;
            }
        }

        #endregion


        #region POST方式的HTTP请求,带Header
        /// <summary>
        /// POST方式的HTTP请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">数据</param>
        /// <param name="responseData">输出数据</param>
        /// <returns></returns>
        public static bool PostHttpResponse(string url, string data, ref string responseData, Dictionary<string, string> headerDict = null)
        {
            HttpWebRequest request = null;
            try
            {
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                        new RemoteCertificateValidationCallback(checkValidationResult);
                }
                request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
             
                if (null != headerDict && 0 != headerDict.Count)
                {
                    if (headerDict.ContainsKey("host"))
                    {
                        request.Host = headerDict["host"];
                    }
                    if (headerDict.ContainsKey("content-type"))
                    {
                        request.ContentType = headerDict["content-type"];
                    }
                    foreach (string key in headerDict.Keys.Except(new List<string>() { "host", "content-type" }))
                    {
                        request.Headers.Set(key, headerDict[key]);
                    }
                }
                using (Stream postStream = request.GetRequestStream())
                {
                    byte[] byteArray = Encoding.GetEncoding("UTF-8").GetBytes(data);
                    postStream.Write(byteArray, 0, byteArray.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseData = reader.ReadToEnd();
                        reader.Close();
                        reader.Dispose();
                    }
                    response.Close();
                    response.Dispose();
                }
                request.Abort();
                return true;
            }
            catch (Exception ex)
            {
                if (request != null)
                {
                    request.Abort();
                }
                return false;
            }
        }
        #endregion

        #region POST方式的HTTP请求
        /// <summary>
        /// POST方式的HTTP请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">数据</param>
        /// <param name="certPath">证书地址</param>
        /// <param name="certPwd">证书密码</param>
        /// <returns></returns>
        public static bool PostHttpResponse(string url, string data, string certPath, string certPwd, ref string responseData)
        {
            HttpWebRequest request = null;
            try
            {
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                        new RemoteCertificateValidationCallback(checkValidationResult);
                }
                request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                X509Certificate2 cert = new X509Certificate2(certPath, certPwd, X509KeyStorageFlags.MachineKeySet);
                request.ClientCertificates.Add(cert);
                using (Stream postStream = request.GetRequestStream())
                {
                    byte[] byteArray = Encoding.GetEncoding("UTF-8").GetBytes(data);
                    postStream.Write(byteArray, 0, byteArray.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseData = reader.ReadToEnd();
                        reader.Close();
                        reader.Dispose();
                    }
                    response.Close();
                    response.Dispose();
                }
                request.Abort();
                return true;
            }
            catch (Exception)
            {
                if (request != null)
                {
                    request.Abort();
                }
                return false;
            }
        }
        #endregion


        #region POST方式的HTTP请求(带cookies)
        /// <summary>
        /// POST方式的HTTP请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">数据</param>
        /// <param name="responseData">输出数据</param>
        /// <returns></returns>
        public static bool PostHttpResponse(string url, string data, ref string responseData, ref CookieContainer cookie)
        {
            HttpWebRequest request = null;
            try
            {
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(checkValidationResult);
                }
                request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.CookieContainer = cookie;


                using (Stream postStream = request.GetRequestStream())
                {
                    byte[] byteArray = Encoding.GetEncoding("UTF-8").GetBytes(data);
                    postStream.Write(byteArray, 0, byteArray.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    cookie.Add(response.Cookies);
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseData = reader.ReadToEnd();
                        reader.Close();
                        reader.Dispose();
                    }
                    response.Close();
                    response.Dispose();
                }
                request.Abort();
                return true;
            }
            catch (Exception ex)
            {
                if (request != null)
                {
                    request.Abort();
                }
                return false;
            }
        }
        #endregion



        #region 获取cookies

        public static CookieContainer GetCookies(string url)
        {
            string responseData = null;
            HttpWebRequest request = null;
            CookieContainer cookie = new CookieContainer();
            try
            {
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(checkValidationResult);
                }
                request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    cookie.Add(response.Cookies);

                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseData = sr.ReadToEnd();
                        sr.Close();
                        sr.Dispose();
                    }
                    response.Close();
                    response.Dispose();
                }
                return cookie;
            }
            catch (Exception ex)
            {
                if (request != null)
                {
                    request.Abort();
                }
                return cookie;
            }

        }

        #endregion


        #region GET方式的HTTP请求
        /// <summary>
        /// GET方式的HTTP请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public static bool GetHttpResponse(string url, ref string responseData,out string stat)
        {
            HttpWebRequest request = null;
            stat = String.Empty;
            try
            {
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(checkValidationResult);
                }
                request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseData = sr.ReadToEnd();
                        sr.Close();
                        sr.Dispose();
                    }
                    stat = response.StatusCode.ToString();
                    response.Close();
                    response.Dispose();
                }
               
                return true;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ConnectFailure)
                {
                    x.Say("数据发送的时候出现了异常： 连接失败");

                    return false;
                }

                try
                {
                    string ErrorPage = String.Empty;
                    //获取响应内容  
                    using (HttpWebResponse response = (HttpWebResponse)ex.Response)
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            ErrorPage = reader.ReadToEnd();
                            reader.Close();
                            reader.Dispose();
                        }
                        stat = response.StatusCode.ToString();
                        response.Close();
                        response.Dispose();
                    }
                    x.Say("数据发送的时候出现了异常： " + ErrorPage);
                    XP.Loger.Error("数据发送的时候出现了异常： " + ErrorPage);
                }
                catch 
                {
                    x.Say("数据发送的时候出现了异常： " + ex.Message);
                    XP.Loger.Error("数据发送的时候出现了异常： " + ex.Message);

                }
                return false;
            }
            catch (Exception ex)
            {
                if (request != null)
                {
                    request.Abort();
                }
                return false;
            }
        }
        #endregion

        #region GET方式的HTTP请求,带Header
        /// <summary>
        /// GET方式的HTTP请求,带Header
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="responseData">输出数据</param>
        /// <param name="stat">状态</param>
        /// <param name="headerDict"></param>
        /// <returns></returns>
        public static bool GetHttpResponse(string url, out string responseData, out string stat, Dictionary<string, string> headerDict = null)
        {
            HttpWebRequest request = null;
            stat = String.Empty;
            responseData = String.Empty;
            try
            {
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(checkValidationResult);
                }
                request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";


                List<string> ExictKeys = new List<string>()
                {
                     "host", "Host",
                     "Content-Type", "content-type", 
                    "User-Agent","Referer","Accept","Connection"
                };

                if (null != headerDict && 0 != headerDict.Count)
                {
                    if (headerDict.ContainsKey("host"))
                    {
                        request.Host = headerDict["host"];
                    }
                    if (headerDict.ContainsKey("Host"))
                    {
                        request.Host = headerDict["Host"];
                    }
                    if (headerDict.ContainsKey("content-type"))
                    {
                        request.ContentType = headerDict["content-type"];
                    }
                    if (headerDict.ContainsKey("Content-Type"))
                    {
                        request.ContentType = headerDict["Content-Type"];
                    }
                    if (headerDict.ContainsKey("Referer"))
                    {
                        request.Referer = headerDict["Referer"];
                    }
                    if (headerDict.ContainsKey("Accept"))
                    {
                        request.Accept = headerDict["Accept"];
                    }

                    if (headerDict.ContainsKey("Connection"))
                    {
                        //request.Connection = headerDict["Connection"];
                        if("keep-alive" == headerDict["Connection"])
                        {
                            request.KeepAlive = true;
                        }
                    }


                    if (headerDict.ContainsKey("User-Agent"))
                    {
                        request.UserAgent = headerDict["User-Agent"];
                    }
                    foreach (string key in headerDict.Keys.Except(ExictKeys))
                    {
                        request.Headers.Set(key, headerDict[key]);
                    }
                }

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    var Return2 = GetStream4Response(response);
                    responseData = Return2.Html;
                    
                    stat = response.StatusCode.ToString();
                    response.Close();
                    response.Dispose();
                    return Return2.IsReady;
                }

                //return true;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ConnectFailure)
                {
                    x.Say("数据发送的时候出现了异常： 连接失败");

                    return false;
                }

                try
                {
                    string ErrorPage = String.Empty;
                    //获取响应内容  
                    using (HttpWebResponse response = (HttpWebResponse)ex.Response)
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            ErrorPage = reader.ReadToEnd();
                            reader.Close();
                            reader.Dispose();
                        }
                        stat = response.StatusCode.ToString();
                        response.Close();
                        response.Dispose();
                    }
                    x.Say("数据发送的时候出现了异常： " + ErrorPage);
                    XP.Loger.Error("数据发送的时候出现了异常： " + ErrorPage);
                }
                catch
                {
                    x.Say("数据发送的时候出现了异常： " + ex.Message);
                    XP.Loger.Error("数据发送的时候出现了异常： " + ex.Message);

                }
                return false;
            }
            catch (Exception ex)
            {
                x.Say("数据发送的时候出现了异常： " + ex.Message);

                if (request != null)
                {
                    request.Abort();
                }
                return false;
            }
        }
        #endregion


        #region 通过字节流读取返回文本，支持Gzip


        public static (bool IsReady , string Html) GetStream4Response(HttpWebResponse response)
        {
            (bool IsReady, string Html) Result = ( IsReady : false, Html:String.Empty);
            try
            {
                if ("gzip" == response.ContentEncoding)
                {
                    using(var ms = response.GetResponseStream())
                    {
                        using (GZipStream decompressedStream = new GZipStream(ms, CompressionMode.Decompress))
                        {
                            using (StreamReader reader = new StreamReader(decompressedStream, Encoding.GetEncoding(response.CharacterSet)))
                            {                              
                                Result.Html = reader.ReadToEnd();
                                Result.IsReady = true;
                            }
                        }
                    }
                }
                else
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        Result.Html = reader.ReadToEnd();
                        Result.IsReady = true;
                        reader.Close();
                        reader.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return Result;
        }




        #endregion







        /// <summary>
        /// 将model数据转换为表单数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string Model2Form(object model, bool enableSkipNullValue = false)
        {
            string json = Json.JsonHelper.ToJson(model);
            Dictionary<string, string> dict = Json.JsonHelper.GetDict(json, enableSkipNullValue);

            if (0 == dict.Count)
            {
                return null;
            }
            if (1 == dict.Count)
            {
                return dict.Keys.First() + "=" + dict[dict.Keys.First()];
            }
            //List<string> Nodes = new List<string>();
            StringBuilder Sb = new StringBuilder();
            foreach (var k in dict.Keys)
            {
                Sb.Append(k);
                Sb.Append("=");
                Sb.Append(dict[k]);
                Sb.Append("&");
            }

            string Result = Sb.ToString();

            Result = Result.Substring(0, Result.Length - 1);
            return Result;
        }



        private static bool checkValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }
    }
}
