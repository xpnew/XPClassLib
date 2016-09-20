using System;
using System.Collections.Generic;
using System.Text;

using System.Data.Common;
using System.Data;

namespace XP.DB.Future
{

    public enum DBTypeDefined
    {

        Access = 1,

        SqlServer = 2,
    }


    /// <summary>
    /// 数据提供者接口
    /// </summary>
    public interface IProvider
    {

        string ConnStr { get; set; }

        DbConnection Conn { get; set; }



        DbConnection CreateConn(string connstr);


        DataTable Select(string sql);


        int ExecuteSql(string sql);


        //DbDataReader GetReader(string sql);


        object SingleColumn(string sql);


        Dictionary<string, object> TopLine2Dict(string sql);


        ITableAnalyze Analyzer { get; set; }


        


    }
}
