using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Comm.Dos
{
    /// <summary>
    /// 带有等待时间的cmd,超时会强制退出
    /// </summary>
    public class WaitingCmd : ConsoleProgramBase
    {
        /// <summary>
        /// 默认的等待时间
        /// </summary>
        private int _WaitSecondDefault = 60;
        /// <summary>
        /// 等待执行的秒数
        /// </summary>
        public int WaitSecond { get; set; }


        public WaitingCmd(string cmd, string[] args = null) : base(cmd, args)
        {

        }

        protected  override void _Init()
        {
            base._Init();
            WaitSecond = _WaitSecondDefault;
        }

        public override void Run()
        {
            //base.Run();

            CmdProcess.Start();
            //myProcess.StandardInput.WriteLine("shutdown -s -t " + time);
            CmdProcess.StandardInput.WriteLine(CmdName);
            //CmdProcess.StartInfo.FileName = CmdName;
            //CmdProcess.StartInfo.Arguments = ArgsLine;
            CmdProcess.StandardInput.WriteLine("exit");

            CmdProcess.WaitForExit(WaitSecond * 1000);

            if (!CmdProcess.HasExited)
                CmdProcess.Kill();
            OutputString = CmdProcess.StandardOutput.ReadToEnd();

            CmdProcess.Close();

        }
    }
}
