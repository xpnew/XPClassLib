using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XP.DB.Models;

namespace XP.Web.Permission.Utils
{
    /// <summary>
    /// 用来对操作日志进行国际化的类
    /// </summary>
    public class OperateLogTrancer
    {
        private static bool _HasRegexInit = false;

        private static Regex _RegexExpression;
        private static string _Pattern = "Enum_[A-Za-z0-9_]+_[A-Za-z0-9_]+";
        public static Regex RegexExpression
        {

            get
            {
                if (_HasRegexInit)
                {
                    return _RegexExpression;
                }
                _RegexExpression = new Regex(_Pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
                _HasRegexInit = true;
                return _RegexExpression;
            }
        }


        public Sys_ActionLogV Model { get; set; }

        public OperateLogTrancer(Sys_ActionLogV model)
        {

            Model = model;
        }

        public void FormatContent()
        {
            string RightGlobalResult = "";
            //if (!String.IsNullOrEmpty(Model.RightGlobalDesc))
            //{
            //    RightGlobalResult = Model.RightGlobalDesc;
            //}
            //else 
            if (!String.IsNullOrEmpty(Model.PageName) && !String.IsNullOrEmpty(Model.RightName))
            {
                RightGlobalResult = Model.PageName + "-" + Model.RightName;
            }


            if (String.IsNullOrEmpty(Model.LogContent))
            {
                Model.LogContent = RightGlobalResult;

            }
            else
            {
                Model.LogContent = Model.LogContent.Replace("{TM:RightGlobalDesc}", RightGlobalResult);
                Model.LogContent = Model.LogContent.Replace("{TM:RightGlobalName}", RightGlobalResult);
            }
            var LogMatch = RegexExpression.Match(Model.LogContent);

            //string TT_TM = "准备匹配和替换的文本[{0}]，匹配结果[{1}]，替换之后的文本[{2}]";
            //string TT_Before = Model.LogContent;
            if (LogMatch.Success)
            {
               // string TransDesc = ControllerUtility.Transfer(LogMatch.Value);
                //Model.LogContent = RegexExpression.Replace(Model.LogContent, TransDesc);
            }
            //string TT_Result = String.Format(TT_TM, TT_Before, LogMatch.Success, Model.LogContent);


            //SA.WebUtil.Loger.Info(TT_Result);
        }


    }
}
