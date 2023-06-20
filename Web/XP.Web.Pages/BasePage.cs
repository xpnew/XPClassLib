using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Web.Pages
{
    public class BasePage : System.Web.UI.Page
    {
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
