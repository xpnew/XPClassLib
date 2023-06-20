using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.DB.DAL
{
    public class DbHelper
    {



        private static string _ConnStr = null;
        private static bool _HasInitConnStr = false;


        /// <summary>
        /// 获取连接字符串，允许通过Global.asax或者其它方法覆盖默认的数据库连接
        /// </summary>
        public static string ConnString
        {

            get
            {
                if (_HasInitConnStr) return _ConnStr;
                _InitConnStr();
                return _ConnStr;
            }
            set
            {
                _ConnStr = value;
                _HasInitConnStr = true;
            }
        }

        private static void _InitConnStr()
        {
            _ConnStr = XP.Util.Conf.GetConnString("XP.BaseDB"); //默认使用基础数据库
            _HasInitConnStr = true;
        }



        public static SqlSugarClient Instance
        {
            get
            {
                lock ("DbHelper")
                {
                    return new SqlSugarClient(new ConnectionConfig()
                    {
                        ConnectionString = ConnString, //必填
                        DbType = DbType.SqlServer, //必填
                        IsAutoCloseConnection = true, //默认false
                        InitKeyType = InitKeyType.SystemTable,
                        ConfigureExternalServices = new ConfigureExternalServices()
                        {
                            EntityService = (property, column) =>
                            {
                                //if (property.Name == "xxx")
                                //{//根据列名    
                                //    column.IsIgnore = true;
                                //}
                                var attributes = property.GetCustomAttributes(true);//get all attributes     
                                if (attributes.Any(it => it is Comm.Attributes.DBSkipColumnAttribute))//根据自定义属性 跳过一些不需要处理的列、字段
                                {
                                    column.IsIgnore = true;
                                    //column.IsPrimarykey = true;
                                }
                            },
                            //EntityNameService = (type, entity) =>
                            //{
                            //    var attributes = type.GetCustomAttributes(true);
                            //    if (attributes.Any(it => it is TableAttribute))
                            //    {
                            //        entity.DbTableName = (attributes.First(it => it is TableAttribute) as TableAttribute).Name;
                            //    }
                            //}
                        }
                    });
                }
            }
        }
    }
}
