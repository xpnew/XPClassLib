using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm
{
    public class DataMsg<T> : CommMsg where T : class,new()
    {

        public DataMsg()
            : base()
        {
            DataInfo = new T();
        }



        public T DataInfo { get; set; }
    }
}
