using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using XP.Comm.Enums;

namespace XP.Comm.Task
{
    /// <summary>
    /// 任务基类，实现了ITaskItem接口的主要内容
    /// </summary>
    public class BaseTaskItem : ITaskItem
    {


        /// <summary>任务创建的时间</summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>任务开始运行的时间</summary>
        public DateTime? StartTime { get; set; }

        /// <summary>任务进入等待的时间</summary>
        public DateTime? WaitTime { get; set; }


        /// <summary>任务索引数</summary>
        public long TaskIdx { get; set; }

        /// <summary>
        /// 任务号
        /// </summary>
        public Guid TaskNo { get; set; }


        public string TaskName { get; set; }

        /// <summary>任务任务完成的时间</summary>
        public DateTime? FinishTime { get; set; }

        /// <summary>任务已经开始</summary>
        public bool IsStart { get; set; }

        /// <summary>任务已经结束，不论成功还是失败</summary>
        public bool IsFinished { get; set; }

        /// <summary>
        /// 任务成功标志
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 是否存在错误
        /// </summary>
        /// <remarks>
        /// 运行的时候出现错误也可能会继续进行
        /// </remarks>
        protected bool _HasError { get; set; }
        /// <summary>
        /// 任务中断，后面的程序判断是不是中断状态，跳过执行
        /// </summary>
        public bool IsBroken { get; set; }

        public TaskStatusDef Status { get; set; }

        public CommMsg Msg { get; set; }

        public ManualResetEvent ManualResetEvent { get; set; }

        public BaseTaskItem()
        {
            _Init();
        }

        protected virtual void _Init()
        {
            Msg = new CommMsg();
            IsFinished = false;
            IsStart = false;
            IsSuccess = false;
            IsBroken = false;
            _HasError = false;
            Status = TaskStatusDef.Defult;
            CreateTime = DateTime.Now;
            ManualResetEvent = new ManualResetEvent(true);
            TaskNo = new Guid();
        }

        public virtual void Start()
        {
            StartTime = DateTime.Now;
            Status = TaskStatusDef.Start;
            IsStart = true;
            // throw new NotImplementedException();
            Msg.AddLog("任务开始");
            try
            {
                Work();
                if (IsBroken)
                {
                    Status = TaskStatusDef.Error;
                    Stop();
                    Finished();
                    return;
                }
                else
                {
                    Status = TaskStatusDef.Success;
                    IsSuccess = true;
                    Msg.AddLog("任务完成 ");
                }
            }
            catch (Exception ex)
            {
                _SayError(WorkStepName+"出现异常任务结束", ex);
                Status = TaskStatusDef.Fail;
                Stop();
            }
            finally
            {
                Finished();
            }

        }

        public virtual async System.Threading.Tasks.Task StartAsync()
        {
            StartTime = DateTime.Now;
            Status = TaskStatusDef.Start;
            IsStart = true;
            // throw new NotImplementedException();
            Msg.AddLog("任务开始");
            try
            {

                var k = WorkAsync();
                await k;

                if (IsBroken)
                {
                    Status = TaskStatusDef.Error;
                    Stop();
                    Finished();
                    return;
                }
                else
                {
                    Status = TaskStatusDef.Success;
                    IsSuccess = true;
                    Msg.AddLog("任务完成 ");
                }
            }
            catch (Exception ex)
            {
                _SayError(WorkStepName + "出现异常 任务结束", ex);
                Status = TaskStatusDef.Fail;
                Stop();
            }
            finally
            {
                Finished();
            }
        }



        public virtual async System.Threading.Tasks.Task WorkAsync()
        {
            if (TaskNo == Guid.Empty)
            {
                TaskNo = Guid.NewGuid(); 
            }
        }


        public string WorkStepName { get; set; }

        protected void _SayError(string str, Exception ex = null)
        {
            if (null != ex)
            {
                Loger.Error(str, ex);
                Msg.AddLog(str + ex.Message);
                x.SayError(str + ex.Message);
            }
            else
            {
                Loger.Error(str);
                Msg.AddLog(str);
                x.SayError(str);
            }
        }

        public virtual void Stop()
        {
            Status = TaskStatusDef.Stop;
            //throw new NotImplementedException();
            IsFinished = true;
        }

        public void Pause()
        {
            Status = TaskStatusDef.Pause;
            //throw new NotImplementedException();
        }

        public virtual void Abort()
        {
            if (IsFinished) return;
            Log("任务意外中止");

            Status = TaskStatusDef.Fail;
            Finished();
        }

        /// <summary>
        /// 任务超时
        /// </summary>
        public void Timeout()
        {
            if (IsFinished) return;
            Log("任务超时");

            Status = TaskStatusDef.Timeout;
            Finished();
        }

        /// <summa- ry>
        ///  实际的工作 
        /// </summary>
        public virtual void Work()
        {
            if (TaskNo == Guid.Empty)
            {
                TaskNo = Guid.NewGuid();
            }

        }

        protected virtual void FailTask(string msg)
        {
            if (IsFinished) return;

            Msg.SetFail(msg);
            Status = TaskStatusDef.Fail;
            Finished();

        }

        public void SetSuccess()
        {
            if (IsFinished) return;
            Status = TaskStatusDef.Success;
            Msg.SetOk("任务完成");
            IsSuccess = true;
            Finished();
        }


        public void Finished()
        {
            FinishTime = DateTime.Now;
            if (!IsSuccess)
            {
                Loger.Info("任务结束：" + TaskName + "（" + TaskNo + "）,任务状态：" + Status);
            }
            IsFinished = true;
        }


        public void Log(string log)
        {
            Msg.AddLog(log);
            x.Say(log);
        }

        public void LogErr(string log)
        {
            Msg.AddLog(log);
            x.Say(log);
        }
    }
}
