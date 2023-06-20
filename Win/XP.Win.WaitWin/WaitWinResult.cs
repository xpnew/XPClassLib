using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Win.WaitWin
{
    /// <summary>
    /// 等级窗口的执行结果
    /// </summary>
    public class WaitWinResult
    {

        public bool IsFinished { get; set; } = false;

        public bool IsCanceled { get; set; } = false;
        public bool IsCompleted { get; set; } = false;

        public bool IsPause { get; set; } = false;


        public DateTime CreateTime { get; set; } = DateTime.Now;


        public TimeSpan WorkTime { get; set; }


    }
}
