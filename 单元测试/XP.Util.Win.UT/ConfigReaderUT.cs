using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XP.Util.Configs;

namespace XP.Util.Win.UT
{
    [TestClass]
    public class ConfigReaderUT
    {
        [TestMethod]
        public void 基本测试_读取一个选项()
        {


            var cr = ConfigReader.Self;
            Assert.IsNotNull(cr);

            int Port = cr.GetInt("ProxyServerPort", -5555);

            Assert.AreEqual(Port, 6677);
            //System.Threading.Thread.Sleep(5000);

        }

        [TestMethod]
        public void 配置文件更新测试()
        {


            var cr = ConfigReader.Self;
            Assert.IsNotNull(cr);

            int Port = cr.GetInt("ProxyServerPort", -5555);

            //Assert.AreEqual(Port, 6677);


            var cw = new ConfigWriter(cr.FilePath);
            string setName = "ProxyServerPort";
            int NewValue = Port == 6677 ? 5678 : 6677;
            cw.SaveSet(setName, NewValue.ToString());

            System.Threading.Thread.Sleep(15000);

            int ChangedPort = cr.GetInt("ProxyServerPort", -5555);

            Assert.AreEqual(NewValue, ChangedPort);

        }
        [TestMethod]
        public void 事件监控测试()
        {


            var cr = ConfigReader.Self;
            Assert.IsNotNull(cr);

            int Port = cr.GetInt("ProxyServerPort", -5555);

            //Assert.AreEqual(Port, 6677);

            cr.ChangedNotify += (o, arg) => {
                x.Say("配置文件已经修改！" +DateTime.Now);
            
            };


            var cw = new ConfigWriter(cr.FilePath);
            string setName = "ProxyServerPort";
            int NewValue = Port == 6677 ? 5678 : 6677;
            cw.SaveSet(setName, NewValue.ToString());

            System.Threading.Thread.Sleep(5000);


            x.Say("测试结束！" + DateTime.Now);


            //int ChangedPort = cr.GetInt("ProxyServerPort", -5555);

            //Assert.AreEqual(NewValue, ChangedPort);



        }
    }
}
