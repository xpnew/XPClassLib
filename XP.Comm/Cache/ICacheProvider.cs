using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Cache
{
    public interface ICacheProvider
    {


        object GetCache(string cacheName);

        void Add(string cacheName, object cacheInstance);

        void Insert(string cacheName, object cacheInstance);

        object this[string cacheName] { get; set; }


        void Remove(string cacheName);

    }
}
