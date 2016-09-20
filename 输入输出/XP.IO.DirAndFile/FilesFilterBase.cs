using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XP.IO.DirAndFile
{
    /// <summary>
    /// 文件过滤器基类
    /// </summary>
    public class FilesFilterBase
    {

        /// <summary>
        /// 需要处理的目录
        /// </summary>
        public string DirPath { get; set; }

        public DirectoryInfo WorkDir { get; set; }

        /// <summary>
        /// 允许搜索子目录，默认：是
        /// </summary>
        public bool EnableSub { get; set; }


        /// <summary>
        /// 已经就绪
        /// </summary>
        protected bool _HasReady = false;

        /// <summary>
        /// 过滤的结果
        /// </summary>
        public FilesResultInfo ResultInfo { get; set; }

        public FilesFilterBase()
            : this(String.Empty)
        {

        }

        public FilesFilterBase(string path)
        {
            _Init();
            SetDirPath(path);
        }



        protected void _Init()
        {
            _HasReady = false;


            EnableSub = true;
            ResultInfo = new FilesResultInfo();
        }


        protected void SetDirPath(string path)
        {
            this.DirPath = path;
            if (String.IsNullOrEmpty(path))
            {
                _HasReady = false;
            }
            DirectoryInfo d = new DirectoryInfo(path);
            if (d.Exists)
            {
                _HasReady = true;
            }
            WorkDir = d;
        }

        /// <summary>
        /// 获取所有的文件
        /// </summary>
        public void LoadFiles()
        {
            ResultInfo.AllFiles = GetFiles(WorkDir, EnableSub);
        }


        public virtual void StartFilter()
        {


        }


        /// <summary>
        /// 循环获取 一个目录和其子目录下的全部文件
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static List<FileInfo> GetFiles(DirectoryInfo d)
        {

            FileInfo[] fis = d.GetFiles();
            List<FileInfo> FileList = new List<FileInfo>();
            FileList.AddRange(fis.ToList());
            foreach (DirectoryInfo subdir in d.GetDirectories())
            {
                FileList.AddRange(GetFiles(subdir));
            }
            return FileList;
        }

        public static List<FileInfo> GetFiles(DirectoryInfo d, bool enableSub)
        {

            FileInfo[] fis = d.GetFiles();
            List<FileInfo> FileList = new List<FileInfo>();
            FileList.AddRange(fis.ToList());
            if (enableSub)
            {
                foreach (DirectoryInfo subdir in d.GetDirectories())
                {
                    FileList.AddRange(GetFiles(subdir));
                }
            }
            return FileList;
        }

    }
}
