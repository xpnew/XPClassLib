using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace XP.Util.TextFile
{
    public class BaseReader
    {



        public static string ReadFile(string path, Encoding enc = null)
        {
            string Result = String.Empty;
            if(null == enc)
            {
                enc = Encoding.Default;
            }

            try
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                TextReader sw = new StreamReader(fs, enc);

                string str = sw.ReadToEnd();
                Result = str;

                //关闭流
                sw.Close();
                fs.Close();
                return Result;
            }
            catch (Exception ex)
            {
                x.Say("保存文件 [" + path + " ] 时发现异常:" + ex);
            }

            return Result;


        }

    }
}
