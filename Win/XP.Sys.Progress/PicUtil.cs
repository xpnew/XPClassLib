using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Sys.Progress
{
    public class PicUtil
    {

        public static void Color2Gray(Bitmap map)
        {

            int x = map.Width;
            int y = map.Height;

            for (int i = 0; i < x; ++i)
            {
                for (int j = 0; j < y; ++j)
                {

                    Color color;
                    color = map.GetPixel(i, j);
                    var px = (Math.Pow(color.R, 2.2) + Math.Pow(1.5 * color.G, 2.2) + Math.Pow(0.6 * color.B, 2.2))
                            / (Math.Pow(1, 2.2) + Math.Pow(1.5, 2.2) + Math.Pow(0.6, 2.2));
                    px = Math.Pow(px, 0.4545f);
                    var data = Color.FromArgb((int)px, (int)px, (int)px);
                    map.SetPixel(i, j, data);

                }
            }
//————————————————
//版权声明：本文为CSDN博主「纸墨青鸢」的原创文章，遵循CC 4.0 BY - SA版权协议，转载请附上原文出处链接及本声明。
//原文链接：https://blog.csdn.net/qq_39935495/article/details/123727411
        }
    }
}
