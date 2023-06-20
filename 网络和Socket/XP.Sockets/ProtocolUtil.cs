using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Sockets
{

    /// <summary>
    /// 协议工具
    /// </summary>
    public class ProtocolUtil
    {

        /// <summary>
        /// 从一个缓冲区复制数据到另一个缓冲区
        /// </summary>
        /// <param name="input">需要复制数据的缓冲区</param>
        /// <param name="output">保存结果的缓冲区</param>
        /// <returns></returns>
        public static int CopyBuffer(byte[] input, byte[] output)
        {
            //if (input.Length < output.Length)
            //{
            //    return false;
            //}
            int Counter = 0;
            for (int i = 0; i < input.Length && i < output.Length; i++)
            {
                output[i] = input[i];
                Counter++;
            }
            return Counter;

        }


        /// <summary>
        /// 端口转换成字节
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static string Port2Hix(int port)
        {
            string Result = port.ToString("X4");
            return Result;
        }


        /// <summary>
        /// 从字节当中获取整数，高位在前，可用于端口号、长度
        /// </summary>
        /// <remarks>
        /// 注意 16进制和10进制的换算。
        /// byte[] {30,22} 对应的是 7702
        /// 但是16进制的是3022，对应的是 12322
        /// </remarks>
        /// <param name="inputBytes"></param>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static int GetInt(byte[] inputBytes, int start, int size)
        {
            if (start >= 0 && start < inputBytes.Length && start + size - 1 < inputBytes.Length)
            {
                int Result = 0;
                int k = 0;
                for (int i = size; i > 0; i--)
                {
                    int m = inputBytes[start + i - 1];
                    int n = m * (int)Math.Pow(256f, k);

                    Result += n;
                    k++;
                }
                return Result;
            }

            return 0;
        }

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
        /// 十六进制的字符串FFEE形式转换为二进制
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] X2String2Bytes(string s)
        {
            int Len = 0;
            if (s.Length % 2 == 0)
            {
                Len = s.Length / 2;
            }
            else
            {
                Len = s.Length / 2 + 1;
            }

            byte[] Arr1 = new byte[Len];

            for (int i = 0; i < Len; i++)
            {
                string MM = s.Substring(i * 2, 2);
                Arr1[i] = String2Byte(MM);

                x.Say(i + " : 十六进制显示 " + MM + "  byte显示 " + Arr1[i] + "  二进制显示：" + OneZeroString(Arr1[i]));
            }
            return Arr1;
        }


        /// <summary>
        /// 二进制的01形式转换成字符串形式，结果就是01110110
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string OneZeroString(byte input)
        {
            return Convert.ToString(input, 2);
        }

        public static string Byte2String(byte input)
        {

            return input.ToString("X2");
        }

        public static byte String2Byte(string input)
        {
            //把字符串认为是16进制数据，转换成byte
            return byte.Parse(input, System.Globalization.NumberStyles.HexNumber);

        }


    }
}
