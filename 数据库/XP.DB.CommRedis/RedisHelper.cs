using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using XP.Util.Json;

namespace XP.DB.CommRedis
{
    public class RedisHelper
    {
        #region //属性

        //与Redis数据库链接的对象
        private ConnectionMultiplexer _connect;

        //链接信息
        private string _connectInfo;

        //Redis链接ID
        private int _connectID;
        public int ConnectID
        {
            get { return _connectID; }
        }

        //默认使用的库分区号0-15
        private int _dbNum;
        public int DBIndex
        {
            get { return _dbNum; }
        }

        #endregion


        #region //普通方法

        public RedisHelper(int id, int dbNum)
        {
            //与Redis数据库链接的对象
            _connect = null;

            //链接信息
            _connectInfo = string.Empty;

            //Redis链接ID
            _connectID = id;

            //默认使用的库分区号0-15
            _dbNum = dbNum;
        }

        ////链接数据库
        //public bool Connect(RedisDBParam param)
        //{
        //    return Connect(param.Ip, param.Port, param.Pwd);
        //}
        public bool Connect(DbConnParams param)
        {
            return Connect(param.Ip, param.Port, param.Password);
        }


        public bool Connect(string strIP, int port, string passWord, bool admin = false)
        {
            _connectInfo = string.Empty;

            if (admin)
            {
                _connectInfo = string.Format("{0}:{1},allowadmin=true,password={2}", strIP, port, passWord);
            }
            else
            {
                _connectInfo = string.Format("{0}:{1},password={2}", strIP, port, passWord);
            }

            return Connect();

        }
        public bool Connect()
        {
            if (_connect == null || _connect.IsConnected == false)
            {

                System.Threading.ThreadPool.SetMinThreads(400, 400);
                _connect = ConnectionMultiplexer.Connect(_connectInfo);

                //注册如下事件
                _connect.ConnectionFailed += ConnectFailed;
                _connect.ConnectionRestored += ConnectRestored;
                _connect.ErrorMessage += ErrorMessage;
                _connect.ConfigurationChanged += ConfigChange;
                _connect.HashSlotMoved += HashSlotMoved;
                _connect.InternalError += InternalError;

                return true;
            }

            string szLog = string.Format("RedisHelper::Connect()链接失败");
            ////LogOut.Instance.PrintLog(szLog);

            return false;
        }

        //判断是否链接
        public bool IsConnect()
        {
            if (_connect == null || _connect.IsConnected == false)
            {
                return false;
            }
            return true;
        }

        //释放数据库
        public void Release()
        {
            if (_connect != null && _connect.IsConnected)
            {
                _connect.CloseAsync();
                _connect = null;
            }
        }


        //获取数据库接口对象
        public IDatabase GetDataBase(int dbNum = -1)
        {
            if (dbNum <= -1 || dbNum >= 16)
            {
                //如果传进来的分区号不是有效的,使用默认的
                dbNum = _dbNum;
            }

            //if(_Idb == null)
            //{
            //   _Idb =  _connect.GetDatabase(dbNum);
            //}

            if (IsConnect() == false)
            {
                Connect();
            }

            return _connect.GetDatabase(dbNum);
        }


        //获取服务接口对象
        public IServer GetServer()
        {
            if (IsConnect() == false)
            {
                Connect();
            }

            return _connect.GetServer(_connect.GetEndPoints()[0]);
        }
        public IServer GetServer(string host, int port)
        {
            if (IsConnect() == false)
            {
                Connect();
            }

            return _connect.GetServer(host, port);
        }


        //设置数据库分区
        public bool SetDBIndex(int nIndex)
        {
            if (nIndex <= -1 || nIndex >= 16)
            {
                return false;
            }
            _dbNum = nIndex;
            return true;
        }

        #endregion


        #region //Key方法

        #region 同步方法

        /// <summary>
        /// 删除某一个KEY
        /// </summary>
        /// <param name="szKey">KEY值</param>
        /// <returns>成功true,失败false</returns>
        public bool KeyDelete(string szKey)
        {
            try
            {
                szKey = GetCustomKey(szKey);
                var db = GetDataBase();
                return db.KeyDelete(szKey);
            }
            catch (Exception error)
            {
                BeginPrint("KeyDelete(1)", szKey);
                Print(error);
            }
            return false;
        }


        /// <summary>
        /// 删除多个KEY
        /// </summary>
        /// <param name="keys">KEY 列表</param>
        /// <returns>删除的个数</returns>
        public long KeyDelete(string[] keys)
        {
            try
            {
                List<string> newKeys = keys.Select(p => (p = GetCustomKey(p))).ToList();
                var db = GetDataBase();
                return db.KeyDelete(ConvertKey(newKeys.ToArray()));
            }
            catch (Exception error)
            {
                BeginPrint("KeyDelete(2)", keys);
                Print(error);
            }
            return -1;
        }


        /// <summary>
        /// 判断key是否存储
        /// </summary>
        /// <param name="key">redis key</param>
        /// <returns>存在true,没有false</returns>
        public bool KeyExists(string key)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.KeyExists(key);
            }
            catch (Exception error)
            {

                BeginPrint("KeyExists()", key);
                Print(error);
            }
            return false;
        }


        /// <summary>
        /// 重新命名key
        /// </summary>
        /// <param name="key">就的redis key</param>
        /// <param name="newKey">新的redis key</param>
        /// <returns></returns>
        public bool KeyRename(string key, string newKey)
        {
            try
            {
                key = GetCustomKey(key);
                newKey = GetCustomKey(newKey);
                var db = GetDataBase();
                return db.KeyRename(key, newKey);
            }
            catch (Exception error)
            {
                string szLog = string.Format("Redis::KeyRename()   OldKey:{0},  NewKey{1},", key, newKey);
                BeginPrint(szLog);
                Print(error);
            }
            return false;
        }


        /// <summary>
        /// 设置一个KEY的生存周期(单位秒: Redis中最小的超时单位是秒,所以小于秒的生命周期没有意义会立即被删除)
        /// </summary>
        /// <param name="key">redis key</param>
        /// <param name="expiry">从现在起多少秒后失效</param>
        /// <returns></returns>
        public bool KeyExpire(string key, TimeSpan? expiry = default(TimeSpan?))
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.KeyExpire(key, expiry);
            }
            catch (Exception error)
            {
                BeginPrint("KeyExpire(1)", key);
                Print(error);
            }
            return false;
        }


        /// <summary>
        /// 设置一个KEY的生存周期(单位秒)
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="life">从现在起多少秒后失效</param>
        /// <returns>成功true,失败false</returns>
        public bool KeyExpire(string key, int seconds = 0, int minutes = 0, int hours = 0, int days = 0)
        {
            try
            {
                key = GetCustomKey(key);
                TimeSpan expiry = new TimeSpan(days, hours, minutes, seconds);
                var db = GetDataBase();
                return db.KeyExpire(key, expiry);
            }
            catch (Exception error)
            {
                BeginPrint("KeyExpire(2)", key);
                Print(error);
            }
            return false;
        }


        /// <summary>
        /// 移除一个KEY的生命周期,被移除的KEY将拥有永久的生命周期
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyPersist(string key)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.KeyPersist(key);
            }
            catch (Exception error)
            {
                BeginPrint("KeyPresist()", key);
                Print(error);
            }
            return false;
        }


        /// <summary>
        /// 获取一个KEY的类型
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public int KeyType(string key)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return (int)db.KeyType(ConvertKey(key));
            }
            catch (Exception error)
            {
                BeginPrint("KeyType()", key);
                Print(error);
            }
            return -1;
        }


        /// <summary>
        /// 模糊查询,不建议使用此方法,此方法的算法复杂度为O(n)(n为数据库中KEY的个数),
        /// 因为此方法效率极其低下,而且没有异步方法.会导致调用的线程锁死.慎用,或者不用
        /// </summary>
        /// <param name="key"></param>
        /// <returns>返回所有符合条件的KEY</returns>
        public RedisKey[] Keys(string key)
        {
            try
            {
                key = GetCustomKey(key);
                IServer server = GetServer();
                List<RedisKey> keyList = new List<RedisKey>(server.Keys(_dbNum, key));
                return keyList.ToArray();
            }
            catch (Exception error)
            {
                BeginPrint("Keys()", key);
                Print(error);
            }
            return null;
        }

        #endregion

        #region 异步方法

        /// <summary>
        /// 删除多个KEY,如果要删除的KEY的数量过多.请使用此方法
        /// </summary>
        /// <param name="keys">要删除的KEY列表</param>
        /// <param name="func">删除完成后的回调,返回被删除的KE诉个数</param>
        public async void KeyDeleteAsync(string[] keys, Action<long, List<object>> func, List<object> param = null)
        {
            try
            {
                List<string> newKeys = keys.Select(p => (p = GetCustomKey(p))).ToList();
                var db = GetDataBase();
                long value = await db.KeyDeleteAsync(ConvertKey(newKeys.ToArray()));
                if (func != null)
                {
                    func(value, param);
                }
            }
            catch (Exception error)
            {
                string szLog = "Redis::KeyDeleteAsync()   szKey:";

                for (int i = 0; i < keys.Length; i++)
                {
                    szLog += keys[i] + ",  ";
                }

                BeginPrint(szLog);
                Print(error);
            }
        }

        #endregion

        #endregion


        #region //String(字符)

        #region 同步方法

        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool StringSet(string key, RedisValue value, TimeSpan? expiry = default(TimeSpan?))
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.StringSet(key, value, expiry);
            }
            catch (Exception error)
            {
                BeginPrint("StringSet(1)", key);
                Print(error);
            }
            return false;
        }
        /// <summary>
        /// 保存一个key value
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="life">有效时间(秒)</param>
        /// <returns></returns>
        public bool StringSet(string key, RedisValue value, long life)
        {
            try
            {
                key = GetCustomKey(key);
                TimeSpan expiry = life <= 0 ? default(TimeSpan) : new TimeSpan(10 * life);
                var db = GetDataBase();
                return db.StringSet(key, value, expiry);
            }
            catch (Exception error)
            {
                string szLog = string.Format("Redis::StringSet(2)   key:{0}", key);
                ////LogOut.Instance.BeginPrint(szLog);

                szLog = string.Format("Error:{0}", error.ToString());
                ////LogOut.Instance.Print(szLog);
            }
            return false;
        }


        /// <summary>
        /// 保存多个key value
        /// </summary>
        /// <param name="keyValues">键值对</param>
        /// <returns></returns>
        public bool StringSet(KeyValuePair<RedisKey, RedisValue>[] keyValues)
        {
            try
            {
                List<KeyValuePair<RedisKey, RedisValue>> newkeyValues =
                    keyValues.Select(p => new KeyValuePair<RedisKey, RedisValue>(GetCustomKey(p.Key), p.Value)).ToList();
                var db = GetDataBase();
                return db.StringSet(newkeyValues.ToArray());
            }
            catch (Exception error)
            {

                string szLog = "Redis::StringSet(3)   key:{0}";

                foreach (var item in keyValues)
                {
                    szLog += item.Key.ToString() + ",   ";
                }

                ////LogOut.Instance.BeginPrint(szLog);

                szLog = string.Format("Error:{0}", error.ToString());
                ////LogOut.Instance.Print(szLog);
            }
            return false;
        }
        public bool StringSet(string[] keys, RedisValue[] values)
        {
            try
            {
                if (keys.Length != values.Length || keys.Length <= 0)
                {
                    return false;
                }

                List<KeyValuePair<RedisKey, RedisValue>> tempList = new List<KeyValuePair<RedisKey, RedisValue>>();

                for (int i = 0; i < keys.Length; i++)
                {
                    KeyValuePair<RedisKey, RedisValue> item = new KeyValuePair<RedisKey, RedisValue>(ConvertKey(keys[i]),
                        ConvertValue(values[i]));
                    tempList.Add(item);
                }
                var db = GetDataBase();
                return db.StringSet(tempList.ToArray());
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::StringSet(4)   " + error.ToString());
            }
            return false;
        }


        /// <summary>
        /// 获取单个key的值
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        public RedisValue StringGet(string key)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.StringGet(key);
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::StringGet(1)   " + error.ToString());
            }
            return RedisValue.Null;
        }
        /// <summary>
        /// 获取多个Key
        /// </summary>
        /// <param name="listKey">Redis Key集合</param>
        /// <returns></returns>
        public RedisValue[] StringGet(string[] keys)
        {
            try
            {
                var db = GetDataBase();
                return db.StringGet(ConvertKey(keys));
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::StringGet(2)   " + error.ToString());
            }
            return null;
        }


        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public double StringIncr(string key, double val = 1)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.StringIncrement(key, val);
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::StringIncr()   " + error.ToString());
            }
            return 0;
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public double StringDecr(string key, double val = 1)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.StringDecrement(key, val);
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::StringDecr()   " + error.ToString());
            }
            return 0;
        }

        #endregion

        #region 异步方法

        /// <summary>
        /// 保存多个KEY
        /// </summary>
        /// <param name="keyValues">键值列表</param>
        /// <param name="func">操作完成的通知回调,无参数</param>
        public async void StringSetAsync(KeyValuePair<RedisKey, RedisValue>[] keyValues, Action<bool, List<object>> func, List<object> param = null)
        {
            try
            {
                List<KeyValuePair<RedisKey, RedisValue>> newkeyValues =
                keyValues.Select(p => new KeyValuePair<RedisKey, RedisValue>(GetCustomKey(p.Key), p.Value)).ToList();
                var db = GetDataBase();
                await db.StringSetAsync(newkeyValues.ToArray());
                if (func != null)
                {
                    func(true, param);
                }
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::StringSetAsync(1)   " + error.ToString());
            }
        }
        public async void StringSetAsync(RedisKey[] keys, RedisValue[] values, Action<bool, List<object>> func, List<object> param = null)
        {
            try
            {
                if (keys.Length != values.Length || keys.Length <= 0)
                {
                    if (func != null)
                    {
                        func(false, param);
                    }
                    return;
                }

                List<KeyValuePair<RedisKey, RedisValue>> tempList = new List<KeyValuePair<RedisKey, RedisValue>>();

                for (int i = 0; i < keys.Length; i++)
                {
                    KeyValuePair<RedisKey, RedisValue> item = new KeyValuePair<RedisKey, RedisValue>(keys[i], values[i]);
                    tempList.Add(item);
                }
                var db = GetDataBase();
                await db.StringSetAsync(tempList.ToArray());
                if (func != null)
                {
                    func(true, param);
                }
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::StringSetAsync(2)   " + error.ToString());
            }
        }
        public async void StringSetAsync(string[] keys, string[] values, Action<bool, List<object>> func, List<object> param = null)
        {
            try
            {
                if (keys.Length != values.Length || keys.Length <= 0)
                {
                    if (func != null)
                    {
                        func(false, param);
                    }

                }

                List<KeyValuePair<RedisKey, RedisValue>> tempList = new List<KeyValuePair<RedisKey, RedisValue>>();

                for (int i = 0; i < keys.Length; i++)
                {
                    KeyValuePair<RedisKey, RedisValue> item = new KeyValuePair<RedisKey, RedisValue>(ConvertKey(keys[i]),
                        ConvertValue(values[i]));
                    tempList.Add(item);
                }
                var db = GetDataBase();
                await db.StringSetAsync(tempList.ToArray());
                if (func != null)
                {
                    func(true, param);
                }
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::StringSetAsync(3)   " + error.ToString());
            }
        }
        public async void StringSetAsync<T>(string[] keys, T[] values, Action<bool, List<object>> func, List<object> param = null)
        {
            try
            {
                if (keys.Length != values.Length || keys.Length <= 0)
                {
                    if (func != null)
                    {
                        func(false, param);
                    }
                    return;
                }

                List<KeyValuePair<RedisKey, RedisValue>> tempList = new List<KeyValuePair<RedisKey, RedisValue>>();

                for (int i = 0; i < keys.Length; i++)
                {
                    KeyValuePair<RedisKey, RedisValue> item = new KeyValuePair<RedisKey, RedisValue>(ConvertKey(keys[i]),
                        ConvertValue(values[i]));
                    tempList.Add(item);
                }
                var db = GetDataBase();
                await db.StringSetAsync(tempList.ToArray());
                if (func != null)
                {
                    func(true, param);
                }
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::StringSetAsync(4)   " + error.ToString());
            }
        }


        /// <summary>
        /// 获取多个Key的值
        /// </summary>
        /// <param name="listKey">要获取的KEY的列表</param>
        /// <returns>返回</returns>
        public async void StringGetAsync<T>(string[] keys, Action<T[], List<object>> func, List<object> param = null)
        {
            try
            {
                var db = GetDataBase();
                RedisValue[] values = await db.StringGetAsync(GetCustomKey(keys));
                if (func != null)
                {
                    func(ConvertObj<T>(values), param);
                }
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::StringGetAsync()   " + error.ToString());
            }
        }
        public async void StringGetAsync(string[] keys, Action<RedisValue[], List<object>> func, List<object> param = null)
        {
            try
            {
                var db = GetDataBase();
                RedisValue[] values = await db.StringGetAsync(GetCustomKey(keys));
                if (func != null)
                {
                    func(values, param);
                }
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::StringGetAsync()   " + error.ToString());
            }
        }

        #endregion

        #endregion


        #region //Hash方法(哈希表)

        #region 同步方法

        /// <summary>
        /// 判断某个哈希key是否存在
        /// </summary>
        /// <param name="key">哈希KEY</param>
        /// <param name="field">哈希域</param>
        /// <returns>当KEY和域都存在时返回true,只要有一个不存在就返回false</returns>       
        public bool HashExists(string key, RedisValue field)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.HashExists(key, field);
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::HashExists()   " + error.ToString());
            }
            return false;
        }


        /// <summary>
        /// 添加一个哈希,如果已经存在覆盖旧数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="field">域</param>
        /// <param name="value">值</param>
        /// <returns>如果是添加返回true,如果是覆盖返回false</returns>
        public bool HashSet(string key, RedisValue field, RedisValue value)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.HashSet(key, field, value);
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::HashSet(1)   " + error.ToString());
            }
            return false;
        }
        //public bool HashSet(string key, object field, object value)
        //{
        //    try
        //    {
        //        key = GetCustomKey(key);
        //        var db = GetDataBase();
        //        return db.HashSet(key, field, value);
        //    }
        //    catch (Exception error)
        //    {
        //        ////LogOut.Instance.PrintLog("Redis::HashSet(1)   " + error.ToString());
        //    }
        //    return false;
        //}
        public void HashSet(string key, HashEntry[] entry)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                db.HashSet(key, entry);
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::HashSet(2)   " + error.ToString());
            }
        }
        public void HashSet(string key, RedisValue[] fields, RedisValue[] values)
        {
            try
            {
                if (fields.Length != values.Length || fields.Length <= 0)
                {
                    return;
                }

                key = GetCustomKey(key);
                List<HashEntry> list = new List<HashEntry>();

                for (int i = 0; i < fields.Length; i++)
                {
                    HashEntry item = new HashEntry(fields[i], values[i]);
                    list.Add(item);
                }
                var db = GetDataBase();
                db.HashSet(key, list.ToArray());
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::HashSet(3)   " + error.ToString());
            }

        }


        /// <summary>
        /// 删除哈希中某个域以及域值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="field">域</param>
        /// <returns>成功为true,失败为false或返回删除的个数</returns>
        public bool HashDelete(string key, RedisValue field)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.HashDelete(key, field);
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::HashDelete(1)   " + error.ToString());
            }
            return false;
        }
        public long HashDelete(string key, RedisValue[] fields)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.HashDelete(key, fields);
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::HashDelete(2)   " + error.ToString());
            }
            return 0;
        }


        /// <summary>
        /// 从哈希表中的某个域取域值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="field">域值</param>
        /// <returns></returns>
        public RedisValue HashGet(string key, RedisValue field)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.HashGet(key, field);
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::HashGet(1)   " + error.ToString());
            }
            return RedisValue.Null;
        }
        public RedisValue[] HashGet(string key, RedisValue[] fields)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.HashGet(key, fields);
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::HashGet(2)   " + error.ToString());
            }
            return null;
        }


        /// <summary>
        /// 为数字增长val,如果该键,域 不存在则设置初始值为0,然后设置操作
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负,如果为负,实为减少</param>
        /// <returns>增长后的值</returns>
        public double HashIncr(string key, RedisValue field, double val = 1)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.HashIncrement(key, field, val);
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::HashIncr()   " + error.ToString());
            }
            return 0;
        }


        /// <summary>
        /// 为数字减少val,如果该键,域不存在,设置初始值为0,再操作
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负,如果为负,实为增加</param>
        /// <returns>减少后的值</returns>
        public double HashDecr(string key, RedisValue field, double val = 1)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.HashDecrement(key, field, val);
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::HashDecr()   " + error.ToString());
            }
            return 0;
        }


        /// <summary>
        /// 获取哈希中所有的域
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public RedisValue[] HashKeys(string key)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.HashKeys(key);
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::HashKeys()   " + error.ToString());
            }
            return null;
        }


        /// <summary>
        /// 返回哈希表中所有的域和值,此方法经测试,在大数据量的情况下(十万个以上),会造成读取缓慢.连接超时,所以慎用,推荐使用异步方法
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public HashEntry[] HashGetAll(string key)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.HashGetAll(key);
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::HashGetAll()   " + error.ToString());
            }
            return null;
        }


        /// <summary>
        /// 获取哈希表中所有域的值(只返回值,不返回域)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public RedisValue[] HashAllValue(string key)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.HashValues(key);
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::HashAllValue()   " + error.ToString());
            }
            return null;
        }


        /// <summary>
        /// 获取哈希表中,域的个数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long HashLenght(string key)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.HashLength(key);
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::HashLenght()   " + error.ToString());
            }
            return -1;
        }


        /// <summary>
        /// 迭代集合中符合条件的成员,支持模摸查义
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="math">匹配条件 math = he*o? 可以是*?的通配符形式的字符串</param>
        /// <param name="cursor">起始位置, 如果为0开始新一轮的迭代查询 </param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageOffice">页偏移</param>
        /// <returns>返回一个HashEntry类型的迭代器,可以通过IScanningCursor 将返回值转换为与光标,页相关的对象,用于二次迭代
        /// 返回的结果可能为空,但这并不代表着已经迭代了整个集合,只有当返回结果中Cursor为0是,才可表已经遍历了整个集合</returns>
        public IEnumerable<HashEntry> HashScan(string key, RedisValue math, long cursor = 0, int pageSize = 0, int pageOffice = 0)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                IEnumerable<HashEntry> list = db.HashScan(key, math, pageSize, cursor, pageOffice);
                return list;
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::HashScan()   " + error.ToString());
            }
            return null;
        }


        #endregion

        #region 异步方法

        /// <summary>
        /// 设置一组哈希
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entry"></param>
        /// <param name="func"></param>
        public async void HashSetAsync(string key, HashEntry[] entry, Action<bool, List<object>> func, List<object> param = null)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                await db.HashSetAsync(key, entry);
                if (func != null)
                {
                    func(true, param);
                }
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::HashSetAsync(1)   " + error.ToString());
            }
        }
        public async void HashSetAsync(string key, RedisValue[] fields, RedisValue[] values, Action<bool, List<object>> func, List<object> param = null)
        {
            try
            {
                if (fields.Length != values.Length || fields.Length <= 0)
                {
                    if (func != null)
                    {
                        func(false, param);
                    }
                    return;
                }

                key = GetCustomKey(key);
                List<HashEntry> list = new List<HashEntry>();

                for (int i = 0; i < fields.Length; i++)
                {
                    HashEntry item = new HashEntry(fields[i], values[i]);
                    list.Add(item);
                }
                var db = GetDataBase();
                await db.HashSetAsync(key, list.ToArray());
                if (func != null)
                {
                    func(true, param);
                }
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::HashSetAsync(2)   " + error.ToString());
            }
        }



        /// <summary>
        /// 删除多个哈希的域
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public async void HashDeleteAsync(string key, RedisValue[] fields, Action<long, List<object>> func, List<object> param = null)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                long num = await db.HashDeleteAsync(key, fields);
                if (func != null)
                {
                    func(num, param);
                }
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::HashDeleteAsync()   " + error.ToString());
            }
        }


        /// <summary>
        /// 获取哈希的多个域值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public async void HashGetAsync(string key, RedisValue[] fields, Action<RedisValue[], List<object>> func, List<object> param = null)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                RedisValue[] values = await db.HashGetAsync(key, fields);
                if (func != null)
                {
                    func(values, param);
                }
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis:HashGetAsync()   " + error.ToString());
            }
        }


        /// <summary>
        /// 获取哈希中所有的域
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async void HashKeysAsync(string key, Action<RedisValue[], List<object>> func, List<object> param = null)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                RedisValue[] values = await db.HashKeysAsync(key);
                if (func != null)
                {
                    func(values, param);
                }
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::HashKeysAsync()   " + error.ToString());
            }
        }

        /// <summary>
        /// 获取哈希表中所有值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="callBack"></param>
        public async void HashValueAsync(string key, Action<RedisValue[], List<object>> func, List<object> param = null)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                RedisValue[] value = await db.HashValuesAsync(key);
                if (func != null)
                {
                    func(value, param);
                }
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis:HashValueAsync()   " + error.ToString());
            }
        }


        /// <summary>
        /// 返回哈希表中所有的域和值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async void HashGetAllAsync(string key, Action<HashEntry[], List<object>> func, List<object> param = null)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                HashEntry[] values = await db.HashGetAllAsync(key);
                if (func != null)
                {
                    func(values, param);
                }
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::HashGetAllAsync()   " + error.ToString());
            }
        }

        #endregion

        #endregion


        #region //List方法(列表)

        #region 同步方法

        /// <summary>
        /// 通过索引区取列表中的一个元素 下标以0开始 表示第一个元素.1为第二个元素.如果传的是负数 -表示最后一个元素
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="index">索引</param>
        /// <returns>如果索引超出数组长度,返回null</returns>
        public RedisValue ListIndex(string key, long index)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.ListGetByIndex(key, index);
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::ListIndex()   " + error.ToString());
            }
            return RedisValue.Null;
        }


        /// <summary>
        /// 向列表中某个元素之前插入数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="point">查找位置的元素</param>
        /// <param name="value">要插入的元素</param>
        /// <returns>如果point找到了,插入成功,返回插入后列表的长度,如果point没有找到返回-1,如果KEY不存在返回0</returns>
        public long ListInsertBefore(string key, RedisValue point, RedisValue value)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.ListInsertBefore(key, point, value);
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::ListInsertBefore()   " + error.ToString());
            }
            return -1;
        }


        /// <summary>
        /// 向列表中某个元素之后插入数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="point">查找位置的元素</param>
        /// <param name="value">要插入的元素</param>
        /// <returns>如果Point找到了,插入成功,返回插入后列表的长度,如果point没有找到返回-1,如果KEY不存在返回0</returns>
        public long ListInsertAfter(string key, RedisValue point, RedisValue value)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.ListInsertAfter(key, point, value);
            }
            catch (Exception error)
            {
                ////LogOut.Instance.PrintLog("Redis::ListInsertAfter()   " + error.ToString());
            }
            return -1;
        }


        /// <summary>
        /// 获取列表长度
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>返回列表长度,如果KEY不存在,返回0,如果KEY不是列表类型,返回一个错误</returns>
        public long ListLenght(string key)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.ListLength(key);
            }
            catch (Exception error)
            {
                XP.Loger.Error("查询Redis出错误，准备获取一个List但是可能类型不对", error);
                //return -1;
                //LogOut.Instance.PrintLog("Redis::ListLenght()   " + error.ToString());
            }
            return 0;
        }


        /// <summary>
        /// 删除并返回列表中第一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <returns>当KEY不存在是返回null,如果key不是列表类型,返回一个错误</returns>
        public RedisValue ListFirstPop(string key)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.ListLeftPop(key);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::ListFirstPop()   " + error.ToString());
            }
            return RedisValue.Null;
        }


        /// <summary>
        /// 删除并返回列表中最后一个元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>当KEY不存在返回null,如果KEY不是列表类型,返回一个错误</returns>
        public RedisValue ListLastPop(string key, RedisValue value)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.ListRightPop(key);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::ListLastPop()   " + error.ToString());
            }
            return RedisValue.Null;
        }


        /// <summary>
        /// 将元素插入到列表头部
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>如果列表不存在, 新建列表,返回插入后列表的长度</returns>
        public long ListFirstPush(string key, RedisValue value)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.ListLeftPush(key, value);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::ListFirstPush(1)   " + error.ToString());
            }
            return -1;
        }
        public long ListFirstPush(string key, RedisValue[] values)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.ListLeftPush(key, values);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis:ListFirstPush(2)   " + error.ToString());
            }
            return -1;
        }


        /// <summary>
        /// 将元素添加到列表的尾部
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>如果列表不存在,新建列表,返回插入后列表长度</returns>
        public long ListLastPush(string key, RedisValue value)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.ListRightPush(key, value);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::ListLastPush(1)   " + error.ToString());
            }
            return -1;
        }
        public long ListLastPush(string key, RedisValue[] values)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.ListRightPush(key, values);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::ListLastPush(2)  " + error.ToString());
            }
            return -1;
        }


        /// <summary>
        /// 获取指定范围内的列表元素,如果使用默认参数,则获取整个列表的元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns>如果key不存在,返回null,如果KEY不是列表类型,返回一个错误</returns>
        public RedisValue[] ListRange(string key, long startIndex = 0, long endIndex = -1)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.ListRange(key, startIndex, endIndex);
            }
            catch (Exception error)
            {
                XP.Loger.Error("查询Redis出错误，准备获取一个List但是可能类型不对", error);
                //LogOut.Instance.PrintLog("Redis::ListRange()   " + error.ToString());
            }
            return null;
        }


        /// <summary>
        /// 从列表中移除一个与value相同的元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param count="count"> count > 0从表头向表尾搜索,删除count个数据 </param>
        /// count < 0 从表尾向表头搜索,删除count个数据, count = 0 删除所有符合的数据
        /// <returns>返回被删除的元素的个数</returns>
        public long ListRemove(string key, RedisValue value, long count = 0)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.ListRemove(key, value);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::ListRemove(1)   " + error.ToString());
            }
            return 0;
        }


        /// <summary>
        /// 从列表中index的位置移除count个数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public long ListRemove(string key, long index, long count = 1)
        {
            try
            {
                if (count <= 0)
                {
                    return 0;
                }

                key = GetCustomKey(key);
                long nCnt = 0;
                for (int i = 0; i < count; i++)
                {
                    RedisValue value = ListIndex(key, index);
                    if (value.IsNullOrEmpty)
                    {
                        return nCnt;
                    }
                    nCnt += ListRemove(key, value, 1);
                }
                return nCnt;
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::ListRemove(2)   " + error.ToString());
            }
            return 0;
        }


        /// <summary>
        /// 通过索引修改元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public void ListSet(string key, long index, RedisValue newValue)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                db.ListSetByIndex(key, index, newValue);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::ListSet()   " + error.ToString());
            }
        }

        #endregion


        #region 异步方法

        /// <summary>
        /// 将元素插入到列表头部
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>如果列表不存在, 新建列表</returns>
        public async void ListFirstPushAsync(string key, RedisValue[] values, Action<long, List<object>> func, List<object> param = null)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                long num = await db.ListLeftPushAsync(key, values);
                if (func != null)
                {
                    func(num, param);
                }
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::ListFirstPushAsync()   " + error.ToString());
            }
        }


        /// <summary>
        /// 将元素添加到列表的尾部
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>如果列表不存在,新建列表</returns>
        public async void ListLastPushAsync(string key, RedisValue[] values, Action<long, List<object>> func, List<object> param = null)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                long num = await db.ListRightPushAsync(key, values);
                if (func != null)
                {
                    func(num, param);
                }
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::ListLastPushAsync()   " + error.ToString());
            }
        }


        /// <summary>
        /// 获取指定范围内的列表元素,如果使用默认参数,则获取整个列表的元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns>如果key不存在,返回null,如果KEY不是列表类型,返回一个错误</returns>
        public async void ListRangeAsync(string key, Action<RedisValue[], List<object>> func, List<object> param = null, long startIndex = 0, long endIndex = -1)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                RedisValue[] values = await db.ListRangeAsync(key, startIndex, endIndex);
                if (func != null)
                {
                    func(values, param);
                }
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::ListRangeAsync()   " + error.ToString());
            }
        }


        /// <summary>
        /// 从列表中移除一个与value相同的元素
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param count="count"> count > 0从表头向表尾搜索,删除count个数据 </param>
        /// count < 0 从表尾向表头搜索,删除count个数据, count = 0 删除所有符合的数据
        /// <returns>返回被删除的元素的个数</returns>
        public async void ListRemoveAsync(string key, RedisValue value, Action<long, List<object>> func, List<object> param = null, long count = 0)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                long num = await db.ListRemoveAsync(key, value);
                if (func != null)
                {
                    func(num, param);
                }
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::ListRemoveAsync()   " + error.ToString());
            }
        }

        #endregion

        #endregion


        #region //Set方法(集合)

        #region 同步方法

        /// <summary>
        /// 向集合中添加一个或多个成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>返回被添加的成员的数量,如果表不存在,新建,如果表不是集合类型,返回一个错误</returns>
        public bool SetAdd(string key, RedisValue value)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.SetAdd(key, value);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SetAdd(1)   " + error.ToString());
            }
            return false;
        }
        public long SetAdd(string key, RedisValue[] values)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.SetAdd(key, values);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SetAdd(2)   " + error.ToString());
            }
            return 0;
        }


        /// <summary>
        /// 获取集合中成员的个数
        /// </summary>
        /// <param name="key"></param>
        /// <returns>key不存在时返回0,key不是集合时返回一个错误</returns>
        public long SetLength(string key)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.SetLength(key);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SetLength()   " + error.ToString());
            }
            return 0;
        }


        /// <summary>
        /// 获取集合中所有的成员
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public RedisValue[] SetMembers(string key)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.SetMembers(key);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SetMembers()   " + error.ToString());
            }
            return null;
        }


        /// <summary>
        /// 获取多个集合的合集
        /// </summary>
        /// <param name="keys">集合列表</param>
        /// <param name="combine">0 取所有合集的合,1 取所有合集的并集, 2取差集 </param>
        /// <returns></returns>
        public RedisValue[] SetCombine(string[] keys, int combine)
        {
            try
            {
                if (combine < 0 || combine > 2)
                {
                    return null;
                }
                var db = GetDataBase();
                return db.SetCombine((SetOperation)combine, ConvertKey(keys));
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SetCombine()   " + error.ToString());
            }
            return null;
        }


        /// <summary>
        /// 判断集合中是否包含此成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetContains(string key, RedisValue value)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.SetContains(key, value);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SetContains()   " + error.ToString());
            }
            return false;
        }


        /// <summary>
        /// 删除集合中一个或多个成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>返回删除是否成或或删除了多少个成员</returns>
        public bool SetRemove(string key, RedisValue value)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.SetRemove(key, value);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SetRemove(1)   " + error.ToString());
            }
            return false;
        }
        public long SetRemove(string key, RedisValue[] values)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.SetRemove(key, values);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SetRemove(2)   " + error.ToString());
            }
            return 0;
        }


        /// <summary>
        /// 迭代集合中符合条件的成员,支持模摸查义
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="math">匹配条件 math = he*o? 可以是*?的通配符形式的字符串</param>
        /// <param name="cursor">起始位置, 如果为0开始新一轮的迭代查询 </param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageOffice">页偏移</param>
        /// <returns>返回一个RedisValue类型的迭代器,可以通过IScanningCursor 将返回值转换为与光标,页相关的对象,用于二次迭代
        /// 返回的结果可能为空,但这并不代表着已经迭代了整个集合,只有当返回结果中Cursor为0是,才可表已经遍历了整个集合</returns>
        public IEnumerable<RedisValue> SetScan(string key, RedisValue math, long cursor = 0, int pageSize = 0, int pageOffice = 0)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                IEnumerable<RedisValue> list = db.SetScan(key, math, pageSize, cursor, pageOffice);
                return list;
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SetScan()   " + error.ToString());
            }
            return null;
        }

        #endregion


        #region 异步方法

        /// <summary>
        /// 向集合中添加一个或多个成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>返回被添加的成员的数量,如果表不存在,新建,如果表不是集合类型,返回一个错误</returns>
        public async void SetAddAsync(string key, RedisValue[] values, Action<long, List<object>> func, List<object> param = null)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                long num = await db.SetAddAsync(key, values);
                if (func != null)
                {
                    func(num, param);
                }
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SetAddAsync()   " + error.ToString());
            }
        }


        /// <summary>
        /// 获取集合中所有的成员
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async void SetMembersAsync(string key, Action<RedisValue[], List<object>> func, List<object> param = null)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                RedisValue[] values = await db.SetMembersAsync(key);
                if (func != null)
                {
                    func(values, param);
                }
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SetMembersAsync()   " + error.ToString());
            }
        }


        /// <summary>
        /// 获取多个集合的合集
        /// </summary>
        /// <param name="keys">集合列表</param>
        /// <param name="combine">0 取所有合集的合,1 取所有合集的并集, 2取差集 </param>
        /// <returns></returns>
        public async void SetCombineAsync(string[] keys, int combine, Action<RedisValue[], List<object>> func, List<object> param = null)
        {
            try
            {
                if (combine < 0 || combine > 2)
                {
                    if (func != null)
                    {
                        func(null, param);
                    }
                    return;
                }
                var db = GetDataBase();
                RedisValue[] values = await db.SetCombineAsync((SetOperation)combine, ConvertKey(keys));
                if (func != null)
                {
                    func(values, param);
                }
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SetCombineAsync()   " + error.ToString());
            }
        }


        /// <summary>
        /// 删除列表中多个成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <param name="func"></param>
        public async void SetRemoveAsync(string key, RedisValue[] values, Action<long, List<object>> func, List<object> param = null)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                long num = await db.SetRemoveAsync(key, values);
                if (func != null)
                {
                    func(num, param);
                }
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SetRemoveAsync()   " + error.ToString());
            }
        }

        #endregion

        #endregion


        #region //SortedSet方法(有序集合)

        #region 同步方法

        /// <summary>
        /// 向有集集合中添加一个成员,并设置score值,如果此成员已经存在更新score值,并重新排序
        /// </summary>
        /// <param name="key"></param>
        /// <param name="score"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SortedAdd(string key, double score, RedisValue value)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.SortedSetAdd(key, value, score);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedAdd(1)   " + error.ToString());
            }
            return false;
        }
        public long SortedAdd(string key, SortedSetEntry[] values)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.SortedSetAdd(key, values);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedAdd(2)   " + error.ToString());
            }
            return 0;
        }
        public long SortedAdd(string key, double[] scores, RedisValue[] values)
        {
            try
            {
                if (scores.Length != values.Length || scores.Length <= 0)
                {
                    return 0;
                }

                List<SortedSetEntry> list = new List<SortedSetEntry>();
                for (int i = 0; i < scores.Length; i++)
                {
                    SortedSetEntry item = new SortedSetEntry(values[i], scores[i]);
                    list.Add(item);
                }

                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.SortedSetAdd(key, list.ToArray());
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedAdd(3)   " + error.ToString());
            }
            return 0;
        }



        /// <summary>
        /// 获取有序集合中成员的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long SortedLength(string key)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.SortedSetLength(key);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedLength(1)   " + error.ToString());
            }
            return -1;
        }
        /// <summary>
        /// 获取有序集合score 在min与max之前成员的数量
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public long SortedLength(string key, double min, double max)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.SortedSetLengthByValue(key, min, max);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedLength(2)   " + error.ToString());
            }
            return -1;
        }


        /// <summary>
        /// 根据排名范围,从有序集合中获取成员,如果排名是0,-1取集合中所有数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">起始位置</param>
        /// <param name="max">结束位置</param>
        /// <param name="orderby">根据排名大小排序 0 从小到大排,1从大到小排</param>
        /// <returns></returns>
        public RedisValue[] SortedRangeRank(string key, long min = 0, long max = -1, int orderby = 1)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.SortedSetRangeByRank(key, min, max, (Order)orderby);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedRangeRank()   " + error.ToString());
            }
            return null;
        }
        /// <summary>
        /// 根据排名范围,从有序集合中获取成员,并带上Score值,其它的同ScoreSetRangeRank方法一样
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public SortedSetEntry[] SortedRangeRankWithScore(string key, long min = 0, long max = -1, int orderby = 1)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.SortedSetRangeByRankWithScores(key, min, max, (Order)orderby);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedRangeRankWithScore()   " + error.ToString());
            }
            return null;
        }


        /// <summary>
        /// 根据score值的范围,从有序集合中获取符合条件的成员,如果min,max使用默认值,则返回表中所有的数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">score的最小值</param>
        /// <param name="max">score的最大值</param>
        /// <param name="orderby">按score值的排序方式 0从小到大排序, 1从大到小排序</param>
        /// <param exclude="exclude">对min,max查找范围的限制也就是开闭区间 0(min<= value <= max), 1(min< value <= max), 2(min <= value < max),3(main < value < max)</param>
        /// <param name="offset">偏移量,从第多少个位置后开如取</param>
        /// <param name="nCnt">取多少个</param>
        /// <returns></returns>
        public RedisValue[] SortedRangeScore(string key, double min = double.NegativeInfinity, double max = double.PositiveInfinity, int orderby = 1, int exclude = 0, long offset = 0, long nCnt = -1)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.SortedSetRangeByScore(key, min, max, (Exclude)exclude, (Order)orderby, offset, nCnt);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedRangeScore()   " + error.ToString());
            }
            return null;
        }
        /// <summary>
        /// 根据Score值的范围,从有序集合中获取符合条件的成员并带Score一起返回,其它的同SortSetRangeScore方法
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min">最小区间</param>
        /// <param name="max">最大区间</param>
        /// <param name="orderby">排序规则 0 从小到大, 1从大到小</param>
        /// <param name="exclude">包含规则 </param>
        /// <param name="offset">偏移量,从第多少个位置后开如取</param>
        /// <param name="nCnt">取多少个</param>
        /// <returns></returns>
        public SortedSetEntry[] SortedRangeScoreWithScore(string key, double min = double.NegativeInfinity, double max = double.PositiveInfinity, int orderby = 1, int exclude = 0, long offset = 0, long nCnt = -1)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.SortedSetRangeByScoreWithScores(key, min, max, (Exclude)exclude, (Order)orderby, offset, nCnt);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedRangeScoreWithScore()   " + error.ToString());
            }
            return null;
        }


        /// <summary>
        /// 获取某个成员在集合中的排名,
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="orderby">排序方式 0从小到大, 1从大到小 </param>
        /// <returns>找到了返回排名,没找到-1</returns>
        public long SortedRank(string key, RedisValue value, int orderby = 1)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                long? index = db.SortedSetRank(key, value, (Order)orderby);
                return index.HasValue ? (long)index : -1;
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedRank()   " + error.ToString());
            }
            return -1;
        }


        /// <summary>
        /// 获取某一个成员的Score值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public double SortedScore(string key, RedisValue value)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                double? score = db.SortedSetScore(key, value);
                return score.HasValue ? (double)score : -1;
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedScore()   " + error.ToString());
            }
            return -1;
        }


        /// <summary>
        /// 从集合中删除一个member
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">member对象</param>
        /// <returns></returns>
        public bool SortedRemove(string key, RedisValue value)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.SortedSetRemove(key, value);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedRemove(1)   " + error.ToString());
            }
            return false;
        }
        /// <summary>
        /// 从集合中删除多个成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values">成员列表</param>
        /// <returns></returns>
        public long SortedRemove(string key, RedisValue[] values)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.SortedSetRemove(key, values);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedRemove(2)   " + error.ToString());
            }
            return -1;
        }
        /// <summary>
        /// 根据排名区间删除成员, 从集合中删除所有在排名范围内的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns>返回删除数据的个数</returns>
        public long SortedRemove(string key, long min, long max)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.SortedSetRemoveRangeByRank(key, min, max);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis:SortedRemove(3)   " + error.ToString());
            }
            return -1;
        }
        /// <summary>
        /// 根据Score区间删除成员,从集合中删除所有在score范围内的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min">score最小值</param>
        /// <param name="max">score最大值</param>
        /// <param name="exclude">根据min,max来计算开闭区间</param>
        /// <returns>返回删除的个数</returns>
        public long SortedRemove(string key, double min, double max, int exclude = 0)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.SortedSetRemoveRangeByScore(key, min, max, (Exclude)exclude);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedRemove(4)   " + error.ToString());
            }
            return -1;
        }
        /// <summary>
        /// 根据成员区间删除成员,删除集合中所有成员在min与max成员之间的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min">起始的成员对象</param>
        /// <param name="max">结整的成员对象</param>
        /// <param name="exclude">开闭区间设置</param>
        /// <returns></returns>
        public long SortedRemove(string key, RedisValue min, RedisValue max, int exclude = 0)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                return db.SortedSetRemoveRangeByValue(key, min, max, (Exclude)exclude);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedRemove(5)   " + error.ToString());
            }
            return -1;
        }


        /// <summary>
        /// 迭代集合中符合条件的成员,支持模摸查义
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="math">匹配条件 math = he*o? 可以是*?的通配符形式的字符串</param>
        /// <param name="cursor">起始位置, 如果为0开始新一轮的迭代查询 </param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageOffice">页偏移</param>
        /// <returns>返回一个SortedSetEntry类型的迭代器,可以通过IScanningCursor 将返回值转换为与光标,页相关的对象,用于二次迭代
        /// 返回的结果可能为空,但这并不代表着已经迭代了整个集合,只有当返回结果中Cursor为0是,才可表已经遍历了整个集合</returns>
        public IEnumerable<SortedSetEntry> SortedScan(string key, RedisValue math, long cursor = 0, int pageSize = 0, int pageOffice = 0)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                IEnumerable<SortedSetEntry> list = db.SortedSetScan(key, math, pageSize, cursor, pageOffice);
                return list;
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedScan()   " + error.ToString());
            }
            return null;
        }

        #endregion


        #region 异步方法

        /// <summary>
        /// 向有集集合中添加一个成员,并设置score值,如果此成员已经存在更新score值,并重新排序
        /// </summary>
        /// <param name="key"></param>
        /// <param name="score"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async void SortedAddAsync(string key, SortedSetEntry[] values, Action<long, List<object>> func, List<object> param = null)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                long num = await db.SortedSetAddAsync(key, values);
                if (func != null)
                {
                    func(num, param);
                }
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedAddAsync(1)   " + error.ToString());
            }
        }
        public async void SortedAddAsync(string key, double[] scores, RedisValue[] values, Action<long, List<object>> func, List<object> param = null)
        {
            try
            {
                if (scores.Length != values.Length || scores.Length <= 0)
                {
                    if (func != null)
                    {
                        func(0, param);
                    }
                    return;
                }

                List<SortedSetEntry> list = new List<SortedSetEntry>();
                for (int i = 0; i < scores.Length; i++)
                {
                    SortedSetEntry item = new SortedSetEntry(values[i], scores[i]);
                    list.Add(item);
                }

                key = GetCustomKey(key);
                var db = GetDataBase();
                long num = await db.SortedSetAddAsync(key, list.ToArray());
                if (func != null)
                {
                    func(num, param);
                }
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedAddAsync(2)   " + error.ToString());
            }
        }


        /// <summary>
        /// 根据排名范围,从有序集合中获取成员,如果排名是0,-1取集合中所有数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">起始位置</param>
        /// <param name="max">结束位置</param>
        /// <param name="orderby">根据排名大小排序 0 从小到大排,1从大到小排</param>
        /// <returns></returns>
        public async void SortedRangeRankAsync(string key, Action<RedisValue[], List<object>> func, List<object> param = null, long min = 0, long max = -1, int orderby = 1)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                RedisValue[] values = await db.SortedSetRangeByRankAsync(key, min, max, (Order)orderby);
                if (func != null)
                {
                    func(values, param);
                }
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedRangeRankAsync()   " + error.ToString());
            }
        }
        /// <summary>
        /// 根据排名范围,从有序集合中获取成员,并带上Score值,其它的同ScoreSetRangeRank方法一样
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public async void SortedRangeRankWithScoreAsync(string key, Action<SortedSetEntry[], List<object>> func, List<object> param = null, long min = 0, long max = -1, int orderby = 1)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                SortedSetEntry[] values = await db.SortedSetRangeByRankWithScoresAsync(key, min, max, (Order)orderby);
                if (func != null)
                {
                    func(values, param);
                }
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedRangeRankWithScoreAsync()   " + error.ToString());
            }
        }


        /// <summary>
        /// 根据score值的范围,从有序集合中获取符合条件的成员,如果min,max使用默认值,则返回表中所有的数据
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="min">score的最小值</param>
        /// <param name="max">score的最大值</param>
        /// <param name="orderby">按score值的排序方式 0从小到大排序, 1从大到小排序</param>
        /// <param exclude="exclude">对min,max查找范围的限制也就是开闭区间 0(min<= value <= max), 1(min< value <= max), 2(min <= value < max),3(main < value < max)</param>
        /// <returns></returns>
        public async void SortedRangeScoreAsync(string key, Action<RedisValue[], List<object>> func, List<object> param = null, double min = double.NegativeInfinity, double max = double.PositiveInfinity, int orderby = 1, int exclude = 0, long offset = 0, long nCnt = -1)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                RedisValue[] values = await db.SortedSetRangeByScoreAsync(key, min, max, (Exclude)exclude, (Order)orderby, offset, nCnt);
                if (func != null)
                {
                    func(values, param);
                }
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedRangeScoreAsync()   " + error.ToString());
            }
        }
        /// <summary>
        /// 根据Score值的范围,从有序集合中获取符合条件的成员并带Score一起返回,其它的同SortSetRangeScore方法
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="orderby"></param>
        /// <param name="exclude"></param>
        /// <returns></returns>
        public async void SortedRangeScoreWithScoreAsync(string key, Action<SortedSetEntry[], List<object>> func, List<object> param = null, double min = double.NegativeInfinity, double max = double.PositiveInfinity, int orderby = 1, int exclude = 0, long offset = 0, long nCnt = -1)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                SortedSetEntry[] values = await db.SortedSetRangeByScoreWithScoresAsync(key, min, max, (Exclude)exclude, (Order)orderby, offset, nCnt);
                if (func != null)
                {
                    func(values, param);
                }
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedRangeScoreWithScoreAsync()   " + error.ToString());
            }
        }


        /// <summary>
        /// 从集合中删除多个成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values">成员列表</param>
        /// <returns></returns>
        public async void SortedRemoveAsync(string key, RedisValue[] values, Action<long, List<object>> func, List<object> param = null)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                long num = await db.SortedSetRemoveAsync(key, values);
                if (func != null)
                {
                    func(num, param);
                }
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedRemoveAsync(1)   " + error.ToString());
            }
        }
        /// <summary>
        /// 根据排名区间删除成员, 从集合中删除所有在排名范围内的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public async void SortedRemoveAsync(string key, long min, long max, Action<long, List<object>> func, List<object> param = null)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                long num = await db.SortedSetRemoveRangeByRankAsync(key, min, max);
                if (func != null)
                {
                    func(num, param);
                }
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedRemoveAsync(2)   " + error.ToString());
            }
        }
        /// <summary>
        /// 根据Score区间删除成员,从集合中删除所有在score范围内的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min">score最小值</param>
        /// <param name="max">score最大值</param>
        /// <param name="exclude">根据min,max来计算开闭区间</param>
        /// <returns></returns>
        public async void SortedRemoveAsync(string key, double min, double max, Action<long, List<object>> func, List<object> param = null, int exclude = 0)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                long num = await db.SortedSetRemoveRangeByScoreAsync(key, min, max, (Exclude)exclude);
                if (func != null)
                {
                    func(num, param);
                }
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedRemoveAsync(3)   " + error.ToString());
            }
        }
        /// <summary>
        /// 根据成员区间删除成员,删除集合中所有成员在min与max成员之间的成员
        /// </summary>
        /// <param name="key"></param>
        /// <param name="min">起始的成员对象</param>
        /// <param name="max">结整的成员对象</param>
        /// <param name="exclude">开闭区间设置</param>
        /// <returns></returns>
        public async void SortedRemoveAsync(string key, RedisValue min, RedisValue max, Action<long, List<object>> func, List<object> param = null, int exclude = 0)
        {
            try
            {
                key = GetCustomKey(key);
                var db = GetDataBase();
                long num = await db.SortedSetRemoveRangeByValueAsync(key, min, max, (Exclude)exclude);
                if (func != null)
                {
                    func(num, param);
                }
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SortedRemoveAsync(4)   " + error.ToString());
            }
        }

        #endregion

        #endregion


        #region //订阅,发布方法

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="subChannel">消息名称</param>
        /// <param name="handler">回调句柄</param>
        public void SubScribe(string subChannel, Action<RedisChannel, RedisValue> handler = null)
        {
            try
            {
                ISubscriber sub = _connect.GetSubscriber();
                sub.Subscribe(subChannel, (channel, message) =>
                {
                    if (handler == null)
                    {
                        Console.WriteLine(subChannel + " 订阅收到消息：" + message);
                    }
                    else
                    {
                        handler(channel, message);
                    }
                });
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SubScribe()   " + error.ToString());
            }
        }


        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel">消息名称</param>
        /// <param name="msg">消息内容</param>
        /// <returns>返回这条消息被多少个临听处理</returns>
        public long SendScribe<T>(string channel, T msg)
        {
            try
            {
                ISubscriber sub = _connect.GetSubscriber();
                return sub.Publish(channel, ConvertJson(msg));
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SendScribe()   " + error.ToString());
            }
            return -1;
        }
        public long SendScribe(string channel, RedisValue msg)
        {
            try
            {
                ISubscriber sub = _connect.GetSubscriber();
                return sub.Publish(channel, msg);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::SendScribe(2)   " + error.ToString());
            }
            return -1;
        }


        /// <summary>
        /// 取消某条消息的订阅 
        /// </summary>
        /// <param name="channel">消息名称</param>
        public void UnSubScribe(string channel)
        {
            try
            {
                ISubscriber sub = _connect.GetSubscriber();
                sub.Unsubscribe(channel);
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::UnSubScribe()   " + error.ToString());
            }
        }


        /// <summary>
        /// 取消全部消息的订阅
        /// </summary>
        public void UnSubScribeAll()
        {
            try
            {
                if (_connect != null)
                {
                    ISubscriber sub = _connect.GetSubscriber();
                    if (sub != null)
                    {
                        sub.UnsubscribeAll();
                    }
                }
            }
            catch (Exception error)
            {
                //LogOut.Instance.PrintLog("Redis::UnSubScribeAll()   " + error.ToString());
            }
        }

        #endregion


        #region //各种辅助函数


        //获取完整的KEY
        public RedisKey GetCustomKey(string szKey)
        {
            return (RedisKey)szKey;
        }

        public RedisKey[] GetCustomKey(string[] keys)
        {
            return keys.Select(redisKey => (RedisKey)GetCustomKey(redisKey)).ToArray();
        }

        //获取Key类型的文字描述
        public static string GetKeyType(int type)
        {
            switch (type)
            {
                case 1:
                    {
                        return "String";
                    }
                case 2:
                    {
                        return "List";
                    }
                case 3:
                    {
                        return "Set";
                    }
                case 4:
                    {
                        return "SortedSet";
                    }
                case 5:
                    {
                        return "Hash";
                    }
            }
            return "UnKnow";
        }
        public static string GetKeyType(RedisType type)
        {
            return GetKeyType((int)type);
        }


        //将RedisValue,对象 转换成字符串
        public static string ConvertJson(RedisValue value)
        {
            return value.ToString();
        }
        public static string ConvertJson<T>(T value)
        {
            return value is string ? value.ToString() : JsonHelper.Serialize(value);
        }
        public static string ConvertJson(RedisKey key)
        {
            return key.ToString();
        }
        public static string[] ConvertJson(RedisValue[] values)
        {
            List<string> list = new List<string>();

            for (int i = 0; i < values.Length; i++)
            {
                if (!values[i].IsNullOrEmpty)
                {
                    list.Add(ConvertJson(values[i]));
                }
            }
            return list.ToArray();
        }
        public static string[] ConvertJson<T>(T[] values)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != null)
                {
                    list.Add(ConvertJson(values[i]));
                }
            }
            return list.ToArray();
        }
        public static string[] ConvertJson(RedisKey[] keys)
        {
            return keys.Select(redisKey => redisKey.ToString()).ToArray();
        }


        //将RedisVal, json  数据转换成对象
        public static T ConvertObj<T>(RedisValue value)
        {
            if (value.IsNullOrEmpty)
            {
                return default(T);
            }
            return JsonHelper.Deserialize<T>(value);
        }
        public static T ConvertObj<T>(string szJson)
        {
            if (string.IsNullOrEmpty(szJson))
            {
                return default(T);
            }

            return JsonHelper.Deserialize<T>(szJson);
        }
        public static T[] ConvertObj<T>(RedisValue[] values)
        {
            List<T> result = new List<T>();

            foreach (RedisValue item in values)
            {
                if (!item.IsNullOrEmpty)
                {
                    result.Add(ConvertObj<T>(item));
                }
            }
            return result.ToArray();
        }
        public static T[] ConvertObj<T>(string[] szJson)
        {
            List<T> result = new List<T>();
            foreach (string item in szJson)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    result.Add(ConvertObj<T>(item));
                }
            }
            return result.ToArray();
        }


        //将字符串转换成RedisKey
        public static RedisKey ConvertKey(string redisKey)
        {
            return (RedisKey)redisKey;
        }
        public static RedisKey[] ConvertKey(string[] redisKeys)
        {
            return redisKeys.Select(redisKey => (RedisKey)redisKey).ToArray();
        }


        //将字符串,对象 转换成RedisValue
        public static RedisValue ConvertValue(string szJson)
        {
            return (RedisValue)szJson;
        }
        public static RedisValue ConvertValue<T>(T obj)
        {
            return obj is string ? obj.ToString() : JsonHelper.Serialize(obj);
        }
        public static RedisValue[] ConvertValue(string[] szJsonList)
        {
            return szJsonList.Select(value => (RedisValue)value).ToArray();
        }
        public static RedisValue[] ConvertValue<T>(T[] objList)
        {
            List<RedisValue> tempList = new List<RedisValue>();
            foreach (var item in objList)
            {
                string szJson = ConvertValue(item);
                tempList.Add((RedisValue)szJson);
            }
            return tempList.ToArray();
        }


        public static List<T> ConvetList<T>(RedisValue[] values)
        {
            List<T> result = new List<T>();
            foreach (var item in values)
            {
                var model = ConvertObj<T>(item);
                result.Add(model);
            }
            return result;
        }

        private T Do<T>(Func<IDatabase, T> func)
        {
            var database = _connect.GetDatabase(_dbNum);
            return func(database);
        }
        #endregion


        #region //错误通知回调

        public Action<object, ConnectionFailedEventArgs> OnConnectFailed;
        public Action<object, ConnectionFailedEventArgs> OnConnectRestored;

        /// <summary>
        /// 与Redis数据库链接失败的通知回调
        /// </summary>
        /// <param name="error">错误信息</param>
        private void ConnectFailed(object sender, ConnectionFailedEventArgs error)
        {
            //LogOut.Instance.PrintLog("Redis数据库对象链接失败,ID:" + _connectID + "  Endpoint failed: " + error.EndPoint + ", " + error.FailureType + (error.Exception == null ? "" : (", " + error.Exception.Message)));

            OnConnectFailed?.Invoke(sender, error);
            //重链
            Connect();



            //释放对象
            //Release();
        }

        /// <summary>
        /// 重新与Redis建立链接之前的错误通知回调
        /// </summary>
        /// <param name="error">错误信息</param>
        private void ConnectRestored(object sender, ConnectionFailedEventArgs error)
        {
            string szError = string.Format("ConnectRestored()  Redis数据库对象重链失败,ID:{0},  {1}", _connectID, error.EndPoint);
            //LogOut.Instance.PrintLog(szError);
            OnConnectRestored?.Invoke(sender, error);


            //重链
            Connect();

            ////释放对象
            //Release();
        }

        /// <summary>
        /// 发生错误时,通知回调
        /// </summary>
        /// <param name="error">错误信息</param>
        private void ErrorMessage(object sender, RedisErrorEventArgs error)
        {
            string szError = string.Format("ErrorMessage()  Redis数据库错误,对象ID:{0},  {1}", _connectID, error.Message);
            //LogOut.Instance.PrintLog(szError);
        }

        /// <summary>
        /// redis数据库发生配置改变时的通知回调
        /// </summary>
        /// <param name="arge">通知信息</param>
        private void ConfigChange(object sender, EndPointEventArgs arge)
        {
            string szError = string.Format("ConfigChange()  Redis数据库配置改变,  对象ID:{0}, 改变描述:{1}", _connectID, arge.EndPoint);
            //LogOut.Instance.PrintLog(szError);
        }

        /// <summary>
        /// 更改集群时
        /// </summary>
        /// <param name="arge">通知信息</param>
        private void HashSlotMoved(object sender, HashSlotMovedEventArgs arge)
        {
            string szError = string.Format("HashSlotMoved()   Redis数据库集群改变,对象ID:{0},   NewEndPoint:{1},  OldEndPoint:{1}",
                _connectID, arge.NewEndPoint, arge.OldEndPoint);

            //LogOut.Instance.PrintLog(szError);
        }

        /// <summary>
        /// redis类库错误
        /// </summary>
        /// <param name="error">错误信息</param>
        private void InternalError(object sender, InternalErrorEventArgs error)
        {
            string szError = string.Format("InternalError()   Redis数据库错误,对象ID:{0},   {1}",error.EndPoint, error.Exception.Message);
            //LogOut.Instance.PrintLog(szError);
        }

        #endregion


        #region //私有辅助


        private void BeginPrint(string log)
        {
            //LogOut.Instance.BeginPrint(log);
        }
        private void BeginPrint(string func, string key)
        {
            string szLog = string.Format("Redis::{0}   Key:{1}", func, key);
            //LogOut.Instance.BeginPrint(szLog);
        }
        private void BeginPrint(string func, string[] keys)
        {
            string szLog = string.Format("Redis::{0}   Keys:", func);

            for (int i = 0; i < keys.Length; i++)
            {
                szLog += keys[i] + ",   ";
            }
            //LogOut.Instance.BeginPrint(szLog);
        }


        private void Print(string value)
        {
            string szLog = string.Format("Error:{0}", value);
            //LogOut.Instance.Print(szLog);
        }
        private void Print(Exception error)
        {
            string szLog = string.Format("Error:{0}", error.ToString());
            //LogOut.Instance.Print(szLog);
        }



        #endregion
    }
}
 