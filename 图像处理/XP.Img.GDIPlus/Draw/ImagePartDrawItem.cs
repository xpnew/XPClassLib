using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace XP.Img.GDIPlus.Draw
{
    /// <summary>
    /// 图片子区域
    /// </summary>
    public class ImagePartDrawItem : PartDrawItem
    {

        public Image Img { get; set; }


        public string ImgPhysicalPath { get; set; }



        public override void DrawSelf(System.Drawing.Graphics gdi)
        {
            //base.DrawSelf(gdi);
        

            //Pen BorderPen = new Pen(BorderColor, 2.0f);
            Rectangle myRectangle = new Rectangle(X, Y, X + OutputWidth, Y + OutputHeight);
            gdi.DrawImage(Img, myRectangle);

        }



    }
}
