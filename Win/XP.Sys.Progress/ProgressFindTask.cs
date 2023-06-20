using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Comm;
using XP.Comm.Event;
using XP.Comm.Task;

namespace XP.Sys.Progress
{
    /// <summary>
    /// 线程过滤器
    /// </summary>
    public class ProgressFindTask : BaseTaskItem
    {
        public Action<object, MsgEnentArgs<CommMsg>> LogEvent;


        ///// <summary>
        ///// 线程名称（一般是英文名）
        ///// </summary>

        //public string ProcessName { get; set; }
        /// <summary>
        /// 窗口标题
        /// </summary>
        public string WinTitle { get; set; }


        /// <summary>
        /// 主线程名字
        /// </summary>
        public string MainProcessName { get; set; }




        /// <summary>
        /// 子线程名称
        /// </summary>
        public string SubProcessName { get; set; }



        public int Interval { get; set; }

        protected System.Timers.Timer _RefrushTimer;


        public ProgressFilter Filter { get; set; }

        public ProgressFindTask() : base()
        {

            _Init();

        }
        protected void _Init()
        {

            Filter = new ProgressFilter();
            Filter.WinTitle = WinTitle;
            Filter.MainProcessName = MainProcessName;
            Filter.SubProcessName = SubProcessName;
            Filter.SayLogEvent += OnSayLog;
        }


        public void OnSayLog(string tit, string cot)
        {
            LogEvent?.Invoke(null, new MsgEnentArgs<CommMsg>() { Log = new CommMsg() { Title = tit, Body = cot } });
        }

        public override async Task WorkAsync()
        {
            await base.WorkAsync();
            Filter.MainProcessName = MainProcessName;
            Filter.SubProcessName = SubProcessName;

            await Filter.FindAsync().ContinueWith((x) =>
            {
                if (x.IsFaulted)
                {
                    Loger.Error("异步出现异常：", x.Exception);
                    Msg.SetFail("扫描失败，出现异常" + x.Exception.Message, x.Exception);
                    return;
                }
            });
            if (Filter.IsFind)
            {
                IsSuccess = true;
                Msg.SetOk("找到了：" + Filter.Result.ProcessName);
                var p = Filter.Result;

                Msg.Body = "窗口名：" + Filter.Result.MainWindowTitle;
                try
                {
                    Msg.Body += " 程序所在文件名： " + Filter.Result.MainModule.FileName;
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                Msg.SetOk("搜索完毕，没找到 " + MainProcessName);

            }

            //await CloseAsync();

        }



        // 这是从 XP.QQbot.ProgressMonitor  过来的代码 ，原来的地方是需要自动杀死异常窗口的
        protected async Task CloseAsync()
        {
            int TargetTotal = 0;
            if (null != Filter && null != Filter.ResultProcessList && 0 < Filter.ResultProcessList.Count)
            {
                TargetTotal = Filter.ResultProcessList.Count;
            }

            OnSayLog("准备关闭目标线程，线程数量：" + TargetTotal, "");

            if (0 < TargetTotal)
            {
                foreach (var p in Filter.ResultProcessList)
                {
                    string Name = p.ProcessName;
                    //string FileName = p.MainModule.FileName;
                    try
                    {
                        var ttask = Task.Run(() =>
                        {
                            Loger.OriginalInfo("准备杀死线程：" + MainProcessName + " 标题： " + WinTitle);
                            p.Kill();
                        });
                        await ttask;
                    }
                    catch (Exception ex)
                    {
                        Loger.Error("杀死进程失败：" + Name, ex);

                    }

                }

            }

        }


    }
}
