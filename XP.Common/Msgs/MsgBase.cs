using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XP.Common.Serialization;

namespace XP.Common.Msgs
{
    public class MsgBase
    {

        /// <summary>
        /// 消息名称，多个标题组合使用时，互相区分
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 运行记录
        /// </summary>
        public string Log { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get; set; }

        public virtual void AddLog(string logStr)
        {
            this.Log += logStr + "\n";
        }

        public virtual void AddLog(MsgBase msg)
        {
            if (null == msg)
                return;
            if (String.IsNullOrEmpty(msg.Log))
                return;
            if (String.IsNullOrEmpty(this.Log))
            {
                this.Log = msg.Log;
                return;
            }

            this.Log += msg.Log;
        }

        public override string ToString()
        {
            //return base.ToString();

            return JsonHelper.Serialize<MsgBase>(this);
        }
    }
}
