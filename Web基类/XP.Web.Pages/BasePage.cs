using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
