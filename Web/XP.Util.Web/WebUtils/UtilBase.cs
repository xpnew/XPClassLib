using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;

namespace XP.Util.WebUtils
{
   public class UtilBase
   {
       #region 引用一些基础的对象
       protected static HttpRequest Request
        {
            get
            {
                return HttpContext.Current.Request;
            }
        }
       protected static HttpResponse Response { get { return HttpContext.Current.Response; } }
       protected static HttpServerUtility Server { get { return HttpContext.Current.Server; } }
       protected static System.Web.SessionState.HttpSessionState Session { get { return HttpContext.Current.Session; } }
       protected void ss()
       {
           HttpContext.Current.Session[""] = "";
       }
       #endregion
   }
}
