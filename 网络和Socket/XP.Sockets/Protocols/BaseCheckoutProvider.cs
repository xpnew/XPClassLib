using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Sockets.Protocols
{
    /// <summary>
    /// 校验提供者接口基类，只是对接口进行基本的实现
    /// </summary>
    public class BaseCheckoutProvider : ICheckoutProvider
    {
        public string Symbol
        {
            get;
            set;
        }

        public byte[] VerifyEnding
        {
            get;
            set;
        }

        public BaseCheckoutProvider()
        {

        }

        public BaseCheckoutProvider(string symbol)
        {
            this.Symbol = symbol;
        }

        public BaseCheckoutProvider(string symbol, byte[] end)
        {
            this.Symbol = symbol;
            this.VerifyEnding = end;
        }



        public virtual bool Verify()
        {
            throw new NotImplementedException();
        }


        public virtual bool Checkout(byte[] input, byte[] ending)
        {
            //if (0 == ending.Length)
            //    return true;
            return true;
        }


        public virtual byte[] MakeEnding(byte[] input)
        {
            throw new NotImplementedException();
        }
    }
}
