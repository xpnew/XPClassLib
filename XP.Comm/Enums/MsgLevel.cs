using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Comm.Enums
{
    /// <summary>
    /// MsgLevel
    /// </summary>
    /// <remarks>
    /// 创建日期：2025/7/21 13:53:18
    /// 类名：MsgLevel
    /// 创建人： xpnew@126.com
    /// </remarks>
    [Flags]
    public enum MsgLevel
    {
        None = 0,

        Debug = 0x1,


        Sql = 0x10,
        Info = 0x100,


        Warn = 0x10000,


        Error = 0x100000,

        Execption = 0x1000000,


        All = 0x1111111
    }
}
