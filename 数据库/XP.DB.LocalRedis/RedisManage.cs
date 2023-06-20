using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.DB.LocalRedis
{
    public class RedisManage
    {

        public static void StartEngine()
        {
            ////Redis数据库引擎初始化
            //var cr = Util.Config.ConfigReader._Instance;
            //string RedisIP = cr.GetSet("RedisIP");
            //string RedisPort = cr.GetSet("RedisPort");
            //string RedisPassword = cr.GetSet("RedisPassword");
            //string RedisName = cr.GetSet("RedisName");
            //string RedisIsChannel = cr.GetSet("RedisIsChannel");


            //RedisDBParam redisParam = new RedisDBParam(RedisIP, int.Parse(RedisPort), RedisPassword, int.Parse(RedisName), int.Parse(RedisIsChannel));
            //RedisEngine _redisEngine = RedisEngine.InitRedisEngine(redisParam);

            //_redisEngine.StartEngine();
            LocalRedisEngine.Self.StartEngine();
            RedisSysCacheEngine.Self.StartEngine();
        }
    }
}
