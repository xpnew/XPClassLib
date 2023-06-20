using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Attributes
{
    /// <summary>
    /// 数据库里面需要跳过的列
    /// </summary>
    /// <remarks>
    /// 这个特性等价于SqlSugar里面的IsIgnore，但是这么用的话，可以不必引用SqlSugar相关的命名空间。
    /// 同时也需要DbHelper里面增加对这个特性的识别。
    /// 参考地址（具体章节在【4使用自定义特性】）：
    /// http://www.codeisbug.com/Doc/8/115
    /// </remarks>
    public class DBSkipColumnAttribute : Attribute
    {
    }
}
