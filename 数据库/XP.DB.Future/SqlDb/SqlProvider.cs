using System;
using System.Collections.Generic;
using System.Text;

using System.Data.SqlClient;
using System.Data.Common;
using XP.DB.Comm;

namespace XP.DB.Future.SqlDb
{
    public class SqlProvider : BaseProvider
    {
        private SqlConnection _Conn;


        public new DbConnection Conn
        {
            get { return _Conn; }
            set
            {
                if (value is SqlConnection)
                {
                    _Conn = value as SqlConnection;
                    //base.Conn = _Conn;
                }
                else
                {
                    var NewConn = this.CreateConn(value.ConnectionString);
                    _Conn = NewConn as SqlConnection;
                }
            }
        }

        public SqlProvider(string connString)
            : base(connString)
        {

        }


        public SqlProvider(SqlConnection conn)
        {
            this._Conn = conn;

            this.ConnStr = conn.ConnectionString;

        }
        protected override void InitDefault()
        {
            base.InitDefault();

            DBType = DBTypeDefined.SqlServer;
        }
        protected override void InitConn(string connStr)
        {
            base.InitConn(connStr);
            _Conn = new SqlConnection(connStr);
            base.Conn = this._Conn;

        }

        public override DbConnection CreateConn(string connStr)
        {
            return new SqlConnection(connStr);
        }


        public override DbCommand CreateCommand(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, _Conn);

            return cmd;

        }

        public override DataAdapter CreateAdapter(DbCommand cmd)
        {
            SqlDataAdapter da = new SqlDataAdapter();

            da.SelectCommand = cmd as SqlCommand;
            return da;
        }


        public override DataAdapter CreateAdapter(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, _Conn);
            SqlDataAdapter da = new SqlDataAdapter();

            da.SelectCommand = cmd;

            return da;
        }


        public override int InsertAndId(string sql)
        {
            int NewId = -1;
            SqlCommand cmd = new SqlCommand(sql, _Conn);
            _Conn.Open();
            int Result = cmd.ExecuteNonQuery();

           

            string sql2 = "select @@identity AS ID";

            var cmd2 = new SqlCommand(sql2, _Conn);

            SqlDataReader dr = cmd2.ExecuteReader();

            if (dr.Read())
            {
                object o = dr[0];
                if (DBNull.Value == o)
                {
                    o = null;
                }
                else
                {
                    NewId = (int)o;
                }
                _Conn.Close();
                return NewId;
            }

            _Conn.Close();

            return NewId;
        }


        //public override DbDataReader CreateReader(string sql)
        //{
        //    SqlCommand cmd = new SqlCommand(sql, _Conn);
        //    _Conn.Open();
        //    SqlDataReader dr = cmd.ExecuteReader();

        //    _Conn.Close();

        //    return dr;
        //}


        public override object SingleColumn(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, _Conn);
            _Conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                object o = dr[0];
                if (DBNull.Value == o)
                {
                    o = null; 
                }
                _Conn.Close();
                return o;
            }
            _Conn.Close();
            return null;
        }

        public override Dictionary<string, object> TopLine2Dict(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, _Conn);
            _Conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                Dictionary<string, object> Dict = new Dictionary<string, object>();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    string key = dr.GetName(i);
                    if (DBNull.Value == dr[i])
                    {
                        Dict.Add(key, null);
                    }
                    else
                    {
                        Dict.Add(key, dr[i]);
                    }
                }
                _Conn.Close();
                return Dict;
            }
            _Conn.Close();
            return null;
        }


        public override ITableAnalyze GreateTableAnalyzer()
        {
            //return base.GreateTableAnalyzer();

            return new SqlTableAnalyze(this);
        }

    }
}
