using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Enums
{
    /// <summary>
    /// 任务状态枚举
    /// </summary>
    /// <remarks>
    /// Finished表示任务已经结束，跟任务成功和失败无关，所以不要把Finished加入任务状态类型里面。
    /// </remarks>
    public enum TaskStatusDef
    {
        /// <summary>
        /// 未知的
        /// </summary>
        Unknow = -1,
        /// <summary>
        /// 默认
        /// </summary>
        Defult = 0,
        /// <summary>
        /// 已经停止
        /// </summary>
        Stop = -3000,

        /// <summary>
        /// 开始
        /// </summary>
        Start = 1000,


        /// <summary>
        /// 成功
        /// </summary>
        Success = 2000,

        /// <summary>
        /// 暂停
        /// </summary>
        Pause = 3000,


        /// <summary>
        /// 失败
        /// </summary>
        Fail = -5000,

        /// <summary>
        /// 错误
        /// </summary>
        Error = -1000,
        /// <summary>
        /// 超时
        /// </summary>
        Timeout = -2000,


        /// <summary>
        /// 未定义
        /// </summary>
        UnDefined = -9999,

    }
}
