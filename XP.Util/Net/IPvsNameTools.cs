using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.NetworkInformation;
using Microsoft.SqlServer.Server;



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


        public static HostNetInfo GetHostAndIP()
        {
            HostNetInfo Result = new HostNetInfo();

            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

            Result.HostName = properties.HostName;
            Result.HostDomain = properties.DomainName;


            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (!adapter.Supports(NetworkInterfaceComponent.IPv4) && !adapter.Supports(NetworkInterfaceComponent.IPv6))
                {
                    continue;
                }
                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                UnicastIPAddressInformationCollection uniCast = adapterProperties.UnicastAddresses;
                if (null == uniCast) continue;

                PhysicalAddress physicalAddress = adapter.GetPhysicalAddress();
                byte[] bytes = physicalAddress.GetAddressBytes();
                string macAddress = BitConverter.ToString(bytes).Replace("-", ":");
                Console.WriteLine("MAC ..................:{0}", macAddress);
                foreach (UnicastIPAddressInformation uni in uniCast)
                {
                    if (IPAddress.IsLoopback(uni.Address)) continue;
                    IPInfo NewItem = new IPInfo()
                    {
                        IP = uni.Address.ToString(),
                        IPv4Mask = uni.IPv4Mask.ToString(),
                        Type = uni.Address.AddressFamily.ToString(),
                        Mac = macAddress
                    };
                    Result.IpList.Add(NewItem);
                }
            }
            return Result;
        }

        public static List<IPAddressItem> GetActiveIpAddress()
        {

            //var lst = NetworkInterface.GetAllNetworkInterfaces().Where(i => i.OperationalStatus == OperationalStatus.Up).Select(i=> new IPAddressItem() { IP =  i.ToString(), 
            //IsActive= true, Type =  i.Add}).ToList();


            var lst = NetworkInterface.GetAllNetworkInterfaces().Where(i => i.OperationalStatus == OperationalStatus.Up).ToList();

            List<IPAddressItem> Result = new List<IPAddressItem>();

            foreach (var ni in lst)
            {



            }





            return null;

        }

        public static string GetIPType(NetworkInterface ni)
        {
            var p = ni.GetIPProperties();

            if (null == p) return "";



            return "other";
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
    public class IPAddressItem
    {
        public string HostName { get; set; }

        public string IP { get; set; }

        public string Type { get; set; }

        public bool IsActive { get; set; }

        public string IPv4Mask { get; set; }

    }
}

