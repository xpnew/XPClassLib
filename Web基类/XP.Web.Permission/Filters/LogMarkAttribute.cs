using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Web.Permission.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class LogMarkAttribute : Attribute
    {


        public bool SkipLog { get; set; }


        public Type ModelType { get; set; }

        public string PropertyNames { get; set; }
        public string RequestNames { get; set; }
        public string[] RequestNameList { get; set; }

        public List<string> PropertyNameList { get; set; }

    }
}
