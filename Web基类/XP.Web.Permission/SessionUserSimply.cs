using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Web.Permission
{
    public class SessionUserSimply<SessionUserSimplyT> : SessionUserBase<SessionUserSimplyT>
        where SessionUserSimplyT : class, new()
    {

        public readonly string NameOfLoginPage = "DevNeedLoginPage";
        public readonly string NameOfFirstPage = "DevUserFirstPage";

        //为了统一管理，所以把登录前的跳转页面和登录后的跳转页面都放到这里。
        /// <summary>登录页，登录前的页面</summary>
        public string LoginPage
        {
            get
            {
                return GetConfigUrl(NameOfLoginPage);
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


        protected override void _Init()
        {
            base._Init();
            this.NameOfSpecificSession = "DevelopeUser";

            UserId = 1;
            StoreId = 1;
            RoleId = 1;
            UserName = "DevelopeUser";

            IsSysStore = true;

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
            return true;
        }


        /// <summary>
        /// 用户登录之后，标记一个登录状态
        /// </summary>
        public void SetUserLogin()
        {

            SetAuthorized(true);

        }

        /// <summary>
        /// 开发人员默认不区分具体的动作授权，所以，这里直接返回true
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public override bool ExistRightByName(string controllerName, string actionName)
        {
            //return base.ExistRightByName(controllerName, actionName);
            return true;
        }





    }
}
