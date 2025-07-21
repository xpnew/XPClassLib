using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Util.Text;

namespace XP.Compress.Zip
{
    /// <summary>
    ///  猜测密码 GuessPass
    /// </summary>
    /// <remarks>
    /// 创建日期：2025/7/8 16:14:12
    /// 类名：GuessPass
    /// 创建人： xpnew@126.com
    /// </remarks>
    public class GuessPass :UnZip
    {
        #region <属性>

        /// <summary>
        /// 随机字符串生成器
        /// </summary>
        public RandomStringByRangBase StringRandom { get; set; }

        public ZipOption Option { get; set; }


        #endregion <属性>

        #region <构造方法>


        public GuessPass(ZipOption option)
        {

            Option = option;
        }

        #endregion <构造方法>

        #region <内部方法>
        #endregion <内部方法 end>
        #region <外部方法>

        public bool TryList(List<string > pwds)
        {


            return false;
        }
        #endregion <外部方法 end>

        #region <事件>



        public Action<LevelLogMsg> LogMsgEvent { get; set; }

        #endregion <事件>    
    }
}
