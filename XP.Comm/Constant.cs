using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm
{
    /// <summary>
    /// 公共的常量定义
    /// </summary>
    public static class Constant
    {
        #region int 

        /// <summary>
        /// 空的Int，相当于是Nullable<int>的=null,(int的最小值+2)
        /// </summary>
        public const int NullInt = -2147483647;




        /// <summary>
        /// 错误int的型（期待一个int结果，但是格式不对）
        /// </summary>
        public const int ErrorInt = -2147483646;


        /// <summary>
        /// 准备查找一个int值，结果不存在(int的最小值)
        /// </summary>
        public const int NotExistInt = -2147483648;

        /// <summary>
        /// 检查int结果的合规性
        /// </summary>
        /// <remarks>
        /// 查找某个int结果的时候，可能是这个数据不存在，也可以能是这个数据存在但是格式错，不是int型的，有一些时候需要仔细分辨两种不同的类型，有时候不需要。
        /// 不需要的时候就用这个方法一起处理了。
        /// 
        /// </remarks>
        /// <param name="input"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static bool CheckReadyInt(int input, int def = NotExistInt)
        {
            if (input == def || input == ErrorInt || input == NotExistInt)
            {
                return false;
            }
            return true;
        }
        #endregion


        #region long 


        /// <summary>
        /// 空的Int，相当于是Nullable<long>的=null,(long的最小值+2)
        /// </summary>
        public const long NullLong = -9223372036854775806;


        // <summary>
        /// 错误long的型（期待一个long结果，但是格式不对）
        /// </summary>
        public const long ErrorLong = -9223372036854775807;


        /// <summary>
        /// 准备查找一个long值，结果不存在(long的最小值)
        /// </summary>
        public const long NotExistLong = -9223372036854775808;


        /// <summary>
        /// 准备查找一个Int64(long)值，结果不存在(long的最小值)
        /// </summary>
        public const long NotExistInt64 = -9223372036854775808;

        /// <summary>
        /// 检查long结果的合规性
        /// </summary>
        /// <remarks>
        /// 查找某个long结果的时候，可能是这个数据不存在，也可以能是这个数据存在但是格式错，不是int型的，有一些时候需要仔细分辨两种不同的类型，有时候不需要。
        /// 不需要的时候就用这个方法一起处理了。
        /// 
        /// </remarks>
        /// <param name="input"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static bool CheckReadyLong(long input, long def = NotExistLong)
        {
            if (input == def || input == ErrorLong || input == NotExistLong)
            {
                return false;
            }
            return true;
        }

        #endregion








        #region decimal常量,和整数约定是一样的数字
        /// <summary>
        /// 空的Int，相当于是Nullable<decimal>的=null,(long的最小值+2)
        /// </summary>
        public const decimal NullDecimal = -2147483647m;

        public const decimal ErrorDecimal = -2147483646m;


        public const decimal NotExistDecimal = -2147483648m;


        /// <summary>
        /// 检查decimal结果的合规性
        /// </summary>
        /// <remarks>
        /// 查找某个long结果的时候，可能是这个数据不存在，也可以能是这个数据存在但是格式错，不是int型的，有一些时候需要仔细分辨两种不同的类型，有时候不需要。
        /// 不需要的时候就用这个方法一起处理了。
        /// 
        /// </remarks>
        /// <param name="input"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static bool CheckReadyDecimal(decimal input, decimal def = NotExistDecimal)
        {
            if (input == def || input == ErrorDecimal || input == NotExistDecimal)
            {
                return false;
            }
            return true;
        }

        #endregion




    }
}
