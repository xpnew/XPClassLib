using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace XP.Comm.Logs
{
    public class LogElementBase : ILogElement
    {
        /// <summary>
        /// 日志的标题
        /// </summary>
        public string Tit { get; set; }
        /// <summary>
        /// 可能会用到的，日志名字，一般记录控制器、队列、监控器的名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 日志的详细内容
        /// </summary>
        public string Cot { get; set; }
        /// <summary>
        /// 日志产生的时间
        /// </summary>
        public DateTime Dat { get; set; }

        /// <summary>
        /// 添加一条日志
        /// </summary>
        /// <param name="tit"></param>
        /// <param name="cot"></param>
        public void Add(string tit, string cot)
        {

            Tit = tit;
            //Name = log.Name;
            Cot = cot;
        }

        /// <summary>
        /// 将另一条日志的内容复制到当前日志上,注意，时间字段数据是不会复制的
        /// </summary>
        /// <param name="lgo"></param>
        public void AddLog(ILogElement log)
        {
            //this
            //{
            //    Tit = log.Tit,
            //        Cot = log.Cot,
            //}
            //Tit = log.Tit,

            Tit = log.Tit;
            Name = log.Name;
            Cot = log.Cot;
        }

        public virtual void GetName()
        {
            BuildStackName();
        }


        /// <summary>
        /// 根据堆栈获取名称（默认只取最后两层）
        /// </summary>
        protected virtual void BuildStackName()
        {
            BuildStackName(2);
        }

        /// <summary>
        /// 根据堆栈获取名称,这是支持指定层级的重载
        /// </summary>
        /// <param name="max"></param>
        protected virtual void BuildStackName(int max)
        {
            StringBuilder sb = new StringBuilder();
            StackTrace st = new StackTrace(true);
            string stackIndent = "";
            bool HasStart = false;
            for (int i = 0; i < st.FrameCount && i < max; i++)
            {
                // Note that at this level, there are four
                // stack frames, one for each method invocation.
                StackFrame sf = st.GetFrame(i);
                //Console.WriteLine();
                //Console.WriteLine(stackIndent + " Method: {0}", sf.GetMethod());
                //Console.WriteLine(stackIndent + " File: {0}", sf.GetFileName());
                //Console.WriteLine(stackIndent + " Line Number:{0}", sf.GetFileLineNumber()); stackIndent += "  ";

                string LineText = "「{sf.GetFileName()}」≯「{sf.GetMethod()}」";
                if (HasStart)
                {
                    sb.Append("|");
                }

                sb.Append(LineText);
                HasStart = true;
            }

            Name = sb.ToString();
        }
    }
}
