using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Web.Permission
{
    /// <summary>
    /// 提供给普通web页面使用的用户状态管理类，它包含了一个指定的静态方法CreateUser，这个类是提供给工厂类使用的。
    /// </summary>
    /// <remarks>
    /// 详细的说明请参考文档《ISessionUser接口和单例模式技术实现》
    /// 1、这个类使用了sealed关键字，防止被继承。
    /// 2、这个类包括一个指定的静态的方法：CreateUser。
    /// 3、这个类是提供给工厂类SessionUserFactory使用的。
    /// 4、原则上这个类不应该包括具体的功能实现，实际的功能完成，需要在基类当中实现。
    /// 5、这么设计，最终的目标是既体现了ISessionUser接口，又体现了单例模式。
    /// 
    /// </remarks>
    public sealed class WebUser : SessionUserNormal<WebUser>
    {
        public static WebUser CreateUser()
        {
            return CreateInstance();
        }

    }
}
