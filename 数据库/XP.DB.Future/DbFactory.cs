using System;
using System.Collections.Generic;
using System.Text;
using XP.DB.Comm;
using XP.DB.DbEntity;

namespace XP.DB.Future
{
    public class DbFactory
    {


        public static IProvider CreateProvider(ProviderInfo dbProvider)
        {
            if (dbProvider.DbType == DBTypeDefined.Access)
            {

                var Provider = new OleDb.OleProvider(dbProvider.ConnString);
                return Provider;

            }
            else
            {
                var Provider = new SqlDb.SqlProvider(dbProvider.ConnString);
                return Provider;
            }

        }


    }
}
