using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Comm.Finders
{
    /// <summary>
    /// 文本行的查找结果
    /// </summary>
    public class TextLineFinderResult
    {
        /// <summary>
        /// 行号，从1开始
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// 行文本，原始的
        /// </summary>
        public string LineText { get; set; }


        public string PrefText { get; set; }

        public string SuffText { get; set; }

        public string FindText { get; set; }

        public string ReplaceText { get; set; }

        /// <summary>
        /// 找到的字条起始位置，从1开始
        /// </summary>
        public int CharNumber { get; set; }


        /// <summary>
        /// 新文本
        /// </summary>
        public string NewText { get; set; }

    }
}
