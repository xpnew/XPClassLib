using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Comm.Dos
{
    /// <summary>
    /// 简单的Cmd,实际上是对基类的封装
    /// </summary>
    public class SimplyCmd : ConsoleProgramBase
    {

        public SimplyCmd(string cmd, string[] args = null) : base(cmd, args)
        {

        }
    }
}
