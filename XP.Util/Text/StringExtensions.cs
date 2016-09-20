using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// 字符串扩展类
    /// </summary>
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