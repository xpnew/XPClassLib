using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace XP.Server.IISMonitor
{
    public class MonitorManager
    {

        private HttpApplication _CurrentApp = HttpContext.Current.ApplicationInstance;

        private AppMonitorStatus _SiteAppStatus;

        public AppMonitorStatus SiteAppStauts
        {
            get
            {
                if (null == _CurrentApp.Application["SiteAppStauts"])
                {
                    _SiteAppStatus = new AppMonitorStatus();
                    _CurrentApp.Application["SiteAppStauts"] = _SiteAppStatus;
                }
                else
                {
                    _SiteAppStatus = _CurrentApp.Application["SiteAppStauts"] as AppMonitorStatus;
                }
                return _SiteAppStatus;
            }
            set
            {
                _SiteAppStatus = value;
            }
        }



        #region 实现单例模式

        protected MonitorManager() { _Init(); }
        public static readonly MonitorManager _Instance = new MonitorManager();


        public static MonitorManager CreateInstance()
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

        #region 定义和初始化

        protected virtual void _Init()
        {
            //x.Say("缓存已被初始化。");
            //if (null == Items)
            //    Items = new Dictionary<string, EntityTypesCacheItem>();

            //CacheProvider = Cache.CacheManager.Create();
        }






        #endregion

        #region 启动和关闭

        public void StartMonitor()
        {
            SiteAppStauts.StartTime = DateTime.Now;

            x.TimerLog("服务器已经启动！");
        }


        public void StopMonitor()
        {
            SiteAppStauts.StopTime = DateTime.Now;
            x.TimerLog("服务器已经 关闭！");
        }


        /// <summary>
        /// 提供一个静态类形式的启动方法，只是为了在 Application_Start 当中代码简洁一些
        /// </summary>

        public static void Start()
        {
            var Monitor = MonitorManager.CreateInstance();
            Monitor.StartMonitor();
        }
        public static void Stop()
        {
            var Monitor = MonitorManager.CreateInstance();
            Monitor.StopMonitor();
        }


        #endregion

    }
}
