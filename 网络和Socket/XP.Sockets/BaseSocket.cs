using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Net.Sockets;
using System.Net;
using System.Threading;
using XP.Util.Text;


namespace XP.Sockets
{
    public class BaseSocket
    {
        private int _DefaultBufferSize = 1024 * 1024;

        public virtual int DefaultBufferSize
        {
            get { return _DefaultBufferSize; }
            set { _DefaultBufferSize = value; }
        }



        public string IPV4Address { get; set; }

        public IPAddress IP { get; set; }
        /// <summary>
        /// 绑定的端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 协议类型
        /// </summary>
        public ProtocolType ConnectionType { get; set; }
        /// <summary>
        /// 端口实例
        /// </summary>
        public Socket SocketInstance { get; set; }

        public byte[] SendBuffer { get; set; }
        public byte[] ReceiveBuffer { get; set; }
        /// <summary>
        /// 监听的队伍长度
        /// </summary>
        public int ListenLength { get; set; }

        public BaseSocket()
        {
            ListenLength = 10;
        }
        public BaseSocket(Socket socket):this()
        {
            SocketInstance = socket;
            ConnectionType = socket.ProtocolType;


        }
        public BaseSocket(string ipv4, int port)
            : this(ipv4, port, ProtocolType.Tcp)
        {
            //ConnectionType = ProtocolType.Tcp;
        }
        public BaseSocket(string ipv4, int port, ProtocolType type)
            : this()
        {
            IPV4Address = ipv4;
            Port = port;
            ConnectionType = type;
            if (this.IPV4Address == "0.0.0.0")
            {
                IP = IPAddress.Any;
            }
            else
            {
                IP = IPAddress.Parse(IPV4Address);
            }

            SendBuffer = new byte[DefaultBufferSize];
            ReceiveBuffer = new byte[DefaultBufferSize];
        }

    




        public static byte[] GetBytes(string input)
        {
            if (null == input)
                return new byte[1];

            return Encoding.UTF8.GetBytes(input);
        }

        public static string GetString(byte[] input, int start, int length)
        {
            return EncodingUtil.GetStringByBytes(input, start, length);

        }

        public void Close()
        {

            SocketInstance.Shutdown(SocketShutdown.Both);

            if (ConnectionType == ProtocolType.Tcp)
            {
                SocketInstance.Disconnect(true);

            }
            SocketInstance.Close();

        }
    }
}
