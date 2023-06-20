using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using XP.Common;
using XP.Sockets.Protocols;

namespace XP.Sockets
{
    /// <summary>
    /// 解析协议的 TCP服务端
    /// </summary>
    public class ProtocolTcpServer : BaseTcpServer
    {

        public ProtocolTcpServer(string ipv4, int port)
            : base(ipv4, port)
        {
            ListenLength = 20;
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
                    Loger.Debug("远程连接：" + myClientSocket.RemoteEndPoint.ToString() + " ");


                    //接收数据的缓冲区
                    byte[] ReceiveBuffer = new byte[DefaultBufferSize];

                    //通过clientSocket接收数据  
                    int ReceiveLength = myClientSocket.Receive(ReceiveBuffer);
                    byte[] NewBuffer = new byte[ReceiveLength];

                    ProtocolUtil.CopyBuffer(ReceiveBuffer, NewBuffer);

                    MsgProtocol ReceiveProtocol = new MsgProtocol(NewBuffer);

                    ReceiveProtocol.ContentCheckout();
                    //==================返回消息
                    if (ReceiveProtocol.HasError)
                    {
                        // Loger.Error(" 接收数据失败，协议格式不对。");
                        //Console.WriteLine(" 接收数据失败，协议格式不对。");
                        string ResultMsg = GetString(ReceiveBuffer, 0, ReceiveLength);
                        Console.WriteLine("接收客户端{0}的 普通 消息{1}", myClientSocket.RemoteEndPoint.ToString(), ResultMsg);
                        myClientSocket.Send(GetBytes("OK"));

                    }
                    else
                    {
                        MsgProtocolHandle Handle = new MsgProtocolHandle(ReceiveProtocol);
                        Console.WriteLine("接收客户端{0} 协议 消息{1}", myClientSocket.RemoteEndPoint.ToString(), Handle.ReceiveMsg);
                        MsgProtocol SendProtocol = MsgProtocol.MkProtocol<MsgProtocol>(Handle.BackMsg);
                        if (null == SendProtocol)
                        {
                            return;
                        }
                        int SendLength = myClientSocket.Send(SendProtocol.Buffers);

                    }
                }
                #region 异常处理
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

                    if (myClientSocket.Connected)
                    {
                        myClientSocket.Shutdown(SocketShutdown.Both);
                        myClientSocket.Shutdown(SocketShutdown.Both);
                        myClientSocket.Close();


                    }
                    break;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (myClientSocket.Connected)
                    {
                        myClientSocket.Shutdown(SocketShutdown.Both);
                        myClientSocket.Shutdown(SocketShutdown.Both);
                        myClientSocket.Close();


                    }
                    break;
                }
                #endregion
            }
        }
      
    }
}
