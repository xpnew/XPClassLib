using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XP.Util.WebUtils
{
    public class AutoDatePath : PathUtil
    {

        public AutoDatePath(string root)
            : base(root)
        {

        }



        public string MakeFilePath(string filename)
        {
            FileInfo info = new FileInfo(filename);
            string Ext = info.Extension;

            string WebPath = YearMonthPath();

            string NewName = DateTime.Now.ToString("hhmmssffff") + Ext;

            string FullPath = Write2Path(WebPath, NewName);

            return FullPath;


        }
    }
}
