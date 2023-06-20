using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Util.TypeCache
{
    public class EntityCopyUtil<FromEntity, ToEntity> : EntityPropertyCopy<FromEntity, ToEntity>
        where FromEntity : class
        where ToEntity : class, new()
    {

        private DynamicMethod<FromEntity> _FromGeter;
        private DynamicMethod<ToEntity> _ToSeter;

        public EntityCopyUtil() : base()
        {
            //FromType = typeof(FromEntity);
            //ToType = typeof(ToEntity);
            //EntityTypesCache Cache = EntityTypesCache.CreateInstance();
            //FromCache = Cache.GetItem(FromType);
            //ToCache = Cache.GetItem(ToType);
            //FromPropertyNames = FromCache.PropertyNames;
            //ToPropertyNames = ToCache.PropertyNames;


            //_FromGeter = new DynamicMethod<FromEntity>();
            //_ToSeter = new DynamicMethod<ToEntity>();

        }

        public ToEntity CopyEntity(FromEntity source)
        {

            if (null == source)
            {
                return null;
            }
            ToEntity Result = new ToEntity();

            CopyToEntity(source, Result);
            //var CommonPorpertyNames = FromPropertyNames.Where(o => ToPropertyNames.Contains(o));

            //foreach (string PropertyName in CommonPorpertyNames)
            //{
            //    if (FromCache.ClassPropertyList.Contains(PropertyName))
            //    {
            //        if (ToCache.ClassPropertyList.Contains(PropertyName))
            //        { 
            //            var FromPropertyType = FromCache.PropertyTypeDict[PropertyName];
            //            var ToPropertyType = ToCache.PropertyTypeDict[PropertyName];
            //            if (FromPropertyType != ToPropertyType)
            //            {
            //                //var Copier = new EntityCopyUtil<FromPropertyType, ToPropertyType>();
            //                //还有一些问题没有解决，先跳过
            //                //var tcu = new TypeCopyUtil(FromPropertyType, ToPropertyType);

            //                //_ToSeter.SetValue(output, PropertyName, tcu.CopyEntity(_FromGeter.GetValue(source, PropertyName)));
            //                continue;
            //            }
            //            else
            //            {

            //            }
            //        }
            //        else
            //        {
            //            continue;
            //        }
            //    }
            //    else
            //    {
            //        if (ToCache.ClassPropertyList.Contains(PropertyName))
            //        {
            //            continue;
            //        }
            //    }
            //    object NewValue = GetToPropertyValue(source, PropertyName);
            //    if (ToPropertyNames.Exists(o => o == PropertyName))
            //    {
            //        _ToSeter.SetValue(output, PropertyName, _FromGeter.GetValue(source, PropertyName));
            //    }
            //}





            return Result;
        }
    }
}
