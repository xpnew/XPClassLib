using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Util.Win.BaseForms
{

    /// <summary>
    /// 基础测试窗口返回结果，最初的目的是让上级窗口跟着直接关闭
    /// </summary>
    public enum FormResultDef
    {
       
        UnKnow = -1,

        Default = 0,

        /// <summary>
        /// 上线窗口跟着关闭
        /// </summary>
        CloseAll = 4,


       
    }
}
