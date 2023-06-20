using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Comm.Entity;

namespace XP.DB.BLL.Base
{
    public class BLLBase4StoreEntitye<DalEntity, ModelEntity, TIdType>: BLLBase4IdEntity<DalEntity, ModelEntity, TIdType>
        where ModelEntity : class, Comm.Entity.IStoreEntity, IdPrimaryKey<TIdType>, new()
        where DalEntity : DAL.Bases.StoreEntityDALBase<ModelEntity, TIdType>, new()
    {

        #region 商户有关的基础处理

        public virtual ModelEntity GetItemById(int id, int currentCompId)
        {
            return dal.GetItemById(id,currentCompId);

        }
        public virtual int Update(ModelEntity item, int currentCompId, List<string> skipColumnNames = null)
        {
            return dal.Update(item,currentCompId,skipColumnNames);

        }



        #endregion


        /// <summary>
        /// 处理查询条件（主要是关于系统商户的）
        /// </summary>
        /// <param name="condition"></param>
        protected  virtual void _InitCondition(XP.Comm.QCondition.BaseQCondition condition)
        {
            if (!condition.CurrentStoreId.HasValue)
            {
                condition.CurrentIsSysStoreId= false;
            }

            //var bllStore = Store_InfoBLL.Instance;
            //condition.CurrentIsSysStoreId = bllStore.CheckSysStore(condition.CurrentStoreId.Value);
            condition.CurrentIsSysStoreId = condition.CurrentStoreId.HasValue && condition.CurrentStoreId.Value == 1;
        }


    }
}
