using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XP.Common;
using XP.Common.DosCmd;

namespace XP.Sockets.Protocols
{

    /// <summary>
    /// 控制协议
    /// </summary>
    public class ControlPotocol : BaseProtocol
    {

        private string _HeadSymbol = "FC01";

        public override string HeadSymbol
        {
            get
            {
                return _HeadSymbol;
            }
            set
            {
                _HeadSymbol = value;
            }
        }
        public ControlPotocol() : base() { }

        public ControlPotocol(byte[] input)
            : base(input)
        {

        }

        public void RunCommand()
        {
            if (1 == Content[0])
            {
                Loger.Debug("准备关闭服务器！");
                bool WinCloseResult = NormalCmd.RebootWindow(30);
                if (!WinCloseResult)
                {
                    XP.Common.API.CloseWin.DoExitWindows(Common.API.CloseWin.ExitWindows.Reboot);
                }
            }
        }


        protected override void InitProviders()
        {
            base.InitProviders();
            Providers.Add(new XORCheckoutProvider());
            Providers.Add(new SimplySecurityProvider());
        }

        #region 根据内容创建协议


        public override byte[] MakeEnding(byte[] input)
        {
            var p = new XORCheckoutProvider();
            var s = new SimplySecurityProvider();
            byte[] Ending = new byte[2];
            Ending[1] = p.MakeEnding(input)[0];
            Ending[0] = s.MakeEnding(input)[0];

            return Ending;
        }
        public override byte[] MakeCheckOutSymbol()
        {
            XORCheckoutProvider p = new XORCheckoutProvider();

            string s = p.Symbol;
            byte[] Result = ProtocolUtil.X2String2Bytes(s);
            return Result;
        }

        #endregion


    }
}
