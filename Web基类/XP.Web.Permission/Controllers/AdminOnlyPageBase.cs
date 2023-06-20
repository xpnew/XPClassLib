using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace XP.Web.Permission.Controllers
{
    [AdminOnlyFilter]
    public class AdminOnlyPageBase: BMPageBase
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


        #region 通用的一些功能



        protected int? CurrentStoreId
        {
            get
            {


                if (!UserSessionManage.CheckUserSession())
                {
                    return null;
                }
                return UserSessionManage.Cache.UserInfo.StoreId;
            }
        }

        public ActionResult RedirectNoCurrentData()
        {
            return new RedirectResult(UserSessionManage.NoCurrentDataPage);
        }

        #endregion

        #region 查询条件

        protected override int GetStoreId()
        {
            return UserSessionManage.Cache.UserInfo.StoreId;
        }

        protected override bool CheckSysStore()
        {
            return UserSessionManage.IsSysStore;
        }

        protected override List<int> GetSelfStoreIdList()
        {
            var bll = new DB.BLL.Store_InfoBLL();
            var list = bll.GetAll().Select(s=>s.Id).ToList();
            return list;

        }

        protected override string GetStatisticStoreNames()
        {
            return "全部商户";
        }

        #endregion
    }
}
