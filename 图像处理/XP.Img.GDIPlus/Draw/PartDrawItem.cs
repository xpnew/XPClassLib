using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace XP.Img.GDIPlus.Draw
{

    /// <summary>
    /// 用来拼接的部分子项
    /// </summary>
    public class PartDrawItem
    {
      
        /// <summary>
        /// 原来的大小
        /// </summary>
        public int SourceWidth { get; set; }

        public int SourceHeight { get; set; }

        /// <summary>
        /// 输出的大小
        /// </summary>
        public int OutputWidth { get; set; }

        public int OutputHeight { get; set; }

       

        /// <summary>
        /// 输出的起点
        /// </summary>
        public int X { get; set; }

        public int Y { get; set; }

        /// <summary>
        /// 图像绘制的基准
        /// </summary>
        public Rectangle PartBase { get; set; }
       

        public virtual void DrawSelf(Graphics gdi)
        {

        }

    }
}
