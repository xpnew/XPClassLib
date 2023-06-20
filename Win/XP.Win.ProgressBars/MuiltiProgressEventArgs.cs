using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace XP.Win.ProgressBars
{

    /// <summary>
    /// 功能进度显示参数
    /// </summary>
    public class MuiltiProgressEventArgs: ProgressChangedEventArgs
    {
        public MuiltiProgressEventArgs(int progressPercentage, object userState) : base(progressPercentage, userState)
        {

        }
        public MuiltiProgressEventArgs(int progressPercentage,string txt, object userState) : base(progressPercentage, userState)
        {
            TextNotice = txt;
        }

        public string TextNotice { get; set; }
    }
}
