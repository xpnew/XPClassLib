using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using XP.Comm.Msgs;

namespace XP.Web.Permission
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ExceptionFilterAttribute : ActionFilterAttribute, IExceptionFilter
    {

        private const string _defaultView = "Error";
        private Type _exceptionType = typeof(Exception);
        private string _master;
        private readonly object _typeId = new object();
        private string _view;

        public virtual void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (!CheckContent(filterContext))
            {
                return;
            }
            string CurrentControllerName = (string)filterContext.RouteData.Values["controller"];
            string CurrentActionName = (string)filterContext.RouteData.Values["action"];
            Exception CurrentException = filterContext.Exception;
            string AreaName = String.Empty;
            var RouteArea = filterContext.RouteData.DataTokens["area"];
            if (null != RouteArea) { AreaName = RouteArea.ToString().ToLower(); }

            FiltersUtil.LogException(filterContext);

            //LogException(AreaName,CurrentControllerName, CurrentActionName, CurrentException);


            if (!CheckException(CurrentException))
            {
                return;
            }



            //AJAX异常交给AjaxDbErrorAttribute处理
            //var AjaxView = AjaxException(innerException);
            //if (null != AjaxView)
            //{
            //    filterContext.Result = AjaxView;
            //    filterContext.ExceptionHandled = true;
            //    filterContext.HttpContext.Response.Clear();
            //    //filterContext.HttpContext.Response.StatusCode = 500;
            //    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

            //}


            if (!filterContext.IsChildAction && (!filterContext.ExceptionHandled && filterContext.HttpContext.IsCustomErrorEnabled))
            {
                if ((new HttpException(null, CurrentException).GetHttpCode() == 500) && this.ExceptionType.IsInstanceOfType(CurrentException))
                {
                    HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, CurrentControllerName, CurrentActionName);
                    ViewResult result = GetView(CurrentException);
                    if (null == result)
                    {
                        result = new ViewResult
                        {
                            ViewName = this.View,
                            MasterName = this.Master,
                            ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                            TempData = filterContext.Controller.TempData
                        };
                    }
                    filterContext.Result = result;
                    filterContext.ExceptionHandled = true;
                    filterContext.HttpContext.Response.Clear();
                    //filterContext.HttpContext.Response.StatusCode = 500;
                    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                }
            }
            else if (!filterContext.IsChildAction && !filterContext.ExceptionHandled)
            {
                if ((new HttpException(null, CurrentException).GetHttpCode() == 500) && this.ExceptionType.IsInstanceOfType(CurrentException))
                {
                    HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, CurrentControllerName, CurrentActionName);
                    ViewResult result = GetView(CurrentException);
                    if (null == result)
                    {
                        result = new ViewResult
                        {
                            ViewName = this.View,
                            MasterName = this.Master,
                            ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                            TempData = filterContext.Controller.TempData
                        };
                    }
                    filterContext.Result = result;
                    filterContext.ExceptionHandled = true;
                    filterContext.HttpContext.Response.Clear();
                    //filterContext.HttpContext.Response.StatusCode = 500;
                    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
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


            return null;

        }

        /* 无法获取动作的方法和状态，无法判断否为AJAX动作
        /// <summary>
        /// 检查是否为AJAX动作
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        public bool CheckAjaxView(ExceptionContext filterContext)
        {
            if(filterContext is ActionExecutingContext)
            {
                //在这里允许一种情况,如果已经登陆,那么不对于标识了LoginAllowView的方法就不需要验证了
                object[] attrs = filterContext.ParentActionViewContext.ActionDescriptor.GetCustomAttributes(typeof(AjaxActionFilterAttribute), true);
                //是否是LoginAllowView
                var ViewMethod = attrs.Length == 1;
                return ViewMethod;


            }
            return false;
        }
         * */
        /// <summary>
        /// 检查异常上下文，尤其是判断已经被处理过的异常（AJAX异常）
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        public bool CheckContent(ExceptionContext filterContext)
        {
            if (null == filterContext)
                return false;

            if (filterContext.ExceptionHandled == true)
                return false;

            return true;
        }


        public virtual bool CheckException(Exception innerException)
        {

            return true;
        }
     

        protected ViewResult GetView(Exception ex)
        {
            ViewResult result;
            try
            {
                result = new ViewResult
                {
                    ViewName = "Alert",
                };

                //捕获自定义异常，可以得知具体错误代码，然后翻译成相应的提示
                //if (ex.GetType() == typeof(CustomException))
                //{
                //    CustomException exception = ex as CustomException;
                //    string ErrorCode = SA.WebUtil.Common.NumberFormat.FormatHex(exception.ErrorCode, "X", "0x{0}");
                //    result.ViewBag.AlertMsg = ControllerUtility.TransferCode(ErrorCode); //数据服务异常
                //    result.ViewBag.ErrorCode = -1;
                //}
                //if (ex.GetType() == typeof(System.ServiceModel.FaultException<SA.DataObjects.FaultData>))
                //{
                //    System.ServiceModel.FaultException<SA.DataObjects.FaultData> exception = ex as System.ServiceModel.FaultException<SA.DataObjects.FaultData>;
                //    string ErrorCode = SA.WebUtil.Common.NumberFormat.FormatHex(exception.Detail.ErrorCode, "X", "0x{0}");
                //    result.ViewBag.AlertMsg = ControllerUtility.TransferCode(ErrorCode); //数据服务异常
                //    result.ViewBag.ErrorCode = -1;
                //}
                //else if (ex.GetType() == typeof(System.IO.IOException))
                //{
                //    result.ViewBag.AlertMsg = ControllerUtility.Transfer("Public_Error_DataServiceException"); //数据服务异常
                //    result.ViewBag.ErrorCode = -60305110;
                //}
                //else if (ex.GetType() == typeof(System.ServiceModel.ServiceActivationException))
                //{
                //    result.ViewBag.AlertMsg = ControllerUtility.Transfer("Public_Error_MoniterServiceException"); //监控服务异常
                //    result.ViewBag.ErrorCode = -60305111;

                //}
                //else if (ex.GetType() == typeof(SA.WebUtil.Exceptions.ReportWcfUnknowException))
                //{
                //    result.ViewBag.AlertMsg = ControllerUtility.Transfer("Public_Error_ReportServiceException"); //报表服务异常
                //    result.ViewBag.ErrorCode = -60305112;

                //}
                //else
                //{
                //    //WCF传递的异常经过了消息的封装，所以要用下面的方式来处理
                //    //WCF的消息封装成了System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>
                //    System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail> fx = ex as System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail>;
                //    if (null != fx)
                //    {
                //        //FaultException.Detail 保留了原始的异常信息
                //        //ExceptionDetail.Type 属性，保留了原始的异常类型名称
                //        if (fx.Detail.Type == typeof(SA.WebUtil.Exceptions.ReportWcfUnknowException).FullName)
                //        {
                //            result.ViewBag.AlertMsg = ControllerUtility.Transfer("Public_Error_ReportServiceException");
                //            result.ViewBag.ErrorCode = -60305112;
                //            return result;
                //        }
                //    }
                //    result = null;
                //}

            }
            catch (Exception newEx)
            {
                result = null;
            }

            return result;

        }
        public Type ExceptionType
        {
            get
            {
                return this._exceptionType;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (!typeof(Exception).IsAssignableFrom(value))
                {
                    Loger.Error("出现了一个没有预定义的异常");
                    throw new ArgumentException("一个没有预定义的异常");
                }
                this._exceptionType = value;
            }
        }

        public string Master
        {
            get
            {
                return (this._master ?? string.Empty);
            }
            set
            {
                this._master = value;
            }
        }

        public override object TypeId
        {
            get
            {
                return this._typeId;
            }
        }

        public string View
        {
            get
            {
                if (string.IsNullOrEmpty(this._view))
                {
                    return "Error";
                }
                return this._view;
            }
            set
            {
                this._view = value;
            }
        }


    }
}
