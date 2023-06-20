using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using XP.Comm.Msgs;

namespace XP.Web.Permission.Filters
{
    /// <summary>
    /// Ajax动作上的数据异常处理过滤器
    /// </summary>
    /// <remarks>
    /// 注意：：：原计划是使用小于3000的Order可以优先于ExceptionFilterAttribute执行。
    /// 但是控制器上的过滤器会优先于动作上面的过滤器，所以这个类的方式，已经作废了。
    /// 这个过滤器提供了关于Ajax动作的数据异常处理。
    /// 需要注意的是，通用异常处理过滤器ExceptionFilterAttribute的Order一般是3000或者5000，
    /// 所以应用这个过滤器的场合建议使用小于3000的Order
    /// 
    /// 
    /// </remarks>
    public class JsonDbErrorAttribute : HandleErrorAttribute
    {

        public override void OnException(ExceptionContext filterContext)
        {
            //filterContext.ExceptionHandled = true;
            Exception CurrentException = filterContext.Exception;
            var AjaxView = AjaxException(CurrentException);
            if (null != AjaxView)
            {
                filterContext.Result = AjaxView;
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
                //filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                string CurrentControllerName = (string)filterContext.RouteData.Values["controller"];
                string CurrentActionName = (string)filterContext.RouteData.Values["action"];

                //MvcLoger.ErrorMvc(CurrentException, CurrentControllerName, CurrentActionName);

            }
            FiltersUtil.LogException(filterContext);



        }
        /// <summary>
        /// 检查并且生成AJAX异常
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public JsonResult AjaxException(Exception ex)
        {

            var res = new JsonResult();
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            MsgResult result = new MsgResult()
            {
                Name = "JsonMsg",
                Body = "登录超时或者没有登录。",
                StatusCode = -60103251//默认是0
            };
            res.Data = result;

            //TODO 处理好异常的具体信息，准备交给前端来进行

            //if (ex.GetType() == typeof(CustomException))
            //{
            //    CustomException exception = ex as CustomException;
            //    string ErrorCode = SA.WebUtil.Common.NumberFormat.FormatHex(exception.ErrorCode, "X", "0x{0}");
            //    result.GlobalErrorMessage = ControllerUtility.TransferCode(ErrorCode); //数据服务异常
            //    result.StatusCode = -1;
            //    return res;
            //}
            //if (ex.GetType() == typeof(System.ServiceModel.FaultException<SA.DataObjects.FaultData>))
            //{
            //    System.ServiceModel.FaultException<SA.DataObjects.FaultData> exception = ex as System.ServiceModel.FaultException<SA.DataObjects.FaultData>;
            //    string ErrorCode = SA.WebUtil.Common.NumberFormat.FormatHex(exception.Detail.ErrorCode, "X", "0x{0}");
            //    result.GlobalErrorMessage = ControllerUtility.TransferCode(ErrorCode); //数据服务异常
            //    result.StatusCode = -1;
            //    return res;
            //}


            return res;

        }

    }
}
