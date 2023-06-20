using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Entity
{
    [Serializable]
    public class IdPrimaryKeyBase<TId> : IdPrimaryKey<TId>
    {


        public virtual TId Id { get; set; }


        /// <summary>
        /// 无法正常获取Id时，返回的值 
        /// </summary>
        /// <returns></returns>
        public virtual TId GetNullId()
        {
            return GetDefaultId();

        }

        protected virtual TId GetDefaultId()
        {
            throw new NotImplementedException();
        }

        //public Expression<Func<TId, bool>> IdxPredicate { get; set; }
        ////public Expression<Func<TId, bool>> IdxListPredicate { get; set; }
        //public Expression<Func<IdPrimaryKey<TId>, bool>> ModelPredicate(TId id)
        //{

        //}

        //public Expression<Func<TId, bool>> IdxListPredicate(IdPrimaryKey<TId> entity, IList<TId> idlist)
        //{
        //    Expression<Func<TId, bool>> Rusult =  u => idlist.Contains(entity.Id);
        //    return Rusult;
        //}
    }
}
