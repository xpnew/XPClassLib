using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XP.Common.Text;

namespace XP.Sockets
{

    /// <summary>
    /// 基础的协议处理
    /// </summary>
    /// <remarks>
    /// 作者：xpnew
    /// 联系：xpnew@126.com
    /// 基本的定义是：
    /// 2字节头+2字节长度+2字节校验类型+2字节校验结果长度+M字节正文+N字节校验结果
    /// 
    /// 
    /// </remarks>
    public class BaseProtocol
    {
        /// <summary>
        /// 最小长度，除了开始的8字节固定内容以外，至少要有1个字节的内容
        /// </summary>
        public static readonly int MiniLength = 9;
        private string _HeadSymbol = "0102";

        /// <summary>
        /// 协议头
        /// </summary>
        public virtual string HeadSymbol { get { return _HeadSymbol; } set { _HeadSymbol = value; } }


        private bool _HasError = false;
        /// <summary>
        /// 存在错误
        /// </summary>
        public bool HasError
        {
            get { return _HasError; }
            set { if (_HasError) return; _HasError = value; }
        }

        /// <summary>
        /// 协议长度
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 内容长度
        /// </summary>
        public int ContentSize { get; set; }
        /// <summary>
        /// 协议的内容
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// 整个缓存
        /// </summary>
        public byte[] Buffers { get; set; }

        /// <summary>
        /// 校验符号
        /// </summary>
        public string CheckSymbol { get; set; }


        public byte[] CheckoutEnd { get; set; }
        /// <summary>
        /// 全部校难验器集合
        /// </summary>
        protected List<XP.Sockets.Protocols.ICheckoutProvider> Providers { get; set; }

        public BaseProtocol()
        {
            Length = 0;
            ContentSize = 0;

            CheckoutEnd = new byte[] { };
            Content = new byte[] { };
            InitProviders();
        }

        public BaseProtocol(byte[] buffers)
            : this()
        {
            InitProtocol(buffers);
        }

        #region 初始化和一般检查
        /// <summary>
        /// 初始化协议 
        /// </summary>
        /// <param name="inputBytes"></param>
        protected void InitProtocol(byte[] inputBytes)
        {
            BaseCheck(inputBytes);

            if (HasError)
                return;



            this.Buffers = inputBytes;
            this.Length = inputBytes.Length;
            this.CheckSymbol = inputBytes[4].ToString("X2") + inputBytes[5].ToString("X2");
            int CheckoutSize = ProtocolUtil.GetInt(inputBytes, 6, 2);
            CheckoutEnd = inputBytes.Skip(Length - CheckoutSize).ToArray();
            //内容长度，必须大于0
            ContentSize = Length - 8 - CheckoutSize;
            if (ContentSize <= 0)
            {
                HasError = true;
                return;
            }
            Content = inputBytes.Skip(8).Take(ContentSize).ToArray();
        }
        /// <summary>
        /// 基本的检查
        /// </summary>
        /// <param name="inputBytes"></param>
        protected void BaseCheck(byte[] inputBytes)
        {
            if (inputBytes.Length < MiniLength)
            {
                HasError = true;
                return;
            }
            string first = inputBytes[0].ToString("X2");
            string second = inputBytes[1].ToString("X2");

            string Head = first + second;

            //长度校验
            int InnerSize = ProtocolUtil.GetInt(inputBytes, 2, 2);

            if (InnerSize != inputBytes.Length)
            {
                HasError = true;
                return;
            }

            //头检查
            if (Head != HeadSymbol)
            {
                HasError = true;
                return;
            }

        }

        /// <summary>
        /// 初始化数据校验提供者，派生类重写并且自己处理数据校验的方式
        /// </summary>
        protected virtual void InitProviders()
        {
            this.Providers = new List<Protocols.ICheckoutProvider>();
        }
        public void ReadBuffers(byte[] input)
        {
            InitProtocol(input);
        }
        #endregion


        #region 校验
        /// <summary>
        /// 内容校验
        /// </summary>
        public void ContentCheckout()
        {
            if (HasError)
            {
                return;
            }

            int CheckoutSize = ProtocolUtil.GetInt(Buffers, 6, 2);
            //校验位长度＝0的时候，跳过校验
            if (CheckoutSize == 0)
                return;
            if (null == Providers || 0 == Providers.Count)
                return;
            foreach (var provider in Providers)
            {
                if (CheckSymbol == provider.Symbol)
                {
                    bool Result = provider.Checkout(Content, CheckoutEnd);
                    if (!Result)
                    {
                        HasError = true;
                        return;
                    }
                }

            }

        }
        #endregion
        #region 获取内容

        public string GetMsg()
        {
            ContentCheckout();

            if (HasError)
                return null;

            string Result = EncodingUtil.GetStringByBytes(Content);

            return Result;
        }
        #endregion

        #region 根据内容创建协议

        public virtual byte[] MakeEnding(byte[] input)
        {
            return new byte[] { };
        }
        public virtual byte[] MakeCheckOutSymbol()
        {
            return new byte[] { };
        }
        public static T MkProtocol<T>(string input) where T : BaseProtocol, new()
        {
            if (String.IsNullOrEmpty(input))
                return null;
            var Buffer = Encoding.UTF8.GetBytes(input);
            return MkProtocol<T>(Buffer);
        }

        public static T MkProtocol<T>(byte[] input) where T : BaseProtocol, new()
        {

            T Result = new T();

            Result.Content = input;
            Result.ContentSize = Result.Content.Length;

            byte[] CheckoutEnding = Result.MakeEnding(input);

            int EndSize = CheckoutEnding.Length;

            Result.Length = 8 + Result.ContentSize + EndSize;
            Result.Buffers = new byte[Result.Length];
            byte[] Head = ProtocolUtil.X2String2Bytes(Result.HeadSymbol);

            Result.Buffers[0] = Head[0];
            Result.Buffers[1] = Head[1];


            Result.Buffers[2] = (byte)(Result.Length / 256);
            Result.Buffers[3] = (byte)(Result.Length % 256);


            byte[] CheckputSymbol = Result.MakeCheckOutSymbol();

            Result.Buffers[4] = CheckputSymbol[0];
            Result.Buffers[5] = CheckputSymbol[1];

            Result.Buffers[6] = (byte)(EndSize / 256);
            Result.Buffers[7] = (byte)(EndSize % 256);
            //填充内容
            for (int i = 0; i < input.Length; i++)
            {
                Result.Buffers[i + 8] = input[i];
            }

            //填充校验
            for (int i = 0; i < CheckoutEnding.Length; i++)
            {
                Result.Buffers[i + 8 + input.Length] = CheckoutEnding[i];
            }
            return Result;
        }


        #endregion
        #region 工具性的方法

        public byte[] GetBytes(string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }

    

        #endregion

    }
}
