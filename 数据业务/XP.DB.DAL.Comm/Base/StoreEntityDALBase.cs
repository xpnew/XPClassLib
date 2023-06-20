using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace XP.DB.DAL.Bases
{

    /// <summary>
    /// 通用商户DAL（带Id带StoreId）
    /// </summary>
    /// <typeparam name="TEntity">实体类泛型</typeparam>
    /// <typeparam name="TId">Id泛型</typeparam>
    public class StoreEntityDALBase<TEntity, TId> : NormalSIUDDALBase<TEntity, TId>
        where TEntity : class, Comm.Entity.IStoreEntity, Comm.Entity.IdPrimaryKey<TId>, new()
    {

        public virtual TEntity GetItemById(int id, int currentCompId)
        {
            using (SqlSugarClient db = DbHelper.Instance)
            {
                TEntity item = db.Queryable<TEntity>().Where(s => s.Id.Equals(id)).First();
                if (null != item)
                {
                    int TargCompId = item.StoreId;
                    if (!CheckEnableCompId(currentCompId, TargCompId))
                    {
                        Msg.SetFail("没有权限");
                        return null;
                    }
                }
                else
                {
                    Msg.SetFail("指定的id不存在！");
                    return null;
                }

                return item;
            }
        }

        public virtual TId Create(TEntity item)
        {

            TEntity obj = null;
            using (SqlSugarClient db = DbHelper.Instance)
            {
                obj = db.Insertable<TEntity>(item).ExecuteReturnEntity();
                if (null == obj)
                {
                    db.Ado.RollbackTran(); //回滚事务
                    return item.GetNullId();
                }

                if (obj != null)
                {
                    //id = obj.ObjToInt();
                    //item.Id = (TId)obj;
                    db.Ado.CommitTran();

                }

            }

            return obj.Id;
        }

        public virtual int Update(TEntity item, int currentCompId, List<string> skipColumnNames = null)
        {
            //int result = db.Updateable<TEntity>(item).ExecuteCommand();
            using (SqlSugarClient db = DbHelper.Instance)
            {
                return Update(db, item, currentCompId, skipColumnNames);
            }

        }

        public virtual int Update(SqlSugarClient db ,TEntity item, int currentCompId, List<string> skipColumnNames = null)
        {
            //int result = db.Updateable<TEntity>(item).ExecuteCommand();
            //return result;

            bool flag = false;
            int num = 0;
            object obj = null;
   
            int TargCompId = item.StoreId;
            if (!CheckEnableCompId(currentCompId, TargCompId))
            {
                return -1;
            }

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

        public bool BuildStoreViewQuery<TViewEntity>(
            XP.Comm.QCondition.BassEntityCondition<TViewEntity> condition, ISugarQueryable<TViewEntity> query)
            where TViewEntity : class, Comm.Entity.IStoreEntity, new()
        {
            if (!condition.CurrentStoreId.HasValue)
            {
                Msg.SetFail("不能获取商户Id ，权限禁止");
                return false;
            }

            //如果是系统商户，情况比较复杂，详细看分支内部的注释
            if (CheckSysCompId(condition))
            {
                if (null != condition.EntitySelf)
                {
                    //异常数据，没有分配给任何商户，StoreId = 0
                    if (0 == condition.EntitySelf.StoreId)
                    {
                        query = query.Where(k => k.StoreId == condition.EntitySelf.StoreId);
                    }
                    //查询指定的商户
                    else  if (0 < condition.EntitySelf.StoreId)
                    {
                        query = query.Where(k => k.StoreId == condition.EntitySelf.StoreId);
                    }
                    //想查全部商户的数据，StoreId= -1
                    else if (-1 == condition.EntitySelf.StoreId)
                    {
                        //query = query.Where(k => k.StoreId == condition.EntitySelf.StoreId);
                    }
                    //默认情况下（条件二：condition.EntitySelf 不为空，但是StoreId 为-9999）查系统商户自己的数据
                    else if (-9999 == condition.EntitySelf.StoreId)
                    {
                        query = query.Where(k => k.StoreId == condition.CurrentStoreId.Value);

                    }
                    else
                    {
                        query = query.Where(k => k.StoreId == condition.EntitySelf.StoreId);
                    }
                }
                else
                {
                    //默认情况下（条件二：condition.EntitySelf 为空）查系统商户自己的数据
                    query = query.Where(k => k.StoreId == condition.CurrentStoreId.Value);

                }
            }
            //如果不是系统商户，则查询所有的子商户（包括自己）
            else
            {

                if (null == condition.EntitySelf)
                {
                    query = query.Where(k => k.StoreId == condition.CurrentStoreId.Value);
                }
                else
                {
                    if (0 < condition.EntitySelf.StoreId)
                    {
                        //以前的版本策略是：如果不是系统商户，以前一律强制限定为当前商户的自己的数据
                        //if (condition.EntitySelf.StoreId != condition.CurrentStoreId.Value)
                        //{
                        //    Msg.SetFail("商户id 不正确，禁止 数据越权");
                        //    return false;
                        //}
                        if (null == condition.SelfStoreIdList || 0 == condition.SelfStoreIdList.Count)
                        {
                            Msg.SetFail("需要指定商户和子商户的数据范围 不正确，禁止 数据越权");
                            return false;
                        }

                        if (!condition.SelfStoreIdList.Contains(condition.EntitySelf.StoreId))
                        {
                            Msg.SetFail("商户id 不正确，禁止 数据越权");
                            return false;
                        }

                        query = query.Where(k => k.StoreId == condition.EntitySelf.StoreId);
                    }
                    else
                    {
                        query = query.Where(k => condition.SelfStoreIdList.Contains(k.StoreId));
                    }

                    //query = query.Where(k => k.StoreId == condition.CurrentStoreId.Value);
                }
            }

            return true;
        }

        protected bool CheckSysCompId(XP.Comm.QCondition.BaseQCondition condition)
        {
            if (condition.CurrentIsSysStoreId.HasValue)
            {
                return condition.CurrentIsSysStoreId.Value;
            }
            if (!condition.CurrentStoreId.HasValue)
            {
                return false;
            }
            return CheckSysCompId(condition.CurrentStoreId.Value);
        }

        public bool BuildCompanyEntityQuery(XP.Comm.QCondition.BassEntityCondition<TEntity> condition,
            ISugarQueryable<TEntity> query)
        {

            return BuildStoreViewQuery(condition, query);

            #region 原来的代码 

            /*
            if (!condition.CurrentStoreId.HasValue)
            {
                Msg.SetFail("不能获取商户Id ，权限禁止");
                return false;
            }
            //如果是系统商户，情况比较复杂，详细看分支内部的注释
            if (CheckSysCompId(condition.CurrentStoreId.Value))
            {
                if (null != condition.EntitySelf)
                {
                    //想查全部商户的数据，CompanyId = 0
                    if (0 == condition.EntitySelf.StoreId)
                    {

                    }
                    //默认情况下（条件一：condition.EntitySelf 不为空，但是CompanyId 为-1）查系统商户自己的数据
                    else if (-1 == condition.EntitySelf.StoreId)
                    {
                        query = query.Where(k => k.StoreId == condition.EntitySelf.StoreId);

                    }
                    else
                    {
                        query = query.Where(k => k.StoreId == condition.EntitySelf.StoreId);
                    }
                }
                else
                {
                    //默认情况下（条件二：condition.EntitySelf 为空）查系统商户自己的数据
                    query = query.Where(k => k.StoreId == condition.CurrentStoreId.Value);

                }
            }
            //如果不是系统商户，则查询所有的子商户（包括自己）
            else
            {

                if (null == condition.EntitySelf)
                {
                    query = query.Where(k => k.StoreId == condition.CurrentStoreId.Value);
                }
                else
                {
                    if (0 < condition.EntitySelf.StoreId)
                    {
                        //以前的版本策略是：如果不是系统商户，以前一律强制限定为当前商户的自己的数据
                        //if (condition.EntitySelf.StoreId != condition.CurrentStoreId.Value)
                        //{
                        //    Msg.SetFail("商户id 不正确，禁止 数据越权");
                        //    return false;
                        //}
                        if (null == condition.SelfStoreIdList || 0 == condition.SelfStoreIdList.Count)
                        {
                            Msg.SetFail("需要指定商户和子商户的数据范围 不正确，禁止 数据越权");
                            return false;
                        }

                        if (!condition.SelfStoreIdList.Contains(condition.EntitySelf.StoreId))
                        {
                            Msg.SetFail("商户id 不正确，禁止 数据越权");
                            return false;
                        }

                        query = query.Where(k => k.StoreId == condition.EntitySelf.StoreId);
                    }
                    else
                    {
                        query = query.Where(k => condition.SelfStoreIdList.Contains(k.StoreId));
                    }
                    //query = query.Where(k => k.StoreId == condition.CurrentStoreId.Value);
                }
            }

            return true;
        }
        */

            #endregion

        }
    }
}
