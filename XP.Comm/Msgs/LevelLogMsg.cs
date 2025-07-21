using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Comm.Enums;

namespace XP.Comm.Msgs
{
    /// <summary>
    /// LevelLogMsg
    /// </summary>
    /// <remarks>
    /// 创建日期：2025/7/21 13:48:55
    /// 类名：LevelLogMsg
    /// 创建人： xpnew@126.com
    /// </remarks>
    public class LevelLogMsg: CommMsg
    {
        public MsgLevel Level { get; set; } = MsgLevel.Info;
      
    }
}
