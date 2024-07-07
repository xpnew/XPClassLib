using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using XP.Util.TypeCache;

namespace XP.Util
{
    public class DataTable2Entity<ToEntity>
                where ToEntity : class ,new()
    {

        public Type ToType { get; set; }
        public List<string> ToPropertyNames { get; set; }

        public List<string> AllowColumnNames { get; set; }
        private DynamicMethod<ToEntity> _ToSeter;
        public EntityTypesCacheItem ToCache { get; set; }

        public DataTable DbData { get; set; }

        public DataTable2Entity()
        {
            ToType = typeof(ToEntity);
            EntityTypesCache Cache = EntityTypesCache.CreateInstance();
            ToCache = Cache.GetItem(ToType);
            ToPropertyNames = ToCache.PropertyNames;

            _ToSeter = new DynamicMethod<ToEntity>();

        }


        public ToEntity CopyData(DataTable dt)
        {
            ToEntity NullModel = new ToEntity();
            if (null == dt)
            {
                return null;
            }
            if (0 == dt.Rows.Count)
            {
                return NullModel;
            }
            return CopyData(dt.Rows[0]);
        }


        public List<ToEntity> CopyList(DataTable dt)
        {
            List<ToEntity> Result = new List<ToEntity>();
            foreach (DataRow row in dt.Rows)
            {

                ToEntity NewLine = CopyData(row);

                Result.Add(NewLine);

            }

            return Result;
        }

        public ToEntity CopyData(DataRow source)
        {
            if (null == source)
            {
                return null;
            }


            ToEntity Result = new ToEntity();

            var AllowColumn = source.Table.Columns;
            var colList = ToPropertyNames.Where(o => AllowColumn.Contains(o));

            foreach (string PropertyName in colList)
            {
                object NewValue = GetRowValue(source, PropertyName);
                //    continue;
                try
                {
                    if (null != NewValue)
                        SetPropertyValue(Result, PropertyName, NewValue);
                }
                catch (Exception ex)
                {
                    x.Say(String.Format("设置属性[{0}]的值为[{1}]时候，出现了异常：{2}", PropertyName, NewValue, ex.Message));

                }


            }

            return Result;

        }
        public void SetPropertyValue(ToEntity target, string propertyName, object newValue)
        {

            if (ToCache.IsEnumProperty(propertyName))
            {
                var property = ToCache.PropertyDic[propertyName];
                Type PropertyType = EntityTypesCacheItem.GetRealType(property.PropertyType);
                var EnumValue = System.Enum.Parse(PropertyType, newValue.ToString());
                _ToSeter.SetValue(target, propertyName, EnumValue);
                return;
            }
            _ToSeter.SetValue(target, propertyName, newValue);

        }



        public object GetRowValue(DataRow row, string columnName)
        {

            object Result;
            if (DBNull.Value == row[columnName])
            {
                return null;
            }

            Result = row[columnName];
            return Result;
        }
    }
}
