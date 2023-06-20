using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlTypes;


namespace XP.Util
{
    public class Conf
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


        public static bool GetBool(string name, bool def = false)
        {
            if (0 == ConfigurationManager.AppSettings.Count)
                return def;
            string val = ConfigurationManager.AppSettings[name];
            if (null == val || 0 == val.Length)
            {
                return def;
            }

            if ("1" == val || "true" == val.ToLower())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static int GetInt(string name, int def = 0)
        {
            if (0 == ConfigurationManager.AppSettings.Count)
                return def;
            string val = ConfigurationManager.AppSettings[name];
            if (null == val || 0 == val.Length)
            {
                return def;
            }

            int tem = 0;
            if(int.TryParse(val,out tem))
            {
                return tem;
            }
            return def;
        }
        public static long GetLong(string name, long def = 0)
        {
            if (0 == ConfigurationManager.AppSettings.Count)
                return def;
            string val = ConfigurationManager.AppSettings[name];
            if (null == val || 0 == val.Length)
            {
                return def;
            }

            long tem = 0;
            if (long.TryParse(val, out tem))
            {
                return tem;
            }
            return def;
        }

        public static string GetConnString(string name)
        {

            return ConfigurationManager.ConnectionStrings[name].ToString();
        }

    }
}
