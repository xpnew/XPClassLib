using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace XP.IO.DirAndFile
{

    /// <summary>
    /// 文件结果信息
    /// </summary>
    public class FilesResultInfo
    {
        /// <summary>
        /// 全部文件
        /// </summary>
        public List<FileInfo> AllFiles { get; set; }

        /// <summary>
        /// 错误的文件
        /// </summary>
        public List<FileInfo> ErrorFiles { get; set; }

        /// <summary>
        /// 成功的文件
        /// </summary>
        public List<FileInfo> SuccessFiles { get; set; }


        public bool HasError { get; set; }

        public FilesResultInfo()
        {
            AllFiles = new List<FileInfo>();
            ErrorFiles = new List<FileInfo>();
            SuccessFiles = new List<FileInfo>();

            HasError = false;
        }

    }
}
