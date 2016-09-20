using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.IO.ExcelUtil
{
    /// <summary>
    /// Excel信息类型定义
    /// </summary>
    public enum ExcelInfoTypes
    {
        /// <summary>
        /// 无，默认
        /// </summary>
        NONE = 0,
        /// <summary>
        /// 文件部分信息
        /// </summary>
        File = 1000,
        /// <summary>
        /// 地址为空
        /// </summary>
        FilePathNull = 1001,
        /// <summary>
        /// 路径错误 IO.Path提供
        /// </summary>
        FilePathError = 1002,
        /// <summary>
        /// 扩展名错误，一般来说是通过扩展名识别格式
        /// </summary>
        FileExtNameError = 1003,
        /// <summary>
        /// 格式不能读取，读取的时候出现了异常
        /// </summary>
        FormatCannotRead = 1004,


        /// <summary>
        /// 组件错误
        /// </summary>
        ComponentErr = 9001,

        /// <summary>
        /// 引用错误
        /// </summary>
        ComponentErrReference = 9101,

        /// <summary>
        /// ICSharpCode.SharpZipLib 组件引用错误，没有找到
        /// </summary>
        ComponentErrReferenceSharpZip = 9102,


        /// <summary>
        /// 说明用，无意义
        /// </summary>
        Warn = 2000,//说明用，无意义

        /// <summary>
        /// 工作薄警报
        /// </summary>
        WorkbookWarn = 3001,

        /// <summary>
        /// 列警报
        /// </summary>
        ColumnWarn = 2002,
        /// <summary>
        /// 行警报
        /// </summary>
        LineWarn = 2003,
        /// <summary>
        /// 单元格警报 
        /// </summary>
        CellWarn = 2004,

        /// <summary>
        /// 说明用，无意义
        /// </summary>
        Error = 3000,

        /// <summary>
        /// 工作薄错误
        /// </summary>
        WorkbookError = 3001,
        /// <summary>
        /// 列错误
        /// </summary>
        ColumnError = 3002,

        /// <summary>
        /// 行错误
        /// </summary>
        LineError = 3003,

        /// <summary>
        /// 单元格错误
        /// </summary>
        CellError = 3004,

    }


    /// <summary>
    /// 结果类型
    /// </summary>
    public enum ResultTypes
    {
        /// <summary>
        /// 默认
        /// </summary>
        NONE = 0,
        /// <summary>
        /// 正常
        /// </summary>
        Ready = 1,
        /// <summary>
        /// 忽略
        /// </summary>
        Ignore = 2,
        /// <summary>
        /// 跳过
        /// </summary>
        Skip = 4,
        /// <summary>
        /// 停止
        /// </summary>
        Stop = 8,
        /// <summary>
        /// 退出
        /// </summary>
        Exist = 8,
    }

}
