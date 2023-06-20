using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Web.Permission.GlobalLogin
{
    /// <summary>
    /// 保存在全局字典里的用户信息
    /// </summary>
    public class AppUserinfo
    {
        /// <summary>
        /// Session对象的SessionId，也会是字典的Key
        /// 将来需要同时包含AppName和SessionId
        /// </summary>
        public string SessionId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }
        /// <summary>
        /// Session到期时间，踢人下线用的
        /// </summary>
        public DateTime SessionExpiresTime { get; set; }
        /// <summary>
        /// 缓存到期时间，用户更新缓存的标记
        /// </summary>
        /// <remarks>
        /// 默认时间：登录时间+30分钟
        /// 过期：设置这个时间为当前时间-1d(一天）
        /// 系统资料修改（菜单、动作），设置它过期
        /// </remarks>
        public DateTime CacheExpiresTime { get; set; }
        /// <summary>
        /// 商户ID
        /// </summary>
        public int StoreId { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }

    }
}
