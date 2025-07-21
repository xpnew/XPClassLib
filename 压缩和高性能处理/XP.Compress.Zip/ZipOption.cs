using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Compress.Zip
{
    /// <summary>
    /// ZipOption zip参数
    /// </summary>
    /// <remarks>
    /// 创建日期：2025/7/18 9:57:31
    /// 类名：ZipOption
    /// 创建人： xpnew@126.com
    /// </remarks>
    public class ZipOption
    {
        #region <属性>

        public bool EnableSmall { get; set; } = true;

        public bool EnalbleNumber { get; set; } = true;
        public bool EnableLarge { get; set; } = false;



        public string Punctuation { get; set; }


       public string ZipFilepath { get; set; }


        public string TempRoot { get; set; }




        /// <summary>
        /// 随机字符串最大长度
        /// </summary>
        public int RangMax { get; set; } = 8;
        /// <summary>
        /// 随机字符串最小长度
        /// </summary>
        public int RangMin { get; set; } = 3;


        public int ThreadMax { get; set; } = 1;


        public long LoopMax { get; set; } = 1000;






        #endregion <属性>

        #region <构造方法>
        #endregion <构造方法>

        #region <内部方法>
        #endregion <内部方法 end>
        #region <外部方法>
        #endregion <外部方法 end>

        #region <事件>
        #endregion <事件>    
    }
}
