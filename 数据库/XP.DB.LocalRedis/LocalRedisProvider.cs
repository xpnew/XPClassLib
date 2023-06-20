using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.DB.CommRedis;

namespace XP.DB.LocalRedis
{
    /// <summary>
    /// 封装Redis标准功能，因为Helper什么的名字已经在其它地方使用，所以叫RedisProvider
    /// </summary>
    public class LocalRedisProvider
    {

        /// <summary>
        /// 主key，拼接后的完整名
        /// </summary>
        public string FullKey { get; set; }

        /// <summary>
        /// 指定引擎（连接）
        /// </summary>
        private EngineBase Engine { get; set; }

        /// <summary>
        /// 内部调用的解析器
        /// </summary>
        private RedisHelper RedisUtil { get; set; }


        /// <summary>
        /// 基础的构造函数，使用LocalRedisEngine
        /// </summary>
        /// <param name="key"></param>
        public LocalRedisProvider(string key) : this(key, LocalRedisEngine.Self)
        {


        }

        public LocalRedisProvider(string key, EngineBase engine)
        {
            FullKey = key;

            Engine = engine;
            RedisUtil = Engine.Helper;

        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="subkey"></param>
        /// <param name="subval"></param>
        public void Insert(string subkey, string subval)
        {
            Engine.Insert(FullKey, subkey, subval);
        }


        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="subkey"></param>
        /// <param name="subval"></param>
        public void Insert(string subkey, int subval)
        {
            Engine.Insert(FullKey, subkey, subval);
        }



        public int FindInt(string subkey)
        {
            if (Engine.Exist(FullKey, subkey))
            {
                return Engine.FindInt(FullKey, subkey, 0);
            }
            return 0;
        }


    }
}
