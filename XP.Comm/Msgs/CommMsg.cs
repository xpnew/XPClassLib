using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm
{
    public class CommMsg : Msgs.MsgBase
    {
        public CommMsg() : base() { }
        public CommMsg(object entity) : base(entity) { }
        public CommMsg(Type type) : base(type) { }
        public CommMsg(string name) : base(name) { }


    }
}
