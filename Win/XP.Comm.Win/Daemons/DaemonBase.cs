using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using XP.DB.LocalRedis;
using XP.Util.Configs;

namespace XP.Comm.Daemons
{
    /// <summary>
    /// 监控程序基类
    /// </summary>
    public class DaemonBase
    {

        //protected string _TestName = "";

        /// <summary>
        /// 监控类的名称
        /// </summary>
        protected string _DaemonClassName = null;

        /// <summary>
        /// 监控服务的名称，由安装程序决定
        /// </summary>
        public string DeamonSvcName { get; set; }
        /// <summary>
        /// 配置组名，需要派生类自己命名
        /// </summary>
        protected string _ConfingGroupName = null;
        /// <summary>
        /// 当前守护程序自己具体的Redis名称，以“:”开头，派生类自己命名
        /// </summary>
        protected string _CurrentDaemonRedisName = null;
        /// <summary>
        /// 全部守护程序使用的公共前缀
        /// </summary>
        protected string _DaemonRedisPrefix = "LJY:WinSvc:Daemons";

        protected string _HostName = null;
        /// <summary>
        /// 刷新间隔，毫秒，默认为100，可以在配置文件当中指定
        /// </summary>
        private int _RefrushMillisecond { get; set; } = 100;

        /// <summary>
        /// 计时表（累计计算）
        /// </summary>
        private System.Diagnostics.Stopwatch Watcher;

        /// <summary>
        /// ▲▲▲已经关闭▲▲▲内部的计时器，定时完全成任务
        /// </summary>
        System.Timers.Timer RefrushTimer { get; set; }

        /// <summary>
        /// 内部心跳计时器，主要是通过Redis报告运行状态，间隔为1秒
        /// </summary>
        protected System.Timers.Timer _InnerHeartbeatTimer { get; set; }
        /// <summary>
        /// 内部心跳的次数，因为心跳是按秒算的，所以这个数字可以用来间接计算得到分钟
        /// </summary>
        protected long InnerHeartbeatTimes { get; set; }

        /// <summary>
        /// Redis工具，Redis缓存操作封装
        /// </summary>
        protected RedisProvider RedisUtil;


        protected string DictFullKey { get; set; }

        /// <summary>
        /// 守护程序id，为多终端运行准备
        /// </summary>
        public Guid DaemonId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// 序列号，通过DaemonSortDict排序
        /// </summary>
        public int DaemonIndex { get; set; }

        /// <summary>
        /// 程序启动的时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 最后一次开始&恢复的时间
        /// </summary>
        public DateTime LastStartTime { get; set; }


        protected bool _IsWorking = false;

        public bool IsWorking { get => _IsWorking; }

        #region 消息、报告
        protected Action<string, string> _InnerLogEvent;

        public Action<string, string> LogEvent;

        public void OnSendLog(string tit, string body = null)
        {
            LogEvent?.Invoke(tit, body);
        }

        /// <summary>
        /// 最后一个任务的标志
        /// </summary>
        public string LastTaskTag { get; set; }

        #endregion



        #region 任务计数器

        /// <summary>
        /// 允许计数
        /// </summary>
        public bool EnableCounter { get; set; } = false;



        int _Counter4Task = 0;

        protected void StopCounter()
        {

            OnSendLog("计数器关闭。");

        }

        protected void StartCounter()
        {
            _Counter4Task = 0;
            OldMinuteCounter = 0;
            OnSendLog("计数器启动。");
        }
        int OldMinuteCounter = 0;
        int NewMinuteCounter = 0;
        int HistoryCounter = 0;

        protected void MinuteReport()
        {
            if (!_IsWorking)
            {
                return;
            }
            int diff = _Counter4Task - OldMinuteCounter;
            if (0 > diff) return;

            OldMinuteCounter = _Counter4Task;
            OnSendLog($"最近一分钟完成的任务数：{diff},总任务数：{_Counter4Task}", $"最近一分钟完成的任务数：{diff},总任务数(本次启动开始，按下停止重置)：{_Counter4Task},历史任务数（程序启动）：{HistoryCounter}，最后任务的标志：{LastTaskTag}");
        }

        public void NextCount()
        {
            _Counter4Task++;
            HistoryCounter++;
        }



        #endregion


        public DaemonBase()
        {
            //_TestName = "Parent SET !!";

            _Init();
        }

        /// <summary>
        /// 核心初始化方法
        /// </summary>
        protected virtual void _Init()
        {
            _HostName = XP.Util.Net.IPvsNameTools.GetIpAndName();
            _DefaultInitName();
            Loger.Info(_DaemonClassName + "===========已经初始化!");
            Loger.Error(_DaemonClassName + "===========测试日志输出!");

            Loger.StartWatchWeb();
            LocalRedisEngine.Self.StartEngine();
            //RedisUtil = new RedisProvider(DictFullKey, LocalRedisEngine.Self);
            _InitConfig();

            Watcher = new Stopwatch();

            _InnerHeartbeatTimer = new System.Timers.Timer();
            _InnerHeartbeatTimer.Interval = 1000;
            _InnerHeartbeatTimer.Elapsed += InnerHeartbeatTimerOnElapsed;
            _InnerHeartbeatTimer.Start();

            BeginTime = DateTime.Now;
            LastStartTime = DateTime.Now;

            RefrushTimer = new Timer();
            RefrushTimer.Elapsed += RefrushTimer_Elapsed;

            RefrushTimer.Interval = _RefrushMillisecond;

            var Last = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            _InitDictFullKey();

            RedisUtil = new RedisProvider(DictFullKey, EngineTypeDef.BasicEdition);

            RedisUtil.Insert("ProgramStart", Last);
            RedisUtil.Insert("AppName", _CurrentDaemonRedisName);
            RedisUtil.Insert("AppHost", _HostName);
            RedisUtil.Insert("AppPath", System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            Loger.Info(_DaemonClassName + "===========初始化完成!");
        }

        protected virtual void _InitDictFullKey()
        {
            DictFullKey = _DaemonRedisPrefix + _CurrentDaemonRedisName + ":" + _HostName;
        }


        /// <summary>
        /// 通过默认的方式（类名），构造关键的名称
        /// </summary>
        protected void _DefaultInitName()
        {
            _DaemonClassName = this.GetType().Name;
            if (null == _ConfingGroupName || 0 == _ConfingGroupName.Length)
            {
                _ConfingGroupName = _DaemonClassName + "Set";
            }
            if (null == _CurrentDaemonRedisName || 0 == _CurrentDaemonRedisName.Length)
            {
                _CurrentDaemonRedisName = ":" + _DaemonClassName;
            }
        }

        /// <summary>
        /// 初始化配置
        /// </summary>
        private void _InitConfig()
        {
            var cr = ConfigReader._Instance;
            // 设置Redis缓存的连接

            //设置其它参数
            KeyGroupReader sets = new KeyGroupReader(cr, _ConfingGroupName);
            _RefrushMillisecond = sets.GetInt("DaemonRefrushMillisecond", 100);

        }
        /// <summary>
        /// 心跳计时器触发的事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InnerHeartbeatTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            InnerHeartbeatTimes++;
            //throw new NotImplementedException();
            var Last = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            RedisUtil.Insert("LastUpdate", Last);

            if (0 == InnerHeartbeatTimes % 60)
            {
                MinuteReport();
            }

        }
        /// <summary>
        /// 定时完全的工作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void RefrushTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //throw new NotImplementedException();
            //Loger.Debug("定时器执行");

        }

        public virtual void Start()
        {
            //x.Say("到底是基类还是派生类：" + _TestName);
            Loger.Info("程序已经启动");
            _IsWorking = true; ;
            RefrushTimer.Start();
            Watcher.Start();
            LastStartTime = DateTime.Now;
            InnerHeartbeatTimes = 0;
            StartCounter();
        }

        public virtual void Stop()
        {
            _IsWorking = false;
            Loger.Info("程序 停止");

            RefrushTimer.Stop();
            Watcher.Stop();
            StopCounter();
        }



    }

}
