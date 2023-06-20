using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace XP.Web.Permission
{
    /// <summary>
    /// UIData控制器上的权限拦截，登录即可使用
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class UIPermissionFilterAttribute : PermissionFilterAttribute
    {


        public UIPermissionFilterAttribute()
            : base()
        {
            this.UserInstanceConfig = "Comm" + base.UserInstanceConfig;
        }



        /// <summary>
        /// 权限拦截
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            //验证当前Action是否是匿名访问Action
            if (CheckAnonymous(filterContext))
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            if (CheckLogin(filterContext))
            {

            }
            else
            {
                var res = new JsonResult();
                res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                res.Data = "";
                filterContext.Result = res;//
                return;
            }

        }

        protected virtual bool CheckLogin(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            SessionUserFactory Factory = new SessionUserFactory();
            ISessionUser User = SessionUserFactory.CreateUserByConfig(UserInstanceConfig);
            //用户登录检查
            if (!User.CheckLogin())
            {
                return false;
            }
            return true;
        }




        public bool CheckAnonymous(ActionExecutingContext filterContext)
        {
            //验证是否是匿名访问的Action
            object[] attrsAnonymous = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AnonymousAttribute), true);
            //是否是Anonymous
            var Anonymous = attrsAnonymous.Length == 1;
            return Anonymous;
        }

    }
}
