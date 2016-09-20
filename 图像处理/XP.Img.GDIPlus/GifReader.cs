using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace XP.Img.GDIPlus
{
    public class GifReader
    {

        public Image Pic { get; set; }
        public List<Image> Frames { get; set; }



        public GifReader()
        {
            Pic = null;
            Frames = new List<Image>();
        }

        public void Load(string path)
        {
            try
            {
                Image img = Image.FromFile(path);
                Pic = img;

                foreach (Guid gid in img.FrameDimensionsList)
                {
                    FrameDimension f = FrameDimension.Time;
                    //获取总帧数
                    int count = img.GetFrameCount(f);
                    //每一帧
                    for (int c = 0; c < count; c++)
                    {
                        //选择由维度和索引指定的帧
                        img.SelectActiveFrame(f, c);
                        for (int i = 0; i < img.PropertyItems.Length; i++)
                        {
                            var Item = img.PropertyItems[i];
                            string Msg = String.Format("img PropertyItems  Id : {0} , Len: {1} , Type {2},  Value  {3} ", Item.Id, Item.Len, Item.Type, Item.Value);
                            x.Say(Msg);
                            //new_imgs.SetPropertyItem(img.PropertyItems[i]);
                        }
                    }


                }
            }
            catch (Exception ex)
            {


            }
        }

    }
}
