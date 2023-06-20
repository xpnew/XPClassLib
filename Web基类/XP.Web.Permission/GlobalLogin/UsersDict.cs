using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace XP.Web.Permission.GlobalLogin
{

    /// <summary>
    /// 在线用户字典，通过Application实现的。
    /// </summary>
    public static class UsersDict
    {
        private static string NameOfCache = "GlobalLogin_UsersDict";
        public static HttpApplicationState App = HttpContext.Current.Application;
        private static Dictionary<string, AppUserinfo> _AdminArray;

        /// <summary>保存在线用户的数组</summary>
        /// <remarks>
        /// 当前key使用的是SessionID,
        /// 将来要实现多站点管理的时候,需要使用AppName + SessionID组合的方式传送参数
        /// 
        /// </remarks>
        public static Dictionary<string, AppUserinfo> AdminArray
        {
            get
            {
                //HttpApplicationState app = HttpContext.Current.Application;
                if (null == App[NameOfCache])
                {
                    _AdminArray = new Dictionary<string, AppUserinfo>();
                    //app.Lock();
                    App[NameOfCache] = _AdminArray;
                    //app.UnLock();
                }
                else
                {
                    //object app = App[NameOfCache];
                    //_AdminArray = (Dictionary<string, AppUserinfo>)App[NameOfCache];
                }
                return (Dictionary<string, AppUserinfo>)App[NameOfCache];
            }
            set
            {
                App[NameOfCache] = value;
            }
        }

        public static void AddUser(AppUserinfo user)
        {
            string userSessionId = user.SessionId;
            if (AdminArray.ContainsKey(userSessionId))
            {
                AdminArray[userSessionId] = user;
            }
            else
            {
                AdminArray.Add(userSessionId, user);
            }
        }
        /// <summary>设置一个Session过期</summary>
        /// <param name="userSessionId"></param>
        /// <returns></returns>
        public static bool ExistSessionID(string userSessionId)
        {
            return AdminArray.ContainsKey(userSessionId);
        }

        /// <summary>批量移除用户（不设置session到期） </summary>
        /// <param name="idlist"></param>
        public static void BatchRemoveUsers(List<int> idlist)
        {
            var list = from o in AdminArray.Values
                       where idlist.Contains(o.UserId)
                       select o.SessionId;

            //转换成List，要不然会出现“集合已修改;可能无法执行枚举操作”
            List<string> SessionList = list.ToList();

            foreach (var sessionid in SessionList)
            {
                AdminArray.Remove(sessionid);
            }

        }
        /// <summary>批量移除角色（不设置session到期） </summary>
        /// <param name="idlist"></param>
        public static void BatchRemoveRoles(List<int> idlist)
        {
            var list = from o in AdminArray.Values
                       where idlist.Contains(o.RoleId)
                       select o.SessionId;

            //转换成List，要不然会出现“集合已修改;可能无法执行枚举操作”
            List<string> SessionList = list.ToList();

            foreach (var sessionid in SessionList)
            {
                AdminArray.Remove(sessionid);
            }

        }
        /// <summary>
        /// 设置所有的用户缓存到期，这样的话就会强制所有的用户都要更新缓存
        /// </summary>
        public static void ExpiresAll()
        {
            foreach (var user in AdminArray.Values)
            {
                user.CacheExpiresTime = DateTime.Now.AddDays(-1);
            }
        }
        /// <summary>设置缓存到期（需要更新缓存）</summary>
        /// <param name="userSessionId"></param>
        public static void ExpiresCache(string userSessionId)
        {
            if (AdminArray.ContainsKey(userSessionId))
            {
                AppUserinfo user = AdminArray[userSessionId];
                user.CacheExpiresTime = DateTime.Now.AddDays(-1);
            }
        }
        /// <summary>批量设置用户缓存到期 </summary>
        /// <param name="idlist"></param>
        public static void ExpiresUsersCache(List<int> idlist)
        {
            var list = from o in AdminArray.Values
                       where idlist.Contains(o.UserId)
                       select o;
            foreach (var user in list)
            {
                user.CacheExpiresTime = DateTime.Now.AddDays(-1);
            }

        }

        /// <summary>批量设置商户用户缓存到期 </summary>
        /// <param name="idlist"></param>
        public static void ExpireStoresCache(List<int> idlist)
        {
            var list = from o in AdminArray.Values
                       where idlist.Contains(o.StoreId)
                       select o;
            foreach (var user in list)
            {
                user.CacheExpiresTime = DateTime.Now.AddDays(-1);
            }
        }
        /// <summary>批量设置角色用户缓存到期 </summary>
        /// <param name="idlist"></param>
        public static void ExpireRolesCache(List<int> idlist)
        {
            var list = from o in AdminArray.Values
                       where idlist.Contains(o.RoleId)
                       select o;
            foreach (var user in list)
            {
                user.CacheExpiresTime = DateTime.Now.AddDays(-1);
            }
        }


        public static void UpdateCacheExpires(string userSessionId)
        {
            if (AdminArray.ContainsKey(userSessionId))
            {
                AppUserinfo user = AdminArray[userSessionId];
                user.CacheExpiresTime = DateTime.Now.AddMinutes(50d);
            }
        }
        /// <summary>设置用户（Session）到期，需要重新登录</summary>
        /// <param name="userSessionId"></param>
        public static void ExpiresSession(string userSessionId)
        {
            if (AdminArray.ContainsKey(userSessionId))
            {
                AppUserinfo user = AdminArray[userSessionId];
                user.SessionExpiresTime = DateTime.Now.AddDays(-1);
            }
        }
        //SessionID 
        /// <summary>
        /// 检查缓存到期
        /// </summary>
        /// <param name="userSessionId"></param>
        /// <returns></returns>
        public static bool HasCacheExpires(string userSessionId)
        {
            if (AdminArray.ContainsKey(userSessionId))
            {
                AppUserinfo user = AdminArray[userSessionId];
                if (DateTime.Compare(user.CacheExpiresTime, DateTime.Now) >= 0)   //if (user.CacheExpiresTime - DateTime.Now >0)
                {
                    UpdateCacheExpires(userSessionId);
                    return true;
                }
                else
                {

                }
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 检查用户在线到期
        /// </summary>
        /// <param name="userSessionId"></param>
        /// <returns></returns>
        public static bool HasUserExpires(string userSessionId)
        {
            if (AdminArray.ContainsKey(userSessionId))
            {
                AppUserinfo user = AdminArray[userSessionId];
                if (DateTime.Compare(user.SessionExpiresTime, DateTime.Now) > 0)   //if (user.CacheExpiresTime - DateTime.Now >0)
                {
                    user.SessionExpiresTime = DateTime.Now.AddMinutes(50d);
                }                                                                                                           
                else
                {
                    AdminArray.Remove(userSessionId);
                    return true;
                }
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
