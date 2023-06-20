using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.DB.CommRedis
{
    /// <summary>
    /// 连接Redis数据库的参数
    /// </summary>
    public struct DbConnParams
    {
        /// <summary>
        /// 连接名称，配置文件里面KeyGroup的节点名称
        /// </summary>
        public string ConnName { get; set; }
        /// <summary>
        /// 注册服务的时候，使用的名称
        /// </summary>
        public string ServiceName { get; set; }

        public string Ip { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        /// <summary>
        /// 是否以Admin模式连接，可以支持更多的功能，相应的也包含不安全的操作
        /// </summary>
        public bool IsAdmin { get; set; }
        /// <summary>
        /// Redis包括了DB0~DB15,一共16个数据库实例
        /// </summary>
        public int DbNum { get; set; }

        /// <summary>
        /// RedisHelper 内部Id,作用未知
        /// </summary>
        public int HelperId { get; set; }


        //public DbConnParams(string ip,string pwd, int port,bool isAdmin , int dbNum 0)
        //{



        //}

        public DbConnParams(string connName, string serviceName, string ip, string pwd, int port, bool isAdmin = false, int dbNum = 0)
        {
            ConnName = connName;
            ServiceName = serviceName;
            Ip = ip;
            Password = pwd;
            Port = port;
            IsAdmin = isAdmin;
            DbNum = dbNum;
            HelperId = 0;
        }


        public DbConnParams(string connName, string ip, string pwd, int port, bool isAdmin = false, int dbNum = 0)
        {
            ConnName = connName;
            ServiceName = connName;
            Ip = ip;
            Password = pwd;
            Port = port;
            IsAdmin = isAdmin;
            DbNum = dbNum;
            HelperId = 0;
        }


    }
}
