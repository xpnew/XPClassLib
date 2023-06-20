using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XP.Util.Path;

namespace XP.Util.TextFile
{

    /// <summary>
    /// 基础的Xml写入工具
    /// </summary>
    public class BaseXmlWriter
    {
        public string FileFullPath { get; set; }

        public string FileName { get; set; }


        protected XDocument xd { get; set; }



        public BaseXmlWriter(string fullpath) : this()
        {
            FileFullPath = PathTools.GetFull(fullpath);

            _Init();

        }

        public BaseXmlWriter()
        {
        }

        protected virtual void _Init()
        {
            xd = GetFile(FileFullPath);

        }


        public void Save()
        {
            SaveFile(FileFullPath, xd);
        }


        private  XDocument GetFile(string filename)
        {
            string Fullname = PathTools.GetFull(filename);



            XDocument xd = XDocument.Load(Fullname);
            return xd;
        }

        private static void SaveFile(string filename, XDocument xd)
        {
            string Fullname = PathTools.GetFull(filename);


            xd.Save(Fullname);

        }

    }
}
