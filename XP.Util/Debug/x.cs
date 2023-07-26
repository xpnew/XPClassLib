using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using XP.Util.Debug;
using XP.Util;


namespace XP
{

    public delegate void SayMsg(string msg);

    public delegate void DebugMsg(object sender, DebugMsgEventArgs entArgs);

    /// <summary>
    /// 调试的时候，用来跟踪过程和反应信息类
    /// </summary>
    public class x
    {


        /// <summary>
        /// 单纯的消息内容处理广播事件
        /// </summary>
        public static event SayMsg MsgBroatCast;
        /// <summary>
        /// 带级别的消息处理广播事件
        /// </summary>
        public static event DebugMsg DebugMsgBroatCast;



        private static object _MsgSource = null;

        private static bool _HasReadyStart = false;

        public static object MsgSource
        {
            get {
                if (null == _MsgSource)
                {
                    return "默认内部消息源";
                }
                return x._MsgSource; }
            set { x._MsgSource = value; }
        }


        public static XPMsg SayErrorMsg(string msg)
        {
            XPMsg Result = new XPMsg()
            {
                StatusCode = -1,
                Name = "Error"
            };


            Result.Title = msg;
            return Result;
        }
        public static void Say(string msg)
        {
            if (_HasReadyStart)
            {
            }
            else
            {
                InitSay();
            }
            MsgBroatCast(msg);

          
        }

     



        private static void InitSay()
        {
            MsgBroatCast += DefaultSay;

            _HasReadyStart = true;
          
        }

        private static void DefaultSay(string msg)
        {
            if (EnableDebug)
            {
                Trace.WriteLine(msg);
            }
        }
       



        public static void Say(string msg, DebugLevel leve)
        {
            DebugMsgEventArgs arg = new DebugMsgEventArgs(new Util.Debug.DebugMsg() { Level = leve, Msg = msg });
            DebugMsgBroatCast?.Invoke(MsgSource, arg);
        }


        public static void TimerLog(string log)
        {
            log += "[当前时间：" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss.ffff") + "]";
            Say(log);
        }


        public static void SayError(string msg)
        {
            Say(msg, DebugLevel.Error);
        }


        public static void Error(Exception ex)
        {
            if (ex is System.NullReferenceException)
            {
                TimerLog(" 对象引用失败 ：" + ex.Source + "   异常： " + ex.Message);
            }
            if (null == ex.InnerException)
            {
                TimerLog(" 异常： " + ex.Message);
            }
            else
            {
                TimerLog(" 异常： " + ex.Message + " 并且，内部异常：" + ex.InnerException.Message);
            }
        }


        /// <summary>是否允许开启Debug模式</summary>
        /// <remarks>
        /// 通过配置文件开启Debug模式以后可以在网站运行时提供一些额外的调试信息，辅助诊断问题。
        /// </remarks>
        public static bool EnableDebug
        {
            get
            {
#if DEBUG
                bool Result = true;
                if (Result)
                    return true;
#endif
                string EnableDebugConfig = Util.Conf.GetConfigItem("EnableDebug");
                //string EnableDebugConfig = "1";
                if (String.IsNullOrEmpty(EnableDebugConfig))
                {
                    return false;
                }
                if ("1" == EnableDebugConfig || "true" == EnableDebugConfig.ToLower())
                    return true;
                return false;
            }

        }

    }
}
