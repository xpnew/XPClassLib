using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using XP.Comm.Filters;

namespace XP.Util.TypeCache
{
    public class EntityPropertyCopy<FromEntity, ToEntity>
        where FromEntity : class
        where ToEntity : class
    {
        public Type FromType { get; set; }
        public Type ToType { get; set; }

        public List<string> FromPropertyNames { get; set; }
        public List<string> ToPropertyNames { get; set; }


        public EntityTypesCacheItem FromCache { get; set; }
        public EntityTypesCacheItem ToCache { get; set; }



        private DynamicMethod<FromEntity> _FromGeter;
        private DynamicMethod<ToEntity> _ToSeter;

        public EntityPropertyCopy()
        {
            FromType = typeof(FromEntity);
            ToType = typeof(ToEntity);
            EntityTypesCache Cache = EntityTypesCache.CreateInstance();
            FromCache = Cache.GetItem(FromType);
            ToCache = Cache.GetItem(ToType);
            FromPropertyNames = FromCache.PropertyNames;
            ToPropertyNames = ToCache.PropertyNames;


            _FromGeter = new DynamicMethod<FromEntity>();
            _ToSeter = new DynamicMethod<ToEntity>();
        }



        public void CopyToEntity(FromEntity source, ToEntity output)
        {
            if (null == source)
            {
                return;
            }
            var CommonPorpertyNames = FromPropertyNames.Where(o => ToPropertyNames.Contains(o));

            foreach (string PropertyName in CommonPorpertyNames)
            {
                if (FromCache.ClassPropertyList.Contains(PropertyName))
                {
                    if (ToCache.ClassPropertyList.Contains(PropertyName))
                    {
                        var FromPropertyType = FromCache.PropertyTypeDict[PropertyName];
                        var ToPropertyType = ToCache.PropertyTypeDict[PropertyName];
                        if (FromPropertyType != ToPropertyType)
                        {

                            if (typeof(string) == FromPropertyType || typeof(string) == ToPropertyType)
                            {
                                continue;
                            }
                            //var Copier = new EntityCopyUtil<FromPropertyType, ToPropertyType>();
                            //还有一些问题没有解决，先跳过
                            //var tcu = new TypeCopyUtil(FromPropertyType, ToPropertyType);
                            object NewProperInstance = Activator.CreateInstance(ToPropertyType);
                            EntityList.CopyToType(FromPropertyType, ToPropertyType, _FromGeter.GetValue(source, PropertyName), NewProperInstance);
                            _ToSeter.SetValue(output, PropertyName, NewProperInstance);
                            //_ToSeter.SetValue(output, PropertyName, tcu.CopyEntity(_FromGeter.GetValue(source, PropertyName)));
                            continue;
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    if (ToCache.ClassPropertyList.Contains(PropertyName))
                    {
                        continue;
                    }
                }
                object NewValue = GetToPropertyValue(source, PropertyName);
                if (ToPropertyNames.Exists(o => o == PropertyName))
                {
                    _ToSeter.SetValue(output, PropertyName, _FromGeter.GetValue(source, PropertyName));
                }
            }

        }
        public object GetToPropertyValue(FromEntity source, string propertyName)
        {
            object Result;
            if (FromCache.IsEnumProperty(propertyName))
            {
                var EnumValue = _FromGeter.GetValue(source, propertyName);
                //x.Say(" 发现了枚举类型的数据：" + propertyName);
                Result = Convert.ToInt32(EnumValue);
            }
            else if (FromCache.IsListProperty(propertyName))
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
