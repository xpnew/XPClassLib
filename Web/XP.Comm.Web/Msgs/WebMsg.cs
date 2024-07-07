using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XP.Comm.Msgs;
using XP.Common.Serialization;

namespace XP.Comm
{
    public class WebMsg : XP.Comm.CommMsg
    {

        public override string ToString()
        {
            //return base.ToString();
            return JsonConvert.SerializeObject(this);

            //return JsonHelper.Serialize<WebMsg>(this);
        }
    }
}
