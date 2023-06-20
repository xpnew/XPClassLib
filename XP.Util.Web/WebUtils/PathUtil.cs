using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace XP.Util.WebUtils
{
    public class PathUtil : UtilBase
    {


        public string RootPath { get; set; }

        public PathUtil(string root)
        {
            RootPath = root;
        }


        public string Write2Path(string path, string filename)
        {
            string wlPath = Server.MapPath(path);

            if (!Directory.Exists(wlPath))
                Directory.CreateDirectory(wlPath);

            string FullPath = path +"/"+ filename;


            return FullPath;
        }


        public string YearMonthPath()
        {
            DateTime today = DateTime.Now;
            string path = string.Format("{0}/{1}/{2}", RootPath, today.Year.ToString("0000"), today.Month.ToString("00"));


            return path;
        }


        public static string GetRootPath()
        {
            string AppPath = "";
            HttpContext HttpCurrent = HttpContext.Current;
            if (HttpCurrent != null)
            {
                AppPath = HttpCurrent.Server.MapPath("~");
            }
            else
            {
                AppPath = AppDomain.CurrentDomain.BaseDirectory;
                if (Regex.Match(AppPath, @"\\$", RegexOptions.Compiled).Success)
                    AppPath = AppPath.Substring(0, AppPath.Length - 1);
            }
            return AppPath;
        }

        public static bool ExistDir(string dir ,bool force)
        {
            string wlPath = Server.MapPath(dir);

            if (!Directory.Exists(wlPath))
            {
                if (force)
                {
                    try
                    {
                        Directory.CreateDirectory(wlPath);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        x.Say("创建文件夹出错。");
                        return false;
                    }
                }
                return false;
            }
            return true;
        }

        private static string _HostName = null;

        public static void InitHost()
        {
            if (null != _HostName)
            {
                return;
            }

            //string hhhost = HttpRuntime.AspClientScriptVirtualPath;
            if (null != HttpContext.Current && null != HttpContext.Current.Request)
            {
                var rq = HttpContext.Current.Request;
                var urls = rq.Url;
                _HostName = urls.Scheme + "://" + urls.Host + ":" + urls.Port + "/";
            }

        }
        public static string GetSiteRoot()
        {
            return _HostName;
            // return HttpRuntime.AppDomainAppVirtualPath;

        }

    }
}
