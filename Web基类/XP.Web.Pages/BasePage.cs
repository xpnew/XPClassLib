using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XP.Comm;
using XP.Comm.Msgs;
using XP.Util.Json;
using XP.Util.Web;
using XP.Util.WebUtils;

namespace XP.Web.Pages
{
    public class BasePage : System.Web.UI.Page
    {

        /// <summary>
        /// 总返回结果（返回的是信息不是html）
        /// </summary>
        public string ResultText { get; set; }

        /// <summary>
        /// 各种错误信息列表
        /// </summary>
        public List<string> ErrorLines { get; set; } = new List<string>();


        public bool HasError { get; set; } = false;


        protected XPMsg _PageMsg;


        public XPMsg PageMsg
        {
            get
            {
                if (null == _PageMsg)
                    _PageMsg = new XPMsg() { StatusCode = 1 };
                return _PageMsg;
            }
            set
            {
                _PageMsg = value;
            }
        }

        /// <summary>
        /// Action 动作的名称，从动网论坛学来的模仿 MVC
        /// </summary>
        public string ActionName { get; set; }

            
        protected string BuildLI(List<string> lines)
        {
            string result = "<ul>";

            foreach (var item in lines)
            {

                result += "<li>" + item + "</li>";


            }

            result += "</ul>";

            return result;

        }



        protected virtual void Page_PreLoad(object sender, EventArgs e)
        {


            string PageName = Request.Url.LocalPath;

            string ActName = Request["act"];


            if (String.IsNullOrEmpty(ActName))
            {
                ActName = Request["action"];
            }
            this.ActionName = ActName;

            if (String.IsNullOrEmpty(ActName))
            {
                PageMsg.Name = PageName;
            }
            else
            {
                PageMsg.Name = PageName + "." + ActName;
            }


        }

        protected void SayError(string msg)
        {

            XP.Util.WebUtils.PageUtil.xpnewAlert(msg);
        }

        protected void SayError(string msg, string url)
        {
            XP.Util.WebUtils.PageUtil.xpnewAlert(msg, url);

        }


        protected void Say(string msg)
        {

            XP.Util.WebUtils.PageUtil.xpnewAlert(msg);
        }
        /// <summary>
        /// 弹出提示并且关闭窗口
        /// </summary>
        /// <param PropertyTypeName="msg"></param>
        protected void Alert00Close(string msg)
        {
            XP.Util.WebUtils.PageUtil.xpnewClose(msg);

        }


        protected void SayJson(string title, string body)
        {
            PageMsg.StatusCode = 1;
            PageMsg.Title = title;
            PageMsg.Body = body;
            SayJson();

        }
        protected void SayJson(string title)
        {
            PageMsg.StatusCode = 1;
            PageMsg.Status = true;
            PageMsg.Title = title;
            SayJson();
        }
        protected void SayJsonOk(string title)
        {
            PageMsg.StatusCode = 1;
            PageMsg.Status = true;
            PageMsg.Title = title;
            SayJson();
        }

        protected void SayJsonOk(string title, int code)
        {
            PageMsg.StatusCode = 1;
            PageMsg.Title = title;
            var msg = MsgBase.CloneMsg<TruantDataMsg>(PageMsg);
            msg.DataInfo = code;

            SayJson(msg);
        }
        protected void SayJson()
        {
            PageUtil.Json(PageMsg.ToString());
        }

        protected void SayJson(CommMsg msg)
        {

            PageUtil.Json(JsonHelper.ToJson(msg));

        }
        protected void JsonErr(string err)
        {
            var Msg = new XPMsg();
            Msg.StatusCode = -1;
            Msg.Title = err;
            PageUtil.Json(Msg.ToString());
        }
    }
}
