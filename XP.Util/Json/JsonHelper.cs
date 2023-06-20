using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Util.Json
{
    /// <summary>
    /// json格式化的常规选项
    /// </summary>
    public enum JsonToolFormatSet
    {
        /// <summary>
        /// 默认的情况，无参，日期未处理
        /// </summary>
        Default,
        /// <summary>
        /// 常规的用法，指定了日期的格式化方式，以及跳过空白字段
        /// </summary>
        Normal,
        /// <summary>
        /// 除了Normal的条件以外，还会忽略字段上的【Ignore】设置，即输出全部字段。
        /// </summary>
        NoIgnore,
        /// <summary>
        /// java的日期格式，没有毫秒
        /// </summary>
        JavaTimeFormat,

    }

    public class JsonHelper
    {
        #region 升级的序列化

        public static string ToJson(object o, JsonToolFormatSet set = JsonToolFormatSet.Normal, bool enableIndented = false)
        {
            JsonSerializerSettings setting;

            if (set == JsonToolFormatSet.Normal)
            {
                setting = new JsonSerializerSettings();
                setting.NullValueHandling = NullValueHandling.Ignore;
                setting.DateFormatString = "yyyy-MM-dd HH:mm:ss.sss";

            }
            else if (set == JsonToolFormatSet.NoIgnore)
            {
                setting = new JsonSerializerSettings();
                setting.NullValueHandling = NullValueHandling.Ignore;
                setting.DateFormatString = "yyyy-MM-dd HH:mm:ss.sss";
                setting.ContractResolver = new Comm.Json.NoIgnoreContractResolver();
            }
            else if (set == JsonToolFormatSet.JavaTimeFormat)
            {
                setting = new JsonSerializerSettings();
                setting.NullValueHandling = NullValueHandling.Ignore;
                setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";

            }
            else
            {
                setting = null;
            }
            if (null != setting)
            {
                if (enableIndented)
                {
                    setting.Formatting = Formatting.Indented;
                }
                else
                {
                    setting.Formatting = Formatting.None;
                }
            }
            return ToJson(o, setting);
        }

        public static string ToJson(object o, Type type, JsonToolFormatSet set = JsonToolFormatSet.Normal)
        {
            JsonSerializerSettings setting;

            if (set == JsonToolFormatSet.Normal)
            {
                setting = new JsonSerializerSettings();
                setting.NullValueHandling = NullValueHandling.Ignore;
                setting.DateFormatString = "yyyy-MM-dd HH:mm:ss.sss";

            }
            else if (set == JsonToolFormatSet.NoIgnore)
            {
                setting = new JsonSerializerSettings();
                setting.NullValueHandling = NullValueHandling.Ignore;
                setting.DateFormatString = "yyyy-MM-dd HH:mm:ss.sss";
                setting.ContractResolver = new Comm.Json.NoIgnoreContractResolver();
            }
            else if (set == JsonToolFormatSet.JavaTimeFormat)
            {
                setting = new JsonSerializerSettings();
                setting.NullValueHandling = NullValueHandling.Ignore;
                setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";

            }
            else
            {
                setting = null;
            }
            return JsonConvert.SerializeObject(o, type, setting);
        }

        private static string ToJson(object o, JsonSerializerSettings setting)
        {
            if (null == setting)
            {
                return JsonConvert.SerializeObject(o);
            }
            return JsonConvert.SerializeObject(o, setting);
        }


        public static T ToEntity<T>(string input, JsonToolFormatSet set = JsonToolFormatSet.Normal)
        {
            JsonSerializerSettings setting;

            if (set == JsonToolFormatSet.Normal)
            {
                setting = new JsonSerializerSettings();
                setting.NullValueHandling = NullValueHandling.Ignore;
                setting.DateFormatString = "yyyy-MM-dd HH:mm:ss.sss";

            }
            else if (set == JsonToolFormatSet.NoIgnore)
            {
                setting = new JsonSerializerSettings();
                setting.NullValueHandling = NullValueHandling.Ignore;
                setting.DateFormatString = "yyyy-MM-dd HH:mm:ss.sss";
                setting.ContractResolver = new Comm.Json.NoIgnoreContractResolver();
            }
            else
            {
                setting = null;
            }
            return ToEntity<T>(input, setting);
        }
        public static T ToEntity<T>(string input, JsonSerializerSettings setting)
        {
            if (null == setting)
            {
                return JsonConvert.DeserializeObject<T>(input);
            }
            try
            {
                return JsonConvert.DeserializeObject<T>(input, setting);

            }
            catch (Exception e)
            {
                return default(T);
            }
        }

        #endregion

        #region 基本的序列化、反序列化

        public static string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data);
        }


        public static T Deserialize<T>(string json)
        {
            if (String.IsNullOrEmpty(json))
            {
                return default(T);
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(json);

            }
            catch (Exception e)
            {
                return default(T);
            }

        }

        public static T Clone<T>(T source)
        {
            var ttt = default(T);
            if (source.Equals(ttt))
            {
                return default(T);
            }
            string json = Serialize(source);

            return Deserialize<T>(json);

        }

        #endregion





        #region 使用匿名类 做模板 反序列化

        public static T? ToAnyObj<T>(string jsonInput, T definition) where T: class
        {
            if (String.IsNullOrEmpty(jsonInput)) return null;
            return Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(jsonInput, definition);
        }

        #endregion



        /// <summary>
        /// 针对不特定的实体，反序列化成字典，不适用于数组类型
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDict(string jsonStr, bool enableNull = false)
        {
            var Result = new Dictionary<string, string>();
            JToken SourceToken = JToken.Parse(jsonStr);

            var oType = SourceToken.Type;
            if (oType != JTokenType.Object)
            {
                return null;
            }
            JObject jobj = JObject.Parse(jsonStr);


            //var reader = SourceToken.CreateReader();
            //while (reader.Read())
            //{
            //    string key =  reader.DateParseHandling
            //}
            foreach (JToken child in jobj.Children())
            {
                if (child is JProperty)
                {
                    var property1 = child as JProperty;
                    var val = property1.Value;
                    if (val.Type == JTokenType.Array || val.Type == JTokenType.Object)
                    {
                        continue;
                    }
                    if (!enableNull && (val.Type == JTokenType.Null || val.Type == JTokenType.None))
                    {
                        continue;
                    }
                    Result.Add(property1.Name, property1.Value.ToString());
                }
            }

            return Result;
        }


        public static bool EixstProperty(string json, string key)
        {
            JToken SourceToken;

            try
            {
                SourceToken = JToken.Parse(json);
            }
            catch (Exception ex)
            {
                return false;
            }

            JToken ExistNode = SourceToken.SelectToken(key);

            if (null == ExistNode)
            {
                return false;
            }
            if (ExistNode.Type == JTokenType.Null || ExistNode.Type == JTokenType.None)
            {
                return false;
            }

            //判断节点类型
            if (ExistNode.Type == JTokenType.String)
            {
                //返回string值
                var val = ExistNode.Value<String>();
                if (0 == val.Trim().Length)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
