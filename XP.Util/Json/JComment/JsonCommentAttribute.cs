using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Util.Json
{


    /// <summary>
    /// ▲▲▲(否决)添加在模型上的注释，可以是类，也可以是属性
    /// </summary>
    /// <remarks>
    /// 同时在实体类和实体属性上上添加这个属性，会造成递归异常
    /// 所以，改为分别设置类和属性的注释
    /// JCommentEntityAttribute
    /// JCommentPropertyAttribute
    /// </remarks>
    /// 
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]

     public class JsonCommentAttribute:Attribute
    {

        public JsonCommentAttribute(string comment)
        {
            Comment = comment;
        }

        public string Comment { get; set; }

    }
}
