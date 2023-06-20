using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Web.Permission.Controllers
{
    /// <summary>
    /// 技术管理员页面基类，通过DevPermissionFilter过滤器实现权限的控制
    /// </summary>
    [DevPermissionFilter]
    public class DevUserPageBase:BMPageBase
    {

        private DevUser _SessionUser;


        /// <summary>
        /// 使用用户管理
        /// </summary>
        public DevUser UserSessionManage
        {
            get
            {
                if (null == _SessionUser)
                {
                    _SessionUser = DevUser.CreateUser();
                }
                return _SessionUser;
            }
        }


        #region  通用的功能




        protected override int GetStoreId()
        {
            //return base.GetStoreId();
            return 1;
        }

        protected override bool CheckSysStore()
        {
            return UserSessionManage.IsSysStore;
        }
        protected override List<int> GetSelfStoreIdList()
        {
            var bll = new DB.BLL.Store_InfoBLL();
            var list = bll.GetAll().Select(s => s.Id).ToList();
            return list;
        }
        protected override string GetStatisticStoreNames()
        {
            return "全部商户";
        }
        #endregion


    }
}
