using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Web.Permission
{
    /// <summary>
    /// 技术管理员（开发人员）使用的权限拦截
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class DevPermissionFilterAttribute : PermissionFilterAttribute
    {

        public DevPermissionFilterAttribute()
            : base()
        {
            this.UserInstanceConfig = "Dev" + base.UserInstanceConfig;
            //todo: 改用配置文件
            //NeedDo: 改用配置文件
            NeedLoginUrl = "/DLogin/Index/";
            TimeoutUrl = "/DLogin/Timeout/";
        }


    }
}
