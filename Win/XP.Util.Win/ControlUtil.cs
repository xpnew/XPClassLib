using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XP.Util.Win
{
    /// <summary>
    /// ControlUtil控件工具
    /// </summary>
    /// <remarks>
    /// 创建日期：2025/7/18 11:33:16
    /// 类名：ControlUtil
    /// 创建人： xpnew@126.com
    /// </remarks>
    public class ControlUtil
    {
        #region <属性>
        #endregion <属性>

        #region <构造方法>
        #endregion <构造方法>

        #region <内部方法>
        #endregion <内部方法 end>
        #region <外部方法>

        public static int? GetInt(Control control, int? def = null)
        {
            if (control is TextBox)
            {
                var tb =  (TextBox)control;
                return GetInt(tb, def);
            }
               
            
            if (control is Label)
            {
                var tb =  (Label)control;
                return GetInt(tb, def);
            }
              
            if (control is Button)
            {
                var tb =  (Button)control;
                return GetInt(tb, def);
            }


            return def;
        }

        public static int? GetInt(TextBox ctr, int? def = null)
        {
            if (null == ctr) return def;

            var str = ctr.Text;
            if (vbs.IsInt(str))
            {
                return int.Parse(str);
            }
            return def;
        }   
        public static int? GetInt(Label ctr, int? def = null)
        {
            if (null == ctr) return def;

            var str = ctr.Text;
            if (vbs.IsInt(str))
            {
                return int.Parse(str);
            }
            return def;
        }  public static int? GetInt(Button ctr, int? def = null)
        {
            if (null == ctr) return def;

            var str = ctr.Text;
            if (vbs.IsInt(str))
            {
                return int.Parse(str);
            }
            return def;
        }
        #endregion <外部方法 end>

        #region <事件>
        #endregion <事件>    
    }
}
