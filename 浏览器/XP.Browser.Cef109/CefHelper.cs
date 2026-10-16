using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Browser.Cef109
{
    /// <summary>
    /// CefHelper 的摘要说明
    /// </summary>
    /// <remarks>
    /// 作    者：Administrator
    /// 创建时间：2026/8/30 13:51:47	
    /// 
    /// </remarks>
    public class CefHelper
    {

        public static void Initialize()
        {
            // 1. 配置 CEF 设置
            var settings = new CefSettings()
            {
                BrowserSubprocessPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "CefSharp.BrowserSubprocess.exe"),
                CachePath = Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData), "MyApp\\Cache"),
                LogSeverity = LogSeverity.Info,
            };

            // 2. 初始化 CEF（必须在主线程上调用，且只能执行一次）
            Cef.Initialize(settings);
        }


        /// <summary>
        /// 宽松的模式初始化
        /// </summary>
        public static void InitRelaxed()
        {
            // 1. 配置 CEF 设置
            var settings = new CefSettings()
            {
                BrowserSubprocessPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "CefSharp.BrowserSubprocess.exe"),
                CachePath = Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData), "MyApp\\Cache"),
                LogSeverity = LogSeverity.Info,
            };


            // 关闭同源策略，允许跨域
            settings.CefCommandLineArgs.Add("disable-web-security", "1");

            // 关闭站点隔离策略
            settings.CefCommandLineArgs.Add("disable-site-isolation-trials", "1");


            // 允许本地文件互相访问
            settings.CefCommandLineArgs.Add("allow-file-access-from-files", "1");

            // 允许本地文件访问任意来源
            settings.CefCommandLineArgs.Add("allow-universal-access-from-files", "1");


            // 2. 初始化 CEF（必须在主线程上调用，且只能执行一次）
            Cef.Initialize(settings);

        }

    }
}
