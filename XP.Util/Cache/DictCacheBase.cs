using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Cache
{
    /// <summary>
    /// 字典类型的缓存的基类，已经“提前实现”了派生类的单例模式，字典功能的基础实现
    /// </summary>
    /// <typeparam name="TCache">派生类的类型</typeparam>
    /// <typeparam name="TItem">字典值的类型</typeparam>
    public class DictCacheBase<TCache, TKey, TItem> : CachebasedImplementaionBase
        where TCache : class, new()
        where TItem : class
    {

        #region 实现单例模式

        protected DictCacheBase()
        {

            _Init();
        }
        public static readonly TCache _Instance = new TCache();


        public static TCache CreateInstance()
        {
            return _Instance;
        }
        #endregion

        #region 扩展和实现构造函数，便于派生类继承和重写

        private bool _HasCacheName = false;
        private string _CacheName;
        public virtual string CacheName
        {
            get
            {
                if (_HasCacheName)
                    return _CacheName;
                _CacheName = this.GetType().FullName;
                _HasCacheName = true;
                return _CacheName;
            }
            set
            {
                _CacheName = value;
                _HasCacheName = true;
            }
        }

        protected virtual void _Init()
        {

            //InitCache();
        }

        public virtual void InitCache()
        {
            _Dict = new Dictionary<TKey, TItem>();
            Provider[CacheName] = _Dict;
        }

        #endregion
        #region 实现缓存的字典功能

        private Dictionary<TKey, TItem> _Dict;

        public Dictionary<TKey, TItem> Dict
        {
            get
            {
                //System.Web.Caching.Cache WebCache = System.Web.HttpRuntime.Cache;

                if (null == Provider[CacheName])
                {
                    InitCache();
                }
                else
                {
                    _Dict = Provider[CacheName] as Dictionary<TKey, TItem>;
                }
                return _Dict;
            }
            set
            {
                _Dict = value;
                Provider[CacheName] = _Dict;
            }
        }

        public bool ExistKey(TKey key)
        {
            return Dict.Keys.Contains(key);
        }

        #endregion
    }
}
