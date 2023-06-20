using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace XP.Util.Json
{
    public class JsonHelperVSComment
    {


        
        public static string ToJson(object o)
        {
            JsonSerializerSettings setting;


            setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;
            setting.DateFormatString = "yyyy-MM-dd HH:mm:ss.sss";
            setting.ContractResolver = new JCommentContractResolver();


            return JsonConvert.SerializeObject(o, setting);


        }


    }
}
