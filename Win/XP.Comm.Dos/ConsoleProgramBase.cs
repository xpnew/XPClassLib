using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace XP.Comm.Dos
{


    /// <summary>
    /// C#调用控制台程序的基类
    /// </summary>
    public class ConsoleProgramBase
    {

        /// <summary>
        /// 命令的名称，核心参数
        /// </summary>
        public string CmdName { get; set; }

        /// <summary>
        /// 命令行的参数列表
        /// </summary>
        public List<string> CmdArgsList { get; set; }

        /// <summary>
        /// 组合完成的参数行
        /// </summary>
        public string ArgsLine { get; set; }

        /// <summary>
        /// 输出的文本
        /// </summary>
        public  string OutputString { get; set; }

        /// <summary>
        /// 错误的文本
        /// </summary>
        public string ErrorString { get; set; }


        protected System.Diagnostics.Process CmdProcess { get; set; }


        public ConsoleProgramBase(string cmd, string[] args = null)
        {
            CmdName = cmd;
            _Init();
        }

        protected virtual void _Init()
        {
            OutputString = "需要获取返回信息的，请使用带有等待时间的派生类";
            _BuildProcess();

        }
        public bool IsShow { get; set; } = false;



        protected virtual  void _BuildProcess()
        {
            //https://docs.microsoft.com/zh-cn/dotnet/api/system.diagnostics.processstartinfo.createnowindow?redirectedfrom=MSDN&view=net-5.0
            //如果 UseShellExecute 属性为 true，或者 UserName 和 Password 属性不为 null，则将忽略 CreateNoWindow 属性值并创建一个新窗口。

            var startInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe");
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;//hide a window

            var proc = new System.Diagnostics.Process();
            proc.StartInfo = startInfo;

            CmdProcess = proc;

        }

        public virtual void Run()
        {

            CmdProcess.Start();
            //myProcess.StandardInput.WriteLine("shutdown -s -t " + time);
            CmdProcess.StandardInput.WriteLine(CmdName);
            //CmdProcess.StartInfo.FileName = CmdName;
            //CmdProcess.StartInfo.Arguments = ArgsLine;
            CmdProcess.StandardInput.WriteLine("exit");

            //需要获取返回信息的，请使用带有等待时间的派生类
            //OutputString = CmdProcess.StandardOutput.ReadToEnd();

            CmdProcess.Close();
        }


    }
}
