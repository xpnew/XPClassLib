using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Util
{
    /// <summary>数组管理工具</summary>
    public class ArrayUtility
    {

        /// <summary>从字符串数组当中移除空字符串</summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string[] RemoveNullString(string[] arr)
        {
#if NET_2_0

            string[] resultArr = arr.Where(s => !String.IsNullOrEmpty(s)).ToArray();
            return resultArr;
#else

            List<string> list = new List<string>();
            foreach (string str in arr)
            {
                if (!String.IsNullOrEmpty(str))
                {
                    list.Add(str);
                }
            }
            return list.ToArray();

#endif

        }

        /// <summary>字符串解析成int list</summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<int> String2IdList(string str)
        {
            if (null == str)
            {
                return new List<int>();
            }
            string[] IdArray = str.Split(new char[] { ',', '|' });
            List<int> IdList = new List<int>();
            foreach (string idStr in IdArray)
            {
                IdList.Add(Convert.ToInt32(idStr));
            }

            return IdList;
        }
        /// <summary>将字符串解析成List的泛型方法</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<T> StringSplitList<T>(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return null;
            }
            string[] IdArray = str.Split(new char[] { ',', '|' });
            List<T> IdList = new List<T>();
            foreach (string idStr in IdArray)
            {
                if (!String.IsNullOrEmpty(idStr))
                    IdList.Add((T)Convert.ChangeType(idStr, typeof(T)));
            }

            return IdList;
        }
    }
}
