using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Tokens
{
    public class TokenBase
    {


        /// <summary>
        /// Token值，字典Key
        /// </summary>
        public string Token { get; set; }


        public long CreateTS { get; set; }

        /// <summary>
        /// 创建时间（服务器本地时间）
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Token过期的时间戳，UTC时间
        /// </summary>
        public long ExpireTS { get; set; }

        /// <summary>
        /// Token过期的时间，只用来显示、或者辅助开发人员诊断，代码中计算请使用时间戳
        /// </summary>
        public DateTime ExpireTime { get; set; }

    }
}
