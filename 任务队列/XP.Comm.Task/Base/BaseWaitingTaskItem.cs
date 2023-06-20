using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace XP.Comm.Task.Base
{



    /// <summary>
    /// 需要等待完成的任务基类
    /// </summary>
    public class BaseWaitingTaskItem : BaseTaskItem
    {
        #region 初始化

        public BaseWaitingTaskItem() : base()
        {
            _InitWaitingBase();
        }

        protected virtual void _InitWaitingBase()
        {
            TaskReadyFlagSlim = new ManualResetEventSlim(false);
            CancelToken = CancelTokenSource.Token;
        }

        #endregion

        #region  异步和超时处理

        /// <summary>
        /// 心跳数据返回就续，线程回旋锁
        /// </summary>
        public System.Threading.ManualResetEventSlim TaskReadyFlagSlim { get; set; }



        /// <summary>
        /// 外部程序控制工作取消
        /// </summary>
        public CancellationToken CancelToken
        {
            get { return _CancelToken; }
            set { _CancelToken = value; }
        }

        private System.Timers.Timer _timer;

        private int _ProjectTimeoutSecond = 60;

        private CancellationToken _CancelToken;
        private volatile CancellationTokenSource _CancelTokenSource = new CancellationTokenSource();


        public CancellationTokenSource CancelTokenSource
        {
            get { return _CancelTokenSource; }
            set { _CancelTokenSource = value; }
        }

        /// <summary>
        /// 任务等待时间，如果这个时间内还没有运行完毕就自动结束，释放资源
        /// </summary>
        /// <remarks>
        /// 计时开始的时间是第一次获取本地资产的时间，而不是实例初始化
        /// </remarks>
        public int WaitingTimeoutMillSecond { get; set; } = 500;
        public bool IsNeedWaitting { get; set; } = true;
        #endregion


        public override async System.Threading.Tasks.Task WorkAsync()
        {
            //return base.StartAsync();
            if (!IsNeedWaitting)
            {
                await _DoWorkAsync();
                return;
            }

            TaskReadyFlagSlim.Reset();
            await _DoWorkAsync();
            try
            {
                //有一个延迟等待，防止没有获取到数字。
                if (TaskReadyFlagSlim.Wait(WaitingTimeoutMillSecond, CancelToken))
                {
                    x.Say("挂起被激活。");
                    SetSuccess();
                }
                else
                {
                    x.Say("等待超时了。");
                    x.Say("CancellToken " + CancelToken.IsCancellationRequested);
                    await _TimeoutAsync();
                }
            }
            //提前终止
            catch (OperationCanceledException ex)
            {

                FailTask("提前终止。");
                x.Say("提前终止。");

            }
            catch (Exception ex)
            {
                FailTask($"任务异常。{ex}");
                x.Say($"任务异常。{ex}");

            }
        }



        public virtual async System.Threading.Tasks.Task WaitingAsync()
        {

            TaskReadyFlagSlim.Reset();
            try
            {
                //有一个延迟等待，防止没有获取到数字。
                if (TaskReadyFlagSlim.Wait(WaitingTimeoutMillSecond, CancelToken))
                {
                    x.Say("挂起被激活。");
                    SetSuccess();

                }
                else
                {


                    x.Say("等待超时了。");
                    x.Say("CancellToken " + CancelToken.IsCancellationRequested);
                    await _TimeoutAsync();
                }


            }
            //提前终止
            catch (OperationCanceledException ex)
            {


                FailTask("提前终止。");
                x.Say("提前终止。");

            }
            catch (Exception ex)
            {

                FailTask($"任务异常。{ex}");
                x.Say($"任务异常。{ex}");

            }

        }

        /// <summary>
        /// 实际的工作
        /// </summary>
        /// <returns></returns>
        protected virtual async System.Threading.Tasks.Task _DoWorkAsync()
        {

        }

        protected virtual async System.Threading.Tasks.Task _TimeoutAsync()
        {

        }

        public void Set()
        {
            TaskReadyFlagSlim.Set();
        }

    }
}
