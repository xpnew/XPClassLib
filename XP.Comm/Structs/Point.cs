using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP
{
    public struct XPoint
    {
        public int X;
        public int Y;

        public XPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            //return base.ToString();

            return "x:" + X + ",y:" + Y;
        }
    }
}
