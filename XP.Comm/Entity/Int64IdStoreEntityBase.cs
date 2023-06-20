using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Entity
{

    /// <summary>
    /// 64位的整数型商户实体
    /// </summary>
    [Serializable]
    public class Int64IdStoreEntityBase : StoreEntityBase<long>
    {

        protected override long GetDefaultId()
        {
            return -1;
        }


    }
}
