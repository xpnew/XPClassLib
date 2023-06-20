using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace XP.Util.Json
{
    public    class DynamicTools
    {

        public static bool IsPropertyExist(dynamic data, string propertyname)
        {
            if (data is ExpandoObject)
                return ((IDictionary<string, object>)data).ContainsKey(propertyname);

            if (data is JObject)
            {
                var jo = data as JObject;
                if (0 == jo.Count)
                {
                    return false;
                }
                return jo.Property(propertyname) != null;
            }
            var t = data.GetType();

            var p = t.GetProperties();

            return data.GetType().GetProperty(propertyname) != null;
        }
    }
}
