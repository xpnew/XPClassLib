using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Cache
{


    /// <summary>  
    /// 缓存移除  
    /// </summary>  
    /// <param name="a">委托传递的参数</param>  
    public delegate void CacheRemoveEventHander(Object o, CacheRemoveEventArgs arg);


    /// <summary>  
    /// 缓存重新加载  
    /// </summary>  
    /// <param name="a">委托传递的参数</param>  
    public delegate void CacheReloadEventHander();

 

    public interface ICacheProvider
    {

        /// <summary>
        /// 缓存移除事件
        /// </summary>
        event CacheRemoveEventHander CacheRemoveEvent;
        int SlidingExpiration { get; set; }


        object GetCache(string cacheName);

        void Add(string cacheName, object cacheInstance);

        object this[string cacheName] { get; set; }


        void Remove(string cacheName);


        void Insert(string cacheName, object cacheInstance);
        void Insert(string cacheName, object cacheInstance, string path);
        /// <summary>
        /// 插入一个对象到缓存当中，并且指定过期的方式和过期时间
        /// </summary>
        /// <param name="cacheName">缓存项名称</param>
        /// <param name="cacheInstance">实际插入的数据</param>
        /// <param name="expireType">过期类型</param>
        /// <param name="second">过期时间（秒）</param>
        void Insert(string cacheName, object cacheInstance, CacheExpireTypeDef expireType, int second);

    }
}
