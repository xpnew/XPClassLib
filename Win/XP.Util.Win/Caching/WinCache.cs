using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Comm.Cache;

namespace XP.Util.Win.Caching
{
    /// <summary>定义在从 <see cref="T:System.Web.Caching.Cache" /> 移除缓存项时通知应用程序的回调方法。</summary>
    /// <param name="key">从缓存中移除的键。</param>
    /// <param name="value">与从缓存中移除的键关联的 <see cref="T:System.Object" /> 项。</param>
    /// <param name="reason">
    /// <see cref="T:System.Web.Caching.CacheItemRemovedReason" /> 枚举指定的、从缓存移除项的原因。</param>
    public delegate void CacheItemRemovedCallback(string key, object value, CacheItemRemovedReason reason);



    internal delegate void FileChangeEventHandler(object sender, FileChangeEvent e);
    public class WinCache
    {
        public event CacheItemRemovedCallback CacheItemRemoveEvent = (name, value, resion) => { };

        internal static Dictionary<string, WinCacheItem> Dict = new Dictionary<string, WinCacheItem>();



        private static readonly WinCache _Instance = new WinCache();



        public static WinCache Self
        {
            get { return _Instance; }
        }

        public WinCacheItem this[string name]
        {

            get
            {
                if (Dict.ContainsKey(name))
                {
                    var item = Dict[name];
                    item.ExpiredTimer.Stop();
                    item.ExpiredTimer.Start();

                    return item;
                }

                return null;
            }
        }

        public void Insert(string key, object val, int millisecond)
        {



            if (Dict.ContainsKey(key))
            {
                var item = Dict[key];
                item.ExpiredTimer.Stop();
                item.BeginRemove(key, CacheItemRemovedReason.Removed);
                item.CacheItemRemoveEvent -= OnCacheItemRemovedCallback;
                //BeginRemove(item.Key, item.Value, CacheItemRemovedReason.Removed);
                item = null;

                //return item;
            }
            var NewItem = new WinCacheItem(key, val, millisecond);
            NewItem.CacheItemRemoveEvent += OnCacheItemRemovedCallback;
            Dict[key] = NewItem;
        }

        public void Insert(string key, object val)
        {



            if (Dict.ContainsKey(key))
            {
                var item = Dict[key];
                item.ExpiredTimer.Stop();
                item.BeginRemove(key, CacheItemRemovedReason.Replace);
                //BeginRemove(item.Key, item.Value, CacheItemRemovedReason.Removed);
                item = null;

                //return item;
            }
            var NewItem = new WinCacheItem(key, val);
            NewItem.CacheItemRemoveEvent += OnCacheItemRemovedCallback;
            Dict[key] = NewItem;
        }

        public void Insert(string key, object val, string phyPath, CacheItemRemovedCallback removedCallback)
        {



            if (Dict.ContainsKey(key))
            {
                var item = Dict[key];
                item.ExpiredTimer.Stop();
                //BeginRemove(item.Key, item.Value, CacheItemRemovedReason.Removed);
                item.BeginRemove(key, CacheItemRemovedReason.Removed);
                item.CacheItemRemoveEvent -= OnCacheItemRemovedCallback;
                item = null;

                //return item;
            }
            CacheDependency dep = new CacheDependency(phyPath, DateTime.Now);

            var NewItem = new WinCacheItem(key, val, removedCallback);
            NewItem.Depend = dep;
            NewItem.MonitorDependencyChanges();
            NewItem.CacheItemRemoveEvent += OnCacheItemRemovedCallback;

            Dict[key] = NewItem;
        }


        public object GetCache(string key)
        {
            object val = null;

            if (Dict.ContainsKey(key))
            {
                var item = Dict[key];
                item.ResetExpired();
                val = item.Value;
            }
            return val;
        }

        private void HasRemove(string key, object val, CacheItemRemovedReason resion)
        {
            CacheItemRemoveEvent(key, val, resion);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Remove(string key)
        {
            return DoRemove(key, CacheItemRemovedReason.Removed);
        }

        //public object Remove(string key, CacheItemRemovedReason reason)
        //{

        //}
        /// <summary>
        /// 真正处理删除
        /// 微软的缓存管理通过复杂的UpdateCache来实现的，这里只是简单的从字典里面拿掉了
        /// </summary>
        /// <param name="key"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public object DoRemove(string key, CacheItemRemovedReason reason)
        {
            object val = null;
            if (Dict.ContainsKey(key))
            {
                var item = Dict[key];
                item.CacheItemRemoveEvent -= OnCacheItemRemovedCallback;

                val = item.Value;
                Dict.Remove(key);
                item = null;
            }
            return val;
        }

        /// <summary>
        /// 处理缓存移除回调
        /// </summary>
        /// <remarks>
        /// 这里没有微软实现那么多的功能，特别是间接依赖和缓存更新回调 
        ///
        /// </remarks>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="reason"></param>
        public static void OnCacheItemRemovedCallback(string key, CacheItemRemovedReason reason)
        {
            //CacheItemUpdateReason reason1;

            x.Say("缓存管理 需要处理回调： 【" + key + "】 将被删除！！！ 移除原因： " + reason);

            switch (reason)
            {
                case CacheItemRemovedReason.DependencyChanged:

                    break;

                case CacheItemRemovedReason.Expired:
                    break;
                case CacheItemRemovedReason.Removed:
                case CacheItemRemovedReason.Underused:

                default:
                    return;
                    ;
            }

            AppRuntime.Cache.DoRemove(key,reason); 
        }
    }
}
