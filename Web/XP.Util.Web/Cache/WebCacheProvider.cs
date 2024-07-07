using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using XP.Comm.Cache;

namespace XP.Util.Cache
{
    public class WebCacheProvider : ICacheProvider
    {
        /// <summary>
        /// 缓存移除事件
        /// </summary>
        public event CacheRemoveEventHander CacheRemoveEvent;

        /// <summary>
        /// 缓存的默认滑动时间为600秒
        /// </summary>
        private int _SlidingExpiration = 600;


        public int SlidingExpiration
        {
            get { return _SlidingExpiration; }
            set { _SlidingExpiration = value; }
        }
        public object GetCache(string cacheName)
        {
            System.Web.Caching.Cache objCache = System.Web.HttpRuntime.Cache;
            if (null != objCache[cacheName])
            {
                return objCache[cacheName];
            }
            return null;
        }


        public object this[string cacheName]
        {
            get
            {
                return GetCache(cacheName);
            }
            set
            {
                Add(cacheName, value);
            }
        }
        public void Add(string cacheName, object cacheInstance)
        {
            System.Web.Caching.Cache objCache = System.Web.HttpRuntime.Cache;
            if (null == objCache[cacheName])
            {
                objCache[cacheName] = cacheInstance;
            }
            else
            {
                objCache[cacheName] = cacheInstance;

            }
        }

        public void Insert(string cacheName, object cacheInstance)
        {
            System.Web.Caching.Cache objCache = System.Web.HttpRuntime.Cache;
            objCache.Insert(cacheName, cacheInstance);

        }



        public void Insert(string cacheName, object cacheInstance, string phyPath)
        {
            System.Web.Caching.Cache objCache = System.Web.HttpRuntime.Cache;
            CacheItemRemovedCallback callback = new CacheItemRemovedCallback(RemovedCallback);
            CacheDependency dep = new CacheDependency(phyPath, DateTime.Now);

            objCache.Insert(cacheName, cacheInstance, dep, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(SlidingExpiration), CacheItemPriority.Default, callback);

        }

        public void Remove(string cacheName)
        {
            System.Web.Caching.Cache objCache = System.Web.HttpRuntime.Cache;
            objCache.Remove(cacheName);
        }


        protected virtual void RemovedCallback(string key, object value, System.Web.Caching.CacheItemRemovedReason reason)
        {
            Console.WriteLine("缓存被移除!");
            Console.WriteLine(reason.ToString());

            var CommReason = (Comm.Cache.CacheItemRemovedReason)((int)reason);
            var NewArgs = new CacheRemoveEventArgs()
            {
                Reason = CommReason,
                CacheKey = key
            };
            AfterRemove(value, NewArgs);
        }

        public void AfterRemove(object o, CacheRemoveEventArgs args)
        {
            CacheRemoveEvent?.Invoke(o, args);

        }

        public void Insert(string cacheName, object cacheInstance, CacheExpireTypeDef expireType, int second)
        {
            System.Web.Caching.Cache objCache = System.Web.HttpRuntime.Cache;

            if (CacheExpireTypeDef.Abesolute == expireType)
            {
                objCache.Insert(cacheName, cacheInstance, null, DateTime.Now.AddSeconds(second), System.Web.Caching.Cache.NoSlidingExpiration);

            }
            else
            {
                objCache.Insert(cacheName, cacheInstance, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(SlidingExpiration));

            }
        }
    }
}
