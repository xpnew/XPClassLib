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
        /// ��������Ĭ�ϳ�ʱ�ļ�ʱ��
        /// </summary>
        internal System.Timers.Timer ExpiredTimer { get; set; }




        private int _ExpiredTimeMillisecond = 60* 60* 1000;
        /// <summary>
        /// Ĭ�ϵĳ�ʱ���������
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

            x.Say("���� ��" + Key + "���Ѿ����ڣ� ");

            if (null != this.ExpiredTimer)
            {
                this.ExpiredTimer.Stop();
                ExpiredTimer.Elapsed -= _AutoExpiredEvent;
                this.ExpiredTimer = null;
            }


            BeginRemove(Key, CacheItemRemovedReason.Expired);


            x.Say("���� �ֵ� ��������� ��" + WinCache.Dict.Count + "���� ");

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
            //�򻯵�ί�е��� if(null != )
            //_CallBack?.Invoke(Key, Value, CacheItemRemovedReason.DependencyChanged);
            this.ExpiredTimer.Stop();

            BeginRemove(Key, CacheItemRemovedReason.DependencyChanged);
        }


        /// <summary>
        /// Ϊ�˷�ֹ�����쳣�������һ�յķ�����Ϊ�¼���Ĭ�ϴ���
        /// </summary>
        /// <param name="key"></param>
        /// <param name="resion"></param>
        private void _DefaultRemove(string key, CacheItemRemovedReason resion)
        {

        }

        #region IDisposable Support
        private bool disposedValue = false; // Ҫ����������

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //dependency = null;
                    // TODO: �ͷ��й�״̬(�йܶ���)��
                }

                // TODO: �ͷ�δ�йܵ���Դ(δ�йܵĶ���)������������������ս�����
                // TODO: �������ֶ�����Ϊ null��
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


        // TODO: �������� Dispose(bool disposing) ӵ�������ͷ�δ�й���Դ�Ĵ���ʱ������ս�����
        // ~WinCacheItem() {
        //   // ������Ĵ˴��롣���������������� Dispose(bool disposing) �С�
        //   Dispose(false);
        // }

        // ��Ӵ˴�������ȷʵ�ֿɴ���ģʽ��
        public void Dispose()
        {
            // ������Ĵ˴��롣���������������� Dispose(bool disposing) �С�
            Dispose(true);
            // TODO: ���������������������ս�������ȡ��ע�������С�
            // GC.SuppressFinalize(this);
        }
        #endregion


    }
}
