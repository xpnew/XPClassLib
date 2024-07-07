using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;


namespace XP.Util.WebUtils
{
    /// <summary>
    /// 清除HTML
    /// </summary>
    public class CleanHtml
    {
        private static List<RegexItem> _BaseList;
        public static List<RegexItem> BaseList
        {

            get
            {
                if (null == _BaseList)
                {
                    _BaseList = new List<RegexItem>();
                    _BaseList.Add(new RegexItem(@"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase));
                    _BaseList.Add(new RegexItem(@"<(.[^>]*)>", "", RegexOptions.IgnoreCase));
                    _BaseList.Add(new RegexItem(@"([\r\n])[\s]+", "", RegexOptions.IgnoreCase));
                    _BaseList.Add(new RegexItem(@"-->", "", RegexOptions.IgnoreCase));
                    _BaseList.Add(new RegexItem(@"<!--.*", "", RegexOptions.IgnoreCase));
                    _BaseList.Add(new RegexItem(@"&(quot|#34);", "\"", RegexOptions.IgnoreCase));
                    _BaseList.Add(new RegexItem(@"&(amp|#38);", "&", RegexOptions.IgnoreCase));
                    _BaseList.Add(new RegexItem(@"&(lt|#60);", "<", RegexOptions.IgnoreCase));
                    _BaseList.Add(new RegexItem(@"&(gt|#62);", ">", RegexOptions.IgnoreCase));

                    _BaseList.Add(new RegexItem(@"&(nbsp|#160);", " ", RegexOptions.IgnoreCase));
                    _BaseList.Add(new RegexItem(@"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase));
                    _BaseList.Add(new RegexItem(@"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase));
                    _BaseList.Add(new RegexItem(@"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase));
                    _BaseList.Add(new RegexItem(@"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase));
                    _BaseList.Add(new RegexItem(@"&#(\d+);", "", RegexOptions.IgnoreCase));
                    _BaseList.Add(new RegexItem(@"<img[^>]*>;", "", RegexOptions.IgnoreCase));
                }
                return _BaseList;
            }
        }

        #region =====过滤html标签 DropHTML(string html)=====
        /// <summary>过滤html
        /// </summary>
        /// <remarks>
        /// 原始代码：http://www.cnblogs.com/elephant-wp/archive/2011/11/01/2231373.html
        /// 这里主要做出如下改动：
        /// 这里是在静态类当中使用实例化的Regex有利于提高运行速度，而.net默认只缓存最近使用的15个Regex对象
        /// 增加了enableEncode参数，并且允许转换“<>”符号和换行。
        /// </remarks>
        /// <param name="html">需要过滤的字符串</param>
        /// <param name="enableEncoding">是否允许转换</param>
        /// <param name="additionList">附加的过滤规则</param>
        /// <returns>过滤html后的字符串</returns>
        public static string DropHTML(string html, bool enableEncoding, List<RegexItem> additionList)
        {
            if (String.IsNullOrEmpty(html))
                return html;
            List<RegexItem> RegList;

            if (null == additionList || 0 == additionList.Count)
            {
                RegList = BaseList;
            }
            else
            {
                RegList = new List<RegexItem>();
                RegList.AddRange(BaseList);
                RegList.AddRange(additionList);
            }
            foreach (RegexItem item in RegList)
            {
                html = item.Replace(html);
            }
            if (enableEncoding)
            {
                html = html.Replace("<", "");
                html = html.Replace(">", "");
                html = html.Replace("\r\n", "");
                html = html.Replace("\n", "");
            }
            else
            {
                html = html.Replace("<", "&lt;");
                html = html.Replace(">", "&gt;");
                html = html.Replace("\r\n", "<br />");
                html = html.Replace("\n", "<br />");
            }
            //html = HttpContext.Current.Server.HtmlEncode(html).Trim();
            //html = HttpContext.Current.Server.HtmlDecode(html).Trim();
            return html;
        }
        /// <summary>
        /// 过滤html
        /// </summary>
        /// <param name="html">需要过滤的字符串</param>
        /// <returns>过滤html后的字符串</returns>
        public static string DropHTML(string html)
        {
            return DropHTML(html, false);
        }
        /// <summary>
        /// 过滤html
        /// </summary>
        /// <param name="html">需要过滤的字符串</param>
        /// <param name="enableEncoding">是否允许转换</param>
        /// <returns></returns>
        public static string DropHTML(string html, bool enableEncoding)
        {
            return DropHTML(html, enableEncoding, null);
        }





        public static string FilterHtml(string html)
        {
            return FilterHtml(html, false);
        }

        public static string FilterHtml(string input, bool enableEncoding)
        {
            return FilterHtml(input, enableEncoding, null);
        }

        /// <summary>
        /// 过滤html
        /// </summary>
        /// <param name="input">需要过滤的字符串</param>
        /// <param name="enableEncoding"></param>
        /// <param name="additionList"></param>
        /// <returns></returns>
        public static string FilterHtml(string input, bool enableEncoding, List<RegexItem> additionList)
        {
            if (input == null || input == "")
                return input;
            input = input.Replace("&amp;", "&");
            input = input.Replace("&lt;", "<");
            input = input.Replace("&gt;", ">");
            input = input.Replace("&nbsp;", " ");
            input = input.Replace("&quot;", "\\");
            input = input.Replace("<br>\r\n", "\r\n");
            input = input.Replace("<img[^>]*>;", "");

            //input = input.Replace("<script[^>]*?>.*?</script>", "");
            //input = input.Replace("<(.[^>]*)>", "");
            //input = input.Replace("-->", "");
            //input = input.Replace("<!--.*", "");
            //input = input.Replace("&(quot|#34);", "'\'");
            //input = input.Replace("&(amp|#38);", "&");
            //input = input.Replace("&(lt|#60);", "<");
            //input = input.Replace("&(gt|#62);", ">");
            //input = input.Replace("&(nbsp|#160);", " ");
            //input = input.Replace("&(iexcl|#161);", "\xa1");
            //input = input.Replace("&(cent|#162);", "\xa2");
            //input = input.Replace("&(pound|#163);", "\xa3");
            //input = input.Replace("&(pound|#163);", "\xa9");
            //input = input.Replace("&(copy|#169);", "\xa3");
            //input = input.Replace("<img[^>]*>;", "");
            //input = input.Replace("<", "");
            //input = input.Replace(">", "");
            //input = input.Replace("\r\n", "");


            return input;
        }
        #endregion


    }
}
