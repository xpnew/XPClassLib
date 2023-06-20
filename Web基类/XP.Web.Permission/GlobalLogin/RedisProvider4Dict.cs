using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.DB.LocalRedis;
using XP.Util.Config;

namespace XP.Web.Permission.GlobalLogin
{
    /// <summary>
    /// 针对用户缓存字典创建的Redis提供者程序
    /// </summary>
    /// <remarks>
    /// 创建原因：
    /// 当Session 使用了 mode=StateServer模式下
    /// 重启站点之后，原来的Session状态仍然是有效的，但是原来版本的UsersDict依托IIS的Appliction,在站点重启以后已经失效了。
    /// 为了保持和Session的同步，把原来在Appliction存储的数据改为Redis。
    /// 实际上，核心的实现完全依赖于XP.DB.LocalRedis.RedisProvider
    /// 按照“对外封闭”的原则，RedisProvider类将【XP.DB.CommRedis】和【StackExchange.Redis】两个上层的命名空间给遮蔽了。
    /// 外面的类库就不必添加相关的引用，同时也不必关心内部实现的细节。
    /// </remarks>
    public class RedisProvider4Dict
    {


        private string _RedisRootKey = "LJY:SysCache:GlobalLogin";


        private string _DictKey = ":UsersDict";
        private  string _ConfigName4ExpireTime = "UsersDictExpiresTimeMin";


        private RedisProvider _Provider;

        #region 实现单例模式

        protected static readonly RedisProvider4Dict _Instance = new RedisProvider4Dict();

        public static RedisProvider4Dict Self
        {
            get { return _Instance; }
        }

        public static RedisProvider4Dict CreateInstance()
        {
            return _Instance;
        }
        #endregion


        public RedisProvider4Dict()
        {
            _Provider  = new RedisProvider(_RedisRootKey +  _DictKey,EngineTypeDef.SystemCache);
            _Init();
        }

        protected void _Init()
        {

            var cr = ConfigReader.Self;
            _ExpiresTimeMin = cr.GetInt(_ConfigName4ExpireTime, 120);
            cr.ChangedNotify += (o, arg) => { _ExpiresTimeMin = ConfigReader.Self.GetInt(_ConfigName4ExpireTime, 120); };
        }


        private int _ExpiresTimeMin = -1;

        private  void _InitExpiresTime()
        {
            var cr = ConfigReader.Self;
            _ExpiresTimeMin = cr.GetInt(_ConfigName4ExpireTime, 120);
        }


        public  int ExpiresTimeMin
        {
            get { return _ExpiresTimeMin; }
            set { _ExpiresTimeMin = value; }
        }


        public AppUserinfo this[string sessionId]
        {
            get { return GetUserBySessionId(sessionId); }
            set { InsertUser(value.SessionId, value); }
        }

        public void InsertUser(string sessionId, AppUserinfo user)
        {
            _Provider.Insert(sessionId, user);
        }

        public bool Exist(string sessionId)
        {
            return _Provider.Exist(sessionId);
        }

        public bool Remove(string sessionId)
        {
            return _Provider.Remove(sessionId);
        }


        public AppUserinfo GetUserBySessionId(string sessionId)
        {
            if (_Provider.Exist(sessionId))
            {
                return _Provider.GetEntityByKey<AppUserinfo>(sessionId, null);
            }
            return null;
        }

        public List<AppUserinfo> GetAll()
        {
            return _Provider.GetAll<AppUserinfo>();
        }

        public void SaveUser(AppUserinfo user)
        {
            SaveUser(user.SessionId, user);
        }
        public void SaveUser(string sessionId,AppUserinfo user)
        {

        }

    }
}
