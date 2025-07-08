using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Util;

namespace XP.Win.Components.Monitors
{
    /// <summary>
    /// 带等级的日志消息  LevelLogMsg
    /// </summary>
    /// <remarks>
    /// 创建日期：2025/7/2 17:26:52
    /// 类名：LevelLogMsg
    /// 创建人： xpnew@126.com
    /// </remarks>
    public class LevelLogMsg : XP.Comm.CommMsg
    {
        public DebugLevel Level { get; set; } = DebugLevel.Info;
        #region <属性>
        #endregion <属性>

        #region <构造方法>
        #endregion <构造方法>

        #region <内部方法>
        #endregion <内部方法 end>
        #region <外部方法>
        #endregion <外部方法 end>

        #region <事件>
        #endregion <事件>    
    }
}
