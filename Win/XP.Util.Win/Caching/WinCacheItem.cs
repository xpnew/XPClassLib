using System;
using System.Timers;
using XP.Comm.Cache;

namespace XP.Util.Win.Caching
{
    public delegate void ChacheItemRemoveHandler(string keyName, Comm.Cache.CacheItemRemovedReason resion);
    public class WinCacheItem : IDisposable
    {
        public event ChacheItemRemoveHandler CacheItemRemoveEvent;// = (name, resion) => { };


        public string Key { get; set; }

        public Object Value { get; set; }

        public DateTime CreateTime { get; set; }

        internal CacheDependency Depend { get; set; }

        private CacheItemRemovedCallback _CallBack;

        /// <summary>
        /// 用来处理默认超时的计时器
        /// </summary>
        internal System.Timers.Timer ExpiredTimer { get; set; }




        private int _ExpiredTimeMillisecond = 60* 60* 1000;
        /// <summary>
        /// 默认的超时间隔，毫秒
        /// </summary>
        public int ExpiredMillisecond
        {
            get
            { return _ExpiredTimeMillisecond; }
        }

        public WinCacheItem()
        {
            //CacheItemRemoveEvent += _DefaultRemove;

            _Init();
            CreateTime = DateTime.Now;
        }

        protected void _Init()
        {
            _InitTimer();
        }

        public WinCacheItem(string key, object val, int miiExpiredTimeMillisecond) : this()
        {
            //CacheItemRemoveEvent += _DefaultRemove;

            Key = key;
            Value = val;
            _ExpiredTimeMillisecond = miiExpiredTimeMillisecond;


            ExpiredTimer.Interval = ExpiredMillisecond;


        }

        public WinCacheItem(string key, object val) : this()
        {
            //CacheItemRemoveEvent += _DefaultRemove;

            Key = key;
            Value = val;

        }
        public WinCacheItem(string key, object val, CacheItemRemovedCallback callback) : this()
        {
            //CacheItemRemoveEvent += _DefaultRemove;

            Key = key;
            Value = val;
            _CallBack = callback;
        }

        protected void _InitTimer()
        {
            ExpiredTimer = new Timer();
            ExpiredTimer.AutoReset = false;
            ExpiredTimer.Interval = ExpiredMillisecond;

            ExpiredTimer.Elapsed += _AutoExpiredEvent;

            ExpiredTimer.Start();

        }


        internal void ResetExpired()
        {
            if (null != ExpiredTimer)
            {
                ExpiredTimer.Stop();
                ExpiredTimer.Elapsed -= _AutoExpiredEvent;

                //if (null == ExpiredTimer.Elapsed)
                //{

                //}
                ExpiredTimer.Elapsed += _AutoExpiredEvent;

                ExpiredTimer.Start();

            }
            else
            {
                _InitTimer();
            }

        }

        internal void BeginRemove(string key, CacheItemRemovedReason resion)
        {
            //CacheItemRemoveEvent(key, resion);
            CacheItemRemoveEvent?.Invoke(key, resion);
            _CallBack?.Invoke(Key, Value, resion);
            _CallBack = null;
            Depend.Dispose();
        }

        private void _AutoExpiredEvent(object sender, System.Timers.ElapsedEventArgs e)
        {

            x.Say("缓存 【" + Key + "】已经过期！ ");

            if (null != this.ExpiredTimer)
            {
                this.ExpiredTimer.Stop();
                ExpiredTimer.Elapsed -= _AutoExpiredEvent;
                this.ExpiredTimer = null;
            }


            BeginRemove(Key, CacheItemRemovedReason.Expired);


            x.Say("缓存 字典 项的数量： 【" + WinCache.Dict.Count + "】！ ");

        }



        internal void MonitorDependencyChanges()
        {
            CacheDependency dependency = Depend;
            if (dependency == null)
            {
                return;
            }

            //if (!dependency.TakeOwnership())
            //    throw new InvalidOperationException(System.Web.SR.GetString("Cache_dependency_used_more_that_once"));
            dependency.SetCacheDependencyChanged((Action<object, EventArgs>)((sender, args) => this.DependencyChanged(sender, args)));
        }
        //internal void RemoveDependencyChanges()
        //{
        //    CacheDependency dependency = Depend;
        //    if (dependency == null)
        //    {
        //        return;
        //    }

        //    //if (!dependency.TakeOwnership())
        //    //    throw new InvalidOperationException(System.Web.SR.GetString("Cache_dependency_used_more_that_once"));
        //    dependency.CleanEvent();
        //}

        private void DependencyChanged(object sender, EventArgs args)
        {
            // throw new NotImplementedException();
            //简化的委托调用 if(null != )
            //_CallBack?.Invoke(Key, Value, CacheItemRemovedReason.DependencyChanged);
            this.ExpiredTimer.Stop();

            BeginRemove(Key, CacheItemRemovedReason.DependencyChanged);
        }


        /// <summary>
        /// 为了防止出现异常，添加了一空的方法作为事件的默认处理
        /// </summary>
        /// <param name="key"></param>
        /// <param name="resion"></param>
        private void _DefaultRemove(string key, CacheItemRemovedReason resion)
        {

        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //dependency = null;
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。
                if (null != this.ExpiredTimer)
                {
                    this.ExpiredTimer.Stop();
                    this.ExpiredTimer.Elapsed -= _AutoExpiredEvent;
                    this.ExpiredTimer = null;
                }
                this.Depend = null;
               if(null != CacheItemRemoveEvent)
                {
                    CacheItemRemoveEvent = null;
                }
               if(null != _CallBack)
                {
                    _CallBack = null;
                }
                disposedValue = true;
            }
        }


        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~WinCacheItem() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion


    }
}
