using XP.Util.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Util;

namespace Ljy
{

    /// <summary>
    /// 通用的配置项，兼容 web.config/App.config  Site.config
    /// </summary>
    /// <remarks>
    /// ▲▲▲ 一般原则：
    /// ■Site.config 优先，如果Site.config不能正常运行，则会尝试读取web.config
    /// ■Site.config 读取到了空值，则认为是有效的。
    /// 
    /// </remarks>
    public static class GenConf
    {


        public static string GetSet(string name)
        {
            string ConfingValue = null;
            var OXConfig = ConfigReader._Instance;

            if (OXConfig.CacheInitReady)
            {
                ConfingValue = OXConfig.GetRealVal(name);
                if (null == ConfingValue)
                {
                    ConfingValue = Conf.GetConfigItem(name);
                }
            }
            else
            {
                ConfingValue = Conf.GetConfigItem(name);
            }
            return ConfingValue;
        }

    }
}
