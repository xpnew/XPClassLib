using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Entity
{
    [Serializable]
    public partial class StoreEntityBase<TId> : IdPrimaryKeyBase<TId>, IStoreEntity
    {

        //public virtual TId Id { get; set; }

        public virtual int StoreId { get; set; }

        //public virtual  TId GetNullId()
        //{
        //    return  GetDefaultId();

        //}

        //protected virtual TId GetDefaultId()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
