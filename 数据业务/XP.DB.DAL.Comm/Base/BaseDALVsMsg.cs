using XP.Comm;
using XP.Comm.Msgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace XP.DB.DAL.Bases
{
    public class BaseDALVsMsg : BaseEntityVSMsg<CommMsg>
    {

        #region  消息和日志

        /// <summary>
        /// 同时在消息和日志上处理错误
        /// </summary>
        /// <param name="errTit"></param>
        /// <param name="ex"></param>
        public void Err4MsgVSLog(string errTit, Exception ex)
        {
            XP.Loger.Error(errTit, ex);
            MsgErr(errTit, ex);
        }

        /// <summary>
        /// 同时在消息和日志上处理错误
        /// </summary>
        /// <param name="errTit"></param>
        /// <param name="ex"></param>
        public void Err4MsgVSLog(string errTit)
        {
            XP.Loger.Error(errTit);
            MsgErr(errTit);
        }

        #endregion


        #region 基础的查询方法


        public virtual List<TModel> GetAllModels<TModel>(SqlSugarClient db = null)
        {

            if (null == db)
            {
                using (db = DbHelper.Instance)
                {
                    return GetAllModels<TModel>(db);
                }
            }

            var Result = db.Queryable<TModel>().ToList();
            return Result;
        }
        public virtual TModel GetModelById<TModel, TIdType>(TIdType id, SqlSugarClient db = null)
        {

            if (null == db)
            {
                using (db = DbHelper.Instance)
                {
                    return GetModelById<TModel, TIdType>(id, db);
                }
            }

            List<IConditionalModel> conModels = new List<IConditionalModel>();
            conModels.Add(new ConditionalModel() { FieldName = "id", ConditionalType = ConditionalType.Equal, FieldValue = id.ToString() });
            var view = db.Queryable<TModel>().Where(conModels).First();
            return view;
        }


        #endregion



        protected int Create4Rows<T>(T item, SqlSugarClient db = null)
        where T:class,new()
        {
            if (null == db)
            {
                using (db = DbHelper.Instance)
                {
                    return Create4Rows(item, db);
                }
            }
            int rows = db.Insertable<T>(item).ExecuteCommand();

            return rows;
        }


        #region 商户功能封装

        /// <summary>
        /// 检查企业id，系统企业可以修改所有的，上级可以修改下级、下级只能改自己的
        /// </summary>
        /// <param name="currentCompId"></param>
        /// <param name="compId"></param>
        /// <returns></returns>
        public bool CheckEnableCompId(int currentCompId, int compId)
        {

            if (CheckSysCompId(currentCompId))
            {
                return true;
            }
            if (currentCompId == compId)
            {
                return true;
            }

            return false;
        }

        public bool CheckSysCompId(int currentId)
        {
            if (1 == currentId)
            {
                return true;
            }
            return false;
        }

        #endregion
    }

    #region 基础注释模板
    #region 单例模式
    #endregion

    #region 基础的查询方法
    #endregion

    #region 添加
    #endregion
    #region 修改
    #endregion
    #region 删除
    #endregion
    #region 复合处理
    #endregion

    #endregion


}


