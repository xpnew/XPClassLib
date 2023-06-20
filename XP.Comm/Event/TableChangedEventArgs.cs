using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.Comm.Event
{

    /// <summary>
    /// 表格修改事件参数
    /// </summary>
    /// <remarks>
    /// 实现了类似于Asp.net数据库缓存依赖的机制
    /// 具体参照：SysTablesMonitor类（Ljy.DB.BLL）
    /// </remarks>
    public class TableChangedEventArgs : EventArgs
    {

        public int ChangeId { get; set; }


        public string TableName { get; set; }


        public DateTime NotificationCreated { get; set; }


        public TableChangedEventArgs(string table, int newId)
        {
            this.TableName = table;
            ChangeId = newId;
        }




    }
}
