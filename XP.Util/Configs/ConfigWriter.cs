using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Util.TextFile;

namespace XP.Util.Configs
{

    /// <summary>
    /// 配置文件写入工具
    /// </summary>
    public class ConfigWriter : BaseXmlWriter
    {

        public ConfigWriter(string path) : base(path)
        {

        }


        protected override void _Init()
        {
            base._Init();


        }


        public void SaveSet(string setName, string val)
        {


            var g1 = xd.Root.Element("SiteSet");


            var note1 = from item in g1.Elements("add")
                        where item.Attribute("name").Value.Equals(setName)
                        select item;

            var TestSet = note1.First();


            var OldValue = TestSet.Attribute("value").Value;

            TestSet.SetAttributeValue("value", val);
            TestSet.SetAttributeValue("LastUpdate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            Save();
        }



    }
}
