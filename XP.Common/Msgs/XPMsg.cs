using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XP.Common.Msgs;

namespace XP
{
    public class XPMsg:MsgBase
    {
        public override void AddLog(string logStr)
        {
            base.AddLog(logStr + "        [记录时间：" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss.ffff") + "]");
        }


        public void AddLog(string logStr, bool enableTime)
        {
            if (enableTime)
            {
                this.AddLog(logStr);
            }
            else
            {
                base.AddLog(logStr);
            }
        }



    }
}
