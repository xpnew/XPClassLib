using System;
using System.Collections.Generic;
using System.Text;

using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using XP.DB.Comm;

namespace XP.DB.Future.SqlDb
{
    public class SqlTableAnalyze : ITableAnalyze
    {
        private SqlConnection _Conn;

        private SqlProvider _Provider;

        public SqlProvider Provider
        {
            get
            {
                if (null == _Provider && null != _Conn)
                {
                    _Provider = new SqlProvider(_Conn);
                }
                return _Provider;
            }
            set { _Provider = value; }
        }



        public SqlTableAnalyze(SqlConnection conn)
        {
            this._Conn = conn;
        }

        public SqlTableAnalyze(SqlProvider provider)
        {
            this._Conn = provider.Conn as SqlConnection;
            this._Provider = provider;
        }


        public List<ColumnDtoItem> GetColumn(string tablename)
        {
            List<ColumnDtoItem> Result = new List<ColumnDtoItem>();


            //基础的查询
            string sql = "select * from information_schema.columns where TABLE_NAME ='" + tablename + "'";
            //查询带字段说明 MS_Description
            string sql_adv____old = @"SELECT information_schema.columns.*,sys.extended_properties.value AS MS_Description
FROM  information_schema.columns 
LEFT JOIN sys.extended_properties   ON   information_schema.columns.ORDINAL_POSITION = sys.extended_properties.minor_id
WHERE TABLE_NAME='" + tablename + "' AND  major_id = OBJECT_ID ('" + tablename + "')";

            string sql_adv = @"SELECT information_schema.columns.*, MS_Description
FROM  information_schema.columns  LEFT JOIN 
( SELECT sys.extended_properties.value AS MS_Description,minor_id
FROM  sys.extended_properties WHERE  major_id = OBJECT_ID ('{TM:TableName}')
) AS B  ON   information_schema.columns.ORDINAL_POSITION = B.minor_id
WHERE TABLE_NAME='{TM:TableName}'  ";
            sql_adv = sql_adv.Replace("{TM:TableName}", tablename);

            var dt = Provider.Select(sql_adv);


            List<string> PkColumnList = GetPkColumnList(tablename);




            foreach (DataRow row in dt.Rows)
            {

                var NewItem = new ColumnDtoItem();

                string Name = row["COLUMN_NAME"].ToString();

                NewItem.ColumnName = Name;
                NewItem.ColumnTypeName = row["DATA_TYPE"].ToString();

                if (0 != PkColumnList.Count && PkColumnList.Contains(Name))
                {
                    NewItem.IsPk = true;
                }
                else
                {
                    NewItem.IsPk = false;
                }

                NewItem.IsNullable = false;
                if (DBNull.Value != row["IS_NULLABLE"])
                {
                    string val = row["IS_NULLABLE"].ToString().ToLower();

                    if ("true" == val || "1" == val || "yes" == val)
                    {
                        NewItem.IsNullable = true;
                    }
                }




                if (DBNull.Value != row["COLUMN_DEFAULT"])
                {
                    NewItem.DefaultRules = row["COLUMN_DEFAULT"].ToString();
                }

                if (DBNull.Value != row["MS_Description"])
                {
                    NewItem.GlobalName = row["MS_Description"].ToString();
                }

                NewItem.CharLength = ConverInt(row["CHARACTER_MAXIMUM_LENGTH"]);
                NewItem.NumericPrecision = ConverInt(row["NUMERIC_PRECISION"]);
                NewItem.NumericScale = ConverInt(row["NUMERIC_SCALE"]);


                //if (DBNull.Value != row["CHARACTER_MAXIMUM_LENGTH"])
                //{
                //    NewItem.CharLength = (int)row["CHARACTER_MAXIMUM_LENGTH"];
                //}
                //if (DBNull.Value != row["NUMERIC_PRECISION"])
                //{
                //    var mm = row["NUMERIC_PRECISION"];
                //    NewItem.NumericPrecision = ConverInt(row["NUMERIC_PRECISION"]);
                //}
                //if (DBNull.Value != row["NUMERIC_SCALE"])
                //{
                //    NewItem.NumericScale = (int)row["NUMERIC_SCALE"];
                //}

                MkType(NewItem);

                Result.Add(NewItem);
            }


            return Result;

        }

        private List<string> GetPkColumnList(string tablename)
        {
            string pksql = " SELECT TABLE_NAME,COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_NAME='" + tablename + "'";

            var pkdt = Provider.Select(pksql);
            List<string> PkColumnList = new List<string>();

            foreach (DataRow row in pkdt.Rows)
            {
                PkColumnList.Add(row["COLUMN_NAME"].ToString());
            }
            return PkColumnList;
        }


        public void MkType(ColumnDtoItem item)
        {
            Type t;
            string typename = item.ColumnTypeName;
            typename = typename.ToLower();

            string PropertyName;
            switch (typename)
            {
                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                case "text":
                case "ntext":
                    t = typeof(string);
                    PropertyName = "string";
                    break;
                case "int":
                case "smallint":
                    t = typeof(int);
                    PropertyName = "int";
                    break;
                case "tinyint":
                    t = typeof(byte);
                    PropertyName = "byte";
                    break;
                case "bigint":
                    PropertyName = "long";
                    t = typeof(long);
                    break;
                case "bit":
                    PropertyName = "bool";
                    t = typeof(bool);
                    break;
                case "decimal":
                    PropertyName = "decimal";
                    t = typeof(decimal);
                    break;
                case "uniqueidentifier":
                    PropertyName = "Guid";
                    t = typeof(Guid);
                    break;
                case "":
                default:
                    PropertyName = "object";
                    t = typeof(object);
                    break;
            }
            item.PropertyType = t;
            item.PropertyTypeName = PropertyName;
            if (typeof(string) == t)
            {
                if (-1 == item.CharLength)
                {
                    item.ColumnTypeVSLength = typename + "(MAX)";
                }
                else
                    item.ColumnTypeVSLength = typename + "(" + item.CharLength + ")";
            }
            else if (typeof(decimal) == t)
            {
                item.ColumnTypeVSLength = typename + "(" + item.NumericPrecision + "," + item.NumericScale + ")";

            }
            else if (typeof(int) == t)
            {
                item.ColumnTypeVSLength = typename + "(" + item.NumericPrecision + ")";
            }
            else
            {
                item.ColumnTypeVSLength = typename;
            }

        }


        public static Type TransSqlType(DbType dbtype)
        {
            Type t;

            switch (dbtype)
            {

                default:
                    t = typeof(object);
                    break;
            }
            return t;
        }

        public int? ConverInt(object rowValue)
        {
            if (DBNull.Value == rowValue || null == rowValue)
            {
                return null;
            }
            int tem = Convert.ToInt32(rowValue);

            return tem;
        }



        public static Type TransSqlType(string typename, int length, int decimalLenght)
        {
            Type t;
            typename = typename.ToLower();
            switch (typename)
            {
                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                case "text":
                case "ntext":
                    t = typeof(string);
                    break;
                case "int":
                case "smallint":
                    t = typeof(int);
                    break;

                case "bigint":
                    t = typeof(long);
                    break;
                case "bit":
                    t = typeof(bool);
                    break;
                case "decimal":
                    t = typeof(decimal);

                    break;

                case "":

                default:
                    t = typeof(object);
                    break;
            }
            return t;
        }



        public DataTable AllTables()
        {
            string sql = "select * from  sysobjects where type='u' ORDER BY [name] ";


            return _Provider.Select(sql);

        }
        public DataTable AllViews()
        {

            string sql = "select * from  sysobjects where xtype='V' ORDER BY [name] ";


            return _Provider.Select(sql);


        }
        public DataTable AllProcedures()
        {

            string sql = "select * from  sysobjects where type='p'";




            return _Provider.Select(sql);


        }
    }
}
