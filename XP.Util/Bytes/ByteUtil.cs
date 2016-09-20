using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Util
{

    /// <summary>
    /// 字节工具：字节、N进制、字符串
    /// </summary>
    public class ByteUtil
    {

        /// <summary>
        /// 单字节的16进制字符串，转换为字节byte[]（通常用于流）
        /// </summary>
        /// <param name="strArr"></param>
        /// <returns></returns>
        public static byte[] StringArr2Buffer(string[] strArr)
        {
            byte[] Arr1 = new byte[strArr.Length];

            for (int i = 0; i < strArr.Length; i++)
            {
                Arr1[i] = String2Byte(strArr[i]);

                x.Say(i + " : 十六进制显示 " + strArr[i] + "  byte显示 " + Arr1[i] + "  二进制显示：" + OneZeroString(Arr1[i]));
            }
            return Arr1;

        }

        /// <summary>
        /// 将字节缓冲byte[]（通常用于流）内的数据，以16进制字符串方式呈现
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static string Buffer2String(byte[] buffer, int max)
        {
            string Result = "";
            for (int i = 0; i < buffer.Length && i < max; i++)
            {
                Result += Byte2String(buffer[i]);

            }
            return Result;
        }
        /// <summary>
        /// 将字节缓冲byte[]（通常用于流）内的数据，以16进制字符串方式呈现
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string Buffer2String(byte[] buffer)
        {
            string Result = "";
            for (int i = 0; i < buffer.Length ; i++)
            {
                Result += Byte2String(buffer[i]);

            }
            return Result;
        }


        /// <summary>
        /// String转换成byte[]
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string input)
        {
            if (null == input)
                return new byte[1];

            return Encoding.UTF8.GetBytes(input);
        }

        /// <summary>
        /// 字节数据按照2进制方式显示
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string OneZeroString(byte input)
        {
            return Convert.ToString(input, 2);

        }

        /// <summary>
        /// 字节数据按照16进制方式显示，字母大写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Byte2String(byte input)
        {
            return input.ToString("X2");
        }

        public static byte String2Byte(string input)
        {
            //把字符串认为是16进制数据，转换成byte
            return byte.Parse(input, System.Globalization.NumberStyles.HexNumber);

        }


        /// <summary>
        /// 端口号(整数)转换成16进制方式显示
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static string Port2Hix(int port)
        {
            string Result = port.ToString("X4");
            return Result;
        }


        /// <summary>
        /// 字母流，变成相应的整数（按照双字节方式转换，假定高位在前）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<int> GetInt(byte[] input)
        {

            List<int> Result = new List<int>();


            for (int i = 0; i < input.Length; i++)
            {



            }


                return Result;

        }


        public static int TowByte2Int(byte low, byte heigh)
        {
            // FF FF =  65,536

            int Int256 =low * 1;
            // 高位  00 FF = 65,280
            int Int65280 = heigh * 256;

            return Int256 + Int65280; 
        }

       





    }
}
