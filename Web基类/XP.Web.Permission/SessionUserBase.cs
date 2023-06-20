using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Web.Permission
{
    /// <summary>
    /// SessionUser基类，主要是完成了单例模式包装
    /// </summary>
    /// <typeparam name="SessionUserT"></typeparam>
    public class SessionUserBase<SessionUserT> : ISessionUser where SessionUserT : class, new()
    {

        private string _NameOfSession = "SessionUserBase";

        /// <summary>
        /// 检查的Session名称，公共的session，
        /// </summary>
        /// <remarks>
        /// 这个session表示当前至少以其中一种方式(例如WebUser/DevUser)登录到了系统
        /// 这个session表示当前浏览器的客户端至少已经获得了其中一项的Session
        /// </remarks>
        public string NameOfCommonSession
        {
            get { return _NameOfSession; }
            set { _NameOfSession = value; }
        }

        /// <summary>
        /// 具体的Session名称，派生类之间用来实现互相区分
        /// </summary>
        /// <remarks>
        /// 这个Session是登录检查的时候真正使用的状态。
        /// 注销的时候，需要做出标记
        /// </remarks>
        /// 
        public virtual string NameOfSpecificSession { get; set; }
        public virtual string UserName { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual int? UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public virtual int? RoleId { get; set; }
        /// <summary>
        /// 商户Id
        /// </summary>
        public virtual int? StoreId { get; set; }


        public virtual bool IsSysStore { get; set; }


        /// <summary>用户登录状态</summary>
        public bool UserAuthorized
        { get { return CheckLogin(); } }


        #region 实现单例模式

        protected SessionUserBase() { _Init(); }
        public static readonly SessionUserT _Instance = new SessionUserT();


        public static SessionUserT CreateInstance()
        {
            return _Instance;
        }
        #endregion

        #region 功能区
        /// <summary>
        /// 派生类使用这个方法代替构造函数进行初始化
        /// </summary>
        /// 
        protected virtual void _Init()
        {
            IsSysStore = false;
        }



        /// <summary>返回保存在web.config的信息</summary>
        /// <param name="controller">要查询的名称</param>
        /// <returns></returns>
        public string GetConfigUrl(string key)
        {
            var cr = XP.Util.Config.ConfigReader._Instance;
            return cr.GetSet(key);
        }
        #endregion


        #region 登录注销

        /// <summary>设定用户登录状态</summary>
        /// <param name="flag"></param>
        public void SetAuthorized(bool flag)
        {
            System.Web.HttpContext.Current.Session[NameOfCommonSession] = flag;
            System.Web.HttpContext.Current.Session[NameOfSpecificSession] = flag;
        }

        /// <summary>
        /// 注销退出，前台用户还要注意处理全局在线状态管理
        /// </summary>
        /// <remarks>
        /// 注意：前台用户和技术管理员使用的Session很可能是共用的（一个浏览器线程），所以不能直接设置Session无效（即已经注释的代码当中的Abandon）
        /// </remarks>
        public virtual void Logout()
        {

            SetAuthorized(false);
            System.Web.HttpContext.Current.Session[NameOfSpecificSession] = false;
            //UsersDict.ExpiresSession(System.Web.HttpContext.Current.Session.SessionID);
            //System.Web.HttpContext.Current.Session.Abandon();

        }
        /// <summary>
        /// 检查登录状态，这里只做简单的Session检查。
        /// </summary>
        /// <returns></returns>
        public virtual bool CheckLogin()
        {
            if (null == System.Web.HttpContext.Current.Session[NameOfCommonSession])
            {
                return false;
            }
            return (bool)System.Web.HttpContext.Current.Session[NameOfCommonSession];
        }



        #endregion
        /// <summary>
        /// 根据控制器名称和动作名称检查动作授权
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public virtual bool ExistRightByName(string controllerName, string actionName)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 根据域名称、控制器名称和动作名称检查动作授权
        /// </summary>
        /// <param name="areaName"></param>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public virtual bool ExistRightByName(string areaName, string controllerName, string actionName)
        {
            throw new NotImplementedException();
        }
    }
}
