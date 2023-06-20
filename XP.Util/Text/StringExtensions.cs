using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    /// <summary>
    /// 字符串扩展类
    /// </summary>
    /// <remark>
    /// </remark>
    ///
    public static class StringExtensions
    {

        public static string Cut(this String txt,  int length, string suffix)
        {
            if (0 >= length) return String.Empty;
            //早先对C#不太熟，所以不放心int会不会被赋0值，所以有下面的代码
            //if (length == 0)
            //{
            //    length = 10;
            //}
            if (txt.Length > length)
            {
                txt = txt.Substring(0, length) + suffix;
            }
            return txt;
        }

        /// <summary>
        /// 全部替换（代替String.Replace()，可以多次替换）
        /// </summary>
        /// <remarks>
        /// 如果是想替换的内容里面包含英文句点（.）那么就要在oldString里面把句点加上斜杠。
        /// ReplaceAll("\\.", "\\.");
        /// </remarks>
        /// <param name="txt">字符串本身</param>
        /// <param name="oldString">准备替换/拿掉的字符串（实际上是正则表达式）</param>
        /// <param name="newString">将会换上的字符串</param>
        /// <returns></returns>
        public static string ReplaceAll(this String txt, string oldString,string newString)
        {
            if (0 >= txt.Length || 0 == oldString.Length) return String.Empty;
            if (null == newString)
            {
                newString = String.Empty;
            }
            //早先对C#不太熟，所以不放心int会不会被赋0值，所以有下面的代码
            //if (length == 0)
            //{
            //    length = 10;
            //}
            if("\\" == oldString)
            {
                oldString = "\\\\";
            }
            string NewStr = Regex.Replace(txt, oldString, newString);
            return NewStr;
        }




    }
}


namespace XP.Util.Text
{

    public static class StringCuting{
        public static string Cut(string txt, int length, string suffix)
        {
            if (0 >= length) return String.Empty;
            //早先对C#不太熟，所以不放心int会不会被赋0值，所以有下面的代码
            //if (length == 0)
            //{
            //    length = 10;
            //}
            if (txt.Length > length)
            {
                txt = txt.Substring(0, length) + suffix;
            }
            return txt;

        }
    }
}