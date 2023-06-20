using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Sockets.Protocols
{
    /// <summary>
    /// 简单安全校验
    /// </summary>
    public class SimplySecurityProvider : BaseCheckoutProvider
    {

        public SimplySecurityProvider()
            : base("CC35")
        {

        }

        public override bool Checkout(byte[] input, byte[] ending)
        {

            byte VerifyByte = 203;

            return VerifyByte == ending[ending.Length - 2];

            //return base.Checkout(input, ending);
        }


        public override byte[] MakeEnding(byte[] input)
        {
            if (null == input || 0 == input.Length)
            {
                return new byte[] { };
            }
            byte VerifyByte = 203;

            return new byte[] { VerifyByte };
        }
    }
}
