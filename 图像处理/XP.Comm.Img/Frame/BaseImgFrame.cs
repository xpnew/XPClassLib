using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Comm.Img.Frame
{
    public class BaseImgFrame : IImgFrame
    {
        public Image Img { get; set; }
        public int Delay { get; set; }




        void IImgFrame.CopyTo<T>(T t)
        {
            t.Delay = Delay;
            t.Img = Img;
        }
    }
}
