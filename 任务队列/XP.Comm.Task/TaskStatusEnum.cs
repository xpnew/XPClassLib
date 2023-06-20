using System;
using System.Collections.Generic;
using System.Text;

namespace XP.Comm.Task
{
    public enum TaskStatusEnum
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
