using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Msgs
{
    public class MsgBase : IMsg
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
        public string Logs { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get; set; }


        /// <summary>
        /// 新增，包含警报
        /// </summary>
        public bool HasWarn { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }


        /// <summary>
        /// 错误的消息，用来在不同的程序之间传递异常
        /// </summary>
        public Exception Exp { get; set; }

        public string ExcptionString { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 更新时间或者完成时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }


        private bool _HasLogStart { get; set; }

      
        #region 构造函数

        public MsgBase()
        {
            StatusCode = 0;
            CreateTime = DateTime.Now;
        }

        public MsgBase(Type type)
            : this()
        {
            this.Name = type.FullName;
        }
        public MsgBase(object Entity)
            : this()
        {
            this.Name = Entity.GetType().FullName;
        }

        public MsgBase(string name)
            : this()
        {
            this.Name = name;
            this.Title = name;
        }


        #endregion



        #region 基础的处理


        public void SetOk(string msg)
        {
            SetOk();
            this.Title = msg;
        }
        public void SetOk()
        {
            SetStatus(true);
        }

        public void SetFail(string msg)
        {
            SetFail();
            this.Title = msg;

        }
        public void SetFail(string msg,int errcode)
        {
            SetFail();
            this.Title = msg;
            this.StatusCode = errcode;
        }


        public void SetFail(string msg, Exception ex)
        {
            SetFail(msg);
            this.Exp = ex;
        }
        public void SetFail()
        {
            SetStatus(false);
        }

        public void SetWarn()
        {
            HasWarn = true;
        }
        public void SetStatus(bool set)
        {
            Status = set;
            StatusCode = set ? 1 : -1;
        }



        #endregion

        #region 传递消息内容和日志


        //public virtual void AddLog(string logStr)
        //{
        //    this.Log += logStr + "\n";
        //}

        public virtual void AddLog(string log)
        {
            UpdateTime = DateTime.Now;
            if (_HasLogStart)
            {
                Logs += "\n" + log;
            }
            else
            {
                Logs = log;
                _HasLogStart = true;
            }
        }


        public virtual void AddLog(IMsg msg)
        {
            UpdateTime = DateTime.Now;

            if (null == msg)
                return;
            if (String.IsNullOrEmpty(msg.Logs))
                return;
            //if (String.IsNullOrEmpty(this.Log))
            //{
            //    this.Log = msg.Log;
            //    return;
            //}

            //this.Log += msg.Log;

            StringBuilder sb = new StringBuilder();

            sb.Append("======================= " + msg.Name + " =======================\n");
            sb.Append("CreateTime: ");
            sb.Append(msg.CreateTime);
            sb.Append("\n");
            sb.Append(msg.Logs);
            sb.Append("\nUpdateTime: ");
            sb.Append(msg.UpdateTime);
            sb.Append("\n======================= " + msg.Name + " =======================");

            sb.Append(this.Logs);
            this.Logs = sb.ToString();

        }


        //public void AddLog(IMsg msg)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    sb.Append("======================= " + msg.Name + " =======================\n");
        //    sb.Append("CreateTime: ");
        //    sb.Append(msg.CreateTime);
        //    sb.Append("\n");
        //    sb.Append(msg.Logs);
        //    sb.Append("\nUpdateTime: ");
        //    sb.Append(msg.UpdateTime);
        //    sb.Append("\n======================= " + msg.Name + " =======================");

        //    sb.Append(this.Log);
        //    UpdateTime = DateTime.Now;

        //    this.Log = sb.ToString();
        //}



        public static T CloneMsg<T>(IMsg msg) where T : IMsg, new()
        {
            T Result = new T();
            //Result.Name = msg.Name;

            Result.Title = msg.Title;
            if (msg.CreateTime.HasValue)
            {
                Result.CreateTime = msg.CreateTime;
            }
            Result.Status = msg.Status;
            Result.StatusCode = msg.StatusCode;
            Result.Logs = msg.Logs;
            Result.Body = msg.Body;
            Result.UpdateTime = DateTime.Now;

            return Result;

        }

        public static void Copy(IMsg input, IMsg output)
        {
            //Result.Name = msg.Name;
            output.Title = input.Title;
            output.Status = input.Status;
            output.StatusCode = input.StatusCode;
            output.Logs = input.Logs;
            output.Body = input.Body;
            output.UpdateTime = DateTime.Now;

        }

        #endregion


        #region 封装tostring


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
            sb.Append(Logs);
            sb.Append("\n");

            sb.Append("【Body】：");
            sb.Append(Body);
            sb.Append("\n");

            return sb.ToString();
        }
        #endregion



    }
}
