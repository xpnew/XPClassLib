using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace XP.Img.GDIPlus.Gif
{
    public class Frame
    {

        public Image Img { get; set; }


        public int Delay { get; set; }

        public Frame(Image img, int delay)
        {

            this.Img = img;
            this.Delay = delay;
        }
    }
}
