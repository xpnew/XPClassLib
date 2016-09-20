using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace XP.Comm.Filters
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NotDataFilterAttribute : Attribute
    {


        public static bool IsDefined(PropertyInfo property)
        {
            object[] attrsAllows = property.GetCustomAttributes(typeof(NotDataFilterAttribute), true);
            if (0 < attrsAllows.Length)
            {
                return true;
            }
            return false;
        }
    }
}
