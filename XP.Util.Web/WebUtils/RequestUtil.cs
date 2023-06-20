using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace XP.Util.WebUtils
{
    /// <summary>
    /// Request工具，获页面上提交的数据
    /// </summary>
    public static class RequestUtil
    {

        public static HttpRequest Request
        {
            get
            {
                return HttpContext.Current.Request;
            }
        }
        /// <summary>
        /// 找不到这个字段，查询的字段为空
        /// </summary>
        private static int _NoIntField = -1;

        
        public static int NoIntField
        {
            get { return RequestUtil._NoIntField; }
            set { RequestUtil._NoIntField = value; }
        }

        /// <summary>
        /// 错误的int输入
        /// </summary>
        public static readonly int ErrorInputInt = -7898654;
        /// <summary>
        /// 无法转换成int输入
        /// </summary>
        public static readonly int NotInitIntInput = -7896543;

        /// <summary>
        /// 获取页面上提交的整数值
        /// </summary>
        /// <param PropertyName="itemname"></param>
        /// <returns></returns>
        public static int? GetInt(string itemname)
        {
            if (null == Request[itemname] || 0 == Request[itemname].Length)
            {
                return null;
            }
            string FieldString = Request[itemname].Trim();
            if (vbs.IsNumric(FieldString))
            {
                return int.Parse(FieldString);
            }
            else
            {
                return ErrorInputInt;
            }
        }

        public static int RequestInt(string itemname,int? def = null)
        {

            if (null == Request[itemname] || 0 == Request[itemname].Length)
            {
                if (def.HasValue) return def.Value;
                return ErrorInputInt;
            }

            string FieldString = Request[itemname].Trim();
            if (vbs.IsNumric(FieldString))
            {
                return int.Parse(FieldString);
            }
            else
            {
                if (def.HasValue) return def.Value;
                return NotInitIntInput;
            }
        }


        public static bool GetBoolean(string key, bool def = false)
        {
           var rq = Request.Params;
            bool Result = def;
            if (null == rq)
            {
              
            }
            string input = rq[key];

            if (!String.IsNullOrEmpty(input))
            {
                if ("1" == input || "true" == input.ToLower())
                {
                    //Result = Boolean.Parse(input);
                    Result = true;
                }
                else
                {
                    Result = false;
                }

                return Result;
            }
            return Result;
        }




        public static string FindString(string name)
        {
            return FindString(new string[] { name });
        }


        /// <summary>
        /// 从指定的多个参数当中查找第一个有值的结果，主要是为了适应传送的参数不确定性。
        /// </summary>
        /// <remarks>
        /// ▲▲▲ !!不区分大小写了
        /// 主要用来适用不同的命名习惯各种情形
        /// 例如：不确定是“OrderName”、“OrderBy”、“Ord”、
        /// 再例如：不确定使用的是“Username”还是“LoginName”
        /// </remarks>
        /// <param PropertyName="nameArgs">一个或者多个参数构成的数组</param>
        /// <returns></returns>
        public static string FindString(string[] nameArgs)
        {
            string Result = null ;
            foreach (string name in nameArgs)
            {
               // var list = from Request.Form.AllKeys
                //增了忽略大小写
                if (Request.Form.AllKeys.Contains(name, StringComparer.OrdinalIgnoreCase) || Request.QueryString.AllKeys.Contains(name, StringComparer.OrdinalIgnoreCase))
                {
                    Result =  Request[name];

                    if (0 != Result.Length)
                    {
                        return Result;
                    }
                }
            }
            return Result;
        }

        /// <summary>
        /// 检查一个整数的输入，不能是空，也不能是格式错误
        /// </summary>
        /// <param PropertyName="input"></param>
        /// <returns></returns>
        public static bool CheckIntInput(int? input)
        {
            if (input.HasValue && input.Value != ErrorInputInt)
            {
                return true;
            }
            return false;
        }


        #region json


        public static string FindValue4Json(string name)
        {
            var json = GetRequestJson();
            //dynamic ParObject = JsonConvert.DeserializeObject<dynamic>(json);
            var obj = Json.JsonHelper.Deserialize<Dictionary<string, string>>(json);
            if (obj.ContainsKey(name))
            {
                return obj[name];
            }
            return null;
        }

        public static string GetRequestJson(Stream stream = null)
        {
            if (null == stream)
            {
                stream = Request.InputStream;
            }
            string json = string.Empty;
            if (stream.Length != 0)
            {

                StreamReader streamreader = new StreamReader(stream);
                if (0 < stream.Position)
                {
                    stream.Position = 0;
                }
                //streamreader.
                json = streamreader.ReadToEnd();
            }
            return json;
        }

        public static string DebugInfo()
        {
            string ResultText = String.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append("---------------    ≯  Request 信息 Debug  ≮  ---------------");

            sb.Append("\n请求地址 ：");
            //获取 完整url （协议名 + 域名 + 站点名 + 文件名 + 参数）
            string url = Request.Url.ToString();
            //url = http://www.jb51.net/aaa/bbb.aspx?id=5&name=kelli
            sb.Append(url);
            sb.Append("\nJson 数据：");

            sb.Append(GetRequestJson());

            if (0 <= Request.Form.Count)
            {
                sb.Append("\n Form 数据：\n");

                for (int i = 0; i < Request.Form.Count; i++)
                {
                    if (0 < i)
                    {
                        sb.Append("&");
                    }
                    sb.Append(Request.Form.Keys[i]);

                    sb.Append("〓");
                    sb.Append(Request.Form[i]);
                }
            }

            ResultText = sb.ToString();


            return ResultText;
        }


        public static T FindModel4Json<T>() where T : class
        {
            var json = GetRequestJson();
            //dynamic ParObject = JsonConvert.DeserializeObject<dynamic>(json);
            var obj = Json.JsonHelper.Deserialize<T>(json);

            return obj;
        }

        public static Dictionary<string, T> FindDict4Json<T>()
        {
            var json = GetRequestJson();
            //dynamic ParObject = JsonConvert.DeserializeObject<dynamic>(json);
            var obj = Json.JsonHelper.Deserialize<Dictionary<string, T>>(json);

            return obj;
        }


        #endregion



        #region 获取IP地址
        /// <summary>
        /// 获取IP地址
        /// </summary>
        public static string GetIp(HttpRequest request =  null)
        {
            //HttpRequest request = HttpContext.Current.Request;
            if (null == request)
            {
                request = HttpContext.Current.Request;
            }
            // 如果使用代理，获取真实IP
            string userIp = request.ServerVariables["HTTP_X_FORWARDED_FOR"] != ""
                ? request.ServerVariables["REMOTE_ADDR"]
                : request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(userIp))
                userIp = request.UserHostAddress;
            return userIp;
        }
        #endregion

    }
}
