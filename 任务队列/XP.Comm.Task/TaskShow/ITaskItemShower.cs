using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Comm.Task.TaskShow
{

    /// <summary>
    /// 任务项的信息显示器
    /// </summary>
    public interface ITaskItemShower
    {

        void Add(string txt);

        int MaxLine { get; set; }
    }
}
