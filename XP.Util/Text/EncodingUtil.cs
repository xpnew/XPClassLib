using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Util.Text
{
    /*******************************************************************************
     * 编码和转换工具
     * 
     * 核心功能是解决 gb2312和UTF8编码识别的问题
     * 
     * 
     * 
     * 
     * 重要参考：http://blog.csdn.net/forsiny/article/details/4813107
     * 
     * 
     * 
     * 
     * 这里还有另一种识别方法，偏重于web 页面：http://www.cnblogs.com/swtseaman/archive/2012/10/04/2711836.html
     * 
     * 
     * 
     * 另外两种判别方法：
     * http://www.cnblogs.com/hhh/archive/2007/01/27/632251.html
     * http://www.cnblogs.com/powertoolsteam/archive/2010/09/20/1831638.html
     * 
     * http://blog.csdn.net/gouguofei/article/details/17023373
     * 
     * 
     * 这个帖子是综合的，上面提到了 MONO 上默认不是GBK的问题
     * http://blog.csdn.net/spritenet/article/details/5650622
     * 
     * 
     * ***************************************************************************/

    /// <summary>
    /// 编码转换工具
    /// </summary>
    public class EncodingUtil
    {



        /// <summary>
        /// GB2312转换成UTF8
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string gb2312_utf8(string text)
        {
            //声明字符集   
            System.Text.Encoding utf8, gb2312;
            //gb2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            //utf8   
            utf8 = System.Text.Encoding.GetEncoding("utf-8");
            byte[] gb;
            gb = gb2312.GetBytes(text);
            gb = System.Text.Encoding.Convert(gb2312, utf8, gb);
            //返回转换后的字符   
            return utf8.GetString(gb);
        }

        /// <summary>
        /// UTF8转换成GB2312
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string utf8_gb2312(string text)
        {
            //声明字符集   
            System.Text.Encoding utf8, gb2312;
            //utf8   
            utf8 = System.Text.Encoding.GetEncoding("utf-8");
            //gb2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            byte[] utf;
            utf = utf8.GetBytes(text);
            utf = System.Text.Encoding.Convert(utf8, gb2312, utf);
            //返回转换后的字符   
            return gb2312.GetString(utf);
        }


        public static string GetStringByBytes(byte[] rawtext, int startIndex, int length)
        {
            byte[] NewBytes = new byte[length];

            for (int i = 0; i < length && i < rawtext.Length; i++)
            {
                NewBytes[i] = rawtext[i + startIndex];
            }
            return GetStringByBytes(NewBytes);
        }
        public static string GetStringByBytes(byte[] rawtext)
        {
            Encoding encode = GetEncoding(rawtext);

            return encode.GetString(rawtext);


        }

        public static Encoding GetEncoding(byte[] rawtext)
        {
            Encoding encode;
            //StreamReader srtest = new StreamReader(file.FullName, Encoding.Default);
            int p = utf8_probability(rawtext);
            if (p > 80)
                encode = Encoding.GetEncoding(65001);//utf8  
            else
                encode = Encoding.Default;
            //srtest.Close();  

            return encode;

        }

        public static int utf8_probability(byte[] rawtext)
        {
            int score = 0;
            int i, rawtextlen = 0;
            int goodbytes = 0, asciibytes = 0;

            // Maybe also use UTF8 Byte Order Mark:  EF BB BF

            // Check to see if characters fit into acceptable ranges
            rawtextlen = rawtext.Length;
            for (i = 0; i < rawtextlen; i++)
            {
                if ((rawtext[i] & (byte)0x7F) == rawtext[i])
                {  // One byte
                    asciibytes++;
                    // Ignore ASCII, can throw off count
                }
                else
                {
                    int m_rawInt0 = Convert.ToInt16(rawtext[i]);
                    int m_rawInt1 = Convert.ToInt16(rawtext[i + 1]);
                    int m_rawInt2 = Convert.ToInt16(rawtext[i + 2]);

                    if (256 - 64 <= m_rawInt0 && m_rawInt0 <= 256 - 33 && // Two bytes
                     i + 1 < rawtextlen &&
                     256 - 128 <= m_rawInt1 && m_rawInt1 <= 256 - 65)
                    {
                        goodbytes += 2;
                        i++;
                    }
                    else if (256 - 32 <= m_rawInt0 && m_rawInt0 <= 256 - 17 && // Three bytes
                     i + 2 < rawtextlen &&
                     256 - 128 <= m_rawInt1 && m_rawInt1 <= 256 - 65 &&
                     256 - 128 <= m_rawInt2 && m_rawInt2 <= 256 - 65)
                    {
                        goodbytes += 3;
                        i += 2;
                    }
                }
            }

            if (asciibytes == rawtextlen) { return 0; }

            score = (int)(100 * ((float)goodbytes / (float)(rawtextlen - asciibytes)));

            // If not above 98, reduce to zero to prevent coincidental matches
            // Allows for some (few) bad formed sequences
            if (score > 98)
            {
                return score;
            }
            else if (score > 95 && goodbytes > 30)
            {
                return score;
            }
            else
            {
                return 0;
            }

        }



    }
}
