using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Comm.Cache;
using XP.Util.Path;

namespace XP.Util.FileCache
{

    /// <summary>
    /// 文件缓存基类
    /// </summary>
    /// <remarks>
    /// ▲▲▲注意！！！
    /// 需要在Web.config 或者 App.config当中指定接口Cache.ICacheProvider的具体实现类！！！
    /// </remarks>
    public class FileCacheBase
    {
        protected ICacheProvider CacheProvider;

        protected bool _CacheInitReady = false;
        /// <summary>
        /// 缓存载入就绪
        /// </summary>
        public bool CacheInitReady
        {
            get { return _CacheInitReady; }
            set { _CacheInitReady = value; }
        }

        /// <summary>
        /// 缓存重载事件
        /// </summary>
        public event CacheReloadEventHander FileCacheReloadEvent;



        /// <summary>
        /// 缓存移出事件
        /// </summary>
        public event CacheRemoveEventHander FileCacheRemoveEvent;

        private string mFilePath;

        protected string NameConfig4FilePath { get; set; }
        /// <summary>
        /// 指定路径 
        /// </summary>
        public string FilePath
        {
            get
            {
                //未指定FilePath的情况下，读取web.config当中的“LanguageCacheFilePath”
                if (String.IsNullOrEmpty(mFilePath))
                {
                    if (String.IsNullOrEmpty(NameConfig4FilePath))
                    {
                        return null;
                    }
                    string ConfingPath = Conf.GetConfigItem(NameConfig4FilePath);
                    if (String.IsNullOrEmpty(ConfingPath))
                    {
                        return null;
                    }

                    mFilePath = PathTools.GetFull(ConfingPath);
                }
                return mFilePath;
            }
            set
            {
                mFilePath = value;
                mFilePath = PathTools.GetFull(mFilePath);
            }
        }
        /// <summary>指定物理路径</summary>
        public string PhysicalFilePath
        {
            get { return mFilePath; }
            set
            {
                mFilePath = value;
            }
        }

        private string _CacheName = "File";

        public virtual string CacheName
        {
            get { return _CacheName; }
            set { _CacheName = value; }
        }

        public virtual string CacheKey { get => _CacheName;   }


        public FileCacheBase()
        {
            _InitCache();

        }
        public FileCacheBase(string filePath)
        {
            _InitCache();

            FilePath = filePath;
        }

        public FileCacheBase(string filePath, FilePathMode mode)
        {
            _InitCache();
            switch (mode)
            {
                case FilePathMode.ConfigPath:
                    NameConfig4FilePath = filePath;
                    break;
                case FilePathMode.PhysiccalPath:
                    PhysicalFilePath = filePath;
                    break;
                case FilePathMode.WebPath:
                    FilePath = filePath;
                    break;
            }
        }

        protected virtual void _InitCache()
        {
            //CacheReloadEvent += _DefaultReload;

            CacheProvider = Cache.CacheManager.Create();
            CacheProvider.CacheRemoveEvent += onCacheProvider_CacheRemoveEvent;
            //这个Load容易有误解，默认的构造函数当中,这方法当中Load时
            // 文件名还没有初始化，Load什么也做不了
            Load();
        }

        protected void onCacheProvider_CacheRemoveEvent(object o, CacheRemoveEventArgs args)
        {
            //CacheReloadEvent();

            //FileCacheReloadEvent?.Invoke();
            if(null != args)
            {
                //添加名称检查，防止无关的更新
                //添加原因判断，防止事件传递变成 无限循环
                if(args.CacheKey ==  this.CacheKey && args.Reason == CacheItemRemovedReason.DependencyChanged)
                {
                    Reload();
                    FileCacheReloadEvent?.Invoke();
                }
            }
        }

        private void _DefaultReload()
        {

        }

        protected virtual void Load()
        {
            //CacheReloadEvent();
        }
        public virtual void Reload()
        {
            x.Say("重新载入文件缓存。");
            //FileCacheReloadEvent?.Invoke();
        }

        

        /// <summary>清除缓存</summary>
        public void Clear()
        {
            CacheProvider.Remove(CacheKey);
            CacheInitReady = false;
        }
        public bool ExistFile()
        {

            return System.IO.File.Exists(FilePath);
        }


    }


    public enum FilePathMode
    {
        Default = 1,
        WebPath = 1,
        PhysiccalPath = 2,
        ConfigPath = 4
    }

}
