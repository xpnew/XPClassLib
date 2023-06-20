using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Attributes
{
    /// <summary>
    /// 辅助用来Json序列化的key
    /// </summary>
    /// <remarks>
    /// 主要的是为了提供给下面这个类：
    /// Ljy.Util.TypeCache.EntityCacheItem4JsonDict
    ///
    /// </remarks>
    public class JsonAlaisKeyAttribute : AlaisKeyAttribute
    {
        public JsonAlaisKeyAttribute(string name) : base(name)
        {
        }
    }
}
