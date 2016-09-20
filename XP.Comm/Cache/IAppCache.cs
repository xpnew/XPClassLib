    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Cache
{

    /// <summary>
    /// 应用程序缓存，持久性缓存
    /// </summary>
    /// <remarks>
    /// 调用System.Web.Cache或者 Windows Form的全局属性
    /// 
    /// </remarks>
    public interface IAppCache
    {
        string CacheName { get; set; }




    }
}
