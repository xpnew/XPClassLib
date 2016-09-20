using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;

namespace XP.DB.Comm
{

    /// <summary>
    /// 数据库列映射单项
    /// </summary>
    public class ColumnDtoItem
    {

        public string ColumnName { get; set; }

        public System.Data.Common.DbParameter DbParameter { get; set; }


        public DbType ColumnType { get; set; }
        public string ColumnTypeName { get; set; }

        public string ColumnTypeVSLength { get; set; }


        /// <summary>
        /// 属性的类型
        /// </summary>
        public Type PropertyType { get; set; }

        /// <summary>
        /// 属性的名称，如果不直接写入，调用“Type.Name”属性,最终呈现的就是 String  Decimal  Int32
        /// </summary>
        public string PropertyTypeName { get; set; }



        /// <summary>中文名称</summary>
        public string GlobalName { get; set; }



        /// <summary>
        /// 字符长度
        /// </summary>
        public int? CharLength { get; set; }


        /// <summary>
        /// 数字精度，整数部分长度
        /// </summary>
        public int? NumericPrecision { get; set; }

        /// <summary>
        /// 数字标度，小数部分长度
        /// </summary>
        public int? NumericScale { get; set; }

        /// <summary>
        /// 默认规则
        /// </summary>
        public string DefaultRules { get; set; }

        public bool IsNullable { get; set; }

        public bool IsPk { get; set; }

    }
}
