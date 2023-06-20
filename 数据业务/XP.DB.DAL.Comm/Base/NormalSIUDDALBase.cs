using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Comm.Entity;
using SqlSugar;
using System.Linq.Expressions;

namespace XP.DB.DAL.Bases
{
    /// <summary>
    /// 标准的增删改查基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdType"></typeparam>
    public class NormalSIUDDALBase<TEntity, TIdType> : BaseDALVsMsg
        where TEntity : class, Comm.Entity.IdPrimaryKey<TIdType>, new()
    {
        /// <summary>
        /// BLL 层需要这个泛型接口
        /// </summary>
        public NormalSIUDDALBase()
        {

        }


        public TDal GreatDal<TDal>()
        where TDal : NormalSIUDDALBase<TEntity, TIdType>, new()
        {
            return new TDal();
        }


        public override void MsgErr(string error, Exception ex)
        {
            base.MsgErr(error, ex);
            Loger.Error(error, ex);

        }

        #region 标准增删改查
         #region 基础的查询方法



        public virtual List<TEntity> GetList(SqlSugarClient db = null)
        {
            if (null == db)
            {
                using (db = DbHelper.Instance)
                {
                    return GetList(db);
                }
            }

            var Result = db.Queryable<TEntity>().ToList();
            return Result;
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate, SqlSugarClient db = null)
        {
            if (null == db)
            {
                using (db = DbHelper.Instance)
                {
                    return GetList(predicate, db);
                }
            }
            var Result = db.Queryable<TEntity>().Where(predicate).ToList();
            return Result;        }


        public virtual List<TView> GetViews<TView>(SqlSugarClient db = null)
        {

            if (null == db)
            {
                using (db = DbHelper.Instance)
                {
                    return GetViews<TView>(db);
                }
            }
            var Result = db.Queryable<TView>().ToList();
            return Result;
        }

        public List<TView> GetQueryViews<TView>(Expression<Func<TView, bool>> predicate, SqlSugarClient db = null)
        {
            if (null == db)
            {
                using (db = DbHelper.Instance)
                {
                    return GetQueryViews<TView>(predicate,db);
                }
            }

            var Result = db.Queryable<TView>().Where(predicate).ToList();
            return Result;

        }

        public List<TView> GetQueryViews<TView>(string sql, SqlSugarClient db = null) where  TView :class,new()
        {
            if (null == db)
            {
                using (db = DbHelper.Instance)
                {
                    return GetQueryViews<TView>(sql,db);
                }
            }

            var Result = db.SqlQueryable<TView>(sql).ToList();
            return Result;

        }
        public virtual TView GetViewById<TView>(TIdType id,SqlSugarClient db = null)
        where TView:IdPrimaryKey<TIdType>
        {

            if (null == db)
            {
                using (db = DbHelper.Instance)
                {
                    return GetViewById<TView>(id,db);
                }
            }

            List<IConditionalModel> conModels = new List<IConditionalModel>();
            conModels.Add(new ConditionalModel() { FieldName = "id", ConditionalType = ConditionalType.Equal, FieldValue = id.ToString() });
            var view = db.Queryable<TView>().Where(conModels).First();
            return view;
        }

        public List<TEntity> Top(SqlSugarClient db, int top)
        {
            var queryable = db.Queryable<TEntity>();
            queryable.Take(top);
            var Result = queryable.ToList();
            return Result;
        }

        public virtual List<TEntity> GetList(string strWhere,string filedOrder =null,int top = -1, SqlSugarClient db = null)
        {
            if (null == db)
            {
                using (db = DbHelper.Instance)
                {
                    return GetList(db);
                }
            }
            var queryable = db.Queryable<TEntity>();

            queryable.Where(strWhere);
            if (!String.IsNullOrWhiteSpace(filedOrder))
            {
                queryable.OrderBy(filedOrder);
            }
            queryable.OrderBy(t => t.Id, OrderByType.Desc);
            if (0 < top)
            {
                queryable.Take(top);
                var Result = queryable.ToList();
                return Result;

            }
            else
            {
                var Result = queryable.ToList();
                return Result;
            }

        }

        public virtual TEntity GetItemById(TIdType id, SqlSugarClient db = null)
        {
            if (null == db)
            {
                using (db = DbHelper.Instance)
                {
                    return GetItemById(id, db);
                }
            }
            TEntity item = db.Queryable<TEntity>().Where(s => s.Id.Equals(id)).Single();
            return item;
        }



        public virtual TEntity GetItemById(TIdType id)
        {

            using (var db = DbHelper.Instance)
            {
                TEntity item = db.Queryable<TEntity>().Where(s => s.Id.Equals(id)).Single();
                return item;
            }
        }


        #endregion


        #region 添加

        public virtual TIdType Create(TEntity inputModel, SqlSugarClient db)
        {

            return Create(inputModel, null, db);
        }
        public virtual  TIdType Create(TEntity inputModel, List<string> skipColumnNames = null, SqlSugarClient db = null)
        {
            if (null == inputModel)
            {
                Msg.SetFail("输入的实体对象是空");

                return (new TEntity()).GetNullId();
            }
            if (null == db)
            {
                using (db = DbHelper.Instance)
                {
                    return Create(inputModel, skipColumnNames, db);
                }
            }
            try
            {
                db.Ado.BeginTran();
                TEntity Result = CreateModel(inputModel, db, skipColumnNames );

                if (null == Result)
                {
                    Msg.SetFail("插入数据库失败");
                    db.Ado.RollbackTran();//回滚事务
                    return inputModel.GetNullId();
                }
                else
                {
                    //id = obj.ObjToInt();
                    //item.Id = (TId)obj;
                    db.Ado.CommitTran();
                    Msg.SetOk();
                    return Result.Id;
                }
            }
            catch (Exception ex)
            {
                string ErrTitle = "插入实体对象（" + typeof(TEntity).FullName + "），出现了异常。";
                Err4MsgVSLog(ErrTitle, ex);

                db.Ado.RollbackTran();
                return inputModel.GetNullId();
            }
        }



        /// <summary>
        /// 不需要外部包装的事务
        /// </summary>
        /// <param name="item"></param>
        /// <param name="db"></param>
        /// <param name="skipColumnNames"></param>
        /// <returns></returns>
        public virtual TEntity CreateModel(TEntity item, SqlSugarClient db, List<string> skipColumnNames = null)
        {
            TEntity obj = null;

            if(null == skipColumnNames)
            obj = db.Insertable<TEntity>(item).ExecuteReturnEntity();
            else
                obj = db.Insertable<TEntity>(item).IgnoreColumns(it => skipColumnNames.Contains(it)).ExecuteReturnEntity();
            return obj;
        }


        //public int Create(Menu inputModel)
        //{
        //    using (var db = DbHelper.Instance)
        //    {
        //        return db.Queryable<Menu>().Where(it => it.ID == id).Single();
        //    }
        //}

        //public virtual int Create(TEntity item)
        //{

        //    using (var db = DbHelper.Instance)
        //    {
        //        int id = db.Insertable<TEntity>(item).ExecuteReturnIdentity();
        //        return id;
        //    }
        //}

        //public virtual TIdType Create(TEntity item)
        //{

        //    TEntity obj = null;
        //    using (SqlSugarClient db = DbHelper.Instance)
        //    {
        //        obj = db.Insertable<TEntity>(item).ExecuteReturnEntity();
        //        if (null == obj)
        //        {
        //            db.Ado.RollbackTran();//回滚事务
        //            return item.GetNullId();
        //        }
        //        if (obj != null)
        //        {
        //            //id = obj.ObjToInt();
        //            //item.Id = (TId)obj;
        //            db.Ado.CommitTran();

        //        }

        //    }
        //    return obj.Id;
        //}
        #endregion
        #region 修改

        public virtual int Update(TEntity item, SqlSugarClient db = null)
        {

            if (null == db)
            {
                using (db = DbHelper.Instance)
                {
                    return Update(item, db);
                }
            }
            //传入实体的时候不用泛型
            int result = db.Updateable<TEntity>(item).ExecuteCommand();
            return result;
        }



        public virtual int Update(SqlSugarClient db, TEntity item, List<string> skipColumnNames = null)
        {
            //int result = db.Updateable<TEntity>(item).ExecuteCommand();
            //return result;

            bool flag = false;
            int num = 0;
            object obj = null;

      
            if (null != skipColumnNames)
            {
                //db.DisableUpdateColumns = skipColumnNames;
                num = db.Updateable(item).IgnoreColumns(it => skipColumnNames.Contains(it)).ExecuteCommand();
            }
            else
            {
                num = db.Updateable(item).ExecuteCommand();
            }
            return num;

        }

        //public virtual int Update(TEntity item)
        //{
        //    using (var db = DbHelper.Instance)
        //    {

        //        int result = db.Updateable<TEntity>(item).ExecuteCommand();
        //        return result;
        //    }

        //}


        #endregion
        #region 删除


        public virtual bool Delete(TIdType id)
        {
            using (var db = DbHelper.Instance)
            {

                int result = db.Deleteable<TEntity>().In(id).ExecuteCommand();
                return result > 0;
            }


        }
        public virtual bool Delete(List<TIdType> ids)
        {
            using (var db = DbHelper.Instance)
            {

                int result = db.Deleteable<TEntity>().In(ids).ExecuteCommand();
                return result > 0;
            }
        }
        #endregion
        #region 复合处理
        #endregion
        #endregion

    }
}
