using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Text;
using XP.DB.Comm;

namespace XP.DB.MiniEF
{
    public class LinqConextClass : IDisposable
    {
        private DataContext context;

        private bool flagOpen = false;

        public IProvider Provider { get; set; }

        public LinqConextClass(IProvider provider)
        {
            this.Provider = provider;
        }

        /// <summary>
        /// 获取执行的上下文
        /// </summary>
        public DataContext Context
        {
            get
            {
                if (Provider.Conn != null && Provider.Conn.State == ConnectionState.Open && flagOpen)
                {
                    context = new DataContext(Provider.Conn);
                    return context;
                }
                else
                {
                    throw new Exception("打开数据库连接失败！");
                    return null;
                }
            }
        }
        public void Open()
        {
            if (Provider.Conn != null)
            {
                Provider.Conn.Open();
                flagOpen = true;
            }
        }
        public void Close()
        {
            if (Provider.Conn != null)
            {
                Provider.Conn.Close();
                flagOpen = false;
            }
        }


        #region IDisposable 成员

        public void Dispose()
        {
            if (Provider.Conn != null)
            {
                Provider.Conn.Close();
                Provider.Conn.Dispose();
            }
            if (context != null)
            {
                if (null != context.Connection)
                    context.Connection.Close();
                context.Dispose();
            }
        }

        #endregion

    }
}
