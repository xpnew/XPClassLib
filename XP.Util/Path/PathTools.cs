using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Util.Path
{
  public  class PathTools
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

        public static string GetRoot()
        {
            string AppPath = AppDomain.CurrentDomain.BaseDirectory;
            if (!AppPath.EndsWith("\\"))
            {
                AppPath += "\\";
            }

            return AppPath;
        }

        public static string GetFull(string path)
        {
            string Root = GetRoot();
            string NewPath = path;
            if (1 == NewPath.IndexOf(":\\"))
            {
                return NewPath;
            }


            if (CheckWebPathType(path))
            {
                if (NewPath.StartsWith("~/"))
                {
                    NewPath = NewPath.Substring(2);
                }
                if (NewPath.StartsWith("/"))
                {
                    NewPath = NewPath.Substring(1);
                }
                NewPath = NewPath.ReplaceAll("/", "\\");
            }
            else
            {
                if (NewPath.StartsWith("\\"))
                {
                    NewPath = NewPath.Substring(1);
                }
            }

            return Root + NewPath;


        }


        public static bool CheckWebPathType(string path)
        {
            int idx1 = path.IndexOf('/');
            int idx2 = path.IndexOf('\\');

            if (0 > idx2)
                return true;
            if (0 > idx2)
                return false;


            if (idx1 < idx2)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
