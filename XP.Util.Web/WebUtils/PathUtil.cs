using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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

    }
}
