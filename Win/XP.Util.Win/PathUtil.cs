using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Util.Win
{
    public class PathUtil
    {

        public static string GetRootPath()
        {
            string AppPath = "";
            AppPath = AppDomain.CurrentDomain.BaseDirectory;
            if (!AppPath.EndsWith("\\"))
            {
                AppPath += "\\";
            }
            return AppPath;
        }
    }
}
