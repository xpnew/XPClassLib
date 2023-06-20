using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Util
{
    /// <summary>
    /// Winform操作App.config（增加、修改、删除、读取等）
    /// https://blog.csdn.net/softimite_zifeng/article/details/60591488
    /// </summary>
    public class AppConfigMng
    {


        #region 快速单例模式

        public static readonly AppConfigMng _Instance = new AppConfigMng();

        public static AppConfigMng CreateInstance()
        {
            return _Instance;
        }

        public static AppConfigMng Self
        {
            get { return _Instance; }
        }
        protected AppConfigMng()
        {
            _Init();
        }
        #endregion


        protected void _Init()
        {

        }

        /// <summary>
        /// 添加键为keyName、值为keyValue的项
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="keyValue"></param>
        public void AddItem(string keyName, string keyValue)
        {
            //添加配置文件的项，键为keyName，值为keyValue
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Add(keyName, keyValue);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
        /// <summary>
        /// 判断键为keyName的项是否存在
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public bool ExistKey(string keyName)
        {
            //判断配置文件中是否存在键为keyName的项
            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key == keyName)
                {
                    //存在
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summ获取键为keyName的项的值：ary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public string GetValue(string keyName)
        {
            //返回配置文件中键为keyName的项的值
            return ConfigurationManager.AppSettings[keyName];
        }

        public void SaveKey(string key ,string val)
        {
            if (ExistKey(key))
            {
                ModifyByKey(key, val);
            }
            else
            {
                AddItem(key, val);
            }
        }
        /// <summary>
        /// 修改键为keyName的项的值
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="newKeyValue"></param>
        public void ModifyByKey(string keyName, string newKeyValue)
        {
            //修改配置文件中键为keyName的项的值
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[keyName].Value = newKeyValue;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 删除键为keyName的项
        /// </summary>
        /// <param name="keyName"></param>
        public void RemoveItem(string keyName)
        {
            //删除配置文件键为keyName的项
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove(keyName);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }


    }
}
