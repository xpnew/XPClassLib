using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Logs
{
    /// <summary>
    /// Log元素接口
    /// </summary>
    /// <remarks>
    /// 缩写请见相关文档
    /// </remarks>
    public interface ILogElement
    {
        /// <summary>
        /// 日志的标题
        /// </summary>
        string Tit { get; set; }
        /// <summary>
        /// 可能会用到的，日志名字，一般记录控制器、队列、监控器的名字
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 日志的详细内容
        /// </summary>
        string Cot { get; set; }
        /// <summary>
        /// 日志产生的时间
        /// </summary>
        DateTime Dat { get; set; }

        /// <summary>
        /// 添加一条日志
        /// </summary>
        /// <param name="tit"></param>
        /// <param name="cot"></param>
        void Add(string tit, string cot);
        /// <summary>
        /// 将另一条日志的内容复制到当前日志上,注意，时间字段数据是不会复制的
        /// </summary>
        /// <param name="log"></param>
        void AddLog(ILogElement log);


        void GetName();

    }
}
