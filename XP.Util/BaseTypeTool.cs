using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Util
{
    /// <summary>
    /// 基本类型工具（int bool string的常规处理）
    /// </summary>
    /// <remarks>
    /// GeneralTool已经比较膨胀了 所以，这里用BaseTypeTool这个词
    /// </remarks>
    public class BaseTypeTool
    {

        #region int和long
        public static int String2Int(string input, int def = XP.Comm.Constant.NotExistInt)
        {
            if (null == input || 0 == input.Length)
            {
                return def;
            }

            if (vbs.IsInt(input))
            {
                return int.Parse(input);
            }
            else
            {
                return XP.Comm.Constant.ErrorInt;
            }

        }

        public static int StringDict2Int(Dictionary<string, string> dict, string key, int def = XP.Comm.Constant.NotExistInt)
        {
            if (dict.ContainsKey(key))
            {
                string val = dict[key];
                return String2Int(val, def);
            }
            return def;
        }
        public static int StringDict2Int(Dictionary<string, object> dict, string key, int def = XP.Comm.Constant.NotExistInt)
        {
            if (dict.ContainsKey(key))
            {
                var val = dict[key];
                if (null == val)
                {
                    return def;
                }
                return String2Int(val.ToString(), def);
            }
            return def;
        }


        public static long String2Long(string input, long def = XP.Comm.Constant.NotExistLong)
        {
            if (null == input || 0 == input.Length)
            {
                return def;
            }

            if (vbs.IsInt(input))
            {
                return long.Parse(input);
            }
            else
            {
                return XP.Comm.Constant.ErrorLong;
            }

        }

        public static long StringDict2Long(Dictionary<string, string> dict, string key, long def = XP.Comm.Constant.NotExistLong)
        {
            if (dict.ContainsKey(key))
            {
                string val = dict[key];
                return String2Long(val, def);
            }
            return def;
        }
        public static long StringDict2Long(Dictionary<string, object> dict, string key, long def = XP.Comm.Constant.NotExistLong)
        {
            if (dict.ContainsKey(key))
            {
                var val = dict[key];
                if (null == val)
                {
                    return def;
                }
                return String2Long(val.ToString(), def);
            }
            return def;
        }


        #endregion



        #region decimal
        public static decimal String2Decimal(string input, decimal def = XP.Comm.Constant.NotExistDecimal)
        {
            if (null == input || 0 == input.Length)
            {
                return def;
            }
            decimal tem;
            if (Decimal.TryParse(input, out tem))
            {
                return tem;
            }
            else
            {
                return XP.Comm.Constant.ErrorDecimal;
            }
        }

        public static decimal StringDict2Decimal(Dictionary<string, string> dict, string key, decimal def = XP.Comm.Constant.NotExistDecimal)
        {
            if (dict.ContainsKey(key))
            {
                string val = dict[key];
                return String2Decimal(val, def);
            }
            return def;
        }
        public static decimal StringDict2Decimal(Dictionary<string, object> dict, string key, decimal def = XP.Comm.Constant.NotExistDecimal)
        {
            if (dict.ContainsKey(key))
            {
                var val = dict[key];
                if (null == val)
                {
                    return def;
                }
                return String2Decimal(val.ToString(), def);
            }
            return def;
        }


        #endregion

        #region bool

        public static bool String2Bool(string input, bool def = false)
        {
            if (null == input || 0 == input.Length)
            {
                return def;
            }
            string val = input.ToLower();

            if ("1" == val || "true" == val)
            {
                return true;
            }
            else if ("0" == val || "false" == val)
            {
                return false;
            }
            else
            {
                return def;
            }
        }

        public static bool StringDict2Bool(Dictionary<string, string> dict, string key, bool def = false)
        {
            if (dict.ContainsKey(key))
            {
                string val = dict[key];
                return String2Bool(val, def);
            }
            return def;
        }


        public static bool? String2NullBool(string input, bool? def = null)
        {
            if (null == input || 0 == input.Length)
            {
                return def;
            }
            string val = input.ToLower();

            if ("1" == val || "true" == val)
            {
                return true;
            }
            else if ("0" == val || "false" == val)
            {
                return false;
            }
            else
            {
                return def;
            }
        }

        public static bool? StringDict2NullBool(Dictionary<string, string> dict, string key, bool? def = null)
        {
            if (dict.ContainsKey(key))
            {
                string val = dict[key];
                return String2NullBool(val, def);
            }
            return def;
        }



        #endregion




        #region 比率换算成int,long

        /// <summary>
        /// 比例换算成long
        /// </summary>
        /// <param name="rate">比例原值</param>
        /// <param name="digits">小数位数</param>
        /// <returns></returns>
        public static long Rate2Long(decimal rate, int digits)
        {
            decimal ddd = Math.Round(rate, digits);

            long result = Convert.ToInt64(rate * Pow4Int(10, digits));

            return result;
        }

        public static int Pow4Int(int input, int digits)
        {
            int result = input;
            if (digits == 0)
            {
                digits = 1;
            }
            for (int i = 0; i < digits - 1; i++)
            {
                result *= input;
            }
            return result;
        }
        public static long Pow4long(long input, int digits)
        {
            long result = input;
            for (int i = 0; i < digits; i++)
            {
                result *= input;
            }
            return result;
        }


        #endregion

    }
}
