using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm
{
    /// <summary>
    /// 信息交换接口（抽象）
    /// </summary>
    /// <remarks>
    /// 抽象信息交换的问题核心内容，包括了传递消息内容和消息日志的方法
    /// </remarks>
    public interface IMsg
    {

        string Title { get; set; }

        string Name { get; set; }

        string Body { get; set; }

        bool Status { get; set; }


        string Logs { get; set; }

        Exception Exp { get; set; }

        int StatusCode { get; set; }

        DateTime? CreateTime { get; set; }
        DateTime? UpdateTime { get; set; }

        void AddLog(string log);



        void AddLog(IMsg msg);
    }
}
