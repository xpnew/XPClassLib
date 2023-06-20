using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using XP.Comm;

namespace XP.Web.ControllerBase
{
    public class BassVsMsg : BassWeb
    {


        public Comm.WebMsg Msg { get; set; }

        public BassVsMsg()
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
            //Msg.Exp = ex;

            Msg.ExcptionString = Util.Json.JsonHelper.Serialize(ex);
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
        public JsonResult SendJsonMsg(CommMsg Msg)
        {
            var Json = new JsonResult();
            Json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;


            if (!x.EnableDebug)
            {
                Msg.Logs = null;
            }


            Json.Data = Msg;


            return Json;

        }

        public JsonResult SendJsonMsg()
        {
            return SendJsonMsg(Msg);
        }

        /// <summary>
        /// 最简单的成功返回
        /// </summary>
        /// <remarks>
        /// 早期开发的时候觉得有这种偷懒的方法会有一些隐患，但是时间久了发现，需要在一个新的Action里面给出一个简单的返回。
        /// </remarks>
        /// <returns></returns>
        public JsonResult SendJsonOK()
        {
            return SendJsonOK("OK",false);
        }


        /// <summary>
        /// 向页面提供成功的json消息，如果需要是发送一个全新的消息，请将参数IsNewMsg设置为true
        /// </summary>
        /// <param name="okMsg"></param>
        /// <param name="IsNewMsg">强制使用新的消息对象，这对隐藏日志或者其它细节很有用。</param>
        /// <returns></returns>
        public JsonResult SendJsonOK(string okMsg, bool IsNewMsg = false)
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
        public JsonResult SendJsonError(string errorMsg, int statusCode = -1)
        {
            WebMsg NewErrorMsg = new WebMsg() { Title = errorMsg,  StatusCode = statusCode };
            NewErrorMsg.Logs = this.Msg.Logs;
            return SendJsonMsg(NewErrorMsg);
        }

        public JsonResult SendJsonError(string errorMsg, Exception ex)
        {
            MsgErr(errorMsg, ex);
            return SendJsonMsg(Msg);
        }

    }
}
