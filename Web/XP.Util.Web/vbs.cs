using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XP
{
    /// <summary>
    /// 封装asp时代一些vbs常用的vbs函数
    /// </summary>
    public static class vbs
    {

        #region 转换文本到HTML
        public static string FormatText2Html(string str)
        {
            string result = str.Replace("\n", "<br />");
            result = result.Replace("&gt;", ">");
            result = result.Replace("<", "&lt;");
            result = result.Replace(" ", "&nbsp;");
            result = result.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");

            return result;
            //htmlText = Replace(htmlText, ">", "&gt;")
            //htmlText = Replace(htmlText, "<", "&lt;")
            //htmlText = Replace(htmlText, CHR(34), "&quot;")
            //htmlText = Replace(htmlText, CHR(39), "&#39;")
            //htmlText = Replace(htmlText, " ", "&nbsp;")
            //htmlText = Replace(htmlText, CHR(9), "&nbsp;&nbsp;&nbsp;&nbsp;")
            //htmlText = Replace(htmlText, CHR(13), "<br>")
            //htmlText = Replace(htmlText, CHR(10), "")


        }
        public static string FormatText2Html(object o)
        {
            return FormatHtml2Text(o.ToString());
        }

        #endregion

        #region 转换HTML到文本

        public static string FormatHtml2Text(string str)
        {
            string result = str.Replace("\n", "<br />");
            result = result.Replace(">", "&gt;");
            result = result.Replace("<", "&lt;");
            result = result.Replace(" ", "&nbsp;");
            result = result.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");

            return result;
            //htmlText = Replace(htmlText, ">", "&gt;")
            //htmlText = Replace(htmlText, "<", "&lt;")
            //htmlText = Replace(htmlText, CHR(34), "&quot;")
            //htmlText = Replace(htmlText, CHR(39), "&#39;")
            //htmlText = Replace(htmlText, " ", "&nbsp;")
            //htmlText = Replace(htmlText, CHR(9), "&nbsp;&nbsp;&nbsp;&nbsp;")
            //htmlText = Replace(htmlText, CHR(13), "<br>")
            //htmlText = Replace(htmlText, CHR(10), "")


        }
        public static string FormatHtml2Text(object o)
        {
            return FormatHtml2Text(o.ToString());
        }


        #endregion

        public static string CutHtml(object o, int length)
        {
            if (null == o)
                return String.Empty;
            return CutHtml(o.ToString(), length);
        }
        public static string CutHtml(string Htmlstring, int length)
        {
            if (String.IsNullOrEmpty(Htmlstring))
            {
                return String.Empty;
            }

            string txt = NoHTML(Htmlstring);
            return Left(txt, length);
        }

        public static string Left(string txt, int length)
        {
            if (0 > length) return String.Empty;
            //早先对C#不太熟，所以不放心int会不会被赋0值，所以有下面的代码
            //if (length == 0)
            //{
            //    length = 10;
            //}
            if (txt.Length > length)
            {
                txt = txt.Substring(0, length);
            }
            return txt;
        }

        public static string Left(string txt, int length, string suffix)
        {
            if (0 >= length) return String.Empty;
            //早先对C#不太熟，所以不放心int会不会被赋0值，所以有下面的代码
            //if (length == 0)
            //{
            //    length = 10;
            //}
            if (txt.Length > length)
            {
                txt = txt.Substring(0, length) + suffix;
            }
            return txt;
        }

        public static string Left(object o, int length, string suffix)
        {
            if (null == o)
                return String.Empty;

            return Left(o.ToString(), length, suffix);
        }

        public static string Summary(object o, int length, string suffix)
        {
            if (null == o)
                return String.Empty;
            string NewsSummary = Left(o.ToString(), length, suffix);
            if (0 == NewsSummary.Length)
                return String.Empty;
            NewsSummary = NewsSummary.Replace("\n", "<br />");
            return NewsSummary;
        }

        /// <summary>
        /// 生成标签
        /// </summary>
        /// <param PropertyName="o">数据库取取的参数值</param>
        /// <param PropertyName="baseURI">用来生成标签链接的基础URI</param>
        /// <returns></returns>
        public static string GetTag(object o, string baseURI)
        {
            if (null == o)
                return String.Empty;
            string NewsTags = o as string;
            if (0 == NewsTags.Length)
                return String.Empty;
            if (String.IsNullOrEmpty(baseURI))
            {
                return String.Empty;
            }
            var Arr = NewsTags.Split(new char[] { '\n', '|' });
            StringBuilder sb = new StringBuilder();
            bool HasStar = false;
            foreach (var s in Arr)
            {
                if (HasStar)
                {
                    sb.Append(" | ");
                }
                else
                {
                    HasStar = true;
                }
                sb.Append("<a href=\"");
                sb.Append(baseURI);
                sb.Append(s);
                sb.Append("\">");
                sb.Append(s);
                sb.Append("</a>");
            }

            return sb.ToString();
        }



        #region 过滤HTML

        public static string NoHTML(string Htmlstring)  //替换HTML标记   
        {
            //删除脚本   
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML   
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<img[^>]*>;", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = System.Web.HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;

        }

        public static string checkStr(string html)
        {
            System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[\s\S]+</script *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@" href *= *[\s\S]*script *:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@" no[\s\S]*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<iframe[\s\S]+</iframe *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex5 = new System.Text.RegularExpressions.Regex(@"<frameset[\s\S]+</frameset *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex6 = new System.Text.RegularExpressions.Regex(@"\<img[^\>]+\>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex7 = new System.Text.RegularExpressions.Regex(@"</p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex8 = new System.Text.RegularExpressions.Regex(@"<p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex9 = new System.Text.RegularExpressions.Regex(@"<[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记 
            html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性 
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件 
            html = regex4.Replace(html, ""); //过滤iframe 
            html = regex5.Replace(html, ""); //过滤frameset 
            html = regex6.Replace(html, ""); //过滤frameset 
            html = regex7.Replace(html, ""); //过滤frameset 
            html = regex8.Replace(html, ""); //过滤frameset 
            html = regex9.Replace(html, "");
            html = html.Replace(" ", "");
            html = html.Replace("</strong>", "");
            html = html.Replace("<strong>", "");
            return html;
        }


        #endregion



        #region  MD5 的有关实现

        public static string MD5(string str, int size)
        {
            if (str == null && str == String.Empty)
                return str;
            str = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
            if (size >= 32)
                return str;
            str = str.Substring((32 - size) / 2, size);
            return str;


        }
        public static string SHA1(string str)
        {
            str = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1");

            return str.ToLower();

        }
        public static string SHA2(string str)
        {
            str = SHA1(str);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i += 2)
            {
                sb.Append(str[i]);
            }
            for (int i = str.Length; i > 0; i -= 2)
            {
                sb.Append(str[i - 1]);
            }
            return sb.ToString();
        }


        #region 再次加密和解密

        public static string SHA3(string str)
        {
            str = SHA1(str);
            //SHA1密码长度
            int SHA1PwdLenght = str.Length;
            //每组随机数长度4
            int SubRandomLength = 4;
            //最终结果存储在这数组当中(长度是：SHA1密码长度+3组随机数）
            char[] ResultChar = new char[SHA1PwdLenght + SubRandomLength * 3];
            //SHA1加密密码的新序列索引
            int[] PasswordNewIndexArray = new int[SHA1PwdLenght];
            //下面用来存储索引的临时变量
            int PasswordNewIndexArray_Index = 0;
            for (int i = 0; i < str.Length; i += 2)
            {
                PasswordNewIndexArray[PasswordNewIndexArray_Index] = i;
                PasswordNewIndexArray_Index++;
            }
            for (int i = str.Length; i > 0; i -= 2)
            {
                PasswordNewIndexArray[PasswordNewIndexArray_Index] = i - 1;
                PasswordNewIndexArray_Index++;
            }
            Random rand = new Random();
            //下面是两个末尾识别码
            int EndIndex1 = rand.Next(0, 16);
            int EndIndex2 = rand.Next(0, 16);
            //头部随机长度
            int PreRandomLength = rand.Next(3, 16);
            //Trace.WriteLine("EndIndex1:" + EndIndex1);
            //Trace.WriteLine("EndIndex2:" + EndIndex2);
            //Trace.WriteLine("\n PreRandomLength : " + PreRandomLength);
            char[] SubRandChar1 = new char[SubRandomLength];
            char[] SubRandChar2 = new char[SubRandomLength];
            char[] SubRandChar3 = new char[SubRandomLength];
            //头部随机字符串
            char[] PreRandom = new char[PreRandomLength];

            for (int i = 0; i < SubRandomLength; i++)
            {
                SubRandChar1[i] = GetChr(rand.Next(0, CharArr.Length - 1));
                SubRandChar2[i] = GetChr(rand.Next(0, CharArr.Length - 1));
                SubRandChar3[i] = GetChr(rand.Next(0, CharArr.Length - 1));
            }

            for (int i = 0; i < PreRandomLength; i++)
            {
                PreRandom[i] = GetChr(rand.Next(0, CharArr.Length - 1));
            }


            //填充数据
            PasswordNewIndexArray_Index = 0;
            for (int i = 0; i < ResultChar.Length; i++)
            {
                if (i == EndIndex1)
                {
                    for (int k = 0; k < SubRandChar1.Length; k++)
                    {
                        ResultChar[i + k] = SubRandChar1[k];
                    }
                    i += SubRandomLength - 1;
                    continue;
                }
                else
                {
                    if (i == EndIndex2 + 4 + EndIndex1)
                    {

                        for (int k = 0; k < SubRandomLength; k++)
                        {
                            ResultChar[i + k] = SubRandChar2[k];
                        }
                        i += SubRandomLength - 1;
                        continue;
                    }
                    if (PasswordNewIndexArray_Index < PasswordNewIndexArray.Length)
                    {
                        ResultChar[i] = str[PasswordNewIndexArray[PasswordNewIndexArray_Index]];
                        PasswordNewIndexArray_Index++;
                    }
                    else
                    {
                        for (int k = 0; k < SubRandomLength; k++)
                        {
                            ResultChar[i + k] = SubRandChar2[k];
                        }
                        //i += SubRandomLength ;
                        //ResultChar[i] = GetChr(EndIndex1);
                        //i++;
                        //ResultChar[i] = GetChr(EndIndex2);
                        break;
                    }
                }

            }

            string result;
            //Trace.WriteLine("生成的ResultChar：");
            //Trace.WriteLine(new string(ResultChar));
            result = new string(ResultChar);



            string prefix = new string(PreRandom);
            result = prefix + result + GetChr(PreRandomLength) + GetChr(EndIndex1) + GetChr(EndIndex2);

            //Trace.WriteLine("生成的字符串：");
            //Trace.WriteLine(result);

            //result = sb.ToString();
            return result;

        }


        public static string UnSHA3(string str)
        {
            int FullLength = str.Length;
            if (3 > FullLength)
                return string.Empty;


            //下面是两个末尾识别码
            int EndIndex1 = AllChar.IndexOf(str[FullLength - 2]);
            int EndIndex2 = AllChar.IndexOf(str[FullLength - 1]);
            //头部随机长度
            int PreRandomLength = AllChar.IndexOf(str[FullLength - 3]);
            //Trace.WriteLine("\n PreRandomLength : " + PreRandomLength);
            string str1 = str.Substring(PreRandomLength);
            int SubLength = str1.Length;
            string result = str1;
            char[] ResultCharArr = new char[40];
            int ResultCharArr_Index = 0;
            for (int i = 0; i < SubLength; i++)
            {

                if (i == EndIndex1)
                {
                    i += 4;
                }
                if (i == EndIndex2 + EndIndex1 + 4)
                {
                    i += 4;
                }
                ResultCharArr[ResultCharArr_Index] = str1[i];
                ResultCharArr_Index++;
                if (ResultCharArr_Index >= ResultCharArr.Length)
                    break;
            }

            result = new string(ResultCharArr);
            return result;
        }
        public static char GetChr(int index)
        {
            if (0 > index || index > CharArr.Length - 1)
                return '0';
            return CharArr[index];

        }

        public static string AllChar
        {
            get { return "0123456789abcdefghijklmnopqrstuvwxyz"; }
        }
        public static char[] CharArr
        {
            get
            {
                return AllChar.ToCharArray();
            }
        }

        #endregion


        public static string MD5(string str)
        {
            return MD5(str, 16);

        }


        /*这种MD5的算不可取，舍弃。
         * 
        public static string MD5(string json)
        {
            byte[] data = System.Text.Encoding.Unicode.GetBytes(json.ToCharArray());
            string result = System.Text.Encoding.Unicode.GetString(MD5hash(data));
            return result;
        }

       public static byte[] MD5hash(byte[] data)
        {
            // This is one implementation of the abstract class MD5.
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
           // md5.HashSize = 16;
            byte[] result = md5.ComputeHash(data);

            return result;
        }
        */

        #endregion

        #region 检查类型：是否为数字、整数
        public static bool IsNumric(object o)
        {
            string str = o.ToString();
            if (String.IsNullOrEmpty(str))
                return false;
            char[] c = str.Trim().ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (0 == i && ('-' == c[0] || '+' == c[0])) continue;
                if (!((c[i] >= '0' && c[i] <= '9') || c[i] == '.'))
                    return false;
            }
            return true;
        }
        public static bool IsInt(object o)
        {
            if (null == o)
                return false;
            string str = o.ToString();
            char[] c = str.Trim().ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (0 == i && ('-' == c[0] || '+' == c[0])) continue;
                if (c[i] < '0' || c[i] > '9')
                    return false;
            }
            return true;
        }
        #endregion

        #region 清理不合法的Query字符串
        public static string CleanQueryString(object o)
        {
            string str = o.ToString();
            str = str.Replace(" ", "");
            str = str.Replace("=", "");
            str = str.Replace(">", "");
            return str;
        }
        #endregion

    }
}
