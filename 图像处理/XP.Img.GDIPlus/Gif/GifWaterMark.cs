using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace XP.Img.GDIPlus.Gif
{
    public class GifWaterMark
    {

        public static Bitmap WaterMarkWithText(System.Drawing.Bitmap origialGif, string
text, string filePath)
        {
            //用于存放桢 
            List<Frame> frames = new
            List<Frame>();
            //如果不是gif文件,直接返回原图像 
            if (origialGif.RawFormat.Guid
            != System.Drawing.Imaging.ImageFormat.Gif.Guid)
            {
                return origialGif;

            }
            //如果该图像是gif文件 
            foreach (Guid guid in
            origialGif.FrameDimensionsList)
            {
                System.Drawing.Imaging.FrameDimension
                frameDimension = new System.Drawing.Imaging.FrameDimension(guid);
                int
                frameCount = origialGif.GetFrameCount(frameDimension);
                for (int i = 0; i
                < frameCount; i++)
                {
                    if (origialGif.SelectActiveFrame(frameDimension,
                    i) == 0)
                    {
                        int delay =
                        Convert.ToInt32(origialGif.GetPropertyItem(20736).Value.GetValue(i));
                        Image
                        img = Image.FromHbitmap(origialGif.GetHbitmap());
                        Font font = new Font(new
                        FontFamily("宋体"), 35.0f, FontStyle.Bold);
                        Graphics g =
                        Graphics.FromImage(img);
                        g.DrawString(text, font, Brushes.BlanchedAlmond,
                        new PointF(10.0f, 10.0f));
                        Frame frame = new Frame(img, delay);

                        frames.Add(frame);
                    }
                }
                //Gif.Components.AnimatedGifEncoder gif =
                //new Gif.Components.AnimatedGifEncoder();
                //gif.Start(filePath);

                //gif.SetDelay(100);
                //gif.SetRepeat(0);
                //for (int i = 0; i <
                //frames.Count; i++)
                //{
                //    gif.AddFrame(frames[i].Image);
                //}

                //gif.Finish();
                try
                {
                    Bitmap gifImg =
                    (Bitmap)Bitmap.FromFile(filePath);
                    return gifImg;
                }
                catch
                {

                    return origialGif;
                }
            }
            return origialGif;
        } 
    }
}
