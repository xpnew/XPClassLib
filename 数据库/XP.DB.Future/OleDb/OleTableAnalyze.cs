using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Text;
using XP.DB.Comm;

namespace XP.DB.Future.OleDb
{
    public class OleTableAnalyze : ITableAnalyze
    {
        private OleDbConnection _Conn;

        private OleProvider _Provider;


        public OleTableAnalyze(OleProvider provider)
        {
            this._Provider = provider;
        }


        public List<ColumnDtoItem> GetColumn(string tablename)
        {

            List<ColumnDtoItem> Result = new List<ColumnDtoItem>();
            return Result;

        }



        public DataTable AllTables()
        {
            string sql = "select * from  sysobjects where type='u'";


            return _Provider.Select(sql);

        }
        public DataTable AllViews()
        {

            string sql = "";


            return _Provider.Select(sql);


        }
        public DataTable AllProcedures()
        {

            string sql = "";


            return _Provider.Select(sql);


        }
    }
}
