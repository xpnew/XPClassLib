using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Msgs
{
    public class BaseEntityVSMsg<TMsg> where TMsg : MsgBase, new()
    {

        public TMsg Msg { get; set; }

        public BaseEntityVSMsg()
        {
            Msg = new TMsg();
            Msg.Name = this.GetType().FullName;
        }


        public virtual void MsgLog(string log)
        {
            Msg.AddLog(log);
        }


        public virtual void MsgOk(string msg)
        {
            Msg.SetOk();
            Msg.Title = msg;
            Msg.AddLog("操作完成：" + msg);
        }
        public virtual void MsgOk()
        {
            MsgOk("OK");
        }

        public virtual void MsgErr(string error)
        {
            Msg.SetFail();
            Msg.Title = error;
            Msg.AddLog("出现错误：" + error);
        }
        public virtual void MsgErr(string title, int code)
        {
            MsgErr(title);
            Msg.StatusCode = code;
        }

        public virtual void MsgErr(string error, Exception ex)
        {
            Msg.SetFail();
            Msg.Title = error;
            Msg.AddLog("出现错误：" + error);
            Msg.Exp = ex;
        }
    }
}
