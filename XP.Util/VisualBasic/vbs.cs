using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Util
{
    /// <summary>
    /// 封装asp时代一些vbs常用的vbs函数
    /// </summary>
    public static partial class vbs
    {

        #region 检查类型：是否为数字、整数
        public static bool IsNumric(object o)
        {
            string str = o.ToString();
            if (String.IsNullOrEmpty(str))
                return false;
            char[] c = str.Trim().ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (0 == i && ('-' == c[0] || '+' == c[0])) continue;
                if (!((c[i] >= '0' && c[i] <= '9') || c[i] == '.'))
                    return false;
            }
            return true;
        }
        public static bool IsInt(object o)
        {
            if (null == o)
                return false;
            
            string str = o.ToString();
            if(0 == str.Length)
            {
                return false;
            }
            char[] c = str.Trim().ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (0 == i && ('-' == c[0] || '+' == c[0])) continue;
                if (c[i] < '0' || c[i] > '9')
                    return false;
            }
            return true;
        }
        #endregion

        #region 文本处理




        public static string Left(string txt, int length)
        {
            if (0 > length) return String.Empty;
            //早先对C#不太熟，所以不放心int会不会被赋0值，所以有下面的代码
            //if (length == 0)
            //{
            //    length = 10;
            //}
            if (txt.Length > length)
            {
                txt = txt.Substring(0, length);
            }
            return txt;
        }

        public static string Left(string txt, int length, string suffix)
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

        public static string Left(object o, int length, string suffix)
        {
            if (null == o)
                return String.Empty;

            return Left(o.ToString(), length, suffix);
        }



        #endregion

    }
}
