using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace XP.Img.GDIPlus
{
    public class GifWriter
    {

        public List<Image> Frames { get; set; }


        public int Width { get; set; }

        public int Height { get; set; }


        public GifWriter(int width, int height)
        {
            this.Width = width;
            this.Height = height;

            Frames = new List<Image>();
        }



        public void WriteToFile(string path)
        {

            System.Drawing.Bitmap image = new System.Drawing.Bitmap(Width, Height);

            System.IO.MemoryStream ms = new System.IO.MemoryStream();


            var img = Frames[0];

            Graphics g_new_img = Graphics.FromImage(image);
            //配置新图第一帧GDI+绘图对象
            g_new_img.CompositingMode = CompositingMode.SourceCopy;
            g_new_img.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g_new_img.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g_new_img.SmoothingMode = SmoothingMode.HighQuality;
            g_new_img.Clear(Color.FromKnownColor(KnownColor.Transparent));
            //将原图第一帧画给新图第一帧
            g_new_img.DrawImage(img, new Rectangle(0, 0, 100, 100), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);


            image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);


            using (Stream stream = new System.IO.FileStream(path, System.IO.FileMode.Create))
            {


            }



        }


        public void Say()
        {
            string imgPath = "aaa";
            Image img = Image.FromFile(imgPath);
            //新图第一帧
            Image new_img = new Bitmap(100, 100);
            //新图其他帧
            Image new_imgs = new Bitmap(100, 100);
            //新图第一帧GDI+绘图对象
            Graphics g_new_img = Graphics.FromImage(new_img);
            //新图其他帧GDI+绘图对象
            Graphics g_new_imgs = Graphics.FromImage(new_imgs);
            //配置新图第一帧GDI+绘图对象
            g_new_img.CompositingMode = CompositingMode.SourceCopy;
            g_new_img.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g_new_img.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g_new_img.SmoothingMode = SmoothingMode.HighQuality;
            g_new_img.Clear(Color.FromKnownColor(KnownColor.Transparent));
            //配置其他帧GDI+绘图对象
            g_new_imgs.CompositingMode = CompositingMode.SourceCopy;
            g_new_imgs.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g_new_imgs.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g_new_imgs.SmoothingMode = SmoothingMode.HighQuality;
            g_new_imgs.Clear(Color.FromKnownColor(KnownColor.Transparent));
            //遍历维数
            foreach (Guid gid in img.FrameDimensionsList)
            {
                //因为是缩小GIF文件所以这里要设置为Time
                //如果是TIFF这里要设置为PAGE
                FrameDimension f = FrameDimension.Time;
                //获取总帧数
                int count = img.GetFrameCount(f);
                //保存标示参数
                System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.SaveFlag;
                //
                EncoderParameters ep = null;
                //图片编码、解码器
                ImageCodecInfo ici = null;
                //图片编码、解码器集合
                ImageCodecInfo[] icis = ImageCodecInfo.GetImageDecoders();
                //为 图片编码、解码器 对象 赋值
                foreach (ImageCodecInfo ic in icis)
                {
                    if (ic.FormatID == ImageFormat.Gif.Guid)
                    {
                        ici = ic;
                        break;
                    }
                }
                //每一帧
                for (int c = 0; c < count; c++)
                {
                    //选择由维度和索引指定的帧
                    img.SelectActiveFrame(f, c);
                    //第一帧
                    if (c == 0)
                    {
                        //将原图第一帧画给新图第一帧
                        g_new_img.DrawImage(img, new Rectangle(0, 0, 100, 100), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
                        //把振频和透明背景调色板等设置复制给新图第一帧
                        for (int i = 0; i < img.PropertyItems.Length; i++)
                        {
                            new_img.SetPropertyItem(img.PropertyItems[i]);
                        }
                        ep = new EncoderParameters(1);
                        //第一帧需要设置为MultiFrame
                        ep.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.MultiFrame);
                        //保存第一帧
                        new_img.Save(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"/temp/" + Path.GetFileName(imgPath), ici, ep);
                    }
                    //其他帧
                    else
                    {
                        //把原图的其他帧画给新图的其他帧
                        g_new_imgs.DrawImage(img, new Rectangle(0, 0, 100, 100), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
                        //把振频和透明背景调色板等设置复制给新图第一帧
                        for (int i = 0; i < img.PropertyItems.Length; i++)
                        {
                            new_imgs.SetPropertyItem(img.PropertyItems[i]);
                        }
                        ep = new EncoderParameters(1);
                        //如果是GIF这里设置为FrameDimensionTime
                        //如果为TIFF则设置为FrameDimensionPage
                        ep.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.FrameDimensionTime);
                        //向新图添加一帧
                        new_img.SaveAdd(new_imgs, ep);
                    }
                }
                ep = new EncoderParameters(1);
                //关闭多帧文件流
                ep.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.Flush);
                new_img.SaveAdd(ep);

            }
        }



        public static  void SavePic(Image img)
        {

        }

    }
}
