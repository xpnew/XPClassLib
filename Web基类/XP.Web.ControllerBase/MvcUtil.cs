using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Routing;
using XP.Comm.Web;

namespace XP.Web.ControllerBase
{
    /// <summary>
    /// MVC工具类
    /// </summary>
    public class MvcUtil
    {


        public static MvcNameGroup BuildMvcName(RouteData route)
        {
            string CurrentControllerName = (string)route.Values["controller"];
            string CurrentActionName = (string)route.Values["action"];
            //Exception CurrentException = filterContext.Exception;
            string AreaName = String.Empty;
            var RouteArea = route.DataTokens["area"];
            if (null != RouteArea) { AreaName = RouteArea.ToString(); }

            MvcNameGroup Result = new MvcNameGroup()
            {
                AreaName = AreaName,
                ControllerName = CurrentControllerName,
                ActionName = CurrentActionName,
                RealActionName = CurrentActionName,
                AreaName2Lower = AreaName.ToLower(),
                ControllerName2Lower = CurrentControllerName.ToLower(),
                ActionName2Lower = CurrentActionName.ToLower(),
            };
            return Result;
        }



        public static System.Web.HttpRequestBase GetRequest(HttpActionContext content)
        {
            HttpContextBase context = (HttpContextBase)content.Request.Properties["MS_HttpContext"];//获取传统context
            //var c = content.RequestContext as System.Web.Http.WebHost.WebHostHttpRequestContext;
            if (null == context)
            {
                return null;
            }

            return context.Request;

        }

    }
}
