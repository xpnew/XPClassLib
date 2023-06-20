using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace XP.Util.Json
{
    public class JCommentContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> list = base.CreateProperties(type, memberSerialization);


            //此处自己处理属性（已经放弃）

            // CreateProperty(type, memberSerialization);
            //base.CreateProperties(type, memberSerialization);
            //foreach (var item in list)
            //{
            //    item.Ignored = false;
            //}                        
            return list.ToList();
        }

    }
}
