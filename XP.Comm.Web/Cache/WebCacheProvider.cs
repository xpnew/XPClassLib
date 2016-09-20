using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Cache
{
    public class WebCacheProvider : ICacheProvider
    {



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
            Add(cacheName, cacheInstance);

        }

        public void Remove(string cacheName)
        {
            System.Web.Caching.Cache objCache = System.Web.HttpRuntime.Cache;
            objCache.Remove(cacheName);
        }
    }
}
