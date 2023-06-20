using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Cache
{
    public class CacheRemoveEventArgs: EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public CacheItemRemovedReason Reason { get; set; } = CacheItemRemovedReason.Removed;


        public string CacheKey { get; set; }


    }
}
