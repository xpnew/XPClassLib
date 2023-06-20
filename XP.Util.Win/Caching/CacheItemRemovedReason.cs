using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Util.Win.Caching
{
    /// <summary>
    /// 指定从 Cache 对象移除项的原因。
    /// </summary>
    /// <remarks>
    /// 直接照抄微软，所以，Underused没用上。
    ///https://docs.microsoft.com/zh-cn/dotnet/api/system.web.caching.cacheitemremovedreason?view=netframework-4.0
    /// </remarks>
    //public enum CacheItemRemovedReason
    //{
    //    /// <summary>该项是通过指定相同键的 <see cref="M:System.Web.Caching.Cache.Insert(System.String,System.Object)" /> 方法调用或 <see cref="M:System.Web.Caching.Cache.Remove(System.String)" /> 方法调用从缓存中移除的。</summary>
    //    Removed = 1,
    //    /// <summary>从缓存移除该项的原因是它已过期。</summary>
    //    Expired = 2,
    //    /// <summary>之所以从缓存中移除该项，是因为系统要通过移除该项来释放内存。</summary>
    //    Underused = 3,
    //    /// <summary>从缓存移除该项的原因是与之关联的缓存依赖项已更改。</summary>
    //    DependencyChanged = 4,

    //}
}
