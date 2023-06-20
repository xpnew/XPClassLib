using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Comm.Entity;
using SqlSugar;

namespace XP.DB.DAL.Bases
{

    /// <summary>
    /// 其它商户实体DAL基类（带StoreId而不带Id）
    /// </summary>
    public class StoreOtherDALBase<TEntity> : BaseDALVsMsg
    where TEntity : class ,IStoreEntity,new()
    {

        public virtual int Update(TEntity item, int currentCompId, List<string> skipColumnNames = null)
        {
            //int result = db.Updateable<TEntity>(item).ExecuteCommand();
            //return result;

            bool flag = false;
            int num = 0;
            object obj = null;
            using (SqlSugarClient db = DbHelper.Instance)
            {

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
            }

            return num;

        }

        public bool BuildCompanyEntityQuery(XP.Comm.QCondition.BassEntityCondition<TEntity> condition, IQueryable<TEntity> query)
        {


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
            //如果不是系统商户，一律强制限定为当前商户的自己的数据
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
                        if (condition.EntitySelf.StoreId != condition.CurrentStoreId.Value)
                        {
                            Msg.SetFail("商户id 不正确，禁止 数据越权");
                            return false;
                        }
                    }
                    query = query.Where(k => k.StoreId == condition.CurrentStoreId.Value);
                }
            }

            return true;
        }

    }
}
