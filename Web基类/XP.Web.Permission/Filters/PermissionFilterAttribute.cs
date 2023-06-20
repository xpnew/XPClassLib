using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using XP.Comm.Msgs;

namespace XP.Web.Permission
{
    /// <summary>
    /// 权限拦截
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class PermissionFilterAttribute : ActionFilterAttribute
    {
        private string _NeedLoginUrl;

        public string NeedLoginUrl
        {
            get { return _NeedLoginUrl; }
            set { _NeedLoginUrl = value; }
        }

        public string TimeoutUrl { get; set; }
        private string NeedPremissingUrl;

        private string RedirectUrl;


        protected string LoginUrl { get; set; }


        public string UserInstanceConfig { get; set; }

        public PermissionFilterAttribute()
        {
            this.UserInstanceConfig = "PermissionUserInstace";
            NeedLoginUrl = "/Login/";
            TimeoutUrl = "/Login/Timeout/";

        }


        /// <summary>
        /// 权限拦截
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //权限拦截是否忽略
            bool IsIgnored = false;
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            //NeedLoginUrl = SA.WebUtil.Util.ConfigManager.GetSiteSetting("NeedLoginPage");
            NeedPremissingUrl = XP.Util.Config.ConfigReader.Self.GetSet("NeedPremissingPage");
            //默认情况下跳转到权限页面，如果登录检查失败跳转到登录页面
            RedirectUrl = NeedPremissingUrl;

            var path = filterContext.HttpContext.Request.Path.ToLower();


            //验证当前Action是否是匿名访问Action
            if (CheckAnonymous(filterContext))
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            //登录检查独立出来
            if (!CheckLogin(filterContext))
            {

                if (CheckAjaxView(filterContext))
                {

                    var res = new JsonResult();
                    res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    MsgResult result = new MsgResult()
                    {
                        Name = "JsonMsg",
                        Title= "登录超时或者没有登录。",
                        Body = NeedLoginUrl,
                        StatusCode = -60103251//默认是0
                    };
                    res.Data = result;

                    filterContext.Result = res;//
                    return;

                }

                //登录检查失败，要重新指定跳转页面
                RedirectUrl = NeedLoginUrl;
                if (null != filterContext.RouteData.Values["controller"])
                {
                    string controller = filterContext.RouteData.Values["controller"].ToString().ToLower();
                    string action = filterContext.RouteData.Values["action"].ToString().ToLower();
                    if ("pagecomm" == controller || "devcomm" == controller && "index" == action)
                    {
                        //RedirectUrl = "/Login/";
                    }
                    else
                    {
                        RedirectUrl = TimeoutUrl;// "/Login/Timeout/"
                    }
                }
                filterContext.Result = new RedirectResult(RedirectUrl);
                base.OnActionExecuting(filterContext);
                return;
            }


            if (this.AuthorizeCore(filterContext) == false)//根据验证判断进行处理
            {
                if (CheckAjaxView(filterContext))
                {

                    var res = new JsonResult();
                    res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    MsgResult result = new MsgResult()
                    {
                        Name = "JsonMsg",
                        Body = "没有权限。",
                        StatusCode = -60200591//默认是0
                    };
                    res.Data = result;

                    filterContext.Result = res;//
                    return;

                }
                RedirectUrl = NeedPremissingUrl;

                //
                //原先的流程是：没有权限，跳转的登录页面（~/Login/Index）
                //现在的流程是：没人权限，跳转到没有权限的页面。
                //不要使用Response.Redirect()据说会照样执行后面的语句，造成性能上的损失。
                //filterContext.RequestContext.HttpContext.Response.Redirect(NeedPremissingUrl);
                //filterContext.RequestContext.HttpContext.Response.End();
                filterContext.Result = new RedirectResult(RedirectUrl);
                base.OnActionExecuting(filterContext);
                return;
            }
        }
        /// <summary>
        /// [Anonymous标记]验证是否匿名访问
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        public bool CheckAnonymous(ActionExecutingContext filterContext)
        {
            //验证是否是匿名访问的Action
            object[] attrsAnonymous = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AnonymousAttribute), true);
            //是否是Anonymous
            var Anonymous = attrsAnonymous.Length == 1;
            return Anonymous;
        }

        /// <summary>
        /// [LoginAllowView标记]验证是否登录就可以访问(如果已经登陆,那么不对于标识了LoginAllowView的方法就不需要验证了)
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        public bool CheckLoginAllowView(ActionExecutingContext filterContext)
        {
            //在这里允许一种情况,如果已经登陆,那么不对于标识了LoginAllowView的方法就不需要验证了
            object[] attrs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(LoginAllowViewAttribute), true);
            //是否是LoginAllowView
            var ViewMethod = attrs.Length == 1;
            return ViewMethod;
        }


        /// <summary>
        /// 检查是否为AJAX动作
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        public bool CheckAjaxView(ActionExecutingContext filterContext)
        {
            //在这里允许一种情况,如果已经登陆,那么不对于标识了LoginAllowView的方法就不需要验证了
            object[] attrs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(JsonActionAttribute), true);
            //是否是LoginAllowView
            var ViewMethod = attrs.Length == 1;
            return ViewMethod;
        }



        protected virtual bool CheckLogin(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            //验证当前Action是否是匿名访问Action
            if (CheckAnonymous(filterContext))
                return true;

            SessionUserFactory Factory = new SessionUserFactory();
            ISessionUser User = SessionUserFactory.CreateUserByConfig(UserInstanceConfig);

            //用户登录检查
            if (!User.CheckLogin())
            {
                return false;
            }
            //已经添加了默认数据
            filterContext.Controller.ViewData["CurrentStoreId"] = User.StoreId;
            filterContext.Controller.ViewData["CurrentRoleId"] = User.RoleId;
            filterContext.Controller.ViewData["CurrentUserId"] = User.UserId;
            filterContext.Controller.ViewData["CurrentUserName"] = User.UserName;
            filterContext.Controller.ViewData["IsSysStore"] = User.IsSysStore;
            return true;
        }


        /// <summary>
        /// //权限判断业务逻辑
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="isViewPage">是否是页面</param>
        /// <returns></returns>
        protected virtual bool AuthorizeCore(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            //验证当前Action是否是登录即可访问
            if (CheckLoginAllowView(filterContext))
                return true;
            //调用工厂的方式创建用户对象
            SessionUserFactory Factory = new SessionUserFactory();
            ISessionUser User = SessionUserFactory.CreateUserByConfig(UserInstanceConfig);


            if ("admin" == User.UserName)
            {
                //SavePageData(filterContext, User);
                return true;
            }

            if (null != filterContext.RouteData.Values["controller"])
            {
                ////小写的控制器名称
                //string ControllerActionName2Lower = filterContext.RouteData.Values["controller"].ToString().ToLower();
                ////小写的动作名称
                //string ActionName2Lower = filterContext.RouteData.Values["action"].ToString().ToLower();
                var MvcNames = Web.ControllerBase.MvcUtil.BuildMvcName(filterContext.RouteData);
                string ActionName2Lower = MvcNames.ActionName2Lower;
                //获取依赖名称，通过依赖名称，可以将多个动作的权限指定到一起。
                string DependName = GetDependName(filterContext);
                if (!String.IsNullOrEmpty(DependName))
                {
                    ActionName2Lower = DependName;
                }


                ActionName2Lower = GetAliasName(ActionName2Lower);
                //ISessionUser.ExistRightByName检查用户是否具备权限
                ActionName2Lower = ActionName2Lower.ToLower();

                if (User.ExistRightByName(MvcNames.AreaName2Lower, MvcNames.ControllerName2Lower, ActionName2Lower))
                {
                    //SavePageData(filterContext, User);
                    return true;

                }

                #region 2014年3月9日，直接调用 ISessionUser.ExistRightByName方法检查所以下面的代码停用了。

                //虽然下面的另一个Linq可以直接判断出来，但是这里还是要作出判断，
                //以避免受到他的影响。
                //bool HasController = from p in UserSessionManage.Cache.LeftMenu.Items
                //if (UserSessionManage.Cache.LeftMenu.ExistPageController(ControllerActionName2Lower))
                //{

                //    if (UserSessionManage.Cache.PageRight.EixstPageAction(ControllerActionName2Lower, ActionName2Lower))
                //    {
                //        return true;
                //    }

                #region 2012年12月26日，改为交给Cache类检查页面权限，所以下面的代码停用了。
                /*

                if (null == UserSessionManage.Cache.PageRight[controller])
                {
                    return false;
                }

#if DEBUG
                //用在开发的时候，跟踪数据
                var list = from p in UserSessionManage.Cache.PageRight[controller]
                           select p;
#endif

                bool HasRight = (from u in UserSessionManage.Cache.PageRight[controller]
                                 where u.RightOperate.ToLower().Equals(action)
                                 select u).Any();

                return HasRight;
                 * */
                #endregion
                //}
                #endregion
            }


            return false;
        }

        //protected void SavePageData(ActionExecutingContext filterContext, ISessionUser user)
        //{


        //}

        protected string GetDependName(ActionExecutingContext filterContext)
        {
            //在这里允许一种情况,如果已经登陆,那么不对于标识了LoginAllowView的方法就不需要验证了
            object[] attrs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(ActionDependFilterAttribute), true);
            //是否是LoginAllowView
            var ViewMethod = attrs.Length == 1;
            if (attrs.Length > 0)
            {

                ActionDependFilterAttribute ad = attrs[0] as ActionDependFilterAttribute;

                return ad.ParentActionName;
            }
            return String.Empty;

        }
        public string GetAliasName(string currentName)
        {
            if ("Query".ToLower() == currentName)
            {
                return "index";
            }
            return currentName;
        }
    }
}
