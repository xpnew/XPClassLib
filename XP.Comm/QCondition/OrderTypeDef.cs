using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.QCondition
{
    /// <summary>
    /// 排序类型枚举定义
    /// </summary>
    public enum OrderTypeDef
    {
        /// <summary>
        /// 默认，为指定
        /// </summary>
        Default = 0,
        /// <summary>
        /// 正序
        /// </summary>
        Asc = 1,
        Ascend = Asc,
        /// <summary>
        /// 倒序
        /// </summary>
        Desc = -1,
        Descend = Desc,
    }
}
