using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;

namespace XP.Img.GDIPlus.Draw
{
    /// <summary>
    /// 图像绘制基类
    /// </summary>
    public class BaseDraw
    {

        #region 基础状态

        public int Width { get; set; }

        public int Height { get; set; }


        public Color BgColor { get; set; }

      public   System.Drawing.Bitmap WorkImage { get; set; }

      public Graphics WorkGraphics { get; set; }

        #endregion


        public BaseDraw():this(100,100)
        {

        }
        public BaseDraw(int w, int h):this(w,h, Color.White)
        {

        }


        public BaseDraw(int w, int h, Color c)
        {
            this.Width = w;
            this.Height = h;
            this.BgColor = c;
            _InitCanvas();
        }

        /// <summary>
        /// 初始化画布
        /// </summary>

        protected virtual void _InitCanvas()
        {
            WorkImage = new Bitmap(Width, Height);

            WorkGraphics = Graphics.FromImage(WorkImage);
            WorkGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            WorkGraphics.Clear(BgColor);
        }



        public void ReSet(int w, int h, Color c)
        {
            this.Width = w;
            this.Height = h;
            this.BgColor = c;
            _InitCanvas();
        }
        public bool Draw2Stream(Stream stream, System.Drawing.Imaging.ImageFormat imgFormating = null)
        {
            if (null == imgFormating)
            {
                imgFormating = ImageFormat.Jpeg;
            }

            WorkImage.Save(stream, imgFormating);
            return true;

        }


        public bool Save2File(string filePhysicalPah,ImageFormat imgFormating = null)
        {
            if (null == imgFormating)
            {
                imgFormating = ImageFormat.Jpeg;
            }

            WorkImage.Save(filePhysicalPah, imgFormating);

            return true;
        }
    }
}
