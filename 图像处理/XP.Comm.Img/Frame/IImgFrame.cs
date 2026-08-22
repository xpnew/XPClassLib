using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Comm.Img
{
    public interface IImgFrame
    {


        Image Img { get; set; }


        int Delay { get; set; }


        void CopyTo<T>(T target ) where T : class,IImgFrame;
    }
}
