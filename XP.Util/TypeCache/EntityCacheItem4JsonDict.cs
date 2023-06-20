using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XP.Comm.Attributes;
using XP.Comm.Filters;

namespace XP.Util.TypeCache
{

    /// <summary>
    /// 准备映射成字典再传Josn处理的缓存项
    /// </summary>
    public class EntityCacheItem4JsonDict : EntityTypesCacheItem
    {
        public EntityCacheItem4JsonDict() : base()
        {
            _Init();

        }

        public EntityCacheItem4JsonDict(Type type) : base(type)
        {
            _Init();
        }

        public EntityCacheItem4JsonDict(Type type, bool hasGlobel) : base(type, hasGlobel)
        {
            _Init();
        }

        protected void _Init()
        {
            if (null == AliasNameDict)
                AliasNameDict = new Dictionary<string, string>();
        }

        public Dictionary<string, string> AliasNameDict { get; set; }


        public List<string> SkipColumnsList { get; set; }

        protected override void GetPropertyInfo()
        {
            AliasNameDict = new Dictionary<string, string>();
            SkipColumnsList = new List<string>();
            //base.GetPropertyInfo();
            PropertyInfo[] entityProperties = Type.GetProperties();

            foreach (PropertyInfo property in entityProperties)
            {
                if (NotDataFilterAttribute.IsDefined(property))
                {
                    continue;
                }
                //if ("UserVSRight" == property.Name)
                //{
                //    PropertyInfo[] myPropertyInfo;
                //    // Get the properties of 'Type' class object.
                //    myPropertyInfo = property.PropertyType.GetProperties();
                //    x.Say("Properties of UserVSRight are:");
                //    for (int i = 0; i < myPropertyInfo.Length; i++)
                //    {
                //        x.Say(myPropertyInfo[i].ToString());
                //    }
                //}

                //if (property.PropertyType.IsEnum)
                //{
                //    EnumPropertyList.Add(property.Name);
                //}
                if (IsEnumType(property.PropertyType))
                {
                    EnumPropertyList.Add(property.Name);
                }
                if (IsListType(property.PropertyType))
                {
                    ListPropertyList.Add(property.Name);
                }
                PropertyArr.Add(property);
                PropertyNames.Add(property.Name);
                PropertyDic.Add(property.Name, property);


                Type CurrentPropertyType = GetRealType(property.PropertyType);
                if (CurrentPropertyType.IsClass)
                {
                    ClassPropertyList.Add(property.Name);
                }
                PropertyTypeDict.Add(property.Name, CurrentPropertyType);
                string AlaisName = GetAlaisName(property);
                if (IsSkip(property))
                {
                    SkipColumnsList.Add(AlaisName);
                }
                if (AliasNameDict.ContainsKey(AlaisName))
                {
                    continue;
                }
                AliasNameDict.Add(AlaisName, property.Name);

            }


        }

        protected string GetAlaisName(PropertyInfo p)
        {
            object[] attrsAllows = p.GetCustomAttributes(typeof(JsonAlaisKeyAttribute), true);
            if (0 < attrsAllows.Length)
            {
                var jsk = attrsAllows[0] as JsonAlaisKeyAttribute;
                if (!String.IsNullOrEmpty(jsk.AliasName))
                {
                    return jsk.AliasName;
                }
            }
            return p.Name;
        }

        protected bool IsSkip(PropertyInfo p)
        {
            object[] attrsAllows = p.GetCustomAttributes(typeof(JsonSkipColumnAttribute), true);
            if (0 < attrsAllows.Length)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 将实体数据转化为字典对象
        /// </summary>
        /// <typeparam name="FromEntity"></typeparam>
        /// <param name="from">来源实体，保存数据的实例</param>
        /// <param name="isSkipFlag">是否跳过JsonSkip特性标记的属性</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetDict<FromEntity>(FromEntity from, bool isSkipFlag = false)
            where FromEntity : class
        {

            var result = new Dictionary<string, object>();

            var FromType = typeof(FromEntity);

            EntityTypesCache Cache = EntityTypesCache.CreateInstance();

            EntityCacheItem4JsonDict EntityDict;
            if (Cache.Exist(FromType))
            {
                var FromCache = Cache.GetItem(FromType);
                if ((FromCache is EntityCacheItem4JsonDict))
                {
                    EntityDict = FromCache as EntityCacheItem4JsonDict;
                }
                else
                {
                    EntityDict = new EntityCacheItem4JsonDict(FromType);
                    Cache.AddCache(FromType.FullName, EntityDict);
                }
            }
            else
            {
                EntityDict = new EntityCacheItem4JsonDict(FromType);
                Cache.AddCache(FromType.FullName, EntityDict);
            }



            var dict = EntityDict.AliasNameDict;
            var _FromGeter = new DynamicMethod<FromEntity>();
            var SkipList = new List<string>();
            if (isSkipFlag)
            {
                SkipList = EntityDict.SkipColumnsList;
            }


            List<string> WorkKeys = dict.Keys.Except(SkipList).ToList();


            foreach (var k in WorkKeys)
            {
                string colname = dict[k];
                object v = GetPropertyValue<FromEntity>(from, _FromGeter, EntityDict, colname);
                result.Add(k, v);
            }


            return result;
        }

        public static object GetPropertyValue<FromEntity>(FromEntity source, DynamicMethod<FromEntity> _FromGeter, EntityTypesCacheItem cacheItme, string propertyName)
        {
            object Result;

            if (cacheItme.IsEnumProperty(propertyName))
            {
                var EnumValue = _FromGeter.GetValue(source, propertyName);
                //x.Say(" 发现了枚举类型的数据：" + propertyName);
                Result = Convert.ToInt32(EnumValue);
            }
            else if (cacheItme.IsListProperty(propertyName))
            {
                //x.Say(" 发现了List类型的数据：" + propertyName);
                Result = null;
            }
            else
            {
                Result = _FromGeter.GetValue(source, propertyName);

            }

            return Result;

        }


    }
}
