using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Sockets;
using System.Net;
using System.Threading;


namespace XP.Sockets
{
    public class SimplyListener : BaseTcpServer
    {





        public SimplyListener(string ipv4, int port)
            : base(ipv4, port, ProtocolType.Tcp)
        {
        }
        public SimplyListener(string ipv4, int port, ProtocolType type)
            : base(ipv4, port, type)
        {
        }




    

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientSocket"></param>
        protected override void ReceiveMessage(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;

            while (true)
            {
                try
                {
                    //太坑爹了！！！ Socket.Connected MSDN上说是上一次的状态，这玩意不大准，用心跳包吧
                    if (!myClientSocket.Connected)
                    {
                        Console.WriteLine("客户端已经关闭。");
                        myClientSocket.Shutdown(SocketShutdown.Both);
                        myClientSocket.Close();
                        break;
                    }
                    //接收数据的缓冲区
                    byte[] ReceiveBuffer = new byte[1024];

                    //通过clientSocket接收数据  
                    int ReceiveLength = myClientSocket.Receive(ReceiveBuffer);
                    string ResultMsg = GetString(ReceiveBuffer, 0, ReceiveLength);
                    Console.WriteLine("接收客户端{0}消息{1}", myClientSocket.RemoteEndPoint.ToString(), ResultMsg);
                    myClientSocket.Send(GetBytes("OK"));

                }

                catch (SocketException ex)
                {
                    if (ex.ErrorCode == 10054)
                    {
                        Console.WriteLine("客户端已经关闭！");

                    }
                    else
                    {
                        Console.WriteLine(ex.Message);
                    }
                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Close();
                    break;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Close();
                    break;
                }
            }
        }



    
    }
}
