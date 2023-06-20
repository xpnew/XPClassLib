using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XP.Comm.Cache;
using XP.Util.Cache;

namespace XP.Cache
{


    /// <summary>
    /// 某些“基于缓存实现的”的类，这是它们的基类
    /// </summary>
    public class CachebasedImplementaionBase
    {

        private object _Lock4Update;

        /// <summary>
        /// 互斥锁使用的对象
        /// </summary>
        public object Lock4Update
        {
            get
            {
                if (null == _Lock4Update)
                    _Lock4Update = new object();
                return _Lock4Update;
            }
        }

        protected internal ICacheProvider Provider
        {
            get
            {
                if (null == _Provider)
                {

                    _Provider = CacheManager.Create();
                }
                return _Provider;
            }
            set { _Provider = value; }
        }

        private ICacheProvider _Provider;



    }
}
