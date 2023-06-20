using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.DB.BLL;
using XP.DB.Models;
using XP.Web.Permission.GlobalLogin;
using XP.DB.Models.UserModel;

namespace XP.Web.Permission
{
    /// <summary>
    /// 标准的SessionUser，可以用来继承的的类（不能继承的是WebUser）
    /// </summary>
    public class SessionUserNormal<SessionUserNormalT> : SessionUserBase<SessionUserNormalT>
        where SessionUserNormalT : class, new()
    {
        private static string SessionCacheName = "UserCache";
        public readonly string NameOfLoginPage = "NeedLoginPage";
        public readonly string NameOfFirstPage = "UserFirstPage";
        public readonly string NameOfNoCurrentDataPage = "NoCurrentData";

        protected override void _Init()
        {
            base._Init();
            NameOfSpecificSession = "WebUser";

        }

        #region 接口的方法

        public override bool ExistRightByName(string controllerName, string actionName)
        {
            //return base.ExistRightByName(controllerName, actionName);

            if (!CheckLogin())
            {
                return false;
            }

            return Cache.AllRightList.Any(o => o.PageController.ToLower() == controllerName.ToLower() && o.RightOperate.ToLower() == actionName.ToLower());
        }
        public override bool ExistRightByName(string areaName, string controllerName, string actionName)
        {
            //return base.ExistRightByName(controllerName, actionName);

            if (!CheckLogin())
            {
                return false;
            }

            return Cache.AllRightList.Any(o => o.PageArea.ToLower() == areaName.ToLower() && o.PageController.ToLower() == controllerName.ToLower() && o.RightOperate.ToLower() == actionName.ToLower());
        }




        /// <summary>
        /// 覆写CheckLogin，这里做更复杂的检查
        /// </summary>
        /// <returns></returns>
        public override bool CheckLogin()
        {
            //bool BaseChecked = base.CheckLogin();
            //if (!BaseChecked)
            //    return false;

            return CheckUserSession();
        }

        /// <summary>检查用户的登录状态</summary>
        /// <returns></returns>
        public bool CheckUserSession()
        {
            if (null == System.Web.HttpContext.Current.Session[NameOfSpecificSession])
            {
                return false;
            }
            if ((bool)System.Web.HttpContext.Current.Session[NameOfSpecificSession])
            {
                return !UsersDict4Redis.HasUserExpires(System.Web.HttpContext.Current.Session.SessionID);
            }
            return false;
        }

        #endregion

        #region 用户缓存

        //private UserDataCache mCache = new UserDataCache();
        /// <summary>用户的缓存信息（需要在登录后初始化）</summary>
        public UserDataItem4Session Cache
        {
            get
            {
                if (null == System.Web.HttpContext.Current.Session[SessionCacheName])
                    return null;
                RefrushCacheExpirse();
                return System.Web.HttpContext.Current.Session[SessionCacheName] as UserDataItem4Session;
            }
            set
            {
                System.Web.HttpContext.Current.Session[SessionCacheName] = value;
            }

        }

        /// <summary>
        /// 刷新缓存过期时间
        /// </summary>
        private void RefrushCacheExpirse()
        {

            //TODOYU: 这里的逻辑需要好好处理一下。
            string SessionId = System.Web.HttpContext.Current.Session.SessionID;
            if (!UsersDict4Redis.HasCacheExpires(SessionId))
            {
                if (UsersDict4Redis.ExistSessionID(SessionId))
                {
                    AppUserinfo user = UsersDict4Redis.Dict[SessionId];
                    //5分钟之内过期，那么就刷新
                    if (DateTime.Compare(user.CacheExpiresTime, DateTime.Now.AddMinutes(5)) <= 0)   //if (user.CacheExpiresTime - DateTime.Now >0)
                    {
                        UsersDict4Redis.UpdateCacheExpires(SessionId);
                        InitCache();
                    }
                }
            }
        }

        private void InitCache()
        {
            if (!CheckLogin())
            {
                return;
            }
            InitCache(Cache.UserInfo.Id);
        }

        //private void InitCache(string username)
        //{
        //    if (!CheckLogin())
        //    {
        //        return;
        //    }

        //    //ISysService SysService = ControllerUtility.GetSysService();
        //    View_UserInfo user = GetSysService().GetUserInfoByName(username);
        //    InitCache(user);
        //}
        /// <summary>
        /// 根据用户ID初始化缓存（重新加载用的）
        /// </summary>
        /// <param name="userid"></param>
        private void InitCache(int userid)
        {
            if (!CheckLogin())
            {
                return;
            }

            //ISysService SysService = ControllerUtility.GetSysService();
            var bll = new UserIntegrationBLL();
            User_LoginInfo_ViewModel user = bll.GetUserInfoRightsById(userid);
            InitCache(user);

        }

        private void InitCache(User_LoginInfo_ViewModel user)
        {
            if (!CheckLogin())
            {
                return;
            }
            Cache.UserInfo = user;


            //LanguageMark = "zh";
            int userid = user.Id;
            int roleid = user.RoleId.Value;//暂时使用1来保证程序运行
            int StoreId = user.StoreId;

            //this.StoreId = user.StoreId;
            //this.RoleId = user.RoleId.Value;
            //this.UserId = user.Id;
            //获取角色信息，并且将角色名称存储到userinfo当中。
            //View_RolesGlobal view_Roles = SysService.GetRoleByRoleID(roleid, LanguageMark);
            //if (null != view_Roles && !String.IsNullOrEmpty(view_Roles.RoleGlobalName))
            //{
            //    user.RoleName = view_Roles.RoleGlobalName;//RoleName原本是存储备注信息，这里用国际化之后的名称替代
            //}
            Cache.UserInfo = user;//用户信息
            UserName = user.UserName;
            user.SetSysStore(Store_InfoBLL.Self.CheckSysStore(user.StoreId));

            //IsSysStore = Store_InfoBLL.Self.CheckSysStore(this.StoreId);
            //系统商户直接使用全部系统。

            //Cache.AllRightList = SysService.GetRightByUserID(userid, cookieString, user.StoreID.Value);
            ////根据用户权限，再次筛选权限 
            //IEnumerable<View_Page> RolesPage = SysService.GetPageByRoleID(roleid, cookieString);
            //IEnumerable<View_Page> UserPage;
            //UserPage = from u in RolesPage
            //           from o in Cache.AllRightList
            //           where u.PageID == o.PageID
            //           select u;

            //Cache.AllPageList = UserPage.Distinct().ToList();

            InitRightsAndPages();
            SetMenu();
        }




        #endregion


#region  登录用户的基本信息

        public override int? UserId
        {
            get
            {
                if (null != Cache)
                {
                    return Cache.UserInfo.Id;
                }
                return null;
            }
            set { }
        }
        public override int? RoleId
        {
            get
            {
                if (null != Cache)
                {
                    return Cache.UserInfo.RoleId;
                }
                return null;
            }
            set { }
        }
        public override int? StoreId
        {
            get
            {
                if (null != Cache)
                {
                    return Cache.UserInfo.StoreId;
                }
                return null;
            }
            set { }
        }



        public override string UserName
        {
            get
            {
                if (null != Cache)
                {
                    return Cache.UserInfo.UserName;
                }
                return null;
            }
            set { }
        }


        public override bool IsSysStore
        {
            get
            {
                if (null != Cache)
                {
                    return Cache.UserInfo.IsSysStore;
                }
                return false;
            }
            set { }
        }



        #endregion

        #region 设置菜单和动作
        private void InitRightsAndPages()
        {

            List<int> RightIdList = Cache.UserInfo.Rights.Select(o => o.Id).ToList();
            var svc = new DB.BLL.SysIntegrationBLL();

            List<Sys_RightV> SysRights = svc.GetALLRightViews();
            //var MenuIndex = SysRights.Where(o => o.PageController == "MenusManage" && o.RightOperate=="Index").FirstOrDefault();
            //x.Say("菜单管理的显示控制：" + MenuIndex.RightShow);
            Cache.AllRightList = SysRights.Where(o => RightIdList.Contains(o.Id) && o.RightShow == true).ToList();
            List<Sys_PageT> SysPage = svc.GetALLPageViewss();

            var p = from u in SysPage
                    from m in Cache.AllRightList
                    where u.Id == m.PageId && u.PageShow == true
                    select u;
            Cache.AllPageList = p.Distinct().ToList();


        }


        private void SetMenu()
        {

            Cache.TopMenu = Cache.AllPageList.Where(o => o.PageParentId.Value == 0).ToList();
            //var HasFind = Cache.AllPageList.Exists(o => o.PageController == "MenusManage");
            //x.Say("存在菜单管理：" + HasFind);
            //Cache.LeftMenu = new LeftMenuCollection();
            if (0 != Cache.TopMenu.Count)
            {
                foreach (Sys_PageT topMenuItem in Cache.TopMenu)
                {
                    IEnumerable<Sys_PageT> LeftMenu = from u in Cache.AllPageList
                                                      where u.PageParentId.Value == topMenuItem.Id
                                                      select u;
                    List<Sys_PageT> list4LeftMenu = LeftMenu.ToList();
                    foreach (Sys_PageT leftItem in list4LeftMenu)
                    {
                        int pageid = leftItem.Id;
                        //leftItem.MenuDeptId = Cache.AllPageList.Where(o => o.PageParentId == leftItem.Id).Count();
                        List<Sys_RightV> list;
                        list = Cache.AllRightList.Where(m => m.PageId == pageid).ToList();
                        if (0 != list.Count)
                        {
                            Cache.PageRight.Add(pageid, list);
                        }
                    }

                    Cache.LeftMenu.Add(topMenuItem.Id, list4LeftMenu);

                }
            }

        }


        #endregion


        #region 登录和刷新状态


        public void Login(User_LoginInfo_ViewModel user)
        {
            SetAuthorized(true);


            AppUserinfo appuser = new AppUserinfo()
            {
                SessionId = System.Web.HttpContext.Current.Session.SessionID,
                UserId = user.Id,
                UserName = user.UserName,
                LoginTime = DateTime.Now,
                SessionExpiresTime = DateTime.Now.AddMinutes(10d),
                CacheExpiresTime = DateTime.Now.AddMinutes(10d),
                StoreId = user.StoreId,
                RoleId = user.RoleId.Value
            };
            UsersDict4Redis.AddUser(appuser);
             
            Cache = new UserDataItem4Session();


            InitCache(user);


        }

        /// <summary>
        /// 注销用户，前台web用户增加了全局在线状态的处理
        /// </summary>
        public override void Logout()
        {
            base.Logout();
            UsersDict4Redis.ExpiresSession(System.Web.HttpContext.Current.Session.SessionID);
        }
        public void RefrushLanguage()
        {
            if (!CheckLogin())
            {
                return;
            }

            InitRightsAndPages();
            SetMenu();


        }






        #endregion


        #region 提供一些基础的方法（静态方法）

        //public static SA.Web.DataBuilder.SysService GetSysService()
        //{
        //    return new SA.Web.DataBuilder.SysService();
        //}


        //为了统一管理，所以把登录前的跳转页面和登录后的跳转页面都放到这里。
        /// <summary>登录页，登录前的页面</summary>
        public string LoginPage
        {
            get
            {
                return GetConfigUrl(NameOfLoginPage);
            }
        }
        public string NoCurrentDataPage
        {
            get
            {
                return GetConfigUrl(NameOfNoCurrentDataPage);
            }
        }
        /// <summary>登录之后跳转的地址</summary>
        public string UserFirstPage
        {
            get
            {
                return GetConfigUrl(NameOfFirstPage);
            }
        }




        #endregion
    }
}
