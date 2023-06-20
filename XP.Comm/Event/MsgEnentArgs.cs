using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Event
{
    public class MsgEnentArgs<T> : System.EventArgs
    where T : IMsg, new()
    {
        public T Log { get; set; }


        public MsgEnentArgs() : base()
        {
            Log = new T();
        }

        public MsgEnentArgs(string tit, string cot = null) : this()
        {
            Log.Title = tit;
            Log.Body = cot;
        }
    }
}
