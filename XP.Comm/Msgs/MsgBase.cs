using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Msgs
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
            //string Result = String.Format("",Name,Title,StatusCode,)

            return SpliceProperty();

            //return JsonHelper.Serialize<MsgBase>(this);
        }

        /// <summary>
        /// 拼接属性，提供一个基础的ToString()实现。
        /// </summary>
        /// <returns></returns>
        public string SpliceProperty()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("【Name】：");
            sb.Append(Name);
            sb.Append("\n");

            sb.Append("【Title】：");
            sb.Append(Title);
            sb.Append("\n");

            sb.Append("【StatusCode】：");
            sb.Append(StatusCode);
            sb.Append("\n");

            sb.Append("【Log】：");
            sb.Append(Log);
            sb.Append("\n");

            sb.Append("【Body】：");
            sb.Append(Body);
            sb.Append("\n");

            return sb.ToString();
        }
          
    }
}
