using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Console
{
    /// <summary>
    /// 控制台功能封装静态类
    /// </summary>
    public static class c
    {
        public static void Say(string str)
        {
            System.Console.WriteLine(str);
        }


        public static void Write(string str)
        {
            System.Console.Write(str);
        }

        public static string ReadLine()
        {

            return System.Console.ReadLine();

        }
        public static void Clear()
        {

            System.Console.Clear();

        }

    }
}
