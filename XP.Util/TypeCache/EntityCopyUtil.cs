using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Util.TypeCache
{
    public class EntityCopyUtil<FromEntity, ToEntity>
        where FromEntity : class
        where ToEntity : class ,new()
    {

        public Type FromType { get; set; }
        public Type ToType { get; set; }

        public List<string> FromPropertyNames { get; set; }
        public List<string> ToPropertyNames { get; set; }

        private DynamicMethod<FromEntity> _FromGeter;
        private DynamicMethod<ToEntity> _ToSeter;

        public EntityCopyUtil()
        {
            FromType = typeof(FromEntity);
            ToType = typeof(ToEntity);
            EntityTypesCache Cache = EntityTypesCache.CreateInstance();
            FromPropertyNames = Cache.GetItem(FromType).PropertyNames;
            ToPropertyNames = Cache.GetItem(ToType).PropertyNames;

            _FromGeter = new DynamicMethod<FromEntity>();
            _ToSeter = new DynamicMethod<ToEntity>();

        }

        public ToEntity CopyEntity(FromEntity source)
        {

            if (null == source)
            {
                return null;
            }
            ToEntity Result = new ToEntity();

            foreach (string PropertyName in FromPropertyNames)
            {
                if (ToPropertyNames.Exists(o => o == PropertyName))
                {
                    _ToSeter.SetValue(Result, PropertyName, _FromGeter.GetValue(source, PropertyName));
                }
            }





            return Result;
        }

    }
}
