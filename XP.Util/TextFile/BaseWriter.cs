using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XP.Util.TextFile
{
    public class BaseWriter
    {

        public static bool Write2File(string inputText, string path)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                //开始写入
                sw.Write(inputText);
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();
                return true;
            }
            catch (Exception ex)
            {
                x.Say("保存文件 [" + path + " ] 时发现异常:" + ex);
                return false;
            }


        }
    }
}
