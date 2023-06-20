using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;



namespace XP.Util
{
    public static class InstanceBuilder
    {

        public static TObject CreateInstance<TObject>(string config)
            where TObject : class
        {
            //ConfigReader Config = ConfigReader.CreateInstance();

            string IUserConfig = Conf.GetConfigItem(config);
            if (String.IsNullOrEmpty(IUserConfig))
                return null;

            string[] Arr = IUserConfig.Split(new char[] { ';', ',' });
            if (2 > Arr.Length)
                return null;

            string AssemblyName = Arr[1];
            string ClassName = Arr[0];
            Assembly asm2 = Assembly.Load(AssemblyName);
            object o = asm2.CreateInstance(ClassName);
            if (o == null)
                return null;
            if (o is TObject)
            {
                return o as TObject;
            }
            return null;
        }
        public static T CreateInterface<T>(string classNameInConfig)
        {
            var o = CreateTargetObj(classNameInConfig);
            if (o is T)
            {
                return (T)o;
            }
            return default(T);
        }
        public static object CreateTargetObj(string config)
        {
            if (String.IsNullOrEmpty(config))
                return null;

            string[] Arr = config.Split(new char[] { ';', ',' });
            if (2 > Arr.Length)
                return null;

            string AssemblyName = Arr[1];
            string ClassName = Arr[0];

            return CreateTargetObj(AssemblyName, ClassName);
        }
        public static object CreateTargetObj(string assemblyName, string className)
        {
            Assembly asm2 = Assembly.Load(assemblyName);
            object o = asm2.CreateInstance(className);
            return o;
        }
        public static TObject CreateInstance<TObject>(string assemblyName,string className)
          where TObject : class
        {
            //ConfigReader Config = ConfigReader.CreateInstance();
            Assembly asm2 = Assembly.Load(assemblyName);
            object o = asm2.CreateInstance(className);
            if (o == null)
                return null;
            if (o is TObject)
            {
                return o as TObject;
            }
            return null;
        }

    }
}
