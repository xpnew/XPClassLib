using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace XP.Util
{
    /// <summary>
    /// 通用工具
    /// </summary>
    /// <remarks>
    /// common已经被用做类库名了 所以，这里用 General这个词
    /// </remarks>
    public class GeneralTool
    {

        #region 加密


        public static string SHA256(string input)
        {
            SHA256 mySHA256 = System.Security.Cryptography.SHA256.Create();
            byte[] data = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2").ToLower());
            }
            return sBuilder.ToString();
        }
        public static string EncodeBase64(string code_type, string input)
        {
            string encode = "";
            byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(input);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = input;
            }
            return encode;
        }

        public static string Sha1Base64(string input)
        {
            string encode = "";
            SHA1 md5Crypt = System.Security.Cryptography.SHA1.Create();
            byte[] bytes = md5Crypt.ComputeHash(Encoding.UTF8.GetBytes(input));
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = input;
            }
            return encode;
        }

        public static string SHA1(string input)
        {
            SHA1 md5Crypt = System.Security.Cryptography.SHA1.Create();
            byte[] data = md5Crypt.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2").ToLower());
            }
            return sBuilder.ToString();
        }

        /// <summary>
        /// 基础的MD5加密，默认输出的是小写字母。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MD5(string input)
        {
            MD5 md5Crypt = System.Security.Cryptography.MD5.Create();
            byte[] data = md5Crypt.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2").ToLower());
            }
            return sBuilder.ToString();
        }

        public static string SHAUserPwd(string input, DateTime? dt)
        {
            if (null == dt)
            {
                return SHA256(input);
            }

            string suffix = dt.Value.ToString("YYYYMMddHHmmss");
            string MiddleString = SHA256(input + suffix);
            return MADSHA256(MiddleString);
        }


        public static string MADSHA256(string input)
        {

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i += 2)
            {
                sb.Append(input[i]);
            }

            int Part2Start;
            if (0 == input.Length % 2)
            {
                Part2Start = input.Length - 1;
            }
            else
            {
                Part2Start = input.Length - 2;

            }
            for (int i = Part2Start; i > 0; i -= 2)
            {
                sb.Append(input[i]);
            }
            return sb.ToString();
        }

        #endregion



        #region 随机数和随机字符串

        private static Random rnd = new Random();

        private static readonly object rng_locker = new object();
        /// <summary>
        /// 从一个整数区间获取一个随机数字，max是不包含在内的
        /// </summary>
        /// <param name="min">目标整数的最小值</param>
        /// <param name="max">目标整数的最大值（不包括这个数）</param>
        /// <returns></returns>
        public static int Random(int min, int max)
        {
            lock (rng_locker)
                return rnd.Next(min, max);
        }
        /// <summary>
        /// 获取一组不重复的随机整数
        /// </summary>
        /// <param name="min">目标整数的最小值</param>
        /// <param name="max">目标整数的最大值（不包括这个数）</param>
        /// <param name="resultCount">结果的数量</param>
        /// <returns></returns>
        public static List<int> GetNoRepeatInt(int min, int max, int resultCount)
        {
            List<int> Result = new List<int>();
            if (max - min < resultCount)
            {
                string ErrMsg = "出现了错误：GetNoRepeatInt（获取一组不重复的随机整数）调用错误，可用的范围小于要求的结果";
                x.Say(ErrMsg);
                XP.Loger.Error(ErrMsg);
                return Result;
            }
            for (int i = 0; i < resultCount; i++)
            {
                int NewVal = rnd.Next(min, max);
                if (0 != Result.Count)
                {
                    while (true)
                    {
                        if (!Result.Contains(NewVal))
                        {
                            Result.Add(NewVal);
                            break;
                        }
                        else
                        {
                            NewVal = rnd.Next(min, max);
                        }
                    }
                }
                else
                {
                    Result.Add(NewVal);
                }
            }
            return Result;
        }

        public static string GetRndChars(int len, bool enableLowerLetter = false)
        {
            int TotalCharNum = 36;
            int[,] CharRoomArr = new int[2, 2]
            {
                {48,57 }, {65,90 }
            };
            if (enableLowerLetter)
            {
                CharRoomArr = new int[3, 2]
                {
                    {48,57 }, {65,90 }, { 97,122}
                };
                TotalCharNum += 26;
            }
            char[] Chars = NewMethod(TotalCharNum);
            int CharArrIndex = 0;
            //x.Say("CharRoomArr 索引：" + CharRoomArr.Length);
            for (int i = 0; i < CharRoomArr.GetLength(0); i++)
            {
                for (int k = CharRoomArr[i, 0]; k <= CharRoomArr[i, 1]; k++)
                {
                    Chars[CharArrIndex] = (char)k;
                    CharArrIndex++;
                }
            }


            string Result = null;
            StringBuilder sb = new StringBuilder();
            for (int l = 0; l < len; l++)
            {
                sb.Append(Chars[rnd.Next(TotalCharNum)]);
            }


            Result = sb.ToString();

            return Result;
        }

        private static char[] NewMethod(int TotalCharNum)
        {
            return new char[TotalCharNum];
        }
        #endregion


        #region 时间和时间戳


        #region unix时间戳转换成日期（北京时间）
        /// <summary>
        /// unix时间戳转换成日期（北京时间）
        /// </summary>
        /// <param name="unixTimeStamp">时间戳（秒）</param>
        /// <returns></returns>
        public static DateTime UnixTimestampToDateTime(long unixTimeStamp)
        {
            DateTime startTime = new System.DateTime(1970, 1, 1);
            DateTime dt = startTime.AddSeconds(unixTimeStamp);
            return dt;
        }
        #endregion

        #region 当前时间换算成unix时间戳（北京时间）
        /// <summary>
        /// 当前时间换算成unix时间戳（北京时间）
        /// </summary>
        /// <param name="dt">指定的时间</param>
        /// <returns>计算出来的差，单位是秒</returns>
        public static long DateTime2UnixTimestamp(DateTime? dt = null)
        {
            //long Result;
            if (null == dt)
            {
                dt = DateTime.Now;
            }
            DateTime StartTime = new System.DateTime(1970, 1, 1);

            //DateTime dt = startTime.AddSeconds(unixTimeStamp);

            var ts = dt.Value.Ticks - StartTime.Ticks;
            if (0 == ts) { return ts; }
            //TimeSpan elapsedSpan = new TimeSpan(ts);
            //DateTime.Ticks 单位是 100 毫微秒  1 毫微秒 = 10^-9 秒，
            ts = ts / 10000000;
            return ts;
        }
        #endregion


        #region unix时间戳（UTC）转换为本地时间
        /// <summary>
        /// unix时间戳（UTC）转换为本地时间
        /// </summary>
        /// <param name="unixTimeStamp">时间戳（秒）</param>
        /// <returns></returns>
        public static DateTime UtcUnixTSToDateTime(long unixTimeStamp)
        {
            DateTime startTime = new System.DateTime(1970, 1, 1);
            DateTime dt = startTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dt;
        }
        #endregion

        #region 当前时间换算成unix时间戳（UTC）
        /// <summary>
        /// 当前时间换算成unix时间戳（UTC）
        /// </summary>
        /// <param name="dt">指定的时间</param>
        /// <returns>计算出来的差，单位是秒</returns>
        public static long GetUtcUnixTimestamp(DateTime? dt = null)
        {
            //long Result;
            if (null == dt)
            {
                dt = DateTime.UtcNow;
            }
            else
            {
                dt = dt.Value.ToUniversalTime();
            }
            DateTime StartTime = new System.DateTime(1970, 1, 1);

            //DateTime dt = startTime.AddSeconds(unixTimeStamp);

            var ts = dt.Value.Ticks - StartTime.Ticks;
            if (0 == ts) { return ts; }
            //TimeSpan elapsedSpan = new TimeSpan(ts);
            //DateTime.Ticks 单位是 100 毫微秒  1 毫微秒 = 10^-9 秒，
            ts = ts / 10000000;
            return ts;
        }
        #endregion

        /// <summary>
        /// 毫秒（3位数）级时间戳
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long GetTMS(DateTime? dt = null)
        {
            //long Result;
            if (null == dt)
            {
                dt = DateTime.Now;
            }
            DateTime StartTime = new System.DateTime(1970, 1, 1);

            //DateTime dt = startTime.AddSeconds(unixTimeStamp);

            var ts = dt.Value - StartTime;

            return (long)ts.TotalMilliseconds;
        }


        #endregion


        #region 日期处理
        public static DateTime? Num2Date(int input)
        {
            int Year = input / 10000;
            if (0 == Year)
            {
                return null;
            }
            int Month = (input - Year * 10000) / 100;
            if (0 == Month)
            {
                return null;
            }

            int Day = input % 100;

            if (0 == Month)
            {
                return null;
            }
            return new DateTime(Year, Month, Day);
        }

        public static string DatePart2String(int input)
        {
            int year = input / 10000;

            int month = (input - year * 10000) / 100;

            int day = input % 100;
            return $"{year}-{month}-{day}";
        }
        public static string DateHourPart2String(int input)
        {
            int year = input / 1000000;

            int month = (input - year * 1000000) / 10000;

            int day = (input - year * 1000000 - month * 10000) / 100;
            int hour = input % 100;
            return $"{year}-{month}-{day} {hour}点";
        }


        public static int Date2Num(DateTime? input = null)
        {
            DateTime dt;

            if (input.HasValue)
                dt = input.Value;
            else
                dt = DateTime.Now;

            //统计日期，当天的前一天，可以通过传参指定其它日期
            int DateNum = dt.Year * 10000 + dt.Month * 100 + dt.Day;

            return DateNum;
        }

        public static int Date2DateHour(DateTime? input = null)
        {
            DateTime dt;

            if (input.HasValue)
                dt = input.Value;
            else
                dt = DateTime.Now;

            //统计日期，当天的前一天，可以通过传参指定其它日期
            int DateNum = dt.Year * 1000000 + dt.Month * 10000 + dt.Day * 100 + dt.Hour;

            return DateNum;
        }

        //public static (int Year, int Month, int Day) DateNumBreak(int input)
        //{
        //    (int Year, int Month, int Day) Result = (0, 0, 0);


        //}

        public static int DiffDay(DateTime dt1, DateTime dt2)
        {
            int day = Convert.ToInt32((dt1.Date - dt2.Date).TotalDays);

            return day;
        }

        public static bool IsSameDay(DateTime dt1, DateTime dt2)
        {
            int day = Convert.ToInt32((dt1.Date - dt2.Date).TotalDays);

            return day == 0;
        }
        #endregion


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


        #region 枚举处理

        /// <summary>
        /// 获取枚举的名称
        /// </summary>
        /// <remarks>
        /// c# 竟然不支持泛型当中使用枚举
        ///
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetEnumName<T>(T t) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            return Enum.GetName(typeof(T), t);
        }

        public static T GetEnum<T>(string name) where T : struct, IConvertible
        {

            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            if (Enum.IsDefined(typeof(T), name))
            {
                var Result = (T)Enum.Parse(typeof(T), name);
                return Result;
            }
            return default(T);

        }

        #endregion
    }
}
