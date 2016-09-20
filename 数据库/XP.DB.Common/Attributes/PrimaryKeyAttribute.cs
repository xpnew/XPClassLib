using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Attributes
{
    [AttributeUsage(AttributeTargets.Property| AttributeTargets.Parameter, Inherited = false, AllowMultiple = true)]
    public class PrimaryKeyAttribute : Attribute
    {
    }
}
