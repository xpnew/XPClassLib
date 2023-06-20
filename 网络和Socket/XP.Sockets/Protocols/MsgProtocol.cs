using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Sockets.Protocols
{

    /// <summary>
    /// 消息协议
    /// </summary>
    public class MsgProtocol : BaseProtocol
    {
        private string _HeadSymbol = "FE55";

        public override string HeadSymbol
        {
            get
            {
                return _HeadSymbol;
            }
            set
            {
                _HeadSymbol = value;
            }
        }
        public MsgProtocol() : base() { }

        public MsgProtocol(byte[] input)
            : base(input)
        {

        }





        protected override void InitProviders()
        {
            base.InitProviders();
            Providers.Add(new XORCheckoutProvider());
        }




        #region 根据内容创建协议


        public override byte[] MakeEnding(byte[] input)
        {
            var p = new XORCheckoutProvider();
            return p.MakeEnding(input);
        }
        public override byte[] MakeCheckOutSymbol()
        {
            XORCheckoutProvider p = new XORCheckoutProvider();
            string s = p.Symbol;
            byte[] Result = ProtocolUtil.X2String2Bytes(s); 
            return Result;
        }
        //public MsgProtocol MakeMsgProtocol(string msgText)
        //{
        //    MsgProtocol Result = new MsgProtocol();

        //    if (String.IsNullOrEmpty(msgText))
        //        return null;
        //    Result.Content = Encoding.UTF8.GetBytes(msgText);
        //    Result.ContentSize = Result.Content.Length;

        //    int EndSize = 1;


        //    Result.Length = 8 + Result.ContentSize + EndSize;

        //    Result.Buffers = new byte[Result.Length];


        //    return Result;
        //}

        #endregion

    }
}
