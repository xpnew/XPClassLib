using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XP.Util.Configs;
using XP.Util.Path;

namespace XP.Util.UT
{
    [TestClass]
    public class UnitTest1
    {

        #region 基本的读取测试
        [TestMethod]
        public void GetFile()
        {
            string FileName = "TestWriter01.xml";


            string Fullname = PathTools.GetFull(FileName);


            Assert.IsTrue(System.IO.File.Exists(Fullname));

            XDocument xd = XDocument.Load(Fullname);

            Assert.IsNotNull(xd);


        }



        [TestMethod]
        public void 读取一个配置项EnableDebug()
        {
            XDocument xd = GetTestFile();

            string setName = "EnableDebug";

            var g1 = xd.Root.Element("SiteSet");


            var note1 = from item in g1.Elements("add")
                        where item.Attribute("name").Value.Equals(setName)
                        select item;

            Assert.IsNotNull(note1);

            string val = note1.Attributes("value").First().Value;


            Assert.IsNotNull(val);
            x.Say(" 读取一个配置项EnableDebug " + val);
        }




        [TestMethod]
        public void 测试一个非法的节点()
        {

            //要求配置文件当中存在一个缺少“name”属性的节点

            XDocument xd = GetTestFile();

            string setName = "EnableDebug";

            var g1 = xd.Root.Element("SiteSet");


            var note1 = from item in g1.Elements("add")
                        where item.Attribute("name").Value.Equals(setName)
                        select item;

            Assert.IsNotNull(note1);

            string val = note1.Attributes("value").First().Value;
            Assert.IsNotNull(val);
            x.Say("(辅助参考：) 读取一个配置项EnableDebug " + val);


            var note2 = from item in g1.Elements("add")
                        where item.Attribute("value").Value.Equals("测试一个非法的节点")
                        select item;

            Assert.IsNotNull(note2);

            foreach(var elm in note2)
            {
                x.Say("缺少name属性的节点：" + elm.ToString());

            }


        }

        #endregion


        #region 基本的写入测试
      

        [TestMethod]
        public void 添加一个节点读取()
        {
            XDocument xd = GetTestFile();

            string setName = "EnableDebug";

            var g1 = xd.Root.Element("SiteSet");


            g1.Add(new XElement("Child1", 1));


            string t2 = "TestWriter000000.xml";
            SaveTestFile(t2, xd);

            var xd2 = GetTestFile(t2);


            var g2 = xd.Root.Element("SiteSet");

            var note = g2.Element("Child1");

            Assert.IsNotNull(note);




        }


        [TestMethod]
        public void 修改一个节点属性并读取()
        {
            XDocument xd = GetTestFile();

            string setName = "ProxyServerPort";

            var g1 = xd.Root.Element("SiteSet");


            var note1 = from item in g1.Elements("add")
                        where item.Attribute("name").Value.Equals(setName)
                        select item;

            var TestSet = note1.First();

            int OldValue = int.Parse(TestSet.Attribute("value").Value);

            int NewValue = OldValue == 6677 ? 5678 : 6677;

            TestSet.SetAttributeValue("value", NewValue);
            TestSet.SetAttributeValue("LastUpdate", DateTime.Now.ToString("yyMMdd HHmmss"));

            SaveTestFile(_DefaultName, xd);



            var xd2 = GetTestFile(_DefaultName);


            var g2 = xd.Root.Element("SiteSet");


            var note2 = from item in g1.Elements("add")
                        where item.Attribute("name").Value.Equals(setName)
                        select item;




            Assert.IsNotNull(note2);
            int WillValue = NewValue;
            int RealValue =int.Parse(note2.First().Attribute("value").Value);
            Assert.AreEqual(WillValue, RealValue);




        }



        #endregion
        #region 配置文件修改


        [TestMethod]
        public void 修改一个配置读取()
        {
            var cw = new ConfigWriter(_DefaultName);


            XDocument xd = GetTestFile();

            string setName = "ProxyServerPort";

            var g1 = xd.Root.Element("SiteSet");


            var note1 = from item in g1.Elements("add")
                        where item.Attribute("name").Value.Equals(setName)
                        select item;

            var TestSet = note1.First();

            int OldValue = int.Parse(TestSet.Attribute("value").Value);


            int NewValue = OldValue == 6677 ? 5678 : 6677;

            cw.SaveSet(setName, NewValue.ToString());

            var xd2 = GetTestFile(_DefaultName);


            var g2 = xd.Root.Element("SiteSet");


            var note2 = from item in g1.Elements("add")
                        where item.Attribute("name").Value.Equals(setName)
                        select item;
             



            Assert.IsNotNull(note2);
            int WillValue = NewValue;
            int RealValue = int.Parse(note2.First().Attribute("value").Value);
            Assert.AreEqual(WillValue, RealValue);

        }



        #endregion



        private static string _DefaultName = "TestWriter01.xml";

        [Ignore]
        private static XDocument GetTestFile()
        {
            string FileName = _DefaultName;

            return GetTestFile(FileName);
        }

        [Ignore]
        private static XDocument GetTestFile(string filename)
        {
            string Fullname = PathTools.GetFull(filename);



            XDocument xd = XDocument.Load(Fullname);
            return xd;
        }


        [Ignore]
        private static void SaveTestFile(string filename, XDocument xd)
        {
            string Fullname = PathTools.GetFull(filename);


            xd.Save(Fullname);
           
        }


        //protected XDocument GetTestFile()
        // {


        // }
    }
}
