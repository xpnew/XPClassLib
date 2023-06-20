using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XP.Sockets;

namespace XP.TCP.Server
{
    class Program
    {
        static void Main(string[] args)
        {


           // SimplyListen();

            ProtocolSever();

        }

        private static void ProtocolSever()
        {
            int ServerPort = 2222;
            string Ip = "127.0.0.1";

            ProtocolTcpServer sv = new ProtocolTcpServer(Ip, ServerPort);

            sv.StartWork();

            Console.ReadLine();

        }

        private static void SimplyListen()
        {
            SimplyListener sl = new SimplyListener("127.0.0.1", 2222);

            sl.StartWork();

            Console.ReadLine();
        }
    }
}
