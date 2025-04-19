using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Util.Text
{
    public static class TextUtil
    {
        /// <summary>
        /// 字符串转换为首字母大写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Capitalize(string input)
        {
            if (null == input) return null;
            string worktext = input.Trim();
            if (0 == worktext.Length) return input;

            if (1 == worktext.Length) return worktext.ToUpper();
            worktext = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(worktext);
            return worktext;
        }
        /// <summary>
        /// 字符串转换为首字母小写
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string DisCapitalize(string text)
        {
            if (null == text) return null;
            string worktext = text.Trim();
            if (0 == worktext.Length) return text;

            if (1 == worktext.Length) return worktext.ToLower();

            string all = Char.ToLower(worktext[0]) + worktext.Substring(1);

            return all;

        }

        /// <summary>
        /// 字符串转换，首字大写，其余小写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string OnlyFirstUpper(string input)
        {
            if (null == input) return null;
            string worktext = input.Trim();
            if (0 == worktext.Length) return input;

            if (1 == worktext.Length) return worktext.ToUpper();

            var arr = input.ToCharArray();
            var outarr = new char[arr.Length];

            for (int i = 0; i < arr.Length; i++)
            {
                if (0 == i)
                {
                    outarr[i] = Char.ToUpper(arr[i]);
                }
                else
                {
                    outarr[i] = Char.ToLower(arr[i]);

                }


            }
            return string.Join("", outarr);
        }


        /// <summary>
        /// 创建达梦式大写名称
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string BuildDMUpperName(string input)
        {
            var arr  =input.ToCharArray();
            var outarr = new char[arr.Length];
            var worklst = new List<char>(); 
            
            for (int i = 0; i < arr.Length; i++)
            {
                var ch = arr[i];
                if (IsUpper(ch))
                {
                    worklst.Add('_');
                    worklst.Add(Char.ToUpper(ch));
                }
                else
                {
                    worklst.Add(Char.ToUpper(ch));
                }

            }
            if('_' == worklst[0])
            {
                return string.Join("", worklst.Skip(1).ToArray());
            }
            else
            {
                return string.Join("",  worklst.ToArray());
            }

        }

        /// <summary>
        /// 使用C#判断一个字符串是否包含大写字符的五种方法
        /// https://www.jb51.net/program/310161sox.htm
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static bool IsUpper(char ch)
        {

            return char.IsUpper(ch);

        }
    }
}
