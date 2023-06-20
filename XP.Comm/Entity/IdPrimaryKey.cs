using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Entity
{
    public interface IdPrimaryKey<TId>
    {

        TId Id { get; set; }


        /// <summary>
        /// 无法正常获取Id时，返回的值 
        /// </summary>
        /// <returns></returns>
        TId GetNullId();


        //Expression<Func<TId, bool>> IdxPredicate { get; set; }
        ////Expression<Func<TId, bool>> IdxListPredicate { get; set; }
        //Expression<Func<IdPrimaryKey<TId>, bool>> ModelPredicate { get; set; }


        //Expression<Func<TId, bool>> IdxListPredicate(IdPrimaryKey<TId> entity, IList<TId> idlist);
        //Expression<Func<TId, bool>> ModelPredicate(TId id);

    }
}
