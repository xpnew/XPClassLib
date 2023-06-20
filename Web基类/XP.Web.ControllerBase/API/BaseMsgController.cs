using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using System.Web.Http;
using System.Web.Http.Results;
using XP.Comm;

namespace XP.Web.ControllerBase.API
{
    public class BaseMsgController : BaseController
    {

        public Comm.WebMsg Msg { get; set; }

        public BaseMsgController()
        {
            Msg = new Comm.WebMsg();
            Msg.Name = this.GetType().FullName;
            Msg.Status = true;
        }


        public void MsgLog(string log)
        {
            Msg.AddLog(log);
        }


        public void MsgOk(string msg)
        {
            Msg.StatusCode = 1;
            Msg.Title = msg;
            Msg.AddLog("操作完成：" + msg);
            Msg.Status = true;
        }

        public void MsgErr(string error)
        {
            Msg.StatusCode = -1;
            Msg.Title = error;
            Msg.AddLog("出现错误：" + error);
            Msg.Status = false;
        }

        public void MsgErr(string error, Exception ex)
        {
            Msg.StatusCode = -1;
            Msg.Title = error;
            Msg.AddLog("出现错误：" + error);
            Msg.Status = false;
            Msg.Exp = ex;
        }



        /// <summary>
        /// 向客户端发送JSON消息。
        /// </summary>
        /// <remarks>
        /// 封装的目的有二：
        /// 1、重用
        /// 2、保护消息安全，仅在调试状态下才允许把log发送到客户端
        /// </remarks>
        /// <param name="Msg"></param>
        /// <returns></returns>
        protected JsonResult<CommMsg> SendJsonMsg(CommMsg Msg)
        {


            if (!x.EnableDebug)
            {
                Msg.Logs = null;
            }




            return Json(Msg);

        }

        protected JsonResult<CommMsg> SendJsonMsg()
        {
            return SendJsonMsg(Msg);
        }


        /// <summary>
        /// 向页面提供成功的json消息，如果需要是发送一个全新的消息，请将参数IsNewMsg设置为true
        /// </summary>
        /// <param name="okMsg"></param>
        /// <param name="IsNewMsg">强制使用新的消息对象，这对隐藏日志或者其它细节很有用。</param>
        /// <returns></returns>
        public JsonResult<CommMsg> SendJsonOK(string okMsg, bool IsNewMsg = false)
        {
            if (IsNewMsg)
            {
                WebMsg NewOkMsg = new WebMsg() { Title = okMsg, StatusCode = 1 };
                return SendJsonMsg(NewOkMsg);
            }
            Msg.Title = okMsg;
            Msg.StatusCode = 1;
            Msg.UpdateTime = DateTime.Now;
            return SendJsonMsg();
        }
        public JsonResult<CommMsg> SendJsonError(string errorMsg, int statusCode = -1)
        {
            WebMsg NewErrorMsg = new WebMsg() { Title = errorMsg, StatusCode = statusCode };
            NewErrorMsg.Logs = this.Msg.Logs;
            return SendJsonMsg(NewErrorMsg);
        }
        public JsonResult<CommMsg> SendJsonError(string errorMsg, Exception ex)
        {
            MsgErr(errorMsg, ex);
            return SendJsonMsg(Msg);
        }
        #region 发送文本或者JS到客户端

        /// <summary>通过在控制器上返回一段HTML，可以代替View</summary>
        /// <remarks></remarks>
        /// <param name="html">输入的html，需要自带html、body标签</param>
        /// <returns>将html包装成ActionResult返回</returns>
        public IHttpActionResult SendHtml(string html, string type = null)
        {

            if (type == null)
            {
                //空的时候自动使用：
                //type = "text/plain";
            }
            //result.ContentType = "";
            //result.ContentEncoding = UTF8Encoding.UTF8;
            //result.Content = html;
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "OK");
            response.Content = new StringContent(html, Encoding.UTF8, type);

            //response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue
            //{
            //    MaxAge = TimeSpan.FromMinutes(10)
            //};
            ResponseMessageResult result = new ResponseMessageResult(response);
            //return response;
            return result;
        }


        /// <summary>通过在控制器上返回一段Json，可以代替View</summary>
        /// <remarks></remarks>
        /// <param name="html">输入的html，需要自带html、body标签</param>
        /// <returns>将html包装成ActionResult返回</returns>
        public IHttpActionResult SendJson(string json)
        {
            //ContentResult result = new ContentResult();
            //result.ContentType = "application/json";
            //result.ContentEncoding = UTF8Encoding.UTF8;
            //result.Content = json;
            //return result;
            return SendHtml(json, "application/json");
        }

        /// <summary>通过在控制器上返回一段Json，可以代替View</summary>
        /// <remarks></remarks>
        /// <param name="html">输入的html，需要自带html、body标签</param>
        /// <returns>将html包装成ActionResult返回</returns>
        public IHttpActionResult SendJsonResult(string json)
        {
            return this.SendJson(json);
        }
        /// <summary>返回一个HTML警告，然后跳转到前一页。</summary>
        /// <param name="Result">警告内容</param>
        /// <returns></returns>
        public IHttpActionResult SendAlert(string str)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script language=\"JavaScript\">\n");
            sb.Append("alert('");
            sb.Append(str);
            sb.Append("');\n");
            sb.Append("history.go(-1);\n");
            sb.Append("</script>\n");
            return SendHtml(sb.ToString());
        }
        /// <summary>返回一个HTML警告，然后后退到前一页或者刷新前一页</summary>
        /// <param name="Result">警告内容</param>
        /// <param name="Refresh">是否刷新</param>
        /// <returns></returns>
        public IHttpActionResult SendAlert(string str, bool Refresh)
        {
            if (Refresh)
            {
                string url = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
                return SendAlert(str, url);
            }
            else
            {
                return SendAlert(str);
            }

        }
        /// <summary>返回一个HTML警告，然后跳转到某个地址</summary>
        /// <param name="Result">警告内容</param>
        /// <param name="url">将要跳转的地址</param>
        /// <returns></returns>
        public IHttpActionResult SendAlert(string str, string url)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script language=\"JavaScript\">\n");
            sb.Append("alert('");
            sb.Append(str);
            sb.Append("');\n");
            sb.Append("location.replace('");
            sb.Append(url);
            sb.Append("');\n");
            sb.Append("</script>\n");
            return SendHtml(sb.ToString());
        }
        /// <summary>返回一个HTML警告，然后跳转到前一页。</summary>
        /// <param name="Result">警告内容</param>
        /// <returns></returns>
        public IHttpActionResult SendAlertAndClose(string str)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script language=\"JavaScript\">\n");
            sb.Append("alert('");
            sb.Append(str);
            sb.Append("');\n");
            sb.Append("window.close();\n");
            sb.Append("</script>\n");
            return SendHtml(sb.ToString());
        }

        #endregion



    }
}
