using XP.Comm;
using XP.Comm.Msgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.DB.BLL.Base
{
    public class BLLBaseVsMsg :  BaseEntityVSMsg<CommMsg>
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




    }
}
