using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace XP.Util.Json
{

    /// <summary>
    /// 空白处理模式
    /// </summary>
    [FlagsAttribute]
    public enum NullModeDef
    {

        UnKnow = 0,

        /// <summary>
        /// 某属性即便是null，当作存在
        /// </summary>
        Default = 1,


        /// <summary>
        /// 某属性即便是null，当作存在
        /// </summary>
        NullIsExist = 2,

        /// <summary>
        /// 某属性0长度字符串，当作存在
        /// </summary>
        EmptyIsExist = 4,


        /// <summary>
        /// 某属性即便是null，当作字符串当中的Empty
        /// </summary>
        NullReturnEmpty = 1024,

    }

    /// <summary>
    /// json识别工具
    /// </summary>
    public class JsonDiscern
    {

        /// <summary>
        /// 判断json当中某个项是否存在
        /// </summary>
        /// <param name="json">输入的json字符串，空白则返回false</param>
        /// <param name="key">要查找的项</param>
        /// <param name="nullMode">空白字符串的处理模式</param>
        /// <param name="enableChildren">允许搜索子项</param>
        /// <returns></returns>
        public static bool Exist(string json, string key, NullModeDef nullMode = NullModeDef.Default, bool enableChildren = false)
        {
            if (String.IsNullOrWhiteSpace(json))
            {
                return false;
            }

            JToken SourceToken = JToken.Parse(json);

            var oType = SourceToken.Type;
            if (oType != JTokenType.Object)
            {
                return false;
            }
            JObject jobj = JObject.Parse(json);

            return Exist(SourceToken, key, nullMode, enableChildren);


            return false;
        }

        private static bool Exist(JToken jobj, string key, NullModeDef nullMode = NullModeDef.Default, bool enableChildren = false)
        {
            foreach (JToken child in jobj.Children())
            {
                if (child is JProperty)
                {
                    var property1 = child as JProperty;
                    var val = property1.Value;
                    if (val.Type == JTokenType.Array || val.Type == JTokenType.Object)
                    {
                        if (enableChildren)
                        {
                            var childEixst = Exist(val, key, nullMode, enableChildren);

                        }

                        continue;
                    }


                    if (property1.Name.Equals(key))
                    {
                        //光有同名键还不行，可能还要判断是不是空值键
                        if (((nullMode & NullModeDef.NullIsExist) != NullModeDef.NullIsExist) && (val.Type == JTokenType.Null || val.Type == JTokenType.None))
                        {
                            return false;
                        }

                        string str = property1.Value.ToString();

                        if (String.IsNullOrWhiteSpace(str) && (nullMode & NullModeDef.EmptyIsExist) != NullModeDef.EmptyIsExist)
                        {
                            return false;
                        }
                        return true;
                    }


                }
            }

            return false;
        }
    }
}
