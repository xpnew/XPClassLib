using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.IO.WordUtil
{
    /// <summary>
    /// Word 段落处理信息
    /// </summary>
    public class WordPartInfo
    {

        public WordErrorDef ErrorInfo { get; set; }


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




        public WordFileResultDef ResultType { get; set; }


        public WordPartInfo()
        {
            this.ErrorInfo = WordErrorDef.NONE;
            ResultType = WordFileResultDef.NONE;
            //this.RowNum = 0;
            //this.CellNum = 0;
        }

        public WordPartInfo(WordErrorDef type, string msg)
           : this(type, WordFileResultDef.Stop, msg)
        {
        }
        public WordPartInfo(WordErrorDef type, WordFileResultDef resultType, string msg)
         : this()
        {
            ErrorInfo = type;
            ResultType = resultType;
            ResultMsg = msg;
        }

        public override string ToString()
        {
            return base.ToString();
        }


    }
}
