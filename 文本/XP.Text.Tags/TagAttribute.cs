using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Text.Tags
{
    public class TagAttribute
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public AttributeType Type { get; set; }

        public AttributLocations Location { get; set; }

    }

    /// <summary>
    /// 属性的类型
    /// </summary>
    public enum AttributeType
    {
        /// <summary>
        /// 默认值
        /// </summary>
        NONE = 0,
        /// <summary>
        /// 单独值，readonly  checked disable 
        /// </summary>
        Single = 1,

        /// <summary>
        /// 标准属性
        /// </summary>
        Normal = 2,
        /// <summary>
        /// 错误的属性，没有属性名
        /// </summary>
        Error = -1

    }


    /// <summary>
    /// 关于属性的各种定位
    /// </summary>
    public struct AttributLocations
    {
        /// <summary>
        /// 属性的起始定位
        /// </summary>
        public int Start;
        /// <summary>
        /// 属性的结束位置，即结束的那个引号的位置索引
        /// </summary>
        public int End;


        public int NameStart;
        public int NameEnd;
        public int ValueStart;

        public int ValueEnd;
    }

}
