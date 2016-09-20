using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace XP.DB.Comm
{

    /// <summary>
    /// 数据表解析器接口
    /// </summary>
    public interface ITableAnalyze
    {

        ///// <summary>
        ///// 全部列名
        ///// </summary>
        // List<string> ColumnNames { get; set; }



        //DataTable ShowColumn(string tableName);




         List<ColumnDtoItem> GetColumn(string tablename);



         DataTable AllTables();

         DataTable AllViews();

         DataTable AllProcedures();

    }
}
