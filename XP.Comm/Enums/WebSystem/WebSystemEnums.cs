using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Enums
{
    /// <summary>
    /// 商户类型
    /// </summary>
    public enum StoreTypeDef
    {
        /// <summary> 系统商户 </summary>
        SysStore = 10001000,
        /// <summary> 代理 </summary>
        Agent = 10002000,
        /// <summary> 合作商户 </summary>
        Partner = 10003000,
    }

    /// <summary>
    /// 角色类型
    /// </summary>
    public enum RoleTypeDef
    {
        /// <summary> 内置角色 </summary>
        BuiltInRole = 20001000,
        /// <summary> 系统商户角色 </summary>
        SysStoreRole = 20002000,
        /// <summary> 普通商户角色 </summary>
        NormalStoreRole = 20003000,
    }



    /// <summary>
    /// 用户状态定义
    /// </summary>
    public enum UserStateDef
    {
        /// <summary>
        /// 启用
        /// </summary>
        Enable = 1,
        /// <summary>
        /// 禁用
        /// </summary>
        Disable = 0,


    }



    /// <summary>
    /// 权限动作选项类别
    /// </summary>
    /// <remarks>
    ///  对权限动作设置当中允许单独修改的部分加以限定，防止意外地修改到了不应该的列
    /// </remarks>
    public enum RightPartSetCategory
    {
        /// <summary>
        /// 是否日志
        /// </summary>
        RightLog = 1,
        /// <summary>
        /// 是否显示
        /// </summary>
        RightShow = 2,
        /// <summary>
        /// 是否为邮件事件 
        /// </summary>
        RightEventSign = 3,
        /// <summary>
        /// 是否为按钮
        /// </summary>
        RightIsButton = 4,
        /// <summary>
        /// 是否报表
        /// </summary>
        RightIsReport = 5,
        /// <summary>
        /// 是否数据操作按钮(列表内展示按钮)
        /// </summary>
        RightIsDataButton = 6

    }

}
