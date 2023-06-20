using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.DB.CommRedis
{
    /// <summary>
    /// Redis引擎：泛型单例模式的基类
    /// </summary>
    public class TEntityEngineBase<TEngine> : EngineBase
        where TEngine : class, new()
    {


        #region 实现单例模式

        protected TEntityEngineBase() : base() { }
        public static readonly TEngine _Instance = new TEngine();

        public static TEngine Self
        {
            get { return _Instance; }
        }

        public static TEngine CreateInstance()
        {
            return _Instance;
        }
        #endregion
    }
}
