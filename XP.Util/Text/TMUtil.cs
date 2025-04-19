using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Util.Text
{
    public static class TMUtil
    {



        /// <summary>
        /// 替换模板项，支持一定的大小写转换 $FirstLower $FirstUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>

        public static string ReplaceTmItem(string input, string name, string value)
        {
            string Result = input;
            string FirstCharLower, FirstCharUpper;
            if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(input))
            {
                return Result;
            }
            if (String.IsNullOrEmpty(value))
            {
                value = String.Empty;
                FirstCharLower = value;
                FirstCharUpper = value;
            }
            else
            {
                string FirstCharString = value[0].ToString();
                char FirstChar = value[0];

                char[] Value2Chars = value.ToCharArray();
                Value2Chars[0] = char.ToLower(FirstChar);
                FirstCharLower = new string(Value2Chars);
                Value2Chars[0] = char.ToUpper(FirstChar);
                FirstCharUpper = new string(Value2Chars);

            }

            Result = Result.Replace("{TM:" + name + "$FirstLower}", FirstCharLower);
            Result = Result.Replace("{TM:" + name + "$FirstUpper}", FirstCharUpper);
            Result = Result.Replace("{TM:" + name + "}", value);
            return Result;

        }

    }
}
