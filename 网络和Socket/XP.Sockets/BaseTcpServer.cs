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
    public class BaseTcpServer : BaseSocket
    {

        public BaseTcpServer(string ipv4, int port)
            : base(ipv4, port, ProtocolType.Tcp)
        {
        }
        public BaseTcpServer(string ipv4, int port, ProtocolType type)
            : base(ipv4, port, type)
        {
        }

        /// <summary>
        /// 开始监听
        /// </summary>
        public void StartWork()
        {
            SocketInstance = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ConnectionType);
            IPEndPoint BindPointk = new IPEndPoint(IP, Port);

            Loger.Debug("准备绑定到 地址：" + BindPointk.Address.ToString() + " 端口：" + BindPointk.Port);
          
            try
            {

                SocketInstance.Bind(BindPointk);
                SocketInstance.Listen(10);
                Thread WaitingThread = new Thread(Waiting);
                WaitingThread.Start();
            }
            catch (SocketException ex)
            {
                Loger.Error("Socket 连接失败，错误代码是：" + ex.ErrorCode + " 。您可以参考 MSDN Library 中的 Windows Sockets 第 2 版 API 错误代码文档，获取有关该错误的详细说明。 ", ex);

            }
            catch (System.Security.SecurityException ex)
            {
                Loger.Error("Socket 端口绑定失败，没有权限：" + ex.Message);
            }
            catch (Exception ex)
            {
                Loger.Error("Socket 端口绑定失败：" + ex.Message);

            }
        }

        /// <summary>
        /// 等待客户端的连接
        /// </summary>
        public virtual void Waiting()
        {
            while (true)
            {
                Socket clientSocket = SocketInstance.Accept();
                clientSocket.Send(GetBytes("Server Say Hello"));
                Thread receiveThread = new Thread(ReceiveMessage);
                receiveThread.Start(clientSocket);
            }
        }

        /// <summary>
        /// 接受连接
        /// </summary>
        /// <param name="clientSocket"></param>
        protected virtual void ReceiveMessage(object clientSocket)
        {



        }



        public void Stop()
        {

            if (SocketInstance.Connected)
            {
                SocketInstance.Shutdown(SocketShutdown.Both);
                SocketInstance.Close();

            }
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

    }
}
