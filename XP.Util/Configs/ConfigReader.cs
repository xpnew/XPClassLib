using XP.Util.FileCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XP.Util.Configs
{

    /// <summary>
    /// 自定义配置文件读功能的封装类，使用这个类的优点是修改了配置之后不需要重启整个程序
    /// </summary>
    /// <remarks>
    /// ▲▲▲注意！！！
    /// 需要在Web.config 或者 App.config当中指定接口Cache.ICacheProvider的具体实现类！！！
    /// </remarks>
    public class ConfigReader : XmlCache
    {

        private string NameOfCache = "SiteConfig";
        private string NameOfRoot = "root";
        private string NameOfSiteSetting = "SiteSet";
        private string NameOfUploadSetting = "UploadSet";
        private string NameOfRemoteServiceSetting = "RemoteServiceSet";
        private string _XmlFilePath = "~/App_Data/Site.config";//windows程序在~/App_Data/Run.config



        private string NameOfEntryGroup = "EntryGroups";
        /// <summary>
        /// 缓存更新时，相关类能获得通知
        /// </summary>
        public  Action<object, EventArgs> ChangedNotify;


        /// <summary>
        /// 定义一个值，表示返回整数选项时，格式错误
        /// </summary>
        public readonly int ErrorValueInt = -148756235;
        /// <summary>
        /// 定义一个值，表示返回整数选项时，值为空
        /// </summary>
        public readonly int NullValueInt = -15879624;


        public Dictionary<string, string> SiteSets { get; set; }


        private bool _HasLoadKeyGroup = false;
        private Dictionary<string, Dictionary<string, string>> _KeyGroupDict;
        public Dictionary<string, Dictionary<string, string>> KeyGroupDict
        {
            get
            {
                if (!_HasLoadKeyGroup)
                {
                    _InitKeyGroupDict();
                }
                return _KeyGroupDict;
            }

            set { _KeyGroupDict = value; }
        }

        /// <summary>
        /// 默认的构造函数，已经使用了单例模式，不允许直接使用
        /// </summary>
        protected ConfigReader()
            : base()
        {

            CacheName = NameOfCache;
            FilePath = GetConfigSet();
            base._InitXML();
            _Init();
        }

        protected string GetConfigSet()
        {
            string Set = Conf.GetConfigItem("SetConfigPath");
            if (String.IsNullOrEmpty(Set))
            {
                Set = _XmlFilePath;
            }
            return Set;
        }
        #region 快速单例模式

        public static readonly ConfigReader _Instance = new ConfigReader();

        public static ConfigReader Self
        {
            get { return _Instance; }
        }

        public static ConfigReader CreateInstance()
        {
            return _Instance;

        }

        #endregion
        protected virtual void _Init()
        {
            _InitCollection();

            FileCacheReloadEvent += OnConfigReload;
            //CacheProvider.CacheRemoveEvent += RemoveBack;
        }


        protected void _InitCollection()
        {
            lock (CacheName)
            {
                SiteSets = new Dictionary<string, string>();
                _HasLoadKeyGroup = false;
                _InitKeyGroupDict();
            }

        }

        protected void OnConfigReload()
        {
            x.Say("重新载入配置文件缓存。");

            _InitCollection();
            ChangedNotify?.Invoke(new object(), new EventArgs());

        }

        //public override void Reload()
        //{
        //    base.Reload();

        //}

        /// <summary>
        /// 缓存移除时的通知
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="reason"></param>
        //protected override void RemovedCallback(string key, object value, System.Web.Caching.CacheItemRemovedReason reason)
        //{
        //    base.RemovedCallback(key, value, reason);
        //    //重新载入数据不应该放在这里。。。。
        //    //_InitCollection();
        //    lock (CacheName)
        //    {
        //        _HasLoadKeyGroup = false;
        //        KeyGroupDict = new Dictionary<string, Dictionary<string, string>>();
        //    }
        //}

        protected void RemoveBack()
        {
            lock (CacheName)
            {
                _HasLoadKeyGroup = false;
                KeyGroupDict = new Dictionary<string, Dictionary<string, string>>();
            }
        }


        /// <summary>
        /// 根据名称获取一个设置项
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        public string GetSet(string setName)
        {
            return GetSetting(NameOfSiteSetting, setName);
        }


        public string GetRealVal(string setName)
        {
            var val = GetVal(NameOfSiteSetting, setName);
            if (null == val)
            {
                return null;
            }
            var a = val.Attribute("value");
            if (null != a)
            {
                return a.Value;
            }
            return null;
        }

        public int GetInt(string setName, int? def = null)
        {
            string set = GetSet(setName);
            if (String.IsNullOrWhiteSpace(set))
            {
                if (def.HasValue) return def.Value;
                return NullValueInt;
            }
            if (GeneralTool.IsInt(set))
            {

                return int.Parse(set);
            }
            else
            {
                if (def.HasValue) return def.Value;
                return ErrorValueInt;
            }
        }
        public long GetLong(string setName, long? def = null)
        {
            string set = GetSet(setName);
            if (String.IsNullOrWhiteSpace(set))
            {
                if (def.HasValue) return def.Value;
                return NullValueInt;
            }
            if (GeneralTool.IsInt(set))
            {

                return long.Parse(set);
            }
            else
            {
                if (def.HasValue) return def.Value;
                return ErrorValueInt;
            }
        }
        /// <summary>
        /// 获取一个Boolean类型的配置项
        /// </summary>
        /// <param name="setName"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public bool GetBool(string setName, bool def = false)
        {
            string set = GetSet(setName);
            if (String.IsNullOrWhiteSpace(set))
            {
                return def;
            }
            set = set.ToLower();
            if ("1" == set || "true" == set)
            {
                return true;
            }
            return false;
        }


        public string GetSetting(string setTypeName, string setName)
        {
            if (String.IsNullOrEmpty(setName))
                return String.Empty;
            if (null == LinqCache)
            {
                return String.Empty;
            }
            var setting = from s in LinqCache.Root.Descendants(setTypeName)
                          select s;
            if (setting.Any())
            {
                var find = from item in setting.First().Descendants("add")
                           where null != item.Attribute("name") &&  item.Attribute("name").Value.Equals(setName)
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


        public XElement GetVal(string setTypeName, string setName)
        {
            if (String.IsNullOrEmpty(setName))
                return null;
            if (null == LinqCache)
            {
                return null;
            }
            var setting = from s in LinqCache.Root.Descendants(setTypeName)
                          select s;
            if (setting.Any())
            {
                var find = from item in setting.First().Descendants("add")
                           where null != item.Attribute("name") && item.Attribute("name").Value.Equals(setName)
                           select item;
                if (find.Any())
                {
                    return find.First();
                }
            }
            return null;
        }


        /// <summary>
        /// 获取模板文本
        /// </summary>
        /// <remarks>
        /// ITemplate是将来要实现的目标
        /// </remarks>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetTemplateText(string key)
        {
            string Result = String.Empty;
            if (String.IsNullOrEmpty(key))
                return Result;
            if (null == LinqCache)
            {
                return Result;
            }
            var setting = from s in LinqCache.Root.Descendants("templates")
                          select s;
            if (setting.Any())
            {
                var find = from item in setting.First().Descendants("item")
                           where null != item.Attribute("name") && item.Attribute("name").Value.Equals(key)
                           select item;
                if (find.Any())
                {
                    var a = find.First();
                    if (null != a)
                    {
                        return a.Value;
                    }
                }

            }

            return Result;
        }

        //public ITemplate GetTemplate(string key)
        //{

        //}

        public List<XElement> GetList(string groupName)
        {
            if (String.IsNullOrEmpty(groupName))
                return null;
            if (null == LinqCache)
            {
                return null;
            }
            var groups = from s in LinqCache.Root.Descendants(NameOfEntryGroup)
                    
                          select s;

            if (groups.Any())
            {
                var find = from item in groups.First().Descendants(groupName)
                           select item;

                if (find.Any())
                {
                    return find.Elements().ToList();
                }
            }

            return null;
        }

        #region 分组模板
        /// <summary>
        /// 获取分组模板中的一个模板
        /// </summary>
        /// <remarks>注意：没找到模板返回的是空字符串</remarks>
        /// <param name="groupName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetTemplate(string groupName, string key)
        {
            var groups = from g in LinqCache.Root.Descendants("TemplateGroups")
                         select g;
            if (groups.Any())
            {
                IEnumerable<string> element = from t in groups.First().Descendants(groupName).Descendants("template")
                                              where t.Attribute("key").Value == key
                                              select t.Value;
                if (element.Any())
                {
                    return element.First();
                }
            }
            return String.Empty;
        }



        #endregion

        #region 分组设置

        protected void _InitKeyGroupDict()
        {
            lock (CacheName)
            {
                if (_HasLoadKeyGroup) { return; }
                _HasLoadKeyGroup = true;
            }
            _KeyGroupDict = new Dictionary<string, Dictionary<string, string>>();
            var kg = from s in LinqCache.Root.Descendants("KeyGroup")
                     select s;
            if (!kg.Any())
            {
                return;
            }

            foreach (var Group in kg.First().Descendants())
            {
                string GroupKey = Group.Name.LocalName;
                Dictionary<string, string> Dict = new Dictionary<string, string>();

                var coll = from s in Group.Descendants("add")
                           select s;
                if (!coll.Any())
                {
                    continue;
                }
                foreach (var sub in coll)
                {
                    if (null == sub.Attribute("name"))
                    {
                        continue;
                    }
                    string SetKey = sub.Attribute("name").Value;
                    if (0 == SetKey.Length)
                    {
                        continue;
                    }
                    string SetValue = null;
                    if (null != sub.Attribute("value"))
                    {
                        SetValue = sub.Attribute("value").Value;
                    }
                    Dict.Add(SetKey, SetValue);
                }
                _KeyGroupDict.Add(GroupKey, Dict);
            }
        }

        public bool ExistGroup(string groupName)
        {
            if (!_HasLoadKeyGroup)
            {
                _InitKeyGroupDict();
            }
            if (KeyGroupDict.ContainsKey(groupName))
            {
                return true;
            }
            return false;
        }

        public bool ExistKeyGroupSet(string groupName, string setName)
        {
            if (!_HasLoadKeyGroup)
            {
                _InitKeyGroupDict();
            }

            if (KeyGroupDict.ContainsKey(groupName))
            {
                var dict = KeyGroupDict[groupName];
                if (dict.ContainsKey(setName))
                {
                    return true;
                }
            }

            return false;
        }
        public int GetKeyGroupCount(string groupName)
        {
            if (KeyGroupDict.ContainsKey(groupName))
            {
                var dict = KeyGroupDict[groupName];
                return dict.Count;
            }
            return 0;
        }

        public string GetKeyGroupValue(string groupName, string setName)
        {
            if (!_HasLoadKeyGroup)
            {
                _InitKeyGroupDict();
            }

            if (KeyGroupDict.ContainsKey(groupName))
            {
                var dict = KeyGroupDict[groupName];
                if (dict.ContainsKey(setName))
                {
                    return dict[setName];
                }
            }
            return String.Empty;
        }
        public int GetKeyGroupInt(string groupName, string setName, int? def = null)
        {
            if (KeyGroupDict.ContainsKey(groupName))
            {
                var dict = KeyGroupDict[groupName];
                if (dict.ContainsKey(setName))
                {
                    string SetValue = dict[setName];
                    if (GeneralTool.IsInt(SetValue))
                    {
                        return int.Parse(SetValue);
                    }
                    else
                    {
                        if (def.HasValue) return def.Value;
                        return Comm.Constant.ErrorInt;
                    }
                }
                if (def.HasValue) return def.Value;
                return Comm.Constant.NotExistInt;
            }
            if (def.HasValue) return def.Value;
            return Comm.Constant.NotExistInt;
        }

        public long GetKeyGroupLong(string groupName, string setName, long? def = null)
        {
            if (KeyGroupDict.ContainsKey(groupName))
            {
                var dict = KeyGroupDict[groupName];
                if (dict.ContainsKey(setName))
                {
                    string SetValue = dict[setName];
                    if (GeneralTool.IsInt(SetValue))
                    {
                        return long.Parse(SetValue);
                    }
                    else
                    {
                        if (def.HasValue) return def.Value;
                        return Comm.Constant.ErrorLong;
                    }
                }
                if (def.HasValue) return def.Value;
                return Comm.Constant.NotExistLong;
            }
            if (def.HasValue) return def.Value;
            return Comm.Constant.NotExistLong;
        }

        public bool GetKeyGroupBool(string groupName, string setName, bool def = false)
        {
            var v = GetKeyGroupValue(groupName, setName);
            if (null == v)
            {
                return def;
            }
            if (v.Length == 0)
            {
                return def;
            }
            if ("1" == v || "true" == v.ToLower())
            {
                return true;
            }
            return false;
        }

        public decimal GetKeyGroupDecimal(string groupName, string setName, decimal def = 0.0m)
        {
            var v = GetKeyGroupValue(groupName, setName);
            if (null == v)
            {
                return def;
            }
            if (v.Length == 0)
            {
                return def;
            }
            decimal tem = 0.0m;
            if (Decimal.TryParse(v, out tem))
            {
                return tem;
            }
            else
            {
                return def;
            }
        }

        public string GetKeyGroupSetting(string groupName, string setName)
        {
            if (String.IsNullOrEmpty(groupName))
                return String.Empty;

            if (String.IsNullOrEmpty(setName))
                return String.Empty;
            if (null == LinqCache)
            {
                return String.Empty;
            }



            var kg = from s in LinqCache.Root.Descendants("KeyGroup")
                     select s;
            if (!kg.Any())
            {
                return String.Empty;
            }
            var GroupItem = from s in kg.First().Descendants(groupName)
                            select s;
            if (!GroupItem.Any())
            {
                return String.Empty;
            }

            var setting = from s in GroupItem.First().Descendants(groupName)
                          select s;
            if (setting.Any())
            {
                var find = from item in setting.First().Descendants("add")
                           where null != item.Attribute("name") &&  item.Attribute("name").Value.Equals(setName)
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
