using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace XP.Comm.Json
{
    /// <summary>
    /// 不使用字段设置上的忽略设置，包括全部字段，输出JSON的方式 
    /// </summary>
    public class NoIgnoreContractResolver : DefaultContractResolver
    {

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> list = base.CreateProperties(type, memberSerialization);

            // CreateProperty(type, memberSerialization);
            //base.CreateProperties(type, memberSerialization);
            foreach (var item in list)
            {
                item.Ignored = false;
            }
            return list.ToList();
        }
    }
}
