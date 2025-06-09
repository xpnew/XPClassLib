using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace XP.Comm.Dos
{
    public class NormalCmd
    {

        public static bool RebootWindow(int wait =0)
        {
            try
            {
                System.Diagnostics.Process myProcess = new System.Diagnostics.Process();

                myProcess.StandardInput.WriteLine("shutdown -s -t " + wait);


                myProcess.Start();


                Console.WriteLine("进程启动中...");

                myProcess.WaitForExit(); // 等待进程退出

                Console.WriteLine("进程已结束");

                //CmdProcess.StartInfo.FileName = CmdName;
                //CmdProcess.StartInfo.Arguments = ArgsLine;


                //需要获取返回信息的，请使用带有等待时间的派生类
                //OutputString = CmdProcess.StandardOutput.ReadToEnd();

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }

        }
    }
}
