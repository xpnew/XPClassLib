using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StackExchange.Redis;
using XP.Util.Configs;

namespace XP.DB.CommRedis
{
    /// <summary>
    /// Redis引擎的基类
    /// </summary>
    /// <remarks>
    /// 参考了RedisEngine类
    /// 既支持默认参数，又支持传入参数
    /// 默认参数首先会去配置文件（Site.config或Run.config）直接查找以“引擎类名”为名称的分组
    /// 如果这个分组不存在，则会去查找“【引擎类名】+【GroupName】”的单项配置，再根据这配置去查找相应的组名
    /// 找到这个分组以后，根据分组里面的配置创建参数，初始化引擎
    /// </remarks>
    public class EngineBase : IDisposable
    {
        #region 属性


        //链接对象
        private RedisHelper _Helper;
        public RedisHelper Helper
        {
            get { return _Helper; }
        }

        /// <summary>
        /// 参数对像
        /// </summary>
        public DbConnParams Params
        {
            get { return _Params; }
            set { _Params = value; }
        }

        /// <summary>
        /// 参数对像
        /// </summary>
        private DbConnParams _Params;



        private string _EngineName;



        #endregion

        #region 初始化
        public EngineBase()
        {
            _Init();
        }

        protected virtual void _Init()
        {
            _subScribeList = new Dictionary<string, Dictionary<string, SubScribeItem>>();
            _EngineName = this.GetType().Name;
            _InitDefaultParams();
        }

        private static object _locker4Load = new object();
        private bool _HasLoaded = false;
        private bool _HasParamReady = false;
        protected virtual void _InitDefaultParams()
        {
            DbConnParams p = new DbConnParams();
            lock (_locker4Load)
            {
                string ConfingGroup = _EngineName;
                var cr = ConfigReader.Self;
                if (cr.ExistGroup(ConfingGroup))
                {
                    p = DbParamsHelper.GetParams(ConfingGroup);
                    _HasParamReady = true;
                }
                else
                {
                    string FullConfingGroup = cr.GetSet(_EngineName + "GroupName");
                    if (cr.ExistGroup(FullConfingGroup))
                    {
                        p = DbParamsHelper.GetParams(FullConfingGroup);
                        _HasParamReady = true;
                    }
                }
            }

            if (_HasParamReady)
            {
                _InitParams(p);
            }
        }

        protected void _InitParams(DbConnParams p)
        {
            lock (_locker4Load)
            {
                Params = p;
                _Helper = new RedisHelper(p.HelperId, p.DbNum);
                _Helper.OnConnectFailed += (o, args) => { OnRedisOffline?.Invoke(o, args.FailureType.ToString()); };
                _Helper.OnConnectRestored += (o, args) => { OnRedisReconnection?.Invoke(o, args.FailureType.ToString()); };
            }
        }



        public void SetParams(DbConnParams p)
        {
            _InitParams(p);
        }


        public bool StartEngine(DbConnParams p)
        {
            _InitParams(p);
            return StartEngine();
        }

        public bool StartEngine()
        {
            bool flag = false;

            if (_Helper.Connect(Params))
            {
                RegisterManage.Instance.RegisterService(Params.ServiceName);
                flag = true;
            }
            return flag;
        }


        public bool SetHelper()
        {

            if (_Helper.Connect(Params))
            {
                return true;
            }
            return false;
        }


        public bool StopEngine()
        {
            RegisterManage.Instance.ServiceEvent(Params.ServiceName);
            return true;
        }

        #endregion

        #region 析构和释放
        private bool disposedValue = false; // 要检测冗余调用

        public void Dispose()
        {
            if (!disposedValue)
            {
                if (_Helper != null)
                {
                    _Helper.UnSubScribeAll();
                    _Helper.Release();
                }


                _subScribeList.Clear();
                disposedValue = true;

            }
        }

        #endregion
        #region Redis锁


        /// <summary>
        /// Redis锁 注意，lockName不要和常规的读写key重复了
        /// </summary>
        /// <remarks>
        ///  https://mayb.cn/csharp/2981/
        /// </remarks>
        /// <param name="lockName"></param>
        /// <param name="waitingSecond"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public async Task LockAsync(string lockName, int waitingSecond, Action a)
        {
            var redis = Helper.GetDataBase();

            //获取一个锁，30秒自动释放，如果锁没获取到，1秒后继续获取

            int Max = waitingSecond;
            int Step = 0;

            bool HasBroken = false;

            while (!redis.LockTake(lockName, "lock this", TimeSpan.FromSeconds(waitingSecond)))
            {
                Thread.Sleep(1000);
                Step++;
                if (Step >= Max)
                {
                    redis.LockRelease(lockName, "");
                    HasBroken = true;
                    break;
                }
            }
            if (HasBroken)
            {
                redis.LockTake(lockName, "", TimeSpan.FromSeconds(waitingSecond));
            }

            try
            {
                //获取到锁后执行的业务操作，操作需要在30秒内完成，不然其它线程会获取到锁，会造成数据不安全，或者把锁的过期时间设为足够大
                a.Invoke();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                //业务操作完成释放锁
                redis.LockRelease(lockName, "lock this");
            }
        }

        public async Task<T> LockAsync<T>(string lockName, int waitingSecond, Func<T> a, T def = default(T))
        {
            var redis = Helper.GetDataBase();

            //获取一个锁，30秒自动释放，如果锁没获取到，1秒后继续获取
            while (!redis.LockTake(lockName, "", TimeSpan.FromSeconds(waitingSecond)))
            {
                Thread.Sleep(1000);
            }

            try
            {
                //获取到锁后执行的业务操作，操作需要在30秒内完成，不然其它线程会获取到锁，会造成数据不安全，或者把锁的过期时间设为足够大
                return a.Invoke();

            }
            catch (Exception ex)
            {
                return def;
            }
            finally
            {
                //业务操作完成释放锁
                redis.LockRelease(lockName, "");
            }
        }

        #endregion


        #region  常规增删改查



        /// <summary>
        /// 因为Exist用来判断HashKey,所以，这个方法是判断一组Hash值的总key
        /// </summary>
        /// <param name="fullkey"></param>
        /// <returns></returns>
        public bool ExistParentKey(string fullkey)
        {
            return Helper.KeyExists(fullkey);
        }


        public bool ExpireDay(string fullkey, int days)
        {
            return Helper.KeyExpire(fullkey, days: days);
        }
        public bool Expire(string fullkey, int seconds = 0, int minutes = 0, int hours = 0, int days = 0)
        {
            return Helper.KeyExpire(fullkey, days: days);
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="subkey"></param>
        /// <param name="subval"></param>
        public void Insert(string fullkey, string subkey, string subval)
        {
            Helper.HashSet(fullkey, (RedisValue)subkey, (RedisValue)subval);
        }


        public bool Exist(string fullkey, string subkey)
        {
            return Helper.HashExists(fullkey, subkey);
        }


        public void Insert(string fullkey, string subkey, int subval)
        {
            Helper.HashSet(fullkey, (RedisValue)subkey, (RedisValue)subval);
        }

        /// <summary>
        /// 插入一个Hash值，值会被当作string存储
        /// </summary>
        /// <param name="fullkey"></param>
        /// <param name="subkey"></param>
        /// <param name="subval"></param>
        public void Insert(string fullkey, string subkey, object subval)
        {
            //StackExchange.Redis 不支持泛型，也不支持object，所以这里转换成string存储

            if (null != subval)
            {

                Type t = subval.GetType();
                string strval = subval.ToString();
                if (t.IsValueType)
                {
                    RedisValue vvv;
                    if (strval.IndexOf(".") >= 0)
                    {
                        var v = Convert.ToDouble(subval);
                        vvv = (RedisValue)v;
                    }
                    else
                    {
                        var v = Convert.ToInt64(subval);
                        vvv = (RedisValue)v;
                    }
                    Helper.HashSet(fullkey, (RedisValue)subkey, vvv);
                    return;
                }


                Helper.HashSet(fullkey, (RedisValue)subkey, (RedisValue)strval);
            }

        }

        public int FindInt(string fullkey, string k, int def = 0)
        {


            var val = Helper.HashGet(fullkey, (RedisValue)k);
            if (val.HasValue)
            {
                return (int)val;
            }
            return def;
        }

        public long FindLong(string fullkey, string k, long def = 0)
        {
            var val = Helper.HashGet(fullkey, (RedisValue)k);
            if (val.HasValue)
            {
                return (long)val;
            }
            return def;
        }


        public object FindValue(string fullkey, string subkey)
        {
            var val = Helper.HashGet(fullkey, (RedisValue)subkey);

            if (val.HasValue)
            {
                if (val.IsInteger)
                {
                    long v = (long)val;
                    return v;
                }
                else
                {
                    string strVal = val.ToString();
                    return strVal as object;

                }

            }
            return null;

        }




        #endregion


        #region 基本读写封装SUID(未完成)


        public string FindString(string rootkey, string k, string def = "")
        {
            RedisValue subkey = k;
            var val = Helper.HashGet(rootkey, subkey);
            if (val.HasValue)
            {
                return val.ToString();
            }
            return def;
        }

        //public long FindLong(string rootkey, string k, long def = 0)
        //{
        //    var val = Helper.HashGet(rootkey, k);
        //    if (val.HasValue)
        //    {
        //        return (long)val;
        //    }
        //    return def;
        //}


        #endregion


        #region 断开和重连事件

        public Action<object, string> OnRedisOffline;

        public Action<object, string> OnRedisReconnection;


        #endregion

        #region  子频道订阅


        //订阅项
        private class SubScribeItem
        {
            public string key;                         //订阅的KEY
            public string id;                           //订阅对象的唯一ID
            public Action<string, string, string> callBack;     //订阅回调
        }


        //订阅消息
        private class SubScribeMessage
        {
            public string id;       //订阅消息的ID
            public string msg;      //消息内容
        }

        //订阅回调列表
        private Dictionary<string, Dictionary<string, SubScribeItem>> _subScribeList;

        //添加订阅
        public bool SubScribe(string key, string id, Action<string, string, string> callBack)
        {
            lock (_subScribeList)
            {
                if (_subScribeList.ContainsKey(key))
                {
                    //存在KEY
                    Dictionary<string, SubScribeItem> itemList = _subScribeList[key];

                    if (itemList.ContainsKey(id))
                    {
                        //key相同 id也相同,添加失败
                        return false;
                    }
                    else
                    {
                        //key相同, ID不存在,添加到现在有的KEY列表下
                        SubScribeItem item = new SubScribeItem();
                        item.key = key;
                        item.id = id;
                        item.callBack = callBack;
                        itemList.Add(item.id, item);
                        return true;
                    }
                }
                else
                {
                    //不存在KEY
                    SubScribeItem item = new SubScribeItem();
                    item.key = key;
                    item.id = id;
                    item.callBack = callBack;

                    Dictionary<string, SubScribeItem> itemList = new Dictionary<string, SubScribeItem>();
                    itemList.Add(item.id, item);
                    _subScribeList.Add(key, itemList);

                    //将新订阅的KEY,添加到数据库中
                    _Helper.SubScribe(key, SubScribeEvent);

                    return true;
                }
            }
        }


        //取消订阅
        public bool UnSubScribe(string key, string id)
        {
            lock (_subScribeList)
            {
                if (_subScribeList.ContainsKey(key))
                {
                    //存在KEY
                    Dictionary<string, SubScribeItem> itemlist = _subScribeList[key];

                    if (itemlist.ContainsKey(id))
                    {
                        //key存在, id 也存在,删除ID
                        itemlist.Remove(id);

                        if (itemlist.Count <= 0)
                        {
                            //在KEY下面已经没有 ID挂载了.可以删除KEY了
                            _subScribeList.Remove(key);

                            //将订阅取消
                            _Helper.UnSubScribe(key);
                        }
                    }

                }
            }
            return true;
        }


        //发送订阅
        public bool SendSubScribe(string key, string id, string message)
        {
            SubScribeMessage msg = new SubScribeMessage();
            msg.id = id;
            msg.msg = message;
            _Helper.SendScribe(key, msg);
            return true;
        }




        #endregion

        #region  频道消息订阅

        public long SendChannelScribe(string channelName, string msg)
        {
            return _Helper.SendScribe(channelName, msg);
        }
        public void ChannelSubScribe(string channelName, Action<string, string> callBasek)
        {
            _Helper.SubScribe(channelName, (RedisChannel, RedisValue) =>
            {
                callBasek(RedisChannel, RedisValue);
            });
        }

        /// <summary>
        /// 取消某条消息的订阅 
        /// </summary>
        /// <param name="channel">消息名称</param>
        public void UnChannelSubScribe(string channel)
        {
            _Helper.UnSubScribe(channel);
        }

        #endregion



        #region //私有辅助函数

        //订阅通知事件
        private void SubScribeEvent(RedisChannel key, RedisValue msg)
        {
            string szKey = key.ToString();
            SubScribeMessage subMsg = RedisHelper.ConvertObj<SubScribeMessage>(msg);

            SubScribeItem callBack = null;

            lock (_subScribeList)
            {
                if (_subScribeList.ContainsKey(key))
                {
                    IDictionary<string, SubScribeItem> itemList = _subScribeList[key];

                    if ("0" == subMsg.id)
                    {
                        var AllBack = itemList.Where(s => null != s.Value && s.Value.callBack != null).Select(s => s.Value);
                        if (AllBack.Any())
                        {
                            foreach (var v in AllBack)
                            {
                                v.callBack(key, v.id, subMsg.msg);
                            }
                        }

                        return;
                    }
                    if (itemList.ContainsKey(subMsg.id))
                    {
                        callBack = itemList[subMsg.id];
                    }
                }
            }

            if (callBack != null)
            {
                callBack.callBack(key, subMsg.id, subMsg.msg);
            }
        }


        #endregion
    }
}
