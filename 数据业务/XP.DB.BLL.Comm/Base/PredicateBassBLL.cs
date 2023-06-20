using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using QY.Util.TypeCache;

using System.Linq.Expressions;
//using System.Transactions;

namespace Ljy.DB.BLL.Base
{
    public class PredicateBassBLL<TEntity> : BaseDLL where TEntity : class, new()
    {
        protected Type InnerType { get; set; }

        //public EntityTypesCacheItem TypeCache { get; set; }


        public Expression<Func<TEntity, bool>> Predicate { get; set; }


        public Expression<Func<TEntity, bool>> IdxPredicate { get; set; }
        public Expression<Func<TEntity, bool>> IdxListPredicate { get; set; }

        public Expression<Func<TEntity, bool>> ModelPredicate { get; set; }


        public Guid CurrentIdx { get; set; }

        public List<Guid> IdxList { get; set; }

        public TEntity CurrentModel { get; set; }


        public EntityCloneUtil<TEntity> CloneUtil { get; set; }

        public PredicateBassBLL() : base()
        {
            Init();
        }
        public PredicateBassBLL(string connString)
            : base(connString)
        {
            Init();
        }

        protected virtual void Init()
        {
            InnerType = typeof(TEntity);
            var CacheManage = EntityTypesCache.CreateInstance();

            TypeCache = CacheManage.GetItem(InnerType);

            CloneUtil = new EntityCloneUtil<TEntity>();

            InitPredicate();
            //InitIdentityList();
            //InitUIMemberList();
            //InitPrimaryKey();
        }


        #region  派生类具体实现
        /// <summary>
        /// 初始化谓词，派生类实现这个就可以了。
        /// </summary>
        protected virtual void InitPredicate()
        {

        }

        public virtual void SetPredicate(Guid idx)
        {


        }

        public virtual void SetPredicate(List<Guid> idList)
        {


        }

        #endregion




        public TEntity GetOne(Guid idx)
        {
            //SetPredicate(idx);
            CurrentIdx = idx;
            var DT = DataContext.GetTable<TEntity>();

            if (null == IdxPredicate)
            {

                return DT.First();
            }

            var list = DT.Where(IdxPredicate);

            if (list.Any())
            {
                return list.First();
            }
            return null;
        }


        public List<TEntity> GetAll()
        {
            var DT = DataContext.GetTable<TEntity>();
            return new List<TEntity>();
        }


        public List<TEntity> Search(Expression<Func<TEntity, bool>> predicate)
        {
            var DT = DataContext.GetTable<TEntity>();
            var t = DT.Where(predicate);
            if (t.Any())
            {
                return t.ToList();
            }
            return new List<TEntity>();
        }



        public bool Insert(TEntity inputModel)
        {
            try
            {
                using (TransactionScope tscope = new TransactionScope())
                {
                    var DT = DataContext.GetTable<TEntity>();
                    DT.InsertOnSubmit(inputModel);
                    DataContext.SubmitChanges();
                    tscope.Complete();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }


        }

        public bool Update(TEntity inputModel)
        {

            CurrentModel = inputModel;
            try
            {
                using (TransactionScope tscope = new TransactionScope())
                {
                    var DT = DataContext.GetTable<TEntity>();
                    var ExistModels = DT.Where(ModelPredicate);
                    if (ExistModels.Any())
                    {
                        var DbModel = ExistModels.First();
                        //DataContext.
                        CloneUtil.CloneTo(inputModel, DbModel);
                        //DT.Attach(inputModel, DbModel);
                        DataContext.SubmitChanges();
                        tscope.Complete();
                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public bool DeleteIdx(Guid id)
        {

            return DeleteIdxList(new List<Guid>() { id });
        }

        public bool DeleteIdxList(List<Guid> idList)
        {
            SetPredicate(idList);
            IdxList = idList;
            try
            {
                using (TransactionScope tscope = new TransactionScope())
                {
                    var DT = DataContext.GetTable<TEntity>();
                    var ExistModels = DT.Where(IdxListPredicate);
                    if (ExistModels.Any())
                    {
                        DT.DeleteAllOnSubmit(ExistModels);
                        tscope.Complete();
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }

}
