using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;



namespace XP.Util.Net
{
    /// <summary>
    /// IP地址和名称工具
    /// </summary>
    public static class IPvsNameTools
    {


        public static string GetIp()
        {
            ///获取本地的IP地址
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            return AddressIP;

        }

        public static string GetHostName()
        {
            //获取PCname

            string pcname = Dns.GetHostName();
            return pcname;
        }

        public static string GetIpAndName()
        {
            string Result = "未知名称和地址";
            string IP = GetIp();

            string Name = GetHostName();
            if (String.IsNullOrEmpty(IP) && String.IsNullOrEmpty(Name))
            {
                return Result;
            }
            Result = IP;
            if (!String.IsNullOrEmpty(Name))
            {
                Result += "(" + Name + ")";
            }
            return Result;
        }

    }
}
