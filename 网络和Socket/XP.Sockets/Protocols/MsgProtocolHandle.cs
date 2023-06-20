using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Sockets.Protocols
{
    public class MsgProtocolHandle
    {
        public string ReceiveMsg { get; set; }
        public string BackMsg { get; set; }

        public MsgProtocol Protocol { get; set; }
        public MsgProtocolHandle(MsgProtocol pro)
        {
            Protocol = pro;
            MsgHandle(Protocol);
        }

        public void MsgHandle(MsgProtocol pro)
        {
            string Head = "";
            if (pro.ContentSize > 1)
            {
                string first = pro.Content[0].ToString("X2");
                string second = pro.Content[1].ToString("X2");

                Head = first + second;

            }
            if (Head == "FC01")
            {
                ControlPotocol cp = new ControlPotocol(pro.Content);
                cp.ContentCheckout();
                if (cp.HasError)
                {
                    ReceiveMsg = "这是一条控制命令，但是解析失败";

                }
                else
                {
                    ReceiveMsg = "控制命令已经接收";
                    BackMsg = "服务器准备关闭！";
                    cp.RunCommand();
                }

            }
            else
            {
                ReceiveMsg = pro.GetMsg();
                BackMsg = "OK";
            }


        }

    }
}
