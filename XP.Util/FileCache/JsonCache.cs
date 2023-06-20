using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Util.Path;

namespace XP.Util.FileCache
{
    public class JsonCache : FileCacheBase
    {

        private string _CacheName = string.Empty;
        public override string CacheName { get => base.CacheName; set => base.CacheName = value; }

        internal JsonCache(string path) : base(path)
        {
            _CacheName = PhysicalFilePath;
            lock (_CacheName)
            {
                //base._InitCache();
                CacheProvider.Insert(CacheKey, this, FilePath);
            }
            LoadJson();
        }

        #region 快速单例模式

        //public static readonly JsonCache _Instance = new JsonCache();

        //public static JsonCache Self
        //{
        //    get { return _Instance; }
        //}

        public static JsonCache GetJsonCache(string path)
        {
            string realpath = PathTools.GetFull(path);
            lock (realpath)
            {
                var CacheProvider = Cache.CacheManager.Create();
                if (null == CacheProvider.GetCache(realpath))
                {
                    return new JsonCache(path);
                }

                var obj = CacheProvider.GetCache(realpath);
                if (obj is JsonCache)
                {
                    var _Instance = obj as JsonCache;
                    return _Instance;
                }
                else
                {
                    return new JsonCache(path);

                }

            }


        }

        #endregion



        public string Json { get => _JsonContext; }


        private string _JsonContext = String.Empty;

        public string GetJson()
        {
            return Json;
        }


        //protected override void _InitCache()
        //{
        //    lock (CacheName)
        //    {
        //        base._InitCache();
        //        CacheProvider.Insert(CacheKey, this, FilePath);
        //    }
        //}



        protected virtual void LoadJson()
        {
            //base.Load();
            if (ExistFile())
            {
                var sw = System.IO.File.ReadAllText(FilePath, Encoding.UTF8);

                _JsonContext = sw;

            }
        }

        public override void Reload()
        {
            LoadJson();
        }
    }
}
