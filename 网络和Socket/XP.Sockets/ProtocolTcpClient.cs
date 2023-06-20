using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XP.Sockets.Protocols;

namespace XP.Sockets
{
    public class ProtocolTcpClient : BaseTcpClient
    {

        public ProtocolTcpClient(string ipv4, int port)
            : base(ipv4, port)
        {
            ListenLength = 20;
        }

        public void CloseServer()
        {
            Control(1);

        }

        public void Control(byte cmd)
        {
            ControlPotocol CmdProtocol = ControlPotocol.MkProtocol<ControlPotocol>(new byte[] { cmd });
            if (null == CmdProtocol)
            {
                return;
            }
            Send(CmdProtocol.Buffers);
        }

        public override void Send(byte[] buffers)
        {
            if (SocketInstance.Connected)
            {

                MsgProtocol SendProtocol = MsgProtocol.MkProtocol<MsgProtocol>(buffers);
                if (null == SendProtocol)
                {
                    return;
                }
                int SendLength = SocketInstance.Send(SendProtocol.Buffers);

                //接收数据的缓冲区
                int ReceiveLength = SocketInstance.Receive(this.ReceiveBuffer);


                byte[] NewBuffer = new byte[ReceiveLength];
                ProtocolUtil.CopyBuffer(ReceiveBuffer, NewBuffer);


                MsgProtocol ReceiveProtocol = new MsgProtocol(NewBuffer);

                ReceiveProtocol.ContentCheckout();
                if (ReceiveProtocol.HasError)
                {
                    // Loger.Error(" 接收数据失败，协议格式不对。");
                    //Console.WriteLine(" 接收数据失败，协议格式不对。");
                    string ResultMsg = GetString(ReceiveBuffer, 0, ReceiveLength);

                    ConnectMessage = ResultMsg;

                    Console.WriteLine("接收服务器上的 {0} 消息 {1} ", SocketInstance.RemoteEndPoint.ToString(), ResultMsg);

                }
                else
                {
                    string ResultMsg = ReceiveProtocol.GetMsg();
                    ConnectMessage = ResultMsg;
                    Console.WriteLine("接收服务器上的{0}消息{1}", SocketInstance.RemoteEndPoint.ToString(), ResultMsg);

                }

            }
        }

        public override void SendMsg(string msg)
        {
            //base.SendMsg(msg);

            if (SocketInstance.Connected)
            {

                MsgProtocol SendProtocol = MsgProtocol.MkProtocol<MsgProtocol>(msg);
                if (null == SendProtocol)
                {
                    return;
                }
                int SendLength = SocketInstance.Send(SendProtocol.Buffers);

                //接收数据的缓冲区
                int ReceiveLength = SocketInstance.Receive(this.ReceiveBuffer);


                byte[] NewBuffer = new byte[ReceiveLength];
                ProtocolUtil.CopyBuffer(ReceiveBuffer, NewBuffer);


                MsgProtocol ReceiveProtocol = new MsgProtocol(NewBuffer);

                ReceiveProtocol.ContentCheckout();
                if (ReceiveProtocol.HasError)
                {
                    // Loger.Error(" 接收数据失败，协议格式不对。");
                    //Console.WriteLine(" 接收数据失败，协议格式不对。");
                    string ResultMsg = GetString(ReceiveBuffer, 0, ReceiveLength);
                    ConnectMessage = ResultMsg;
                    Console.WriteLine("接收服务器上的 {0} 消息 {1} ", SocketInstance.RemoteEndPoint.ToString(), ResultMsg);

                }
                else
                {
                    string ResultMsg = ReceiveProtocol.GetMsg();
                    ConnectMessage = ResultMsg;
                    Console.WriteLine("接收服务器上的{0}消息{1}", SocketInstance.RemoteEndPoint.ToString(), ResultMsg);

                }

            }

        }
    }
}
