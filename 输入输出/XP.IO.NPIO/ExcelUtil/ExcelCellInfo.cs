using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.IO.ExcelUtil
{
    /// <summary>
    /// Excel 单元格（Cell）处理信息
    /// </summary>
    public class ExcelCellInfo
    {
        /// <summary>
        /// 信息类型
        /// </summary>
        public ExcelInfoTypes InfoType { get; set; }

        /// <summary>
        /// Row数（0索引）
        /// </summary>
        public int RowNum { get; set; }

        /// <summary>
        /// 行数（1索引）
        /// </summary>
        public int LineNum { get { return RowNum + 1; } set { RowNum = value - 1; } }

        /// <summary>
        /// 行内单元格位置（1索引）
        /// </summary>
        public int CellNum { get; set; }

        /// <summary>
        /// 处理结果消息。（忽略、跳过、退出）
        /// </summary>
        /// <remarks>
        /// 忽略：出现错误，但是可以忽略，继续处理后续的数据。
        /// 跳过：空白单元格，跳过，继续处理后续数据。
        /// 退出：严重的问题，不再继续处理后续的数据
        /// 
        /// 
        /// </remarks>
        public string ResultMsg { get; set; }

        public ResultTypes ResultType { get; set; }
        public ExcelCellInfo()
        {
            this.InfoType = ExcelInfoTypes.NONE;
            ResultType = ResultTypes.NONE;
            this.RowNum = 0;
            this.CellNum = 0;
        }

        public ExcelCellInfo(ExcelInfoTypes type, ResultTypes resultType)
            : this()
        {
            InfoType = type;

            ResultType = resultType;
        }
        public ExcelCellInfo(ExcelInfoTypes type, string msg)
            : this(type, ResultTypes.Stop, msg)
        {
        }
        public ExcelCellInfo(ExcelInfoTypes type, ResultTypes resultType, string msg)
            : this()
        {
            InfoType = type;
            ResultType = resultType;
            ResultMsg = msg;
        }



        public override string ToString()
        {
            return base.ToString();
        }

    }
}
