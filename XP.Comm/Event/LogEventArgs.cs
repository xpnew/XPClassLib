using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XP.Comm.Logs;

namespace XP.Comm.Event
{
    public class LogEventArgs<T> : System.EventArgs
    where T : ILogElement, new()
    {
        public T Log { get; set; }


        public LogEventArgs() : base()
        {
            Log = new T();
        }

        public LogEventArgs(string tit, string cot = null) : this()
        {
            Log.Tit = tit;
            Log.Cot = cot;
        }
    }
}
