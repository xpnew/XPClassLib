using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Sockets.Protocols
{
    public interface ICheckoutProvider
    {
        /// <summary>
        /// 校验标志
        /// </summary>
        string Symbol { get; set; }
        /// <summary>
        /// 校验结尾，预计的值
        /// </summary>
        byte[] VerifyEnding { get; set; }

        /// <summary>
        /// 开始校验
        /// </summary>
        /// <returns></returns>
        bool Verify();

        bool Checkout(byte[] input, byte[] ending);

        byte[] MakeEnding(byte[] input);

    }
}
