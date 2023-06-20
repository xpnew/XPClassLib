using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Sockets.Protocols
{

    /// <summary>
    /// 异或校验提供者
    /// </summary>
    public class XORCheckoutProvider : BaseCheckoutProvider
    {
        public XORCheckoutProvider()
            : base("CC35")
        {

        }

        public override bool Checkout(byte[] input, byte[] ending)
        {
            byte VerifyByte = input[0];

            for (int i = 1; i < input.Length; i++)
            {
                VerifyByte = (byte)(VerifyByte ^ input[i]);
            }

            return VerifyByte == ending[ending.Length - 1];

            //return base.Checkout(input, ending);
        }


        public override byte[] MakeEnding(byte[] input)
        {
            if (null == input || 0 == input.Length)
            {
                return new byte[]{};
            }
            byte VerifyByte = input[0];

            for (int i = 1; i < input.Length; i++)
            {
                VerifyByte = (byte)(VerifyByte ^ input[i]);
            }

            return new byte[] { VerifyByte };
        }
    }
}
