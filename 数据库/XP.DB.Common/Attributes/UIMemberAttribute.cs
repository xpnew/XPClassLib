using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Attributes
{
    /// <summary>
    /// 只用在UI方面的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public class UIMemberAttribute : Attribute
    {
    }
}
