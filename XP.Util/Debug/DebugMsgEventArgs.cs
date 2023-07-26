using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Util.Debug
{
    public class DebugMsgEventArgs : EventArgs
    {

        public DebugMsg MsgContent { get; set; }


        public DebugMsgEventArgs(DebugMsg msg)
            : base()
        {
            MsgContent = msg;
        }

    }
}
