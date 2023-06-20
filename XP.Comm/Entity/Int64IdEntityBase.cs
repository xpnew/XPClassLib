using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Entity
{
    [Serializable]
    public class Int64IdEntityBase : IdPrimaryKeyBase<long>
    {

        protected override long GetDefaultId()
        {
            return -1;
        }

    }
}
