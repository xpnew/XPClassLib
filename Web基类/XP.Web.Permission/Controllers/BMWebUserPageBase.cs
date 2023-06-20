using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using XP.DB.BLL;
using XP.DB.Models;

namespace XP.Web.Permission.Controllers
{
    /// <summary>
    /// 这是用户控制器的基类,使用PermissionFilter过滤器，实现了权限动作的精确控制
    /// </summary>
    [PermissionFilter]
    [ActionLogFilter]

    public class BMWebUserPageBase: BMPageBase
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

        protected override bool CheckSysStore()
        {
            return UserSessionManage.IsSysStore;
        }

        public ActionResult RedirectNoCurrentData()
        {
            return new RedirectResult(UserSessionManage.NoCurrentDataPage);
        }


        public string Post(string url, string json)
        {

            var StoreInfo = Store_InfoBLL.Instance.GetStoreById(CurrentStoreId.Value);

            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/json";   //application/octet-stream
            req.Headers.Set("MerchantID", StoreInfo.StoreCode);
            req.Headers.Set("MerchantPassword", StoreInfo.PassWord);

            #region 添加Post 参数  
            byte[] data = Encoding.UTF8.GetBytes(json);
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容  
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

        #endregion
        #region 查询条件

        protected override int GetStoreId()
        {
            return UserSessionManage.Cache.UserInfo.StoreId;
        }

        protected string CurrentStoreCode
        {
            get
            {
                if (!UserSessionManage.CheckUserSession())
                {
                    return String.Empty;
                }
                return UserSessionManage.Cache.UserInfo.StoreCode;
            }
        }

        #endregion

        #region 路由和视图管理

        protected Sys_RightV GetActionRight()
        {
            return UserSessionManage.Cache.PageRight.GetRight(ControllerName, ActionName);
        }

        /// <summary>页面权限列表</summary>
        public List<Sys_RightV> PageRightList
        {
            get
            {
                List<Sys_RightV> ListAll = UserSessionManage.Cache.PageRight[ControllerName];
                if (null == ListAll)
                    return new List<Sys_RightV>();
                var list = from r in UserSessionManage.Cache.PageRight[ControllerName]
                           where r.RightShow == true
                           select r;
                if (null == list)
                    return new List<Sys_RightV>();
                return list.ToList();
            }
        }
        /// <summary>页面权限按钮列表</summary>
        public List<Sys_RightV> PageRightButtonList
        {
            get
            {
                if (null == PageRightList)
                    return null;
                return PageRightList.Where(o => o.RightIsButton == true).ToList();
            }
        }




        protected override List<int> GetSelfStoreIdList()
        {
            var bll = new DB.BLL.Store_InfoBLL();
            var list = bll.GetSelfIdList(CurrentStoreId.Value);
            return list;
        }

        protected override string GetStatisticStoreNames()
        {
            string Result = "";
            if (!CurrentStoreId.HasValue)
            {
                return Result;
            }
            var bll = new DB.BLL.Store_InfoBLL();
            var list = bll.GetSelfList(CurrentStoreId.Value);
            var names = list.Select(s =>  s.StoreName + "[" + s.StoreTypeName + "]" ).ToList();

            Result = String.Join("|", names);
            return Result;
        }

        public List<Sys_RightV> GetPageRights(int pageid)
        {
            List<Sys_RightV> list = UserSessionManage.Cache.PageRight[pageid];
            return list;
        }
        #endregion

    }
}
