using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Util.Net
{

    [Serializable]
    public class HostNetInfo
    {

        public string HostName { get; set; }

        public string HostDomain { get; set; }

        public List<IPInfo> IpList { get; set; } = new List<IPInfo>();

    }
}
