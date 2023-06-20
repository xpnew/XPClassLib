using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using XP.Util.Configs;

namespace XP.Util.GdiPlus
{
    public class ImgageCode : IDisposable
    {
        private string _Prefix = "";

        public string Prefix
        {
            get { return _Prefix; }
            set { _Prefix = value; }
        }


        private string _SessionName = "DsCode";


        public string SessionName
        {
            get { return _SessionName; }
            set { _SessionName = value; }
        }

        private Random random = new Random();

        public System.Drawing.Bitmap PicImg { get; set; }
        public System.IO.MemoryStream Stream { get; set; }

        public string CreateRandomCode(int codeCount)
        {
            string allChar = "2,3,5,C,E,F,G,H,J,K,L,N,P,R,U,V,X,Y";//删掉了容易混淆的

            string ConfigSet = "1,2,3,4,5,6,7,8,9,0";
            if (!String.IsNullOrEmpty(ConfigSet))
            {
                allChar = ConfigSet;
            }

            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;

            #region  获取一组不重复的字符，新的算法
            int RandMax = allCharArray.Length;

            List<int> CharsOrderList = GeneralTool.GetNoRepeatInt(0, RandMax, codeCount);
            x.Say("字符索引：" + String.Join(",", CharsOrderList));
            for (int i = 0; i < codeCount; i++)
            {
                randomCode += allCharArray[CharsOrderList[i]];
            }
            #endregion
            #region  获取一组不重复的字符，原来的算法

            ////原来随机时指定固定的值 rand.Next(30)，需要手动计算长度
            ////改用了Length这样的话就更有弹性了。
            //int RandMax = allCharArray.Length - 1;


            //Random rand = new Random();
            //for (int i = 0; i < codeCount; i++)
            //{
            //    if (temp != -1)
            //    {
            //        rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
            //    }
            //    int t = rand.Next(RandMax);
            //    if (temp == t)
            //    {
            //        return CreateRandomCode(codeCount);
            //    }
            //    temp = t;
            //    randomCode += allCharArray[t];
            //}
            #endregion

            return randomCode;
        }


        protected List<string> GetFontName(int len)
        {
            List<string> Result = new List<string>();
            var cr = ConfigReader.Self;
            string ConfigSet = cr.GetSet("LoginImageCodeFontList");
            string Default = "Georgia|Arial";
            if (null == ConfigSet || 0 == ConfigSet.Length)
            {
                for (int i = 0; i < len; i++)
                {
                    Result.Add(Default);
                }
            }
            else
            {
                string[] FontArr = ConfigSet.Split(',');
                int Size = FontArr.Length;

                for (int i = 0; i < len; i++)
                {
                    string FontName = FontArr[GeneralTool.Random(0, Size)];
                    Result.Add(FontName);
                }
            }
            return Result;
        }
        public void CreateStream(string checkCode)
        {
            int iwidth = (int)(checkCode.Length * 20);
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(iwidth + 4, 36);
            Graphics g = Graphics.FromImage(image);
            //g.FillRectangle(new System.Drawing.SolidBrush(Color.Blue),0,0,image.Width, image.Height);
            g.Clear(Color.White);



            //画图片的背景噪音线 
            for (int i = 0; i < 10; i++)
            {
                int x1 = random.Next(image.Width);
                int x2 = random.Next(image.Width);
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);

                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }

            var FontNames = GetFontName(checkCode.Length);

            //x.Say("字体列表：" + String.Join(",", FontNames));

            Brush b;
            //g.DrawString(checkCode, f, b, 3, 3);

            for (int i = 0; i < checkCode.Length; i++)
            {
                Font f = new System.Drawing.Font(FontNames[i], 20, System.Drawing.FontStyle.Bold);

                b = new System.Drawing.SolidBrush(getRedomColor());
                g.DrawString(checkCode[i].ToString(), f, b, (f.Size * i - i), random.Next(2, 6));

            }

            Pen blackPen = new Pen(Color.Black, 0);

            ////干扰线
            //Random rand = new Random();
            //for (int i = 0; i < 5; i++)
            //{
            //    int y = rand.Next(image.Height);
            //    g.DrawLine(blackPen, 0, y, image.Width, y);
            //}




            //画图片的前景噪音点 
            for (int i = 0; i < 50; i++)
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);

                image.SetPixel(x, y, getRedomColor());
            }

            image = TwistImage(image, true, random.Next(25, 35) / 10, random.Next(1, 2));//1.0, 1.0);//random.Next(10, 23), random.Next(1, 8)
            //画图片的边框线 
            g.DrawRectangle(new Pen(Color.FromArgb(22, 22, 22)), 0, 0, image.Width - 1, image.Height - 1);


            Stream = new System.IO.MemoryStream();
            image.Save(Stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            //HttpContext.Current.Response.ClearContent();
            //HttpContext.Current.Response.ContentType = "image/Jpeg";
            //HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            g.Dispose();
            image.Dispose();
        }

        /// <summary>
        /// 正弦曲线Wave扭曲图片
        /// </summary>
        /// <param name="srcBmp"></param>
        /// <param name="bXDir"></param>
        /// <param name="nMultValue">波形的幅度倍数</param>
        /// <param name="dPhase">波形的起始相位，取值区间[0-2*PI)</param>
        /// <returns></returns>
        private Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            double PI = 6.283185307179586476925286766559;
            Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);

            // 将位图背景填充为白色
            Graphics graph = Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();

            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;

            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI * (double)j) / dBaseAxisLen : (PI * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);

                    // 取得当前点的颜色
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            srcBmp.Dispose();
            return destBmp;
        }


        public Color getRedomColor()
        {
            //Random random = new Random();//使用类的私用变量，此处注释
            int min = 0;//rgb的起始值
            int max = 255;//rgb的最大值，实际上就是FF。
            int Alpha = random.Next(40, 55) * 4;
            int r = random.Next(min, max);
            r = (r > 200) ? 200 : r;
            max -= r;
            int g = random.Next(min, max);
            g = (g > 200) ? 200 : g;
            max -= g;
            int b = random.Next(min, max);
            b = (b > 200) ? 200 : b;
            return Color.FromArgb(Alpha, r, g, b);
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    if (null != PicImg)
                    {
                        PicImg.Dispose();
                        PicImg = null;
                    }

                    if (null != Stream)
                    {
                        Stream.Dispose();
                        Stream = null;
                    }
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~ImgageCode() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }

    public class DifficultImgageCode : IDisposable
    {
        private string _Prefix = "";

        public string Prefix
        {
            get { return _Prefix; }
            set { _Prefix = value; }
        }


        private string _SessionName = "DsCode";


        public string SessionName
        {
            get { return _SessionName; }
            set { _SessionName = value; }
        }

        private Random random = new Random();

        public System.Drawing.Bitmap PicImg { get; set; }
        public System.IO.MemoryStream Stream { get; set; }

        public string CreateRandomCode(int codeCount)
        {
            string allChar = "2,3,5,4,C,E,F,G,H,J,K,L,N,P,R,U,V,X,Y";//删掉了容易混淆的

            string ConfigSet = ConfigReader.Self.GetSet("LoginImageCodeChars");
            if (!String.IsNullOrEmpty(ConfigSet))
            {
                allChar = ConfigSet;
            }

            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;

            #region  获取一组不重复的字符，新的算法
            int RandMax = allCharArray.Length;

            List<int> CharsOrderList = GeneralTool.GetNoRepeatInt(0, RandMax, codeCount);
            x.Say("字符索引：" + String.Join(",", CharsOrderList));
            for (int i = 0; i < codeCount; i++)
            {
                randomCode += allCharArray[CharsOrderList[i]];
            }
            #endregion
            #region  获取一组不重复的字符，原来的算法

            ////原来随机时指定固定的值 rand.Next(30)，需要手动计算长度
            ////改用了Length这样的话就更有弹性了。
            //int RandMax = allCharArray.Length - 1;


            //Random rand = new Random();
            //for (int i = 0; i < codeCount; i++)
            //{
            //    if (temp != -1)
            //    {
            //        rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
            //    }
            //    int t = rand.Next(RandMax);
            //    if (temp == t)
            //    {
            //        return CreateRandomCode(codeCount);
            //    }
            //    temp = t;
            //    randomCode += allCharArray[t];
            //}
            #endregion

            return randomCode;
        }


        protected List<string> GetFontName(int len)
        {
            List<string> Result = new List<string>();
            var cr = ConfigReader.Self;
            string ConfigSet = cr.GetSet("LoginImageCodeFontList");
            string Default = "Georgia|Arial";
            if (null == ConfigSet || 0 == ConfigSet.Length)
            {
                for (int i = 0; i < len; i++)
                {
                    Result.Add(Default);
                }
            }
            else
            {
                string[] FontArr = ConfigSet.Split(',');
                int Size = FontArr.Length;

                for (int i = 0; i < len; i++)
                {
                    string FontName = FontArr[GeneralTool.Random(0, Size)];
                    Result.Add(FontName);
                }
            }
            return Result;
        }
        public void CreateStream(string checkCode)
        {
            int iwidth = (int)(checkCode.Length * 20);
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(iwidth + 4, 36);
            Graphics g = Graphics.FromImage(image);
            //g.FillRectangle(new System.Drawing.SolidBrush(Color.Blue),0,0,image.Width, image.Height);
            g.Clear(Color.White);



            //画图片的背景噪音线 
            for (int i = 0; i < 100; i++)
            {
                int x1 = random.Next(image.Width);
                int x2 = random.Next(image.Width);
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);

                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }

            var FontNames = GetFontName(checkCode.Length);

            //x.Say("字体列表：" + String.Join(",", FontNames));

            Brush b;
            //g.DrawString(checkCode, f, b, 3, 3);

            for (int i = 0; i < checkCode.Length; i++)
            {
                Font f = new System.Drawing.Font(FontNames[i], 20, System.Drawing.FontStyle.Bold);

                b = new System.Drawing.SolidBrush(getRedomColor());
                g.DrawString(checkCode[i].ToString(), f, b, (f.Size * i - i), random.Next(2, 6));

            }

            Pen blackPen = new Pen(Color.Black, 0);

            ////干扰线
            //Random rand = new Random();
            //for (int i = 0; i < 5; i++)
            //{
            //    int y = rand.Next(image.Height);
            //    g.DrawLine(blackPen, 0, y, image.Width, y);
            //}




            //画图片的前景噪音点 
            for (int i = 0; i < 250; i++)
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);

                image.SetPixel(x, y, getRedomColor());
            }

            image = TwistImage(image, true, random.Next(25, 35) / 10, random.Next(1, 2));//1.0, 1.0);//random.Next(10, 23), random.Next(1, 8)
            //画图片的边框线 
            g.DrawRectangle(new Pen(Color.FromArgb(22, 22, 22)), 0, 0, image.Width - 1, image.Height - 1);


            Stream = new System.IO.MemoryStream();
            image.Save(Stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            //HttpContext.Current.Response.ClearContent();
            //HttpContext.Current.Response.ContentType = "image/Jpeg";
            //HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            g.Dispose();
            image.Dispose();
        }

        /// <summary>
        /// 正弦曲线Wave扭曲图片
        /// </summary>
        /// <param name="srcBmp"></param>
        /// <param name="bXDir"></param>
        /// <param name="nMultValue">波形的幅度倍数</param>
        /// <param name="dPhase">波形的起始相位，取值区间[0-2*PI)</param>
        /// <returns></returns>
        private Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            double PI = 15.283185307179586476925286766559;
            Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);

            // 将位图背景填充为白色
            Graphics graph = Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();

            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;

            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI * (double)j) / dBaseAxisLen : (PI * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);

                    // 取得当前点的颜色
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            srcBmp.Dispose();
            return destBmp;
        }


        public Color getRedomColor()
        {
            //Random random = new Random();//使用类的私用变量，此处注释
            int min = 0;//rgb的起始值
            int max = 255;//rgb的最大值，实际上就是FF。
            int Alpha = random.Next(40, 55) * 4;
            int r = random.Next(min, max);
            r = (r > 200) ? 200 : r;
            max -= r;
            int g = random.Next(min, max);
            g = (g > 200) ? 200 : g;
            max -= g;
            int b = random.Next(min, max);
            b = (b > 200) ? 200 : b;
            return Color.FromArgb(Alpha, r, g, b);
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    if (null != PicImg)
                    {
                        PicImg.Dispose();
                        PicImg = null;
                    }

                    if (null != Stream)
                    {
                        Stream.Dispose();
                        Stream = null;
                    }
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~ImgageCode() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
