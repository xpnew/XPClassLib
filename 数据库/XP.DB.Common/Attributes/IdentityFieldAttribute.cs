using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Attributes
{
    /// <summary>
    /// 自增列标记
    /// </summary>

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public class IdentityFieldAttribute : Attribute
    {

    }
}
