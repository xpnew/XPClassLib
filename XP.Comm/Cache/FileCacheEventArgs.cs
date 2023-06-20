using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Cache
{
    public class FileCacheEventArgs: CacheRemoveEventArgs
    {
        public string Filename { get; set; }

    }
}
