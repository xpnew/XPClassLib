using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using XP.Comm.Msgs;

namespace XP.Web.Permission
{
    /// <summary>
    /// JSON异常过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class JsonExceptionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            base.OnActionExecuted(filterContext);
            if (!CheckAjaxView(filterContext))
            {
                return;
            }

            if (null != filterContext.Exception)
            {
                Exception CurrentException = filterContext.Exception;
                var AjaxView = AjaxException(CurrentException);
                if (null != AjaxView)
                {

                    FiltersUtil.LogException(filterContext);


                    //SA.WebUtil.Loger.Error(CurrentException, CurrentControllerName, CurrentActionName);
                    filterContext.HttpContext.Response.Clear();
                    //filterContext.HttpContext.Response.StatusCode = 500;
                    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                    filterContext.Result = AjaxView;
                    filterContext.Exception = null;
                    filterContext.ExceptionHandled = true;


                }

            }
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




        /// <summary>
        /// [CheckAjaxView标记]检查是否为AJAX动作
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        public bool CheckAjaxView(ActionExecutedContext filterContext)
        {
            //在这里允许一种情况,如果已经登陆,那么不对于标识了LoginAllowView的方法就不需要验证了
            object[] attrs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(JsonActionAttribute), true);
            //是否是LoginAllowView
            var ViewMethod = attrs.Length == 1;
            return ViewMethod;
        }



    }
}
