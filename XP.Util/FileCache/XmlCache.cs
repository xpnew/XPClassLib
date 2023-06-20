using XP.Util.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XP.Util.FileCache
{
    public class XmlCache : FileCacheBase
    {
        private bool _HasInit = false;

        public String LinqCacheName
        {
            get
            {
                return CacheName + "_LinQ";
            }
        }

        public override string CacheKey { get => LinqCacheName; }

        public XmlCache()
            : base()
        {

        }
        public XmlCache(string filePath)
            : base(filePath)
        {
        }
        public XmlCache(string filePath, FilePathMode mode)
            : base(filePath, mode)
        {
        }



        protected override void _InitCache()
        {
            base._InitCache();
            _HasInit = true;

            //默认的构造函数没有文件地址，初始化xml的过程是无效的。
            //lock (CacheName)
            //{
            //    _InitXML();
            //}

            //FileCacheReloadEvent += OnXmlCacheRemove;
        }


        //protected virtual void OnXmlCacheRemove()
        //{
        //    //Reload();
        //}

        public override void Reload()
        {

            base.Reload();

            _InitXML();
        }

        #region 具体功能实现


        public XDocument LinqCache
        {
            get
            {
                return GetLinqCache();
                //if (_HasInit)
                //{
                //    return GetLingCache();

                //}
                //else
                //{
                //    lock (CacheName)
                //    {
                //        return GetLingCache();
                //    }
                //}
            }
        }
        private XDocument GetLinqCache()
        {

            if (null == CacheProvider)
            {
                _CacheInitReady = false;
                return null;
            }
            if (null == CacheProvider[LinqCacheName])
            {
                return _InitXML();
            }
            else
            {
                XDocument xmlDoc = CacheProvider[LinqCacheName] as XDocument;
                if (null != xmlDoc)
                {
                    _CacheInitReady = true;
                }
                return xmlDoc;
            }
        }


        protected XDocument _InitXML()
        {
            x.Say("准备初始化xml文件缓存。");

            _CacheInitReady = false;
            if (null == CacheProvider)
            {
                return null;
            }
            if (!System.IO.File.Exists(FilePath))
            {
                string ErrorStr = "文件没有找到 【" + FilePath + " 】。准备加载缓存文件失败。";
                XP.Loger.Error(ErrorStr);

                throw new ArgumentNullException(ErrorStr);
                return null;
            }

            try
            {
                XDocument xd = XDocument.Load(FilePath);
                _InitSelf(xd);

                CacheProvider.Insert(LinqCacheName, xd, FilePath);

                //Reload();
                _CacheInitReady = true;
                return xd;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// 获取/刷新文档以后，初始化自身参数
        /// </summary>
        /// <param name="xd"></param>
        private void _InitSelf(XDocument xd)
        {
            _InitSlidingExpiration(xd);
        }
        /// <summary>
        /// 初始化滑动过期时间
        /// </summary>
        /// <param name="xd"></param>
        private void _InitSlidingExpiration(XDocument xd)
        {
            string SetType = "Self";
            string SetName = "SlidingExpiration";
            string SetValue = GetSetting(xd, SetType, SetName);
            int Defualt = CacheProvider.SlidingExpiration;
            int New = Defualt;
            if (!String.IsNullOrEmpty(SetValue))
            {
                New = TransInt(SetValue, Defualt);
            }
            CacheProvider.SlidingExpiration = New;
        }
        #endregion


        #region 解析 xml配置文件


        private int TransInt(string input, int def = 0)
        {
            if (GeneralTool.IsInt(input))
            {
                return int.Parse(input);
            }
            else
            {
                return def;
            }

        }

        public string GetSetting(XDocument xd, string setTypeName, string setName)
        {
            if (String.IsNullOrEmpty(setName))
                return String.Empty;
            if (null == xd)
            {
                return String.Empty;
            }
            var setting = from s in xd.Root.Descendants(setTypeName)
                          select s;
            if (setting.Any())
            {
                var find = from item in setting.First().Descendants("add")
                           where item.Attribute("name").Value.Equals(setName)
                           select item;
                if (find.Any())
                {
                    var a = find.First().Attribute("value");
                    if (null != a)
                    {
                        return a.Value;
                    }
                }
            }
            return String.Empty;
        }

        #endregion

    }
}
