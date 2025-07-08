using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XP.Win.Components.Monitors
{
    /// <summary>
    ///   Winform Richtextbox 添加新行且文字为彩色 RichTextBoxColorExtensions
    /// </summary>
    /// <remarks>
    /// 创建日期：2025/7/2 17:21:00
    /// 类名：RichTextBoxColorExtensions
    /// 创建人： xpnew@126.com
    /// 
    ///  参考  RichTextBox  一行内使用多种不同的颜色
    ///  https://www.cnblogs.com/Jamesblog/p/16943708.html
    /// </remarks>
    public static class RichTextBoxColorExtensions
    {

        public static void AppendText(this RichTextBox rtb, string text, Color color, Font font, bool isNewLine = false)
        {
            rtb.SuspendLayout();
            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionLength = 0;

            rtb.SelectionColor = color;
            rtb.SelectionFont = font;
            rtb.AppendText(isNewLine ? $"{text}{Environment.NewLine}" : text);
            rtb.SelectionColor = rtb.ForeColor;
            rtb.ScrollToCaret();
            rtb.ResumeLayout();
        }

        #region <属性>
        #endregion <属性>

        #region <构造方法>
        #endregion <构造方法>

        #region <内部方法>
        #endregion <内部方法 end>
        #region <外部方法>
        #endregion <外部方法 end>

        #region <事件>
        #endregion <事件>    
    }
}
