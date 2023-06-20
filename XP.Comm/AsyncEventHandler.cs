using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Comm
{
    public delegate Task AsyncEventHandler<in TEventArgs>(object sender, TEventArgs e);



    public delegate Task AsyncEventHandler<in TEventArgs, in TEventArgs2>(object sender, TEventArgs e, TEventArgs2 e2);
}
