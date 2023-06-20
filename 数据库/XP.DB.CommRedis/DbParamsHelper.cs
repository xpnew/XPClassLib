using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Util.Configs;

namespace XP.DB.CommRedis
{
    /// <summary>
    /// 用来处理参数的工具类
    /// </summary>
    public class DbParamsHelper
    {


        public static DbConnParams GetParams(string groupName)
        {
            string Ip = String.Empty;
            int Port = 6379;
            string Password = String.Empty;
            bool IsAdmin = false;
            int DbNum = 0;
            bool HasServiceName = false;
            string ServiceName = groupName;



            var cr = ConfigReader._Instance;
            // 设置Redis缓存的连接  
            KeyGroupReader cg = new KeyGroupReader(cr, groupName);

            Ip = cg.GetKey("Ip");
            Password = cg.GetKey("Password");
            Port = cg.GetInt("Port", 6379);
            IsAdmin = cg.GetBool("RedisIsAdmin", false);
            DbNum = cg.GetInt("DbNum", 0);

            if (cg.Exist("ServiceName"))
            {
                ServiceName = cg.GetKey("ServiceName");
            }
            if (cg.Exist("IP"))
            {
                Ip = cg.GetKey("IP");
            }
            if (cg.Exist("Pwd"))
            {
                Ip = cg.GetKey("Pwd");
            }



            DbConnParams Result;

            Result = new DbConnParams(groupName, ServiceName, Ip, Password, Port, IsAdmin, DbNum);
            return Result;
        }


    }
}
