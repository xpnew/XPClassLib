using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Entity
{
    [Serializable]
    public class IntIdEntityBase : IdPrimaryKeyBase<int>
    {
        protected override int GetDefaultId()
        {
            return -1;
        }


    }
}
