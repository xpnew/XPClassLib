using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace XP.Svc.BaseHttp
{

    /// <summary>
    /// 基础的http服务
    /// </summary>
    public class BaseSvc
    {

        private string _HostName_Default = "localhost";
        private string _HostName;


        public int Port { get; set; } = Comm.Constant.NullInt;

        public string HostName { 
            get
            {
                if (String.IsNullOrEmpty(_HostName)) _HostName = _HostName_Default;
                return _HostName;
            } 
            set { _HostName = value; }
        }



        public  void Run()
        {


            RunServer();


        }

        protected virtual void RunServer()
        {
            if (Comm.Constant.NullInt == Port)
            {
                throw new ArgumentNullException("Port", "参数错误，请指定服务使用（绑定）的端口");
            }

            string IP = "127.0.0.1";
            var server = new HttpListener();
            var url = $"http://{HostName}:{Port}/";
            server.Prefixes.Add(url);
            server.Start();
            Console.WriteLine("Http服务器已开启，用浏览器访问:" + url);

        }

    }
}
