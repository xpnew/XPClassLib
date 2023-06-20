using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Comm.Cache;
using XP.Util.Win;
using XP.Util.Win.Caching;

namespace XP.Comm.Cache
{
    public class WinFileCacheProvider : ICacheProvider
    {
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

        //private Dictionary<string, object> _CacheDict  = new Dictionary<string, object>();
        //public Dictionary<string, object> CacheDict
        //{
        //    get { return _CacheDict; }
        //}

        public void Add(string cacheName, object cacheInstance)
        {
            var cacheInternal = AppRuntime.Cache;
            cacheInternal.Insert(cacheName, cacheInstance);
        }

        public object GetCache(string cacheName)
        {
            var cacheInternal = AppRuntime.Cache;

            return cacheInternal.GetCache(cacheName);
            //throw new NotImplementedException();
        }

        public void Insert(string cacheName, object cacheInstance)
        {
            var cacheInternal = AppRuntime.Cache;
            cacheInternal.Insert(cacheName, cacheInstance);
        }

        public void Insert(string cacheName, object cacheInstance, string path)
        {
            // throw new NotImplementedException();
            CacheItemRemovedCallback callback = new CacheItemRemovedCallback(RemovedCallback);
            //CacheDependency dep = new CacheDependency(path, DateTime.Now);

            var cacheInternal = AppRuntime.Cache;

            cacheInternal.Insert(cacheName, cacheInstance, path, callback);

        }

        public void Remove(string cacheName)
        {
            var cacheInternal = AppRuntime.Cache;
            cacheInternal.Remove(cacheName);
            //throw new NotImplementedException();
        }

        public void AfterRemove(object o, CacheRemoveEventArgs args)
        {
            //CacheRemoveEvent();+
            CacheRemoveEvent?.Invoke(o,args);

        }

        protected virtual void RemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            Console.WriteLine("缓存被移除!");
            Console.WriteLine(reason.ToString());
            var NewArgs = new CacheRemoveEventArgs()
            {
                Reason = reason,
                CacheKey = key
            };
            AfterRemove(value,NewArgs);
        }

        public void Insert(string cacheName, object cacheInstance, CacheExpireTypeDef expireType, int second)
        {
            throw new NotImplementedException();
        }
    }
}
