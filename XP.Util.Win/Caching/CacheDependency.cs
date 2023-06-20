using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Util.Win.Caching
{

    /// <summary>
    /// 缓存依赖（默认实现的是文件缓存依赖）
    /// </summary>
    public class CacheDependency : IDisposable
    {
        private Action<object, EventArgs> _objNotify;

        private DateTime _utcLastModified;

        /// <summary>获取依赖项的上次更改时间。</summary>
        /// <returns>依赖项的上次更改时间。</returns>
        public DateTime UtcLastModified
        {
            get
            {
                return this._utcLastModified;
            }
        }

        private FileWatcher _Watcher;

        protected CacheDependency()
        {

        }

        public CacheDependency(string path, DateTime startDt)
        {
            _Watcher = new FileWatcher(path);
            _Watcher.ReloadFile += NotifyDependencyChanged;

        }

        //private void _Watcher_ReloadFile(object sender, EventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}

        public void SetCacheDependencyChanged(Action<object, EventArgs> dependencyChangedAction)
        {
            //this._bits[32] = true;
            //if (this._bits[8])
            //    return;
            this._objNotify = dependencyChangedAction;
        }


        public void RemoveCacheDependencyChanged(Action<object, EventArgs> dependencyChangedAction)
        {
            this._objNotify -= dependencyChangedAction;
        }

        //public void CleanEvent()
        //{
        //    this._objNotify = null;
        //    _Watcher.ReloadFile -= NotifyDependencyChanged;
        //    _Watcher.Stop();
        //    _Watcher = null;
        //}

        /// <summary>通知 <see cref="T:System.Web.Caching.CacheDependency" /> 基对象由派生的 <see cref="T:System.Web.Caching.CacheDependency" /> 类表示的依赖项已更改。</summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">包含事件数据的 <see cref="T:System.EventArgs" /> 对象。</param>
        protected void NotifyDependencyChanged(object sender, EventArgs e)
        {
            //if (!this._bits.ChangeValue(4, true))
            //    return;
            this._utcLastModified = DateTime.UtcNow;
            _objNotify?.Invoke(sender, e);
            //Action<object, EventArgs> objNotify = this._objNotify;
            //if (objNotify != null)
            //    objNotify(sender, e);
            //this.DisposeInternal();
        }


        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    if (null != _Watcher)
                    {
                        _Watcher.ReloadFile -= NotifyDependencyChanged;
                        _Watcher.Dispose();
                    }
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。
                _Watcher = null;
                this._objNotify = null;
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
            //GC.SuppressFinalize(this);
        }
        #endregion
    }
}
