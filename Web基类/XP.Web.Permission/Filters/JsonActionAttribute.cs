/*****************************************************************************
Author: xpnew
Mail: xpnew@126.com
Time: 20:35:21/2019-4-12
Name: 
FileName: Jsonactionfilterattribute.cs
Function:
Version:
*****************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Web.Permission
{
    /// <summary>
    /// Json动作过滤器，标记了这个特性以后，就表示这个动作返回的是JSON对象
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class JsonActionAttribute : Attribute
    {
        
    }
}
