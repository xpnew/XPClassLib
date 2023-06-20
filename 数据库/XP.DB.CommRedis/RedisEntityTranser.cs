using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Util.Json;

namespace XP.DB.CommRedis
{
    /// <summary>
    /// 依托EntityCache对RedisValue转换
    /// </summary>
    public class RedisEntityTranser
    {

        public static List<T> TransValues<T>(RedisHelper helper, string key)
            where T : class, new()
        {

            //var redisKey = helper.GetCustomKey(key);

            RedisValue[] values = helper.HashAllValue(key);

            return ConvetList<T>(values);
        }


        public static T ConvertObj<T>(RedisValue value)
        {
            if (typeof(T).Name.Equals(typeof(string).Name))
            {
                return JsonHelper.Deserialize<T>($"'{value}'");
            }
            return JsonHelper.Deserialize<T>(value);
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


        public static List<T> ConvetList<T>(RedisHelper helper, RedisKey[] values)
        {
            List<T> result = new List<T>();
            foreach (var k in values)
            {
                RedisValue item = helper.StringGet(k);
                var model = ConvertObj<T>(item);
                result.Add(model);
            }
            return result;
        }
    }
}
