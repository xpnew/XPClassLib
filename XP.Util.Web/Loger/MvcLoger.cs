using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Util.Loger
{
    public static partial class MvcLoger
    {
        private static string NameOfError = "AppLoger.Error";


        public static void ErrorMvc(Exception ex, string areaName, string controller, string action)
        {
            string LogTemplate = "程序运行当中出现了异常：{0}\r\n  -------- 堆栈信息 --------  \r\n{1}\r\n  -------- 堆栈信息 -------- \r\n";
            string log = String.Format(LogTemplate, ex.Message, ex.StackTrace);
            ErrorMvc(log, areaName, controller, action);
            //Error(log,Exception) 的方式，只记录前面的信息
            //log4net.ILog log2 = log4net.LogManager.GetLogger(NameOfError);
            //log2.Error("程序出现异常，使用Error(log,Exception)方式记录！！！", ex);
        }

        public static void ErrorMvc(string log, string areaName, string controller, string action)
        {
            string Msg = FormatMvcLog(log, areaName, controller, action);
            log4net.ILog log2 = log4net.LogManager.GetLogger(NameOfError);
            log2.Error(Msg);

        }

        private static string FormatMvcLog(string log, string areaName, string controller, string action)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("日志内容：");
            sb.Append(log);
            sb.Append("\r\n == [附加信息]======\r\n");

            if (!String.IsNullOrEmpty(areaName))
            {
                sb.Append(" 所以的域：[");
                sb.Append(areaName);
                sb.Append("] ");
            }
            if (!String.IsNullOrEmpty(controller))
            {
                sb.Append("控制器：[");
                sb.Append(controller);
                sb.Append("] ");
                if (!String.IsNullOrEmpty(action))
                {
                    sb.Append(" 请求动作：[");
                    sb.Append(action);
                    sb.Append("] ");
                }
            }
            if (null != System.Web.HttpContext.Current)
            {
                sb.Append("请求的完整URL：");
                sb.Append(System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
                sb.Append("\r\n == [附加信息]======\r\n");
            }
            return sb.ToString();
        }
    }
}
