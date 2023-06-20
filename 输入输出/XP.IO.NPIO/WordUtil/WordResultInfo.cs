using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.IO.WordUtil
{
    /// <summary>
    /// Word文件处理结果信息
    /// </summary>
    public class WordResultInfo
    {
        /// <summary>
        /// 成功
        /// </summary>
        public bool Success { get; set; } = false;
        /// <summary>
        /// 成功的行数
        /// </summary>
        public int SuccessLineCount { get; set; } = 0;

        /// <summary>
        /// 失败的行数
        /// </summary>
        public int FailLineCount { get; set; } = 0;

        /// <summary>
        /// 跳过的行数
        /// </summary>
        public int SkipLineCount { get; set; } = 0;

        /// <summary>
        /// 总处理的行数
        /// </summary>
        public int TotalLineCount { get; set; } = 0;



        /// <summary>
        /// 所有的信息集合
        /// </summary>
        public List<WordPartInfo> InfoList { get; set; } = new List<WordPartInfo>();
        /// <summary>
        /// 所有的错误集合
        /// </summary>
        public List<WordPartInfo> ErrorList { get; set; } = new List<WordPartInfo>();



        public void Error(WordErrorDef type, string errorMsg)
        {
            this.Success = false;
            this.ErrorList.Add(new WordPartInfo(type, errorMsg));
        }

    }
}
