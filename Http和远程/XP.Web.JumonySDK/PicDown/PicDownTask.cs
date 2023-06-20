using Ivony.Html;
using Ivony.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Util.Http.Down;
using XP.Util.Win;

namespace XP.Web.JumonySDK.PicDown
{


    /// <summary>
    /// 图片下载任务
    /// </summary>
    public class PicDownTask
    {
        public string PageTitle { get; set; }
        public string PageUrl { get; set; }
        public string SelectorRule { get; set; }


        public List<string> DownUrlList { get; set; } = new List<string>();

        public List<string> FindPath_Mode2()
        {
            DownUrlList = new List<string>();


            var dom = new JumonyParser().LoadDocument(PageUrl);
            var HtmlTitle = dom.Find("head title").First().InnerText();
            if (String.IsNullOrEmpty(HtmlTitle)) return null;
            PageTitle = HtmlTitle.Split('-')[0].Trim();

            var imgs = dom.Find(SelectorRule).ToList();
            foreach (var img in imgs)
            {
                string Src = img.Attribute("ess-data").AttributeValue;
                var attr = img.Attribute("src");
                if (null != attr)
                {
                    Src = attr.AttributeValue;
                }
                Src = GetRealPath(Src);
                DownUrlList.Add(Src);

            }

            return DownUrlList;
        }

        public string GetRealPath(string input)
        {
            string Pattern = "resize?url=";
            string Output = null;
            int PatternStart = input.IndexOf(Pattern);
            if (0 > PatternStart)
            {
                return input;
            }
            string RealUrl = input.Substring(PatternStart + Pattern.Length);

            Output = HttpUtility.UrlDecode(RealUrl);

            return Output;
        }


        public void StartDown( string root)
        {
            PicDownTaskQueue _Queue = new PicDownTaskQueue(PageTitle, root, DownUrlList);
            _Queue.Play();

        }


    }
}
