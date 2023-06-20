using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.DB.Models;
using XP.DB.Models.UserModel;

namespace XP.Web.Permission
{
    /// <summary>
    /// 用户数据缓存类（session级别）
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    [Serializable]
    public class UserDataItem4Session
    {

        /// <summary>保存在session当中的顶部菜单</summary>
        public List<Sys_PageT> TopMenu;

        /// <summary>保存在Session当中的用户信息</summary>
        public User_LoginInfo_ViewModel UserInfo { get; set; }


        /// <summary>左边的菜单集合。</summary>
        public LeftMenuCollection LeftMenu { get; set; }
        /// <summary>页面的权限集合</summary>
        public PageRightCollection PageRight { get; set; }
        /// <summary>直接从数据库当中获取所有的页面列表</summary>
        public List<Sys_PageT> AllPageList { get; set; }

        /// <summary>直接从数据库当中获取的全部权限列表</summary>
        public List<Sys_RightV> AllRightList { get; set; }

        //用得太少，并且为了提高登录的性能，已经废弃了。。。。
        /// <summary>当前用户所在商户的全部下属商户列表</summary>
        public List<Store_InfoV> SubStoreList { get; set; }

        public UserDataItem4Session()
        {
            LeftMenu = new LeftMenuCollection();
            LeftMenu.ParentCache = this;
            PageRight = new PageRightCollection();
            PageRight.ParentCache = this;

            AllPageList = new List<Sys_PageT>();
            AllRightList = new List<Sys_RightV>();


        }

    }
}
