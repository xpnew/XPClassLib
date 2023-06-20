using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace XP.Img.GDIPlus.Draw
{

    /// <summary>
    /// 图形块的子区域（测试用）
    /// </summary>
    public class BlockPartDrawItem : PartDrawItem
    {

        public Color BorderColor { get; set; }

        public override void DrawSelf(System.Drawing.Graphics gdi)
        {
            //base.DrawSelf(gdi);
            if (null == BorderColor)
            {
                BorderColor = Color.Red;
            }

            Pen BorderPen = new Pen(BorderColor, 2.0f);
            Rectangle myRectangle = new Rectangle(X, Y, X+OutputWidth, Y+ OutputHeight);
            gdi.DrawRectangle(BorderPen, myRectangle);

        }
    }
}
