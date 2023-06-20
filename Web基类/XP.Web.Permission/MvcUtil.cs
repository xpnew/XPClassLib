using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using XP.Comm.Web;
using XP.Web.Permission;

namespace XP.Web.Permission
{
    public static class MvcUtil
    {

        public static MvcNameGroup BuildMvcName( ActionExecutingContext filterContext, bool enableDepend, bool enableAlisa)
        {
            var route = filterContext.RouteData;
            string CurrentControllerName = (string)route.Values["controller"];
            string CurrentActionName = (string)route.Values["action"];
            //Exception CurrentException = filterContext.Exception;
            string AreaName = String.Empty;
            var RouteArea = route.DataTokens["area"];
            if (null != RouteArea) { AreaName = RouteArea.ToString(); }

            if (enableDepend)
            {
                string dep = GetDependName(filterContext.ActionDescriptor);
                if (null != dep)
                {
                    CurrentActionName = dep;
                }
            }
            if (enableAlisa)
            {
                string aliasName = GetAliasName(CurrentActionName);
                if (null != aliasName)
                {
                    CurrentActionName = aliasName;
                }
            }
            MvcNameGroup Result = new MvcNameGroup()
            {
                AreaName = AreaName,
                ControllerName = CurrentControllerName,
                ActionName = CurrentActionName,
                AreaName2Lower = AreaName.ToLower(),
                ControllerName2Lower = CurrentControllerName.ToLower(),
                ActionName2Lower = CurrentActionName.ToLower(),
            };
            return Result;
        }
        public static MvcNameGroup GetMvcName(ActionExecutedContext filterContext, bool enableDepend, bool enableAlisa)
        {
            var route = filterContext.RouteData;
            var actDes = filterContext.ActionDescriptor;
            return GetMvcName(route, actDes, enableDepend, enableAlisa);
        }

        public static MvcNameGroup GetMvcName( RouteData route, ActionDescriptor actDes,
            bool enableDepend, bool enableAlisa)
        {
            MvcNameGroup Result = XP.Web.ControllerBase.MvcUtil.BuildMvcName(route);

            if (enableDepend)
            {
                string dep = GetDependName(actDes);
                if (null != dep)
                {
                    Result.ActionName = dep;
                    Result.ActionName2Lower = dep.ToLower();
                }
            }
            if (enableAlisa)
            {
                string aliasName = GetAliasName(Result.ActionName);
                if (null != aliasName)
                {
                    Result.ActionName = aliasName;
                    Result.ActionName2Lower = aliasName.ToLower();
                }
            }

            return Result;
        }

        public static string GetDependName( ActionDescriptor actionDescriptor)
        {
            var attrs = actionDescriptor.GetCustomAttributes(typeof(ActionDependFilterAttribute), true);
            if (attrs.Length > 0)
            {
                ActionDependFilterAttribute ad = attrs[0] as ActionDependFilterAttribute;

                return ad.ParentActionName;
            }
            return null;
        }

        public static string GetAliasName( string currentName)
        {
            if ("Query".ToLower() == currentName)
            {
                return "index";
            }
            return currentName;
        }
    }
}
