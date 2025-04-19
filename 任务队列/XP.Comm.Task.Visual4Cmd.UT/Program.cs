using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Comm.Task.Visual4Cmd.UT
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Test00();


            //Test02();

        }

        private static void Test02()
        {
            Console.WriteLine("=====================  准备输入一行内容： =================");


            string str = Console.ReadLine();

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = false;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = false;//不显示程序窗口
            p.Start();//启动程序

            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine(str);

            p.StandardInput.AutoFlush = true;
            //p.StandardInput.WriteLine("exit");
            //向标准输入写入要执行的命令。这里使用&是批处理命令的符号，
            //表示前面一个命令不管是否执行成功都执行后面(exit)命令，
            //如果不执行exit命令，后面调用ReadToEnd()方法会假死
            //同类的符号还有&&和||前者表示必须前一个命令执行成功才会执行后面的命令，
            //后者表示必须前一个命令执行失败才会执行后面的命令


            //获取cmd窗口的输出信息
            string output = p.StandardOutput.ReadToEnd();

            //StreamReader reader = p.StandardOutput;
            //string line=reader.ReadLine();
            //while (!reader.EndOfStream)
            //{
            //    str += line + "  ";
            //    line = reader.ReadLine();
            //}

            //p.WaitForExit();//等待程序执行完退出进程
            //p.Close();


            Console.WriteLine("=====================  子窗口获得的输入 =================");


            //Console.WriteLine(output);
            Console.ReadKey();
        }

        private static void Test00()
        {
            Console.WriteLine("任务的dos窗口输出 测试");


            var t = System.Threading.Tasks.Task.Run(() =>
            {
                var dos1 = new TaskShowDos();
                dos1.Push("简单测试");

            });







            Console.ReadKey();
        }
    }
}
