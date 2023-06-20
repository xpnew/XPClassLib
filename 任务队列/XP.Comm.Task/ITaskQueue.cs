using System;
using System.Collections.Generic;
using System.Text;

namespace XP.Comm.Task
{

    /// <summary>
    /// 任务队列接口
    /// </summary>
    public interface ITaskQueue
    {

        

        /// <summary>
        /// 任务列表
        /// </summary>
        List<ITaskItem> TaskList { get; set; }
        /// <summary>
        /// 已经开始 
        /// </summary>
        bool HasStart { get; set; }
        /// <summary>
        /// 正在工作
        /// </summary>
        bool HasWorking { get; set; }

        /// <summary>
        /// 任务可以进行，初始化成功、初始条件具备 
        /// </summary>
        bool IsCanDo { get; set; }
        /// <summary>
        /// 成功完成（避免Finish 和complate混淆）
        /// </summary>
        bool IsSuccess { get; set; }
        /// <summary>
        /// 任务结束，不管成功与否
        /// </summary>
        bool IsComplate { get; set; }


        /// <summary>
        /// 下一个
        /// </summary>
        void Next();

        /// <summary>
        /// 暂停
        /// </summary>
        void Pause();   

        /// <summary>
        /// 中止
        /// </summary>
        void Cancel();

        /// <summary>
        /// 清除
        /// </summary>
        void Clear();
        /// <summary>
        /// 开始运行
        /// </summary>
        void Play();

        /// <summary>
        /// 添加一个任务
        /// </summary>
        /// <param name="item"></param>
        void Add(ITaskItem item);
    }
}
