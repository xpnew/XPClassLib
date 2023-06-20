using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace XP.Web.Permission
{
    /// <summary>
    /// 仅限管理员使用的权限
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AdminOnlyFilterAttribute : PermissionFilterAttribute
    {
        protected override bool AuthorizeCore(ActionExecutingContext filterContext)
        {

            if (filterContext.HttpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            //验证当前Action是否是登录即可访问
            if (CheckLoginAllowView(filterContext))
                return true;
            //调用工厂的方式创建用户对象
            SessionUserFactory Factory = new SessionUserFactory();
            ISessionUser User = SessionUserFactory.CreateUserByConfig(UserInstanceConfig);

            if ("admin" == User.UserName)
            {
                return true;
            }

            if (User.StoreId.HasValue && User.RoleId.HasValue)
            {
                if (2 >= User.RoleId.Value && 1 == User.StoreId.Value)
                {
                    return true;
                }
            }


            return false;

        }

    }
}
