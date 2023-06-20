using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace XP.Util.UT
{
    [TestClass]
    public class TextUT
    {
        [TestMethod]
        public void TestMethod1()
        {
            string str1 = @"D:\EDWorks\laserspeed-v1\xunlei_datasrv\API\Export\TeamCount_20220505_205357.xlsx";


            string str2 = str1.ReplaceAll("\\", "\\\\");

            x.Say(str2);
            string str3 = str1.Replace("\\", "\\\\");

            x.Say(str3);

        }
    }
}
