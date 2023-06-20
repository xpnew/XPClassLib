using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace XP.Web.Permission
{
    /// <summary>
    /// 动作权限依赖，添加此特性的动作，其它权限依赖于指定名称的另一个动作的权限
    /// </summary>
    /// <remarks>
    /// 
    /// 例如：
    /// Query动作，依赖于Index动作，那就是要要Query上标记[ActionDepend("Index")]
    /// 例如：
    /// 开启和关闭动作的操作日志，实际上依赖于“修改”这个动作
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ActionDependFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 当前动作依赖的动作名称（父级动作）
        /// </summary>
        public string ParentActionName { get; set; }

        public ActionDependFilterAttribute(string parentActionName)
        {
            this.ParentActionName = parentActionName;
        }

    }
}
