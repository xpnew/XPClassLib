using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Comm.Finders
{

    /// <summary>
    /// 文件夹查找基类
    /// </summary>
    /// <remarks>
    ///  虽然是文件夹查找，但是通常还是为了文件夹里面的文件。
    ///
    /// </remarks>
    /// 
    public class DirFinderBase
    {
        /// <summary>
        /// 原点路径
        /// </summary>
        public string OriginPath { get; set; }

        /// <summary>
        /// 子文件夹数量
        /// </summary>
        public int SubDirNum { get; set; }

        /// <summary>
        /// 区配到的合适的文件数量
        /// </summary>
        public int MatchFilesTotal { get; set; }


        /// <summary>
        /// 已经匹配的文件夹，文件夹中间缓存，通常是过滤了扩展名之后的结果
        /// </summary>
        public List<DirectoryInfo> MatchedSubDirs { get; set; }

        /// <summary>
        /// 需要查找到的文本模式
        /// </summary>
        public string FindPattern { get; set; }
        /// <summary>
        /// 准备替换的文本模式
        /// </summary>
        public string RepalcePattern { get; set; }


        public DirFinderBase()
        {
            _Init();
        }
        public DirFinderBase(string path):this()
        {
            OriginPath = path;
        }

        protected virtual void _Init()
        {
            MatchedSubDirs = new List<DirectoryInfo>();

        }

    }
}
