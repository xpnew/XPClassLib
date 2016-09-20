using System;
using System.Collections.Generic;
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

        public static int RequestInt(string itemname)
        {
            if (null == Request[itemname] || 0 == Request[itemname].Length)
            {
                return ErrorInputInt;
            }

            string FieldString = Request[itemname].Trim();
            if (vbs.IsNumric(FieldString))
            {
                return int.Parse(FieldString);
            }
            else
            {
                return NotInitIntInput;
            }

        }


        public static string FindString(string name)
        {
            return FindString(new string[] { name });
        }


        /// <summary>
        /// 从指定的多个参数当中查找第一个有值的结果，主要是为了适应传送的参数不确定性。
        /// </summary>
        /// <remarks>
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

                if (Request.Form.AllKeys.Contains(name) || Request.QueryString.AllKeys.Contains(name))
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

    }
}
