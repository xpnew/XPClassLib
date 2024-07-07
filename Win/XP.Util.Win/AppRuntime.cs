using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XP.Util.Win.Caching;

namespace XP.Util.Win
{
    public sealed class AppRuntime
    {

        public static WinCache Cache
        {
            get
            {
                return WinCache.Self;
            }
        }
    }
}
