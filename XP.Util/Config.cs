using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;


namespace XP.Util
{
    public class Config
    {

        public static string ConnStr
        {

            get
            {
                //return ConfigurationManager.ConnectionStrings["ConnStr"].ToString();

                return GetConnString("ConnStr");
            }
        }


        public static string GetConfigItem(string name)
        {
            if (0 == ConfigurationManager.AppSettings.Count)
                return String.Empty;
            //System.Configuration.KeyValueConfigurationElement customSetting = ConfigurationManager.AppSettings[name];

            return ConfigurationManager.AppSettings[name];

        }


        public static string GetConnString(string name)
        {

            return ConfigurationManager.ConnectionStrings[name].ToString();
        }

    }
}
