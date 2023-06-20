using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using XP.Web.ControllerBase;

namespace XP.Web.Permission
{

    /// <summary>
    /// 过滤器工具
    /// </summary>
    public static class FiltersUtil
    {


        public static void LogException(ExceptionContext filterContext, Exception ex = null)
        {
            if (null == ex)
            {
                ex = filterContext.Exception;
            }
            ControllerContext ctrlContext = filterContext as ControllerContext;
            LogException(ctrlContext, ex);
        }

        public static void LogException(ActionExecutedContext filterContext, Exception ex = null)
        {
            if (null == ex)
            {
                ex = filterContext.Exception;
            }
            ControllerContext ctrlContext = filterContext as ControllerContext;
            LogException(ctrlContext, ex);
        }

        public static void LogException(ControllerContext filterContext, Exception ex = null)
        {
            //string CurrentControllerName = (string)filterContext.RouteData.Values["controller"];
            //string CurrentActionName = (string)filterContext.RouteData.Values["action"];
            ////Exception CurrentException = filterContext.Exception;
            //string AreaName = String.Empty;
            //var RouteArea = filterContext.RouteData.DataTokens["area"];
            //if (null != RouteArea) { AreaName = RouteArea.ToString(); }

            var MvcNames = XP.Web.ControllerBase.MvcUtil.BuildMvcName(filterContext.RouteData);

            LogException(MvcNames.AreaName, MvcNames.ControllerName, MvcNames.RealActionName, ex);
        }



        private static void LogException(string areaName, string controller, string action, Exception ex)
        {
            MvcLoger.ErrorMvc(ex, areaName, controller, action);


            #region 改用专门的类来写日志

            //log4net.ILog log = log4net.LogManager.GetLogger("ExceptionLog");

            //string ExceptionMsg;
            //if (null == ex.InnerException)
            //{
            //    string fm = "异常的来源：{0}，异常的信息：{1}。";

            //    ExceptionMsg = String.Format(fm,ex.Source,ex.Message);
            //}
            //else
            //{
            //    string fm = "异常的来源：{0}，异常的信息：{1}。内部异常：{2}";
            //    ExceptionMsg = String.Format(fm, ex.Source, ex.Message,ex.InnerException.Message);
            //}
            //string FormatStr = "控制器[{0}]的动作[{1}]在运行的时候出错。异常信息如下：{2}";
            //string Msg;
            //Msg = String.Format(FormatStr, controller, action, ExceptionMsg);
            //log.Error(Msg);

            #endregion
        }

    }
}
