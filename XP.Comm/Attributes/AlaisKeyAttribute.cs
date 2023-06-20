using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Attributes
{
    /// <summary>
    /// 辅助用来序列化的key
    /// </summary>
    /// <remarks>
    /// 
    /// 
    ///
    /// </remarks>
    public class AlaisKeyAttribute : Attribute
    {
        public string AliasName { get; set; }

        public AlaisKeyAttribute(string name)
        {
            AliasName = name;
        }

    }
}
