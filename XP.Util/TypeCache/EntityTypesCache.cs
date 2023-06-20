using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Util.TypeCache
{
    /// <summary>
    /// 实体类型缓存（单例化）
    /// </summary>
    public class EntityTypesCache
    {
        private static object _Locker;

        public static object Locker
        {
            get
            {
                if (null == _Locker)
                    _Locker = new object();
                return _Locker;
            }
            set { _Locker = value; }
        }

        private string NameOfDictionaryCache = "EntityTypesCache";

        private Comm.Cache.ICacheProvider CacheProvider;

        private Dictionary<string, EntityTypesCacheItem> _Items;
        /// <summary>数据集合，缓存的主要实现</summary>
        /// <remarks>
        /// 
        /// 
        /// </remarks>
        public Dictionary<string, EntityTypesCacheItem> Items
        {
            get
            {
                if (null == CacheProvider[NameOfDictionaryCache])
                {
                    _Items = new Dictionary<string, EntityTypesCacheItem>();
                    CacheProvider.Add(NameOfDictionaryCache, _Items);
                    return _Items;
                }
                _Items = CacheProvider[NameOfDictionaryCache] as Dictionary<string, EntityTypesCacheItem>;

                return _Items;
            }
            set
            {

                if (null == CacheProvider[NameOfDictionaryCache])
                {
                    CacheProvider.Add(NameOfDictionaryCache, value);
                }
                else
                {
                    CacheProvider[NameOfDictionaryCache] = value;
                }
            }

        }

        #region 实现单例模式

        protected EntityTypesCache() { _Init(); }
        public static readonly EntityTypesCache _Instance = new EntityTypesCache();


        public static EntityTypesCache CreateInstance()
        {
            //x.TimerLog("------------- 调用了类型缓存的单例模式");
            //if (_Instance.Items == null) {
            //    x.Say("此次调用的字典是 null");
            //}
            //else
            //{
            //    x.Say("此次调用的字典是包括这些数据：　"+ _Instance.Items.Count);
            //}
            return _Instance;
        }
        #endregion

        #region 定义和初始化

        protected virtual void _Init()
        {
            //x.Say("缓存已被初始化。");
            //if (null == Items)
            //    Items = new Dictionary<string, EntityTypesCacheItem>();

            CacheProvider = Cache.CacheManager.Create();
        }


        #endregion

        //public PropertiesCacheManager() { }
        //public PropertiesCacheManager(string cacheName)
        //{
        //    this.NameOfDictionaryCache = cacheName;
        //}

        protected void SetCacheName(string cacheName)
        {

        }

        public EntityTypesCacheItem this[string name]
        {
            get { return GetItem(name); }
        }
        public EntityTypesCacheItem this[Type type]
        {
            get { return GetItem(type); }
        }
        public EntityTypesCacheItem GetItem(Type type)
        {
            string TypeName = type.FullName;
            //x.TimerLog("准备从缓存当中获取这个类型：" + TypeName);
            if (!Items.ContainsKey(TypeName))
            {
                Add(type);
            }
            return Items[TypeName];

        }
        public EntityTypesCacheItem GetItem(string typeName)
        {
            if (Items.ContainsKey(typeName))
            {
                return Items[typeName];
            }
            return null;
        }


        public bool Exist(Type type)
        {
            string TypeName = type.FullName;
            return Exist(TypeName);
        }
        public bool Exist(string typeName)
        {
            if (Items.ContainsKey(typeName))
            {
                return true;
            }
            return false;
        }

        public virtual void Add(Type type)
        {
            string TypeName = type.FullName;
            EntityTypesCacheItem NewCache = new EntityTypesCacheItem(type);
            AddCache(TypeName, NewCache);
        }
        //public void AddGlobalType(Type type)
        //{
        //    string TypeName = type.FullName;
        //    PropertyCache NewCache = new PropertyGlobalCache(type);
        //    AddCache(TypeName, NewCache);
        //}
        public void AddCache(string cachename, EntityTypesCacheItem cache)
        {
            lock (Locker)
            {
                if (Exist(cachename))
                {

                    Items[cachename] = cache;
                    return;
                }
                Items.Add(cachename, cache);
            }
        }
        public void AddCache(EntityTypesCacheItem cache)
        {
            AddCache(cache.CacheName, cache);
        }



        public void Remove(Type type)
        {
            string TypeName = type.FullName;
            Remove(TypeName);
        }
        public void Remove(string typename)
        {
            Items.Remove(typename);
        }

        public void Clear()
        {
            foreach (string key in Items.Keys)
            {
                //Items[controller] = null;
                //Items.Remove(controller);
            }
            Items.Clear();
            RemoveCache();
            //Items = null;
        }

        private void RemoveCache()
        {
            if (null != CacheProvider[NameOfDictionaryCache])
            {
                CacheProvider.Remove(NameOfDictionaryCache);
            }

        }

    }
}
