using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.IO.ExcelUtil
{
    /// <summary>
    /// Excel文件处理结果信息
    /// </summary>
    public class ExcelResultInfo
    {
        /// <summary>
        /// 成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 所有的信息集合
        /// </summary>
        public List<ExcelCellInfo> InfoList { get; set; }
        /// <summary>
        /// 所有的错误集合
        /// </summary>
        public List<ExcelCellInfo> ErrorList { get; set; }

        /// <summary>
        /// 成功的行数
        /// </summary>
        public int SuccessLineCount { get; set; }

        /// <summary>
        /// 失败的行数
        /// </summary>
        public int FailLineCount { get; set; }

        /// <summary>
        /// 跳过的行数
        /// </summary>
        public int SkipLineCount { get; set; }

        /// <summary>
        /// 总处理的行数
        /// </summary>
        public int TotalLineCount { get; set; }


        public ExcelResultInfo()
        {
            Success = true;

            InfoList = new List<ExcelCellInfo>();
            ErrorList = new List<ExcelCellInfo>();

            SuccessLineCount = 0;
            FailLineCount = 0;
            TotalLineCount = 0;

        }


        public void Error(ExcelInfoTypes type, string errorMsg)
        {
            this.Success = false;
            this.ErrorList.Add(new ExcelCellInfo(type, errorMsg));
        }


    }
}
