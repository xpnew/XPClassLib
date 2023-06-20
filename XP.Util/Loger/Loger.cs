using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using XP.Util.Path;

[assembly: log4net.Config.XmlConfigurator(Watch = true, ConfigFile = "log4net.config")]
namespace XP
{
    public static partial class Loger
    {

        private static string NameOfLog = "AppLoger.Log";
        private static string NameOfDebug = "AppLoger.Debug";
        private static string NameOfError = "AppLoger.Error";
        private static string NameOfWarn = "AppLoger.Warn";

        private static string NameOfInfo = "AppLoger.Info";
        private static string NameOfOriginalInfo = "AppLoger.OriginalInfo";


        private static string NameOfDbCount = "Service.DbCount";
        private static string NameOfTimer = "UnitTest.Timer";
        private static bool _HasReady = false;

        public static bool HasReady
        {
            get { return Loger._HasReady; }
            set { Loger._HasReady = value; }
        }

        private static log4net.ILog GetLogger(string name)
        {
            if (!HasReady)
            {
                TryStartLog();
            }

            return log4net.LogManager.GetLogger(name);



        }




        public static void TryStartLog()
        {

            //log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(System.Web.HttpContext.Current.Server.MapPath("/Log4Net.config")));
            HasReady = true;

        }

        public static void ErrorJson(Comm.CommMsg msg)
        {
            if (null == msg) return;
            Error(Util.Json.JsonHelper.Serialize(msg));
        }

        /// <summary>
        /// 根据传入的文件信息参数，确定log4net监听哪个配置文件
        /// </summary>
        /// <param name="file"></param>
        public static void TryInitConfig(System.IO.FileInfo file)
        {

            log4net.Config.XmlConfigurator.Configure(file);
        }

        public static void DoLog(string logname,string log)
        {
            var log2 = GetLogger(logname);
            log2.Info(log);
        }

        //public static enum LogLevel
        //{
        //    Normal = 2,
        //    Info = 1,
        //    Error = 4
        //}


        public static void TimerLog(string msg)
        {

            //msg += "    ※※※  " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss.FFFFFF");

            var log2 = GetLogger(NameOfTimer);
            log2.Info(msg);

        }

        public static void Debug(string msg)
        {
            var log2 = GetLogger(NameOfDebug);
            log2.Debug(msg);

        }

        public static void LogInfo(string log)
        {
            var log2 = GetLogger(NameOfLog);
            log2.Info(log);
        }
        public static void OriginalInfo(string log)
        {
            var log2 = GetLogger(NameOfOriginalInfo);
            log2.Info(log);
        }

        /// <summary>
        /// 数据统计
        /// </summary>
        /// <param name="log"></param>
        public static void DbCount(string log)
        {
            var log2 = GetLogger(NameOfDbCount);
            log2.Info(log);

        }
        public static void Info(string log)
        {

            var log2 = GetLogger(NameOfInfo);
            log2.Info(log);

        }

        public static void Warn(string log)
        {

            var log2 = GetLogger(NameOfWarn);
            log2.Warn(log);

        }

        public static void Error(string log)
        {
            var log2 = GetLogger(NameOfError);
            log2.Error(log);

        }
        public static void Error(string log, Exception ex, int statckPlies = 5)
        {
            var log2 = GetLogger(NameOfError);
            string FullString = null;
            System.Threading.Tasks.Task.Delay(10);

            StringBuilder sb = new StringBuilder();
            sb.Append(log);
            sb.Append("\n");
            sb.Append("程序出现异常，使用Error(log,Exception)方式记录！！！\n");
            sb.Append(Exception2String(ex));
            sb.Append(Util.Debug.StackTraceUtil.BuildStackStr(new StackTrace(ex), statckPlies));
            sb.Append(Util.Debug.StackTraceUtil.BuildStackStr(new StackTrace(true), statckPlies));
            if (null != ex.InnerException)
            {
                sb.Append("\t输出内部异常--------------");
                sb.Append("\n");
                sb.Append(Exception2String(ex.InnerException));
            }

            FullString = sb.ToString();
            log2.Error(FullString);
            //log2.Error("程序出现异常，使用Error(log,Exception)方式记录！！！", ex);
        }
        public static string Exception2String(Exception ex)
        {
            string Tm = "【异常类型】：{0} \n【异常信息】：{1} \n【堆栈调用】：{2}";
            string ExString = string.Format(Tm, new object[] {
                ex.GetType().Name, ex.Message, ex.StackTrace });

            return ExString;
        }




        public static void StartWatchWeb()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(PathTools.GetFull("Log4Net.config")));

        }

    }
}
