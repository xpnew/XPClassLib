using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Entity
{
    /// <summary>
    /// 标准的整数型商户实体
    /// </summary>
    [Serializable]
    public class IntIdStoreEntityBase : StoreEntityBase<int>
    {

        protected override int GetDefaultId()
        {
            return -1;
        }

    }
}
