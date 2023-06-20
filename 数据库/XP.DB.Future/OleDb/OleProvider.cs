using System;
using System.Collections.Generic;
using System.Text;

using System.Data.OleDb;
using System.Data.Common;
using XP.DB.Comm;

namespace XP.DB.Future.OleDb
{
    public class OleProvider : BaseProvider
    {
        private OleDbConnection _Conn;


        public new DbConnection Conn
        {
            get { return _Conn; }
            set
            {
                if (value is OleDbConnection)
                {
                    _Conn = value as OleDbConnection;
                }
                else
                {
                    var NewConn = this.CreateConn(value.ConnectionString);
                    _Conn = NewConn as OleDbConnection;
                }
            }
        }


        public OleProvider(string connString)
            : base(connString)
        {

        }

        protected override void InitDefault()
        {
             base.InitDefault();

             DBType = DBTypeDefined.Access;
        }

        protected override void InitConn(string connStr)
        {
            base.InitConn(connStr);
            _Conn = new OleDbConnection(connStr);
            base.Conn = this._Conn;
        }


        public override DbConnection CreateConn(string connStr)
        {
            return new OleDbConnection(connStr);
        }


        public override DbCommand CreateCommand(string sql)
        {
            OleDbCommand cmd = new OleDbCommand(sql, _Conn);

            return cmd;

        }

        public override DataAdapter CreateAdapter(DbCommand cmd)
        {
            OleDbDataAdapter da = new OleDbDataAdapter();

            da.SelectCommand = cmd as OleDbCommand;
            return da;
        }


        public override DataAdapter CreateAdapter(string sql)
        {
            OleDbCommand cmd = new OleDbCommand(sql, _Conn);
            OleDbDataAdapter da = new OleDbDataAdapter();

            da.SelectCommand = cmd;
            return da;
        }



        //public override DbDataReader CreateReader(string sql)
        //{
        //    OleDbCommand cmd = new OleDbCommand(sql, _Conn);
        //    _Conn.Open();
        //    OleDbDataReader dr = cmd.ExecuteReader();

        //    _Conn.Close();

        //    return dr;
        //}


        public override int InsertAndId(string sql)
        {
            int NewId = -1;
            OleDbCommand cmd = new OleDbCommand(sql, _Conn);
            _Conn.Open();
            int Result = cmd.ExecuteNonQuery();

            #region jet 4 才支持
            string MainVersion = "";

            if (!String.IsNullOrEmpty(_Conn.ServerVersion))
            {
                MainVersion = _Conn.ServerVersion.Split('.')[0];
            }

            if (String.IsNullOrEmpty(MainVersion))
            {
                _Conn.Close();
                return Result;
            }

            int Ver = int.Parse(MainVersion);

            if (Ver<4)
            {
                _Conn.Close();
                return Result;
            }
            #endregion

            string sql2 = "select @@identity AS ID";

            var cmd2 = new OleDbCommand(sql2, _Conn);

            OleDbDataReader dr = cmd2.ExecuteReader();


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


        public override object SingleColumn(string sql)
        {
            OleDbCommand cmd = new OleDbCommand(sql, _Conn);
            _Conn.Open();
            OleDbDataReader dr = cmd.ExecuteReader();

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
            OleDbCommand cmd = new OleDbCommand(sql, _Conn);
            _Conn.Open();
            var dr = cmd.ExecuteReader();

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
            //retu
            base.GreateTableAnalyzer();

            return new OleTableAnalyze(this);
        }


    }
}
