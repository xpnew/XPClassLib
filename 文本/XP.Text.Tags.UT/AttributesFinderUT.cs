using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


using XP.Text.Tags;
using XP.Text.Tags.TMTags;

namespace XP.Text.Tags.UT
{
    [TestClass]
    public class AttributesFinderUT
    {
        [TestMethod]
        public void T_只有一个单属性()
        {

            var Input = "abc";

            AttributesFinder finder = new AttributesFinder(Input);

            var attr = finder.Next();

            Assert.AreEqual(attr.Type,  AttributeType.Single);

            Assert.AreEqual(attr.Name, Input);
            Assert.AreEqual(attr.Name, attr.Value);
        }

        [TestMethod]
        public void T_多个单属性()
        {

            var Input = "abc def";

            AttributesFinder finder = new AttributesFinder(Input);

            var attr = finder.Next();

            Assert.AreEqual(attr.Type, AttributeType.Single);

            Assert.AreEqual(attr.Name, attr.Value);

            var Att2 = finder.Next();
            Assert.AreEqual(Att2.Type, AttributeType.Single);
            Assert.AreEqual(Att2.Name, Att2.Value);

        }
        [TestMethod]
        public void T_错误的标签()
        {

            var Input = "abc def=";

            AttributesFinder finder = new AttributesFinder(Input);

            var attr = finder.Next();

            Assert.AreEqual(attr.Type, AttributeType.Single);

            Assert.AreEqual(attr.Name, attr.Value);

            var AttErr = finder.Next();
            Assert.AreEqual(AttErr.Type, AttributeType.Error);

        }

        [TestMethod]
        public void T_错误的标签__缺少成对的引号()
        {

            var Input = "abc def=\"";

            AttributesFinder finder = new AttributesFinder(Input);

            var attr = finder.Next();

            Assert.AreEqual(attr.Type, AttributeType.Single);

            Assert.AreEqual(attr.Name, attr.Value);

            var AttErr = finder.Next();
            Assert.AreEqual(AttErr.Type, AttributeType.Error);

        }

        [TestMethod]
        public void T_普通属性()
        {

            var Input = "abc=\"0\"";

            AttributesFinder finder = new AttributesFinder(Input);

            var attr = finder.Next();

            Assert.AreEqual(attr.Type, AttributeType.Normal);

            Assert.AreEqual(attr.Name, "abc");
            Assert.AreEqual(attr.Value, "0");

        }

        [TestMethod]
        public void T_单属性混合普通属性()
        {

            var Input = "abc def=\"0\"";

            AttributesFinder finder = new AttributesFinder(Input);

            var attr = finder.Next();

            Assert.AreEqual(attr.Type, AttributeType.Single);

            Assert.AreEqual(attr.Name, attr.Value);

            var AttErr = finder.Next();
            Assert.AreEqual(AttErr.Type, AttributeType.Normal);

        }

        [TestMethod]
        public void T_普通属性混合单属性()
        {

            var Input = "abc=\"0\" def";

            AttributesFinder finder = new AttributesFinder(Input);

            var attr = finder.Next();

            Assert.AreEqual(attr.Type, AttributeType.Normal);


            var AttErr = finder.Next();
            Assert.AreEqual(AttErr.Type, AttributeType.Single);
            Assert.AreEqual(AttErr.Name, AttErr.Value);

        }

        [TestMethod]
        public void T_普通属性x5()
        {

            var Input = "abc=\"0\" def=\"0\" erer=\"0\" ddd=\"0\" ssss=\"0\"";

            AttributesFinder finder = new AttributesFinder(Input);

            var attr1 = finder.Next();

            Assert.AreEqual(attr1.Type, AttributeType.Normal);


            var attr2 = finder.Next();

            Assert.AreEqual(attr2.Type, AttributeType.Normal);
            var attr3 = finder.Next();

            Assert.AreEqual(attr3.Type, AttributeType.Normal);
            var attr4= finder.Next();

            Assert.AreEqual(attr4.Type, AttributeType.Normal);
            var attr5 = finder.Next();

            Assert.AreEqual(attr5.Type, AttributeType.Normal);

        }
    }
}
