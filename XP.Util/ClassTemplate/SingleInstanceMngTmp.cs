using System;
using System.Collections.Generic;
using System.Text;

namespace XP.Util.ClassTemplate
{
    /// <summary>
    /// 单例模式的模板
    /// </summary>
    public class SingleInstanceMngTmp
    {
        #region 快速单例模式

        public static readonly SingleInstanceMngTmp _Instance = new SingleInstanceMngTmp();

        public static SingleInstanceMngTmp Self
        {
            get { return _Instance; }
        }
        #endregion
        #region  基础参数和属性


        public long ItemIndex { get; set; } = 0;


        //public List<Timer4IdItem> Items { get; set; } = new List<Timer4IdItem>();

        public readonly static object _Locker = new object();

        #endregion

        #region  构造函数和初始化
        protected SingleInstanceMngTmp()
        {
            _Init();
        }


        protected void _Init()
        {
            //_InnerNextTimer.Start();
        }


        #endregion
        #region  内部处理


        #endregion
        #region  外部控制

        #endregion
    }
}
