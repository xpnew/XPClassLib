using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.DB.CommRedis;

namespace XP.DB.LocalRedis
{
    /// <summary>
    /// 引擎类型定义
    /// </summary>
    public enum EngineTypeDef
    {
        /// <summary>
        /// 默认的
        /// </summary>
        Default = 0,
        /// <summary>
        /// 基础版LocalRedisEngine
        /// </summary>
        BasicEdition = 0,
        /// <summary>
        /// 本地统计
        /// </summary>
        LocalCount = 2,
        /// <summary>
        /// 系统缓存
        /// </summary>
        SystemCache = 15,

    }

    /// <summary>
    /// 封装Redis标准功能，因为Helper什么的名字已经在其它地方使用，所以叫RedisProvider
    /// </summary>
    public class RedisProvider : RedisProviderBase
    {
       


        /// <summary>
        /// 基础的构造函数，使用LocalRedisEngine
        /// </summary>
        /// <param name="key"></param>
        public RedisProvider(string key) : this(key, EngineTypeDef.BasicEdition)
        {


        }

        public RedisProvider(string key, EngineTypeDef type):base(key)
        {
            FullKey = key;
            EngineBase engine;
            switch (type)
            {
                case EngineTypeDef.SystemCache:
                    Engine = RedisSysCacheEngine.Self;
                    break;
                //case EngineTypeDef.LocalCount:
                //    Engine = LocalCountEngine.Self;
                //    break;
                case EngineTypeDef.BasicEdition:
                default:
                    Engine = LocalRedisEngine.Self;
                    break;
            }

            _InitEngin();
        }
        ///// <summary>
        ///// 核心实现的构造函数
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="engine"></param>
        //public RedisProvider(string key, EngineBase engine)
        //{
        //    FullKey = key;
        //    Engine = engine;
        //    if (null == engine)
        //    {
        //        Engine = LocalRedisEngine.Self;
        //    }
        //    RedisUtil = Engine.Helper;
        //}


        public void SetEngine(EngineBase engine)
        {
            Engine = engine;
            if (null == engine)
            {
                Engine = LocalRedisEngine.Self;
                return;
            }
            RedisUtil = Engine.Helper;
        }




    }
}
