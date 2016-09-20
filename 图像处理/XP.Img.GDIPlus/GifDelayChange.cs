using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace XP.Img.GDIPlus
{
    public class GifDelayChange
    {

        /// <summary>
        /// 原来的延迟时间
        /// </summary>
        public List<int> OldDelays { get; set; }


        /// <summary>
        /// 新的延迟时间
        /// </summary>
        public List<int> NewDelays { get; set; }


        private Image _InnerWorkImg;


        private string _SourcePath;


        public GifDelayChange(string path)
        {
            _SourcePath = path;
            _InnerWorkImg = Image.FromFile(path);
        }

        public void SetNew(int delay)
        {
            var img = _InnerWorkImg;
            foreach (Guid gid in _InnerWorkImg.FrameDimensionsList)
            {
                FrameDimension f = FrameDimension.Time;
                //获取总帧数
                int count = img.GetFrameCount(f);
                //每一帧
                for (int c = 0; c < count; c++)
                {
                    //选择由维度和索引指定的帧
                    img.SelectActiveFrame(f, c);
                    var DelayProperty = img.GetPropertyItem(20736);
                    if (null == DelayProperty)
                    {
                        var DelayValue = DelayProperty.Value.GetValue(0);
                        return;
                    }

                    for (int i = 0; i < img.PropertyItems.Length; i++)
                    {
                        var Item = img.PropertyItems[i];
                        string Msg = String.Format("img PropertyItems  Id : {0} , Len: {1} , Type {2},  Value  {3} ", Item.Id, Item.Len, Item.Type, Item.Value);
                        x.Say(Msg);
                        if (20736 == Item.Id)
                        {
                            x.Say("已经发现了 延时的属性！");

                        }



                        //new_imgs.SetPropertyItem(img.PropertyItems[i]);
                    }
                }

            }
        }

        public void Save()
        {
            
        }


        public void Save(string path)
        {

        }

        /// <summary>
        /// 静态类，设置一个Gif文件的延迟时间
        /// </summary>
        /// <param name="path"></param>
        /// <param name="delay"></param>
        public static void Set(string path, int delay)
        {
            var change = new  GifDelayChange(path);
            change.SetNew(delay);
            change.Save();
        }

    }
}
