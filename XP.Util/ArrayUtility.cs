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
        /// <summary>将字符串解析成List的方法</summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> String2List(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return null;
            }
            string[] IdArray = str.Split(new char[] { ',', '|' });
            List<string> IdList = new List<string>();
            foreach (string idStr in IdArray)
            {
                IdList.Add(idStr);
            }
            return IdList;
        }

        /// <summary>
        /// 对数组/list随机排列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputList"></param>
        /// <returns></returns>
        public static List<T> GetRandomList<T>(List<T> inputList)
        {
            //Copy to a array
            T[] copyArray = new T[inputList.Count];
            inputList.CopyTo(copyArray);

            //Add range
            List<T> copyList = new List<T>();
            copyList.AddRange(copyArray);

            //Set outputList and random
            List<T> outputList = new List<T>();
            Random rd = new Random(DateTime.Now.Millisecond);

            while (copyList.Count > 0)
            {
                //Select an index and item
                int rdIndex = rd.Next(0, copyList.Count - 1);
                T remove = copyList[rdIndex];

                //remove it from copyList and add it to output
                copyList.Remove(remove);
                outputList.Add(remove);
            }
            return outputList;
        }
    }
}
