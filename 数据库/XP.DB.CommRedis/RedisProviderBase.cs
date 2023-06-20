using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Util.Json;

namespace XP.DB.CommRedis
{
    public class RedisProviderBase
    {
        /// <summary>
        /// 主key，拼接后的完整名
        /// </summary>
        public string FullKey { get; set; }

        /// <summary>
        /// 指定引擎（连接）
        /// </summary>
        protected EngineBase Engine { get; set; }

        /// <summary>
        /// 内部调用的解析器
        /// </summary>
        protected RedisHelper RedisUtil { get; set; }


        public RedisProviderBase()
        {

        }
        public RedisProviderBase(string key) : this()
        {

        }
        public RedisProviderBase(string key, EngineBase engin) : this(key)
        {
            Engine = engin;
            _InitEngin();

        }
        protected void _InitEngin()
        {
            RedisUtil = Engine.Helper;
        }



        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="subkey"></param>
        /// <param name="subval"></param>
        public void Insert(string subkey, string subval)
        {
            RedisUtil.HashSet(FullKey, (RedisValue)subkey, (RedisValue)subval);
        }

        /// <summary>
        /// 按照模式（通配符）查找list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keysPattern"></param>
        /// <returns></returns>
        public List<T> FindList<T>(string keysPattern)
        {

            RedisKey[] vals = RedisUtil.Keys(keysPattern);

            return RedisEntityTranser.ConvetList<T>(RedisUtil, vals);

        }

        public bool Exist(string subkey)
        {
            return RedisUtil.HashExists(FullKey, subkey);
        }
        public bool Exist(int subkey)
        {
            return RedisUtil.HashExists(FullKey, subkey);
        }


        public List<T> GetAll<T>() where T : class, new()
        {
            return RedisEntityTranser.TransValues<T>(this.RedisUtil, FullKey);
        }


        public bool Remove(string key)
        {
            return RedisUtil.HashDelete(FullKey, key);
        }

        #region 其它的实现，测试过有可能RedisHelper不支持

        public void Insert<T>(string subkey, T subval)
        {
            var json = JsonHelper.Serialize(subval);
            RedisUtil.HashSet(FullKey, subkey, json);
        }
        public void Insert<T>(int subkey, T subval)
        {
            var json = JsonHelper.Serialize(subval);
            RedisUtil.HashSet(FullKey, subkey, json);
        }



        /// <summary>
        /// 根据模糊查询删除
        /// </summary>
        /// <remarks>
        /// https://q.cnblogs.com/q/86885/
        /// 
        /// https://www.cnblogs.com/felixnet/p/8456154.html
        /// </remarks>
        /// <param name="pattern"></param>
        public virtual void DelKeysPattern(string pattern)
        {

            RedisKey[] vals = RedisUtil.Keys(pattern);


            RedisUtil.KeyDeleteAsync(vals.Select(v=>v.ToString()).ToArray(), (i, pars) => {
            
            
            }, null);


        }

        /// <summary>
        /// 通过key获取一个实体对象，实体是事先已经序列化完成的JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subkey"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public T GetEntityByKey<T>(string subkey, T def)
        {
            var val = RedisUtil.HashGet(FullKey, (RedisValue)subkey);
            if (val.HasValue)
            {
                return RedisEntityTranser.ConvertObj<T>(val);
            }
            return def;
        }

        public T FindOne<T>(object k, T def)
        {
            var val = RedisUtil.HashGet(FullKey, (RedisValue)k);
            if (val.HasValue)
            {
                object o = (object)val;
                return (T)o;
            }
            return def;
        }
        public int FindInt(string k, int def = 0)
        {
            var val = RedisUtil.HashGet(FullKey, (RedisValue)k);
            if (val.HasValue)
            {
                return (int)val;
            }
            return def;
        }

        public long FindLong(string k, long def = 0)
        {
            var val = RedisUtil.HashGet(FullKey, k);
            if (val.HasValue)
            {
                return (long)val;
            }
            return def;
        }
        public string FindString(string k, string def = "")
        {
            var val = RedisUtil.HashGet(FullKey, (RedisValue)k);
            if (val.HasValue)
            {
                return val.ToString();
            }
            return def;
        }


        #endregion
    }
}
