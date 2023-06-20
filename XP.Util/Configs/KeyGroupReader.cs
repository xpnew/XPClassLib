using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Util.Configs
{
    /// <summary>
    /// Site.config内的配置项目太多的时候可以添加分组，这个类是对分组的封装，为代码编写提供了方便。
    /// </summary>
    public class KeyGroupReader
    {
        public ConfigReader ConfigReader { get; set; }


        public string GroupName { get; set; }

        public int Count
        {
            get { return ConfigReader.GetKeyGroupCount(GroupName); }
        }

        public KeyGroupReader(ConfigReader reader, string group)
        {
            ConfigReader = reader;

            GroupName = group;
        }





        public string GetKey(string key)
        {
            return ConfigReader.GetKeyGroupValue(GroupName, key);
        }


        public bool Exist(string key)
        {
            return ConfigReader.ExistKeyGroupSet(GroupName, key);
        }
        public int GetInt(string key, int? def = null)
        {
            return ConfigReader.GetKeyGroupInt(GroupName, key, def);
        }
        public long GetLong(string key, long? def = null)
        {
            return ConfigReader.GetKeyGroupLong(GroupName, key, def);
        }

        public bool GetBool(string key, bool def = false)
        {
            return ConfigReader.GetKeyGroupBool(GroupName, key, def);
        }
        public decimal GetDecimal(string key, decimal def = 0.0m)
        {
            return ConfigReader.GetKeyGroupDecimal(GroupName, key, def);
        }


    }
}
