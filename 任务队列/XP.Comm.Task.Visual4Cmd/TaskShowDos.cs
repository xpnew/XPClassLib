using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Comm.Task.Visual4Cmd
{

    /// <summary>
    /// 任务显示DOS窗口
    /// </summary>
    public class TaskShowDos
    {

        public System.Diagnostics.Process Dos { get; set; }

        public TaskShowDos()
        {


            _Init();

        }
        protected void _Init()
        {
            Dos = new System.Diagnostics.Process();

            Dos.StartInfo.FileName = "cmd.exe";//要执行的程序名称
            Dos.StartInfo.UseShellExecute = true;
            Dos.StartInfo.RedirectStandardInput = false;//可能接受来自调用程序的输入信息
            Dos.StartInfo.RedirectStandardOutput = false;//由调用程序获取输出信息
            Dos.StartInfo.CreateNoWindow = false;//不显示程序窗口
            Dos.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            Dos.Start();//启动程序

            Dos.StartInfo.RedirectStandardInput = true;
            //Dos.StandardInput.WriteLine("cls");

            //Dos.StandardInput.AutoFlush = true;
            //string output = Dos.StandardOutput.ReadToEnd();
            Dos.WaitForExit();

            x.Say("命令窗口结束");

        }


        public void Push(string str)
        {
            Dos.StandardInput.WriteLine("echo " + str);
        }

    }
}
