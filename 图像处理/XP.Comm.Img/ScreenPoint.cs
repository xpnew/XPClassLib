using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace XP.Comm.Img
{
    /// <summary>
    /// 屏幕点
    /// </summary>

    public struct ScreenPoint
    {

        public int X { get; set; }

        public int Y { get; set; }

        public Color PointColor { get; set; }

        public override string ToString()
        {
            //return base.ToString();

            return "x:" + X + ", y:" + Y + ", " + PointColor.ToString();
        }
    }
}
