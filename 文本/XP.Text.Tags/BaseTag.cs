using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Text.Tags
{
    public class BaseTag
    {

        /// <summary>
        /// 标签的名字
        /// </summary>
        public string TagName { get; set; }
        /// <summary>
        /// 标签的前导符
        /// </summary>
        public string TagPrefix { get; set; }

        /// <summary>
        /// 标签的前部符号
        /// </summary>
        public string PrefixChar { get; set; }
        /// <summary>
        /// 标签的尾部符号
        /// </summary>
        public string SuffixChar { get; set; }

        /// <summary>
        /// 标签的全部设定，包括标签的名称和各种属性
        /// </summary>
        public string TagSetting { get; set; }

        /// <summary>
        /// 标签的内容，单标签为空
        /// </summary>
        public string TagContent { get; set; }

        /// <summary>
        /// 节点内的文本，Capture.Value
        /// </summary>
        public string NoteText { get; set; }

        /// <summary>
        /// 节点的起始位置（0索引），Capture.Index
        /// </summary>
        public int NoteStart { get; set; }
        /// <summary>
        /// 节点的大小，Capture.Length
        /// </summary>
        public int NoteSize { get; set; }
        

        /// <summary>
        /// 标签类型
        /// </summary>
        public TagType Type { get;set; }

        public List<TagAttribute> Attributes { get; set; }


        public BaseTag()
        {
            _Init();
        }


        protected virtual void _Init()
        {
            Attributes = new List<TagAttribute>();
        }
    }
}
