using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Sockets;
using System.Net;
using System.Threading;


namespace XP.Sockets
{
    public class BaseTcpClient : BaseSocket
    {
        public bool IsReady { get; set; }
        public string ConnectMessage { get; set; }
        public BaseTcpClient(string ipv4, int port)
            : base(ipv4, port, ProtocolType.Tcp)
        {
            IsReady = false;
        }

        public void Connenction()
        {
            SocketInstance = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ConnectionType);
            IPEndPoint BindPointk = new IPEndPoint(IP, Port);

            try
            {
                SocketInstance.Connect(BindPointk); //配置服务器IP与端口  
                Console.WriteLine("连接服务器成功");

                Loger.Debug("连接端口成功！");

                IsReady = true;
                int receiveLength = SocketInstance.Receive(this.ReceiveBuffer);
                ConnectMessage = Encoding.UTF8.GetString(ReceiveBuffer, 0, receiveLength);

            }
            catch (SocketException ex)
            {
                Loger.Error("Socket 连接失败，错误代码是：" + ex.ErrorCode + " 。您可以参考 MSDN Library 中的 Windows Sockets 第 2 版 API 错误代码文档，获取有关该错误的详细说明。 ", ex);

            }
            catch (System.Security.SecurityException ex)
            {
                Loger.Error("Socket 端口连接失败，没有权限：" + ex.Message);
            }
            catch (Exception ex)
            {
                Loger.Error("Socket 端口连接失败：" + ex.Message);

            }

        }


        public virtual void SendMsg(string msg)
        {
            if (SocketInstance.Connected)
            {
                int SendLength = SocketInstance.Send(Encoding.UTF8.GetBytes(msg));

                int ReceiveLength = SocketInstance.Receive(this.ReceiveBuffer);


                string ResultMsg = GetString(ReceiveBuffer, 0, ReceiveLength);

                ConnectMessage = ResultMsg;

                Console.WriteLine("接收服务器上的{0}消息{1}", SocketInstance.RemoteEndPoint.ToString(), ResultMsg);
            }

        }

        public virtual void Send(byte[] buffers)
        {

        }



        public void Close()
        {
            if (SocketInstance.Connected)
            {
                SocketInstance.Shutdown(SocketShutdown.Both);

                SocketInstance.Disconnect(true);
                SocketInstance.Close();
            }

        }



    }
}
