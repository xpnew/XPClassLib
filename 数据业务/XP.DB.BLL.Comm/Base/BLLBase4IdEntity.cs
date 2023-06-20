using XP.Comm.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.DB.BLL.Base
{
    public class BLLBase4IdEntity<DalEntity, ModelEntity, TIdType>: BLLBaseVsMsg
        where ModelEntity : class, IdPrimaryKey<TIdType>,  new()
        where DalEntity :  DAL.Bases.NormalSIUDDALBase<ModelEntity, TIdType>, new()
    {


        #region 类的基础功能、泛型定义
        private DalEntity _Dal;
        public DalEntity dal
        {
            get
            {
                if (_Dal == null)
                {
                    _Dal = CreateDal();
                }
                return _Dal;
            }
            set
            {
                _Dal = value;
            }
        }


        protected virtual DalEntity CreateDal()
        {

            return new DalEntity();
        }

        #endregion



        #region 标准增删改查



        /// <summary>
        /// 增加一条数据
        /// </summary>
        public TIdType Create(ModelEntity model)
        {
            return dal.Create(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(ModelEntity model)
        {
            return dal.Update(model);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(TIdType id)
        {
            return dal.Delete(id);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(List<TIdType> idlist)
        {
            return dal.Delete(idlist);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ModelEntity GetItemById(TIdType id)
        {
            //var temp = dal.GetModel(id);

            //return temp as ModelEntity;
            return dal.GetItemById(id);
        }

      
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<ModelEntity> GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public List<ModelEntity> GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList( strWhere, filedOrder, Top);
        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<ModelEntity> GetAllList()
        {
            return GetList("");
        }

        ///// <summary>
        ///// 分页获取数据列表
        ///// </summary>
        //public List<ModelEntity> GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        //{
        //    return dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
        //}
        //public List<ModelEntity> GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        //{
        //    return dal.GetList(pageSize, pageIndex, strWhere, filedOrder, out recordCount);
        //}
        /// <summary>
        /// 返回一部分数据，给WCF测试用的
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<ModelEntity> Top(int top = 5)
        {
            using (SqlSugarClient db = DAL.DbHelper.Instance)
            {
                return dal.Top(db, top);
            }
        }




        #endregion


    }
}
