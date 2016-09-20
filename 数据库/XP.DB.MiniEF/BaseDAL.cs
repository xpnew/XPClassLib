using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XP.DB.Comm;
namespace XP.DB.MiniEF
{
    /// <summary>
    /// DAL的基类
    /// </summary>
    public class BaseDAL
    {

        public IProvider Provider { get; set; }

        public BaseDAL()
        {


        }
        public BaseDAL(IProvider provider)
        {
            this.Provider = provider;
        }


        public int Insert(string sql)
        {




            return 1;

        }

        public int ExcuteSql(string sql)
        {

            return Provider.ExecuteSql(sql);
            //return OleFactory.ExcuteLine(sql);
        }


        public int InsertAndId(string sql)
        {
            var conn = Provider.Conn;


            var Result = Provider.InsertAndId(sql);

         

            return Result;

        }

    }
}
