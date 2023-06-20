using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XP.Web.Permission
{
    public class SessionUserFactory
    {



        public static ISessionUser CreateUserByConfig(string configKey)
        {


            Type InstanceType;
            InstanceType = GetCacheType(configKey);
            if (null == InstanceType)
            {
                InstanceType = CreateTypeByConfig(configKey);
                InsertType2Cache(configKey, InstanceType);
            }

            MethodInfo MyMethod = InstanceType.GetMethod("CreateUser");

            if (null != MyMethod)
            {

                object Result = MyMethod.Invoke(null, new object[] { });

                if (null != Result)
                {
                    return Result as ISessionUser;
                }
            }
            return null;
        }

        /// <summary>
        /// 通过配置创建实例类型
        /// </summary>
        /// <param name="configKey"></param>
        /// <returns></returns>
        private static Type CreateTypeByConfig(string configKey)
        {
            string IUserConfig = Util.Config.ConfigReader._Instance.GetSet(configKey);
            string[] Arr = IUserConfig.Split(new char[] { ';', ',' });
            string AssemblyName = Arr[1];
            string ClassName = Arr[0];
            Assembly asm2 = Assembly.Load(AssemblyName);

            Type T = asm2.GetType(ClassName);

            return T;

        }
        /// <summary>
        /// 通过缓存获取已知的实例类型
        /// </summary>
        /// <param name="configKey"></param>
        /// <returns></returns>
        private static Type GetCacheType(string configKey)
        {
            ISessionUserTypeCache Cache = ISessionUserTypeCache.CreateInstance();

            if (Cache.Dict.ContainsKey(configKey))
            {
                return Cache.Dict[configKey];
            }
            return null;
        }

        /// <summary>
        /// 将已知的实例类型添加到缓存
        /// </summary>
        /// <param name="configKey"></param>
        /// <param name="newType"></param>
        public static void InsertType2Cache(string configKey, Type newType)
        {
            ISessionUserTypeCache Cache = ISessionUserTypeCache.CreateInstance();

            if (Cache.Dict.ContainsKey(configKey))
            {
                Cache.Dict[configKey] = newType;
            }
            else
            {
                Cache.Dict.Add(configKey, newType);
            }
        }

    }
}
