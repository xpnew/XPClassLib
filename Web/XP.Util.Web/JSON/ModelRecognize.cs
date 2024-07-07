using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using XP.Util.TypeCache;

namespace XP.Util.JSON
{
   
    /// <summary>
    /// Model识别程序
    /// </summary>
    /// <typeparam name="ToEntity"></typeparam>
    public class ModelRecognize<ToEntity> where ToEntity : class,new()
    {

        /// <summary>
        /// 查找到Model属性的统计
        /// </summary>
        public int FoundNamesCount { get; set; }
        public Type ToType { get; set; }
        public List<string> ToPropertyNames { get; set; }

        public List<string> AllowColumnNames { get; set; }
        private DynamicMethod<ToEntity> _ToSeter;
        public EntityTypesCacheItem ToCache { get; set; }

        private HttpRequest _Request;
        public HttpRequest Request
        {
            get
            {
                if(null == _Request)
                    _Request = HttpContext.Current.Request;
                return _Request;
            }
            set
            {
                _Request = value;
            }
        }

        /// <summary>
        /// 排除的名字
        /// </summary>
        public List<string> ExcludeNames { get; set; }

        /// <summary>
        /// 指定Request当中使用的Model名称
        /// </summary>
        public string ModelName { get; set; }

        public ModelRecognize()
        {
            Init();

        }
        public ModelRecognize(string modelName):this()
        {
            ModelName = modelName;

        }

        protected void Init()
        {

            ToType = typeof(ToEntity);
            EntityTypesCache Cache = EntityTypesCache.CreateInstance();
            ToCache = Cache.GetItem(ToType);
            ToPropertyNames = ToCache.PropertyNames;

            _ToSeter = new DynamicMethod<ToEntity>();
            ExcludeNames = new List<string>();
            FoundNamesCount = 0;
        }


        public string GetRequest(string propertyName)
        {

            string RequestName;

            string Result = null;
            if (null != ModelName)
            {

                RequestName = ModelName + "[" + propertyName + "]";
            }
            else
            {
                RequestName = propertyName;
            }

            if (null != Request[RequestName] && String.Empty != Request[RequestName])
            {
                Result = Request[RequestName];
            }
            return Result;
        }


        public ToEntity GetModel()
        {
            ToEntity Result = new ToEntity();
            var AllowNames = ToPropertyNames.Where(o => !ExcludeNames.Contains(o));
            foreach (var PropertyName in AllowNames)
            {
                if (ToCache.IsListProperty(PropertyName))
                {
                    continue;
                }
                var StingValue = GetRequest(PropertyName);
                if (null == StingValue)
                    continue;
                try
                {
                    Type PropertyType = ToCache.PropertyDic[PropertyName].GetType();
                   
                    SetPropertyValue(Result, PropertyName, StingValue);
                    FoundNamesCount++;
                }
                catch (Exception ex)
                {
                    x.Say(String.Format("设置属性[{0}]的值为[{1}]时候，出现了异常：{2}", PropertyName, StingValue, ex.Message));

                }

            }



            return Result;
        }

        public void SetPropertyValue(ToEntity target, string propertyName, string strValue)
        {
            var property = ToCache.PropertyDic[propertyName];
            Type PropertyType = EntityTypesCacheItem.GetRealType(property.PropertyType);

            if (ToCache.IsEnumProperty(propertyName))
            {
                var EnumValue = System.Enum.Parse(PropertyType, strValue);
                _ToSeter.SetValue(target, propertyName, EnumValue);
                return;
            }
            if (PropertyType == typeof(string))
            {
                _ToSeter.SetValue(target, propertyName, strValue);
                return;
            }
            if (EntityTypesCacheItem.IsNullableType(PropertyType))
            {
                var value = new NullableConverter(PropertyType).ConvertFrom(strValue);
                _ToSeter.SetValue(target, propertyName, value);
                return;
            }
            var SimplyValue = Convert.ChangeType(strValue, PropertyType);
            _ToSeter.SetValue(target, propertyName, SimplyValue);

        }

    }
}
