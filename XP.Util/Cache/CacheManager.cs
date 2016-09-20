using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XP.Comm.Cache;
namespace XP.Util.Cache
{
    public class CacheManager
    {
        private static ICacheProvider _InnerCacheProvider = null;


        public static ICacheProvider Create()
        {
            if (null != _InnerCacheProvider)
            {
                return _InnerCacheProvider;
            }
            string ConfigName = "CacheProvider";

            var Instance = InstanceBuilder.CreateInstance<ICacheProvider>(ConfigName);

            _InnerCacheProvider = Instance;
            return Instance;
        }

    }
}
