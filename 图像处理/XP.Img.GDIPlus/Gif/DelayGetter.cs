using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using XP.Util;

namespace XP.Img.GDIPlus.Gif
{
    public class DelayGetter
    {
        private Image _InnerWorkImg;


        private string _SourcePath;

        public List<int> DelaysResult { get; set; }

        public DelayGetter(string path)
        {
            _SourcePath = path;
            _InnerWorkImg = Image.FromFile(path);
            _Init();
        }


        public DelayGetter(Image img)
        {
            _InnerWorkImg = img;
            _Init();
        }

        protected void _Init()
        {
            DelaysResult = new List<int>();
            Analyze();
        }



        protected void Analyze()
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
                    if (null != DelayProperty)
                    {
                        var DelayValue = DelayProperty.Value;

                        x.Say(ByteUtil.Buffer2String(DelayValue));

                        return;
                    }

                   
                }

            }
        }


        public void ReadExt()
        {


        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        ///  http://cnn237111.blog.51cto.com/2359144/1261422
        /// </remarks>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<int> GetDelay4FileByte(string filePath)
        {
            byte[] bytes = File.ReadAllBytes(filePath);
            List<int> Result = new List<int>();
            x.Say("all bytes in file::");
            x.Say(ByteUtil.Buffer2String(bytes));

            //byte[] delaybyte = BitConverter.GetBytes(delay);//转成16位无符号字节数组。该数组肯定只有2个元素
            for (int i = 0; i < bytes.Length - 1; i++)
            {
                if (bytes[i] == 0x21 && bytes[i + 1] == 0xf9)//GraphicsControlExtension 开始标志
                {
                    //bytes[i + 4] = delaybyte[0];//这两位就是定义延迟时间的，修改就可以了。
                    //bytes[i + 5] = delaybyte[1];

                    int low = bytes[i + 4];

                    int high = bytes[i + 5];

                    int FrameDelay = high * 256 + low;
                    x.Say(" 只计算前2字节的延迟结果 " + FrameDelay);

                    int low2 = bytes[i + 6];

                    int high2 = bytes[i + 7];

                    int s2 = high2 * 256 + low2;
                    x.Say(" 只计算后2字节的延迟结果 " + s2);


                    // 测试发现， 只有 21 F9 这个标志之后的 2位是 延迟时间，后面的不知道是什么。
                    // 所以下面 得到的值是不对的：
                    int delay = BitConverter.ToInt32(bytes, i + 4);

                    Result.Add(FrameDelay);
                }
            }
            return Result;
        }


    }
}
