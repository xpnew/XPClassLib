using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace XP.Util.Configs
{
    /// <summary>
    /// 将配置文件当中的某一系列分组数据转换为List
    /// </summary>
    ///<remarks>
    ///已知问题、限制汇总
    /// 1、大小写的问题注意！
    /// 2、Int类型的节点，数据文本不能为空，即便是int?的属性也不能映射成功
    /// 即不允许这样：《book》《/book》
    /// 3、Int类型的节点如果省略，相应的值会被当作0来处理，而int?类型的数据节点省略的时候则是null
    ///
    /// </remarks>
    /// 
    public class GroupToList<T>
    {

        public bool ConfigReady { get; set; }


        private List<T> _Items;
        private ConfigReader _cr;
        /// <summary>
        /// 经过序列化生成的结果
        /// </summary>
        public List<T> Items
        {
            get
            {
                return _Items;
            }
            set
            {
                _Items = value;
            }
        }
        public string GroupName { get; set; }

        protected GroupToList()
        {
            _Items = new List<T>();
            _cr = ConfigReader.CreateInstance();

        }
        public GroupToList(string groupName) : this()
        {
            GroupName = groupName;
        }

        public List<XElement> GetList()
        {
            return _cr.GetList(GroupName);
        }

        public void StartDeserialize()
        {
            var Nodes = GetList();
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            foreach (XElement node in Nodes)
            {
                string str1 = node.ToString();
                string str2 = node.Descendants().ToString();

                //str1 = @"<?xml version=""1.0"" encoding=""utf-8""?>" + str1;
                //str2 = @"<?xml version=""1.0"" encoding=""utf-8""?>" + str2;

                StringReader sr_onlyNode = new StringReader(str1);
                //★★★ 注意！！！ 反序列化失败的一个很常见也很蛋疼的原因是大小写问题！！！
                T Result = (T)serializer.Deserialize(sr_onlyNode);
                _Items.Add(Result);



            }
        }
    }
}
