using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Util
{
    
    [Flags]
    public enum DebugLevel
    {
        None = 0,

        Debug = 0x1,


        Sql = 0x10,
        Info = 0x100,


        Warn = 0x10000,


        Error = 0x100000,

        Execption  = 0x1000000,


        All = 0x1111111



    }
}
