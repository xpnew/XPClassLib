using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Web.Permission
{
    /// <summary>
    /// 匿名访问标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AnonymousAttribute : Attribute
    {
    }
}
