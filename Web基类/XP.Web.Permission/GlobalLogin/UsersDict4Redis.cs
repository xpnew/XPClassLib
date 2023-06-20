using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Util.Config;

namespace XP.Web.Permission.GlobalLogin
{

    /// <summary>
    /// 通过Redis实现的用户字典（以前的版本UsersDict是通过Application实现的） 
    /// </summary>
    public static class UsersDict4Redis
    {
        public static RedisProvider4Dict Dict = RedisProvider4Dict.Self;

        public static void AddUser(AppUserinfo user)
        {
            user.CacheExpiresTime = DateTime.Now.AddMinutes(Dict.ExpiresTimeMin);
            user.SessionExpiresTime = DateTime.Now.AddMinutes(Dict.ExpiresTimeMin);
            Dict.InsertUser(user.SessionId,user);
        }
        /// <summary>设置一个Session过期</summary>
        /// <param name="userSessionId"></param>
        /// <returns></returns>
        public static bool ExistSessionID(string userSessionId)
        {
            return Dict.Exist(userSessionId);
        }




        #region 直接移除

        /// <summary>批量移除用户（不设置session到期） </summary>
        /// <param name="idlist"></param>
        public static void BatchRemoveUsers(List<int> idlist)
        {
            var alluser = Dict.GetAll();
            var list = alluser.Where(a => idlist.Contains(a.UserId));

            foreach (var user in list)
            {
                Dict.Remove(user.SessionId);
            }

        }

        /// <summary>批量移除角色（不设置session到期） </summary>
        /// <param name="idlist"></param>
        public static void BatchRemoveRoles(List<int> idlist)
        {
            var alluser = Dict.GetAll();
            var list = alluser.Where(a => idlist.Contains(a.RoleId));

            foreach (var user in list)
            {
                Dict.Remove(user.SessionId);
            }

        }


        #endregion


        #region  缓存更新和到期


        /// <summary>
        /// 检查缓存到期
        /// </summary>
        /// <param name="userSessionId"></param>
        /// <returns></returns>
        public static bool HasCacheExpires(string userSessionId)
        {
            if (Dict.Exist(userSessionId))
            {
                AppUserinfo user = Dict[userSessionId];
                if(null == user)
                {
                    return true;
                }
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

        #region  缓存更新

        /// <summary>
        /// 更新缓存到期
        /// </summary>
        /// <param name="userSessionId"></param>
        public static void UpdateCacheExpires(string userSessionId)
        {
            if (Dict.Exist(userSessionId))
            {
                AppUserinfo user = Dict[userSessionId];
                UpdateCacheExpires(user);
            }
        }
        public static void UpdateCacheExpires(AppUserinfo user)
        {
            user.CacheExpiresTime = DateTime.Now.AddMinutes(Dict.ExpiresTimeMin);
            Dict.SaveUser(user);
        }


        #endregion

        #region  缓存到期


        /// <summary>设置缓存到期（需要更新缓存）</summary>
        /// <param name="userSessionId"></param>
        public static void ExpiresCache(string userSessionId)
        {
            if (Dict.Exist(userSessionId))
            {
                AppUserinfo user = Dict[userSessionId];
                ExpiresCache(user);
            }
        }

        public static void ExpiresCache(AppUserinfo user)
        {
            user.CacheExpiresTime = DateTime.Now.AddDays(-1);
            Dict.SaveUser(user);
        }
        /// <summary>
        /// 设置所有的用户缓存到期，这样的话就会强制所有的用户都要更新缓存
        /// </summary>
        public static void ExpiresAll()
        {
            foreach (var user in Dict.GetAll())
            {
                ExpiresCache(user);
            }
        }
        /// <summary>批量设置用户缓存到期 </summary>
        /// <param name="idlist"></param>
        public static void ExpiresUsersCache(List<int> idlist)
        {
            var alluser = Dict.GetAll();
            var list = alluser.Where(a => idlist.Contains(a.UserId));

            foreach (var user in list)
            {
                ExpiresCache(user);
            }
        }

        /// <summary>批量设置商户用户缓存到期 </summary>
        /// <param name="idlist"></param>
        public static void ExpireStoresCache(List<int> idlist)
        {
            var alluser = Dict.GetAll();
            var list = alluser.Where(a => idlist.Contains(a.StoreId));
            foreach (var user in list)
            {
                ExpiresCache(user);
            }
        }
        /// <summary>批量设置角色用户缓存到期 </summary>
        /// <param name="idlist"></param>
        public static void ExpireRolesCache(List<int> idlist)
        {
            var alluser = Dict.GetAll();
            var list = alluser.Where(a => idlist.Contains(a.RoleId));
            foreach (var user in list)
            {
                ExpiresCache(user);
            }
        }


        #endregion



        #endregion

        #region Session更新和到期


        /// <summary>
        /// 检查用户在线到期
        /// </summary>
        /// <param name="userSessionId"></param>
        /// <returns></returns>
        public static bool HasUserExpires(string userSessionId)
        {
            if (Dict.Exist(userSessionId))
            {
                AppUserinfo user = Dict[userSessionId];
                if (DateTime.Compare(user.SessionExpiresTime, DateTime.Now) > 0)   //if (user.CacheExpiresTime - DateTime.Now >0)
                {
                    UpdateSessionExpires(userSessionId);
                }
                else
                {
                    Dict.Remove(userSessionId);
                    return true;
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        #region Session更新

        /// 更新缓存到期
        /// </summary>
        /// <param name="userSessionId"></param>
        public static void UpdateSessionExpires(string userSessionId)
        {
            if (Dict.Exist(userSessionId))
            {
                AppUserinfo user = Dict[userSessionId];
                UpdateSessionExpires(user);
            }
        }
        public static void UpdateSessionExpires(AppUserinfo user)
        {
            user.SessionExpiresTime = DateTime.Now.AddMinutes(Dict.ExpiresTimeMin);
            Dict.SaveUser(user);
        }


        #endregion
        #region Session到期


        /// <summary>设置用户（Session）到期，需要重新登录</summary>
        /// <param name="userSessionId"></param>
        public static void ExpiresSession(string userSessionId)
        {
            if (Dict.Exist(userSessionId))
            {
                AppUserinfo user = Dict[userSessionId];
                ExpiresSession(user);
            }
        }

        public static void ExpiresSession(AppUserinfo user)
        {
            user.SessionExpiresTime = DateTime.Now.AddDays(-1);
            Dict.SaveUser(user);
        }

        #endregion
        #endregion







    }
}
