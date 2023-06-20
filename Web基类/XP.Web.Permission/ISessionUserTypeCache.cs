using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Cache;

namespace XP.Web.Permission
{
    public class ISessionUserTypeCache : DictCacheBase<ISessionUserTypeCache, string, Type>
    {
        private readonly string NameOfCacheName = "ISessionUserTypes";

        protected override void _Init()
        {
            base._Init();

            base.CacheName = NameOfCacheName;
        }

    }
}
