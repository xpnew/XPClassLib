/******************************************************
 * 动作日志过滤器
 * 创建人：于海峰
 * 修改人：于海峰
 * 创建时间：‎2013‎年‎2‎月‎21‎日
 * 修改日期：2013年5月29日
 * 修改日期：2014年4月24日
 * 
 * 
 * 2014年4月24日 修改说明：
 * 增强了捕获日志的方法。
 * 如果在ViewData["EnableViewDataLog"]当中标记为true,那么就会启动视图数据模式。
 * 在“视图数据模式”下，会尝试读取RequestName数组当中同名的ViewData，读取成功，则会覆盖通过RequestName传入的数据
 * 典型应用场景是：添加商户的时候商户编码在提交之前是完整的。而写日志则需要完整的商户编码。
 * 在这种情况下，可以启动“视图数据模式”，并且写日志的时候可以得到完整的商户编码。
 * 
 * 
 * 
 * 
 * 功能说明：
 * 1、给Action添加动作日志。
 * 2、只记录操作成功的动作日志。
 * 
 * 
 * 
 * 
 * 名词术语：
 * 动作：专指针对MVC模式下的"Action"。比如说，添加、修改、列表
 * 动作日志：专指针对MVC模式下的Action，在系统当中生成并且保存到的数据库当中的“操作记录”。动作日志针对的是系统的使用者、操作人员。
 * 
 * 
 * 
 * 
 * 
 * */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using XP.Comm;
using XP.Comm.Msgs;
using XP.DB.BLL;
using XP.DB.Models;
using XP.Util;
using XP.Util.TypeCache;
using XP.Web.Permission.Filters;

namespace XP.Web.Permission
{
    /// <summary>
    /// 动作日志过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ActionLogFilterAttribute : ActionFilterAttribute
    {

        private WebUser _SessionUser;


        /// <summary>
        /// 使用用户管理
        /// </summary>
        public WebUser UserSessionManage
        {
            get
            {
                if (null == _SessionUser)
                {
                    _SessionUser = WebUser.CreateUser();
                }
                return _SessionUser;
            }
        }

        /// <summary>
        /// 动作说明设置标记（防止反复获取影响性能）
        /// </summary>
        private bool _ActionDescSetFlag;
        private string _ActionDesc;
        /// <summary>动作说明</summary>
        public string ActionDesc
        {
            get
            {
                if (_ActionDescSetFlag)
                {
                    return _ActionDesc;
                }
                System.Web.HttpRequest Request = System.Web.HttpContext.Current.Request;
                if (!String.IsNullOrEmpty(Request["RightGlobalDesc"]))
                {
                    _ActionDesc = Request["RightGlobalDesc"];
                    _ActionDescSetFlag = true;
                    return _ActionDesc;

                }
                if (!String.IsNullOrEmpty(Request["ActionDesc"]))
                {
                    _ActionDesc = Request["ActionDesc"];
                    _ActionDescSetFlag = true;
                    return _ActionDesc;
                }
                //if (!String.IsNullOrEmpty(_LogRightData.RightGlobalDesc))
                //    _ActionDesc = _LogRightData.RightGlobalDesc;
                //else if (!String.IsNullOrEmpty(_LogRightData.RightGlobalName))
                //    _ActionDesc = _LogRightData.RightGlobalName;
                if (!String.IsNullOrEmpty(_LogRightData.RightName))
                    _ActionDesc = _LogRightData.RightName;
                else
                    _ActionDesc = String.Empty;
                _ActionDescSetFlag = true;
                return _ActionDesc;
            }
            set
            {
                _ActionDesc = value;
                _ActionDescSetFlag = true;
            }
        }
        /// <summary>对象名称</summary>
        public string ObjectNames { get; set; }
        /// <summary>路过日志</summary>
        public bool SkipLog { get; set; }
        /// <summary>当前日志动作对象</summary>
        private Sys_RightV _LogRightData { get; set; }

        private bool _IsBriefly = false;
        /// <summary>简略模式</summary>
        /// <remarks>
        /// 暂时使用固定值，以后改为读取config
        /// </remarks>
        public bool IsBriefly
        {
            get { return _IsBriefly; }
            set { _IsBriefly = value; }
        }


        protected bool HasException { get; set; }
        /// <summary>
        /// 允许启动视图数据模式
        /// </summary>
        public bool EnableViewDataLog { get; set; }

        /// <summary>
        /// 日志内容 需要检查的表单名称 
        /// </summary>
        public List<string> RequestNameList { get; set; }


        /// <summary>
        /// 日志内容需要记录的表单值
        /// </summary>
        public List<string> RequestValueList { get; set; }

        /// <summary>
        /// 动作执行的结果是失败的。
        /// </summary>
        protected bool HasResultFail { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            //LogFilter(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            base.OnActionExecuted(filterContext);

            if (!UserSessionManage.UserAuthorized)
            {
                return;
            }

            InitLogerStatus(filterContext);

            if (HasException)
            {
                return;
            }


            //已经有专门的异常处理所以这里不必这样复杂。
            //if (null != filterContext.Exception)
            //{
            //    var BusyException = filterContext.Exception as System.ServiceModel.ServerTooBusyException;
            //    if (null != BusyException)
            //    {

            //        filterContext.ResultMsg = new RedirectResult("/PageComm/ServerTooBusy/");
            //        return;
            //    }
            //    var WebException = filterContext.Exception as System.Net.WebException;
            //    if (null != BusyException)
            //    {
            //        filterContext.ResultMsg = new RedirectResult("/PageComm/ServerTooBusy/");
            //        return;
            //    }

            //    x.TimerLog("出现异常，跳出！！！（DEMO当中只有提示不跳出） ");
            //    return;
            //}
            #region LogMark标记 可以通过SkipLog属性实现跳过日志
            //因为ResultFail实现了检查成功代码，所以跳过日志的作用就没有必要，所以注释下面的代码。
            object[] LogMarkArr = filterContext.ActionDescriptor.GetCustomAttributes(typeof(LogMarkAttribute), true);
            if (0 < LogMarkArr.Length)
            {
                x.TimerLog("捕获到参数LogMark");

                for (int i = 0; i < LogMarkArr.Length; i++)
                {
                    LogMarkAttribute Mark = LogMarkArr[i] as LogMarkAttribute;
                    if (Mark.SkipLog)
                    {
                        return;
                    }
                    //Trace.WriteLine("类型：" + Mark.ModelType.Name);
                    //Trace.WriteLine("需要的属性名称：" + Mark.PropertyNames);
                    //Trace.WriteLine("需要处理的参数：" + Mark.RequestNames);
                }
            }
            #endregion

            if (HasResultFail)
            {
                Trace.WriteLine(" 程序 需要退出 ");
                //Demo以外的情况下需要退出。
                return;
            }

            LogFilter(filterContext);
        }


        protected bool CheckLogReady(ActionExecutedContext filterContext)
        {

            return true;

        }

        /// <summary>
        /// 根据传递的动作上下文内容，初始化视图的状态
        /// </summary>
        /// <param name="filterContext">动作上下文</param>
        protected void InitLogerStatus(ActionExecutedContext filterContext)
        {
            #region 检查异常状态
            //如果已经出现了异常，并且异常没有被处理
            if (null != filterContext.Exception && !filterContext.ExceptionHandled)
            {
                HasException = true;
                return;
            }
            else
            {
                HasException = false;
            }
            #endregion

            //普通的动作返回ViewResult
            System.Web.Mvc.ViewResult view = filterContext.Result as ViewResult;

            #region 检查 启动视图数据模式
            if (null != view)
            {

                if (null != view.ViewData["EnableViewDataLog"])
                {
                    EnableViewDataLog = (bool)view.ViewData["EnableViewDataLog"];
                }
                else
                {
                    EnableViewDataLog = false;
                }
            }
            else
            {
                EnableViewDataLog = false;
            }


            #endregion

            #region 根据RequestName获取日志内容项

            RequestNameList = new List<string>();
            RequestValueList = new List<string>();

            var Request = filterContext.HttpContext.Request;
            if (!String.IsNullOrEmpty(Request["RequestName"]))
            {
                RequestNameList = ArrayUtility.StringSplitList<string>(Request["RequestName"]).Where(o => !String.IsNullOrEmpty(o)).ToList();
                for (int i = 0; i < RequestNameList.Count; i++)
                {
                    string RequestValue = Request[RequestNameList[i]];
                    RequestValueList.Add(RequestValue);

                }
                if (EnableViewDataLog)
                {
                    for (int i = 0; i < RequestNameList.Count; i++)
                    {
                        string Name = RequestNameList[i];
                        if (null != view.ViewData[Name])
                        {
                            RequestValueList[i] = view.ViewData[Name] as string;
                        }
                    }
                }
            }
            //去除空白
            if (0 < RequestValueList.Count)
            {
                RequestValueList = RequestValueList.Where(o => !String.IsNullOrEmpty(o)).ToList();
            }
            #endregion
            #region 判断动作是否成功，主要依赖于动作当中的StatusCode

            Nullable<int> StatusCode = null;
            if (null != view)
            {
                Trace.WriteLine("ViewData StatusCode:  " + view.ViewData["StatusCode"]);
                if (null != view.ViewData["StatusCode"])
                {
                    StatusCode = (int)view.ViewData["StatusCode"];
                }

            }



            //Ajax动作，返回JsonResult
            JsonResult json = filterContext.Result as JsonResult;
            if (null != json)
            {
                CommMsg result = json.Data as CommMsg;
                if (null != result)
                {

                    Trace.WriteLine("json StatusCode:  " + result.StatusCode);
                    StatusCode = result.StatusCode;
                }

            }
            if (0 < StatusCode)
            {
                Trace.WriteLine("执行成功！！！ ");
                HasResultFail = false;
            }
            else
            {
                HasResultFail = true;
            }
            #endregion
        }


        #region 检查状态码 已经废弃


        /// <summary>检查状态码</summary>
        /// <remarks>
        /// 通过检查ViewData["StatusCode"]或者result.StatusCode来判断Action是否正常执行。
        /// 
        /// 
        /// </remarks>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        protected bool ResultFail(ActionExecutedContext filterContext)
        {
            //Nullable<int> StatusCode = null;

            ////普通的动作返回ViewResult
            //System.Web.Mvc.ViewResult view = filterContext.ResultMsg as ViewResult;
            //if (null != view)
            //{
            //    Trace.WriteLine("ViewData StatusCode:  " + view.ViewData["StatusCode"]);
            //    if (null != view.ViewData["StatusCode"])
            //    {
            //        StatusCode = (int)view.ViewData["StatusCode"];
            //    }
            //}
            ////Ajax动作，返回JsonResult
            //JsonResult json = filterContext.ResultMsg as JsonResult;
            //if (null != json)
            //{
            //    MsgResult result = json.Data as MsgResult;
            //    if (null != result)
            //    {

            //        Trace.WriteLine("json StatusCode:  " + result.StatusCode);
            //        StatusCode = result.StatusCode;
            //    }

            //}
            //if (0 < StatusCode)
            //{
            //    Trace.WriteLine("执行成功！！！ ");
            //    return false;
            //}
            return true;
        }

        #endregion
        protected void LogFilter(ActionExecutedContext filterContext)
        {

            _ActionDescSetFlag = false;

            if (null != filterContext.RouteData.Values["controller"])
            {
                var MvcNames = MvcUtil.GetMvcName(filterContext.RouteData, filterContext.ActionDescriptor, true,true);

                //View_RightGlobal _LogRightData = UserSessionManage.Cache.PageRight.GetRight(controller, action);
                this._LogRightData = UserSessionManage.Cache.PageRight.GetRight(MvcNames.AreaName2Lower, MvcNames.ControllerName2Lower, MvcNames.ActionName2Lower);
                if (null != _LogRightData && null != _LogRightData.RightLog && _LogRightData.RightLog.Value)
                {
                    if (_LogRightData.RightLog.HasValue && _LogRightData.RightLog.Value)
                    {
                        WriteLog(_LogRightData);
                    }
                }
            }

        }
        protected void Say(string msg)
        {
            //Trace.WriteLine("\n==============================\n");
            Trace.WriteLine(msg);
            //Trace.WriteLine("\n==============================\n");
        }


        //public int GetSystemId()
        //{
        //    return ControllerUtility.GetUserSystemId();
        //}

        /// <summary>写入日志</summary>
        /// <param name="actionRight">当前操作的动作</param>
        protected void WriteLog(Sys_RightV actionRight)
        {

            string LogContent;
            System.DateTime LogDate;
            int? RightId;
            string LogIP;
            int? UserId;
            string UserName;
            int? StoreId;
            string StoreCode;
            int? SystemSupportID;
            string SystemSupportName;

            System.Web.HttpRequest Request = System.Web.HttpContext.Current.Request;

            LogContent = "简单日志，以后再写模板解析程序，URL：" + System.Web.HttpContext.Current.Request.RawUrl;

            RightId = actionRight.Id;
            UserId = UserSessionManage.Cache.UserInfo.Id;
            StoreId = UserSessionManage.Cache.UserInfo.StoreId;
            UserName = UserSessionManage.Cache.UserInfo.UserName;
            StoreCode = UserSessionManage.Cache.UserInfo.StoreCode;

            LogDate = DateTime.Now;

            LogIP = Util.WebUtils.RequestUtil.GetIp();

            var bll = new Sys_ActionLogBLL();

            //ISysService SysService = ControllerUtility.GetSysService();

            Sys_ActionLogT LogObj = new Sys_ActionLogT()
            {
                LogContent = LogContent
                ,
                CreateTime = LogDate,
                CreateTS = GeneralTool.DateTime2UnixTimestamp(LogDate)
                ,
                RightId = RightId
                ,
                UserIP = LogIP
                ,
                UserId = UserId
                ,
                UserName = UserName
                ,
                StoreId = StoreId.Value
                ,
                StoreCode = StoreCode
            };

            #region 获取模板
            //string LanguageMark = CookiesManage.CookieLanguageMark;
            string LanguageMark = "zh-cn";
            string LogText;         //日志文本内容,前面的LogContent变量代表简单日志内容。这里使用新的变量加以区分
            string TemplateText=null;    //模板文本内容
            var cr = Util.Config.ConfigReader.Self;

            if (!String.IsNullOrEmpty(_LogRightData.RightLogTemplate))
            {
                TemplateText = _LogRightData.RightLogTemplate;
            }
            if (String.IsNullOrEmpty(TemplateText))
            {
                TemplateText = cr.GetTemplate("actionlog", LanguageMark);
            }
            if (String.IsNullOrEmpty(TemplateText))
            {
                TemplateText = cr.GetTemplate("actionlog", "default");
            }
            if (String.IsNullOrEmpty(TemplateText))
            {
                IsBriefly = true;
                TemplateText = "{TM:RightGlobalName}";
            }

            #endregion

            #region 处理提交过来的数据
            TemplateText = AnalizingDesc(TemplateText);
            #endregion 处理提交过来的数据

            #region 基础数据处理
            if (IsBriefly)
            {
                LogText = TemplateText;
            }
            else
            {
                LogText = TemplateText;

                if (!String.IsNullOrEmpty(TemplateText))
                {
                    //属性缓存管理
                    //PropertiesCacheManager pcm = new PropertiesCacheManager();
                    EntityTypesCache pcm = EntityTypesCache.CreateInstance();
                    EntityTypesCacheItem CacheRightClass = pcm.GetItem(typeof(Sys_RightV));
                    EntityTypesCacheItem CacheLogClass = pcm.GetItem(typeof(Sys_ActionLogT));

                    //优先处理时间问题：
                    String DateString = LogDate.ToString("MM/dd/yyyy HH:mm:ss");
                    LogText = LogText.Replace("{TM:LogDate2}", DateString);
                    DateString = LogDate.ToString("yyyy-MM-dd HH:mm:ss");
                    LogText = LogText.Replace("{TM:LogDate3}", DateString);

                    foreach (string name in CacheRightClass.PropertyNames)
                    {
                        object value = CacheRightClass.PropertyDic[name].GetValue(actionRight, null);
                        if (null == value)
                            continue;
                        LogText = LogText.Replace("{TM:" + name + "}", value.ToString());
                    }
                    foreach (string name in CacheLogClass.PropertyNames)
                    {
                        object value = CacheLogClass.PropertyDic[name].GetValue(LogObj, null);
                        if (null == value)
                            continue;
                        LogText = LogText.Replace("{TM:" + name + "}", value.ToString());
                    }

                    //if (!String.IsNullOrEmpty(System.Web.HttpContext.Current.Request["ItemName"]))
                    //{
                    //    List<string> NameList = ArrayUtility.StringSplitList<string>(System.Web.HttpContext.Current.Request["ItemName"]).Where(o => !String.IsNullOrEmpty(o)).ToList();
                    //    List<string> IdList = ArrayUtility.StringSplitList<string>(System.Web.HttpContext.Current.Request["ItemName"]).Where(o => !String.IsNullOrEmpty(o)).ToList();

                    //    foreach (string name in NameList)
                    //    {
                    //        LogText += " " + name;
                    //    }
                    //}

                }
            }

            LogObj.LogContent = LogText;

            #endregion   <<=== 模板处理结束



            long ReturnValue = bll.Create(LogObj);


        }

        /// <summary>处理模板当中的明细部分</summary>
        /// <param name="templateText"></param>
        protected string AnalizingDesc(string templateText)
        {
            System.Web.HttpRequest Request = System.Web.HttpContext.Current.Request;
            ObjectNames = String.Empty;
            #region 普通的添加/修改 RequestName

            if (!String.IsNullOrEmpty(Request["RightGlobalDesc"]) && RequestValueList.Count > 0)
            {
                //List<string> FormList = Mvc.Util.ArrayUtility.StringSplitList<string>(Request["RequestName"]);
                //StringBuilder sb_RequestValues = new StringBuilder();

                //int RequestCounter = 0;
                //foreach (string FormItem in FormList)
                //{
                //    string RequestValue = Request[FormItem];
                //    if (!String.IsNullOrEmpty(RequestValue))
                //    {
                //        if (RequestCounter != 0)
                //        {
                //            sb_RequestValues.Append("/");
                //        }
                //        RequestCounter++;
                //        sb_RequestValues.Append(RequestValue);
                //    }
                //}
                //string RequestValues = sb_RequestValues.ToString();
                string RequestValues = String.Join("/", RequestValueList);
                if (!String.IsNullOrEmpty(Request["RightGlobalDesc"]) && !String.IsNullOrEmpty(RequestValues))
                {
                    ActionDesc = Request["RightGlobalDesc"];
                    ObjectNames = RequestValues;
                }

            }
            #endregion 普通的添加/修改

            #region 删除/带ItemName的Ajax

            if (!String.IsNullOrEmpty(Request["ItemName"]))
            {
                List<string> ItemList = ArrayUtility.StringSplitList<string>(Request["ItemName"]).Where(o => !String.IsNullOrEmpty(o)).ToList();
                StringBuilder sb_RequestValues = new StringBuilder();

                int RequestCounter = 0;
                foreach (string Item in ItemList)
                {
                    if (RequestCounter != 0)
                    {
                        sb_RequestValues.Append(",");
                    }
                    RequestCounter++;
                    sb_RequestValues.Append(Item);
                }
                string ItemNames = sb_RequestValues.ToString();
                if (!String.IsNullOrEmpty(ItemNames))
                {
                    ObjectNames = ItemNames;
                }
            }



            #endregion

            #region 开始处理


            //2013年10月17日变更：保存日志的时候不写入动作备注，显示的时候再处理
            if (!String.IsNullOrEmpty(ActionDesc) && !String.IsNullOrEmpty(ObjectNames))
            {
                templateText = templateText.Replace("{TM:RightGlobalDesc}", "{TM:RightGlobalDesc} " + ObjectNames);
                templateText = templateText.Replace("{TM:RightGlobalName}", "{TM:RightGlobalDesc} " + ObjectNames);
            }
            else
            {
                //templateText = templateText.Replace("{TM:RightGlobalDesc}", ActionDescText);
            }

            //if (!String.IsNullOrEmpty(ActionDesc) && !String.IsNullOrEmpty(ObjectNames))
            //{
            //    string ActionDescText = ActionDesc + " " + ObjectNames;
            //    if (!String.IsNullOrEmpty(templateText))
            //    {
            //        if (0 < templateText.IndexOf("{TM:RightGlobalDesc}"))
            //        {
            //            templateText = templateText.Replace("{TM:RightGlobalDesc}", ActionDescText);
            //        }
            //        else
            //        {
            //            templateText = templateText.Replace("{TM:RightGlobalName}", ActionDescText);
            //        }
            //    }
            //}
            //else if (!String.IsNullOrEmpty(ActionDesc))
            //{
            //    if (!String.IsNullOrEmpty(templateText))
            //    {
            //        if (0 < templateText.IndexOf("{TM:RightGlobalDesc}"))
            //        {
            //            templateText = templateText.Replace("{TM:RightGlobalDesc}", ActionDesc);
            //        }
            //        else
            //        {
            //            templateText = templateText.Replace("{TM:RightGlobalName}", ActionDesc);
            //        }
            //    }
            //}
            #endregion 开始处理
            return templateText;
        }

        protected bool ExistAction(string controller, string action)
        {
            return false;
        }






        public class AuthorizeLogin : AuthorizeAttribute
        {


            private WebUser _SessionUser;


            /// <summary>
            /// 使用用户管理
            /// </summary>
            public WebUser UserSessionManage
            {
                get
                {
                    if (null == _SessionUser)
                    {
                        _SessionUser = WebUser.CreateUser();
                    }
                    return _SessionUser;
                }
            }

            protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
            {
                string url = httpContext.Request.RawUrl;
                if (url.Equals("/") || url.Contains("/Login/") || url.Contains("/Partial/"))
                {
                    return true;
                }
                //if (httpContext.Session["UserInfo"] == null)
                if (UserSessionManage.CheckUserSession())
                {
                    return true;
                }
                else
                {
                    httpContext.Response.StatusCode = 404;
                    return false;
                }
            }

            public override void OnAuthorization(AuthorizationContext filterContext)
            {
                base.OnAuthorization(filterContext);
                if (filterContext.HttpContext.Response.StatusCode == 404)
                {
                    filterContext.Result = new RedirectResult("/Account/LogOn");
                }
            }
        }

    }
}
