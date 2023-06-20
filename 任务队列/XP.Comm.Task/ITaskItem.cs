using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace XP.Comm.Task
{
    /// <summary>
    /// 通用任务类的接口定义
    /// </summary>
    public interface ITaskItem
    {
        /// <summary>任务创建的时间</summary>
        DateTime? CreateTime { get; set; }

        /// <summary>任务开始运行的时间</summary>
        DateTime? StartTime { get; set; }

        /// <summary>任务进入等待的时间</summary>
        DateTime? WaitTime { get; set; }

        /// <summary>任务任务完成的时间</summary>
        DateTime? FinishTime { get; set; }

        /// <summary>任务索引数</summary>
        long TaskIdx { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        string TaskName { get; set; }
        /// <summary>
        /// 任务号
        /// </summary>
        Guid TaskNo { get; set; }

        /// <summary>任务状态</summary>
        Enums.TaskStatusDef Status { get; set; }



        /// <summary>任务已经开始</summary>
        bool IsStart { get; set; }

        /// <summary>任务已经结束，不论成功还是失败</summary>
        bool IsFinished { get; set; }

        /// <summary>
        /// 任务成功标志
        /// </summary>
        bool IsSuccess { get; set; }

        CommMsg Msg { get; set; }

        void Start();


        void Stop();


        void Pause();

        /// <summary>
        /// 任务超时
        /// </summary>
        void Timeout();

        /// <summary>
        /// 任务终止（意外退出）
        /// </summary>
        void Abort();

        /// <summary>
        /// 控制线程阻塞和恢复
        /// </summary>
        ManualResetEvent ManualResetEvent { get; set; }

        System.Threading.Tasks.Task StartAsync();
    }
}
