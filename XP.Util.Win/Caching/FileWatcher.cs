using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Util.Path;

namespace XP.Util.Win.Caching
{
    public class FileWatcher
    {
        public DateTime? OldDt { get; set; }
        public DateTime? NewDt { get; set; }
        public Action<object, EventArgs> ReloadFile;

        public Action<object, EventArgs> RemoveFile;
        /// <summary>
        /// 刷新的毫秒
        /// </summary>
        public int RefrushMillisecond
        {
            get
            { return 50; }
        }
        private string _FilePath;
        public string FilePath
        {
            get { return _FilePath; }
            set
            {
                _FilePath = value;
                Init();
            }
        }
        /// <summary>
        /// 内部的计时器
        /// </summary>
        System.Timers.Timer UpdateTimer { get; set; }


        public string CacheKey { get; set; }


        public FileWatcher()
        {

        }

        public FileWatcher(string filePath)
        {
            filePath = PathTools.GetFull(filePath);
            this.FilePath = filePath;
        }

        public FileWatcher(string key, string filePath) : this(filePath)
        {
            CacheKey = key;
        }

        protected void Init()
        {
            if (!System.IO.File.Exists(FilePath))
            {
                throw new ArgumentException("文件不存在！！！");
            }

            UpdateFileDt();
            StartUpdateStatus();
        }
        private void UpdateFileDt(DateTime dt)
        {
            OldDt = NewDt;
            NewDt = dt;
        }
        private void UpdateFileDt()
        {
            if (!CheckFile())
            {

                OldDt = NewDt;
                NewDt = null;
                return;
            }
            DateTime CurrentDt;
            CurrentDt = System.IO.File.GetLastWriteTime(FilePath);
            UpdateFileDt(CurrentDt);
        }

        public void StartUpdateStatus()
        {
            x.Say("文件刷新：" + DateTime.Now.ToString("HHmmsss.FFFF"));
            UpdateTimer = new System.Timers.Timer(RefrushMillisecond);
            UpdateTimer.Elapsed += new System.Timers.ElapsedEventHandler(UpdateStatusEvent);
            UpdateTimer.Start();
        }

        private bool CheckFile()
        {
            if (System.IO.File.Exists(FilePath))
            {
                return true;
            }
            return false;
        }
        private void UpdateStatusEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!CheckFile())
            {
                RemoveFile?.Invoke(null, new EventArgs());
                return;
            }

            UpdateFileDt();
            if(NewDt == OldDt)
            {
                return;
            }
            TimeSpan ts = NewDt.Value - OldDt.Value;

            if (0 < ts.Milliseconds)
            {

                OnReloadFile(new EventArgs());
            }


        }

        public void Stop()
        {
            UpdateTimer.Stop();

        }
        protected void OnReloadFile(EventArgs e)
        {
            ReloadFile?.Invoke(this, e);
        }


        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    UpdateTimer.Stop();
                    UpdateTimer.Elapsed -= UpdateStatusEvent;
                    UpdateTimer.Dispose();
                    UpdateTimer = null;
                    // TODO: 释放托管状态(托管对象)
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~FileWatcher()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            //GC.SuppressFinalize(this);
        }
        #endregion

    }
}
