using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Util.TypeCache;

namespace XP.Util.Json.JComment
{
    /// <summary>
    /// 带有json 注释的类型缓存
    /// </summary>
    public class JCommentTypeCache : EntityTypesCache
    {

        #region 实现单例模式

        protected JCommentTypeCache() :base(){  }
        public static readonly JCommentTypeCache _Instance = new JCommentTypeCache();

        public static JCommentTypeCache Self { get { return _Instance; } }

        public static JCommentTypeCache CreateInstance()
        {
            //x.TimerLog("------------- 调用了类型缓存的单例模式");
            //if (_Instance.Items == null) {
            //    x.Say("此次调用的字典是 null");
            //}
            //else
            //{
            //    x.Say("此次调用的字典是包括这些数据：　"+ _Instance.Items.Count);
            //}
            return _Instance;
        }
        #endregion



        public override void Add(Type type)
        {
            string TypeName = type.FullName;
            JCommentTypeCacheItem NewCache = new JCommentTypeCacheItem(type);
            AddCache(TypeName, NewCache);

        }

    }
}
