using System;
using System.Collections.Generic;
using System.Text;

namespace XP.Comm.Task
{
    /// <summary>
    /// 任务项的信息接口
    /// </summary>
    public interface ITaskItemInfo
    {

        string Title { get; set; }


        int StatusCode { get; set; }



        TaskStatusEnum Status { get; set; }

    }
}
