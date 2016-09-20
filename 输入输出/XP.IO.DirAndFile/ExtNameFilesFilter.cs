using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.IO.DirAndFile
{
   
    /// <summary>
    /// 根据扩展名过滤文件
    /// </summary>
    public class ExtNameFilesFilter : FilesFilterBase
    {



        public override void StartFilter()
        {

            var all = ResultInfo.AllFiles;

            foreach (var f in all)
            {
                string ExtName = f.Extension;

                if (IsOkExtName(ExtName))
                {
                    ResultInfo.SuccessFiles.Add(f);
                }
                else
                {
                    ResultInfo.ErrorFiles.Add(f);
                }

            }
        }



        public bool IsOkExtName(string extName){

            if( "jpg" == extName || "jpge" == extName){

                return true;
            }


            return true;
        }

    }
}
