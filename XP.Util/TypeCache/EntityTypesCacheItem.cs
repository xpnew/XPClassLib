using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using XP.Comm.Filters;



namespace XP.Util.TypeCache
{
    public class EntityTypesCacheItem
    {
        private List<PropertyInfo> _PropertyArr;
        /// <summary>把对象的属性保存起来降低反射带来的性能损失</summary>
        public List<PropertyInfo> PropertyArr
        {
            get
            {
                if (null == _PropertyArr)
                {
                    _PropertyArr = new List<PropertyInfo>();
                }
                return _PropertyArr;
            }
            set { _PropertyArr = value; }
        }
        private Dictionary<string, PropertyInfo> _PropertyDic;

        /// <summary>
        /// 属性信息字典
        /// </summary>
        public Dictionary<string, PropertyInfo> PropertyDic
        {
            get
            {
                if (null == _PropertyDic)
                {
                    _PropertyDic = new Dictionary<string, PropertyInfo>();
                }
                return _PropertyDic;
            }
            set { _PropertyDic = value; }
        }

        private List<string> _EnumPropertyList;
        /// <summary>
        /// 枚举类型的属性列表
        /// </summary>
        /// <remarks>
        /// 添加日期：2014年3月27日
        /// 添加原因：
        /// SA.DataObjects.Mapper命名空间下的Model定义，将原来一部分Int类型的数据改成了枚举，所以在直接复制数据的时候会造成数据错误
        /// 所以，增加了这个列表，以便于在复制的时候对枚举类型的数据作特殊的处理
        /// 
        /// </remarks>
        public List<string> EnumPropertyList
        {
            get
            {
                if (null == _EnumPropertyList)
                    _EnumPropertyList = new List<string>();
                return _EnumPropertyList;

            }
            set
            {
                _EnumPropertyList = value;
            }
        }

        public List<string> ListPropertyList { get; set; }


        private List<string> _PropertyNames = new List<string>();

        public List<string> PropertyNames
        {
            get { return _PropertyNames; }
            set { _PropertyNames = value; }
        }


        private string _CacheName = "";
        public string CacheName
        {
            get { return _CacheName; }
            set { _CacheName = value; }
        }

        private object _Locker;

        public object Locker
        {
            get
            {
                if (null == _Locker)
                    _Locker = new object();
                return _Locker;
            }
            set { _Locker = value; }
        }
        public bool NeedGlobaled { get; set; }

        private Type _type;

        public Type Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public EntityTypesCacheItem()
        {
            NeedGlobaled = false;
        }
        public EntityTypesCacheItem(Type type)
            : this(type, false)
        {
        }
        public EntityTypesCacheItem(Type type, bool hasGlobel)
        {
            NeedGlobaled = hasGlobel;
            this.Type = type;
            _CacheName = _type.FullName;
            ListPropertyList = new List<string>();
            GetPropertyInfo();

        }



        /// <summary>获取属性的信息</summary>
        protected virtual void GetPropertyInfo()
        {
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
            }

        }
        public bool IsEnumProperty(string propertyName)
        {
            if (null == _EnumPropertyList || 0 == EnumPropertyList.Count)
                return false;
            return EnumPropertyList.Contains(propertyName);
        }

        public bool IsListProperty(string propertyName)
        {
            if (null == ListPropertyList || 0 == ListPropertyList.Count)
                return false;
            return ListPropertyList.Contains(propertyName);
        }

        /// <summary>
        /// 获得一个对象真实类型，支持从Nullable<>获取
        /// </summary>
        /// <param name="theType"></param>
        /// <returns></returns>
        public static Type GetRealType(Type theType)
        {
            Type realType = theType;
            // We need to check whether the property is NULLABLE
            if (theType.IsGenericType && theType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // If it is NULLABLE, then get the underlying type. eg if "Nullable<int>" then this will return just "int"
                realType = theType.GetGenericArguments()[0];
            }
            return realType;
        }

        /// <summary>
        /// 判断Nullable类型
        /// </summary>
        /// <param name="theType"></param>
        /// <returns></returns>
        public static bool IsNullableType(Type theType)
        {
            return (theType.IsGenericType && theType.
              GetGenericTypeDefinition().Equals
              (typeof(Nullable<>)));
        }

        public static bool IsEnumType(Type theType)
        {
            Type RealType = GetRealType(theType);
            if (RealType.IsEnum)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 判断一个类型是否是List<T>
        /// </summary>
        /// <param name="theType"></param>
        /// <returns></returns>
        public static bool IsListType(Type theType)
        {
            if (theType.IsGenericType && theType.GetGenericTypeDefinition() == typeof(List<>))
            {
                return true;
            }
            return false;
        }


    }
}
