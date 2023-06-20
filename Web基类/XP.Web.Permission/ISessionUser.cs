using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Web.Permission
{
    public interface ISessionUser
    {

        bool CheckLogin();

        bool ExistRightByName(string controllerName, string actionName);
        bool ExistRightByName(string areaName,string controllerName, string actionName);

        string UserName { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        int? RoleId { get; set; }
        /// <summary>
        /// 商户Id
        /// </summary>
        int? StoreId { get; set; }
        int? UserId { get; set; }
        /// <summary>
        /// 是否为系统商户
        /// </summary>
        /// <remarks>
        /// 由Store_InfoBLL.Self.CheckSysStore()提供
        /// </remarks>
        bool IsSysStore { get; set; }
    }
}
