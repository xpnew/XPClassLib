using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Server.IISMonitor
{

    /// <summary>
    /// 状态基类
    /// </summary>
    public class BaseStatus
    {
        /// <summary>
        /// 启动时间
        /// </summary>
        public DateTime? StartTime { get; set; }


        /// <summary>
        /// 停止时间
        /// </summary>
        public DateTime? StopTime { get; set; }

    }
}
