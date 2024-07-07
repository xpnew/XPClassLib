using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Comm
{
    /// <summary>
    /// 偷懒的数据结构消息，DataMsg<T>的简化版，方便生成Json
    /// </summary>
    public class TruantDataMsg : CommMsg
    {

        public object DataInfo { get; set; }
    }
}
