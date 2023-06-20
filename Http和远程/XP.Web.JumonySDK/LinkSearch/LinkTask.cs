using Ivony.Html;
using Ivony.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Comm.Task;

namespace XP.Web.JumonySDK.LinkSearch
{

    /// <summary>
    /// 一个关于 LinkSearch 的任务
    /// </summary>
    public class LinkTask : BaseTaskItem
    {
        /// <summary>
        /// 需要搜索的关键字
        /// </summary>
        public string SearchKey { get; set; }

        /// <summary>
        /// 准备搜索的页面地址、模板
        /// </summary>
        public string PageUrl { get; set; }

        /// <summary>
        /// dom选择器（类似于jQuery）
        /// </summary>
        public string DomSelector { get; set; }


        public LinkQueue Queue { get; set; }



        public List<LinkResultItem> Result { get; set; }


        public override async Task WorkAsync()
        {
            await base.WorkAsync();
            await LoadAndParse();
            this.Queue = null;
        }


        async Task LoadAndParse()
        {
            var t = Task.Run(() =>
            {

                var dom = new JumonyParser().LoadDocument(PageUrl);
                XP.x.Say("页面地址：： " + PageUrl);

                if (String.IsNullOrEmpty(dom.RawHtml))
                {
                    dom = Reload(PageUrl).Result;
                    if (null == dom)
                        return;
                }
                var HtmlTitle = dom.Find("head title").First().InnerText();
                XP.x.Say("页面标题： " + HtmlTitle);
                var links = dom.Find(DomSelector).ToList();
                foreach (var link in links)
                {
                    string Src = link.Attribute("href").AttributeValue;
                    var attr = link.Attribute("src");
                    string DomText = link.InnerText();
                    if (!CheckKey(DomText)) continue;

                    var NewItem = new LinkResultItem()
                    {
                        LinkInnerText = DomText,
                        LinkUrl = Src,
                    };

                    Queue.AddReuslt(NewItem);
                    //Result.Add(NewItem);

                }
            });

            await t;

        }
        private bool CheckKey(string input)
        {
            if (input.Contains(SearchKey))
            {
                return true;
            }
            return false;
        }


        async Task<IHtmlDocument> Reload(string url)
        {
            string html = String.Empty;

            var HeadDict = new Dictionary<string, string>();
            HeadDict.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            HeadDict.Add("Referer", "https://www.t66y.com/thread0806.php?fid=8&search=&page=1");
            HeadDict.Add("Content-Type", "text/html");
            HeadDict.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.45 Safari/537.36");
            if (XP.Util.Http.HttpTool.GetHttpResponse(url, out html, out string stat, HeadDict))
            {

                var NewDom = new JumonyParser().Parse(html);
                return NewDom;
            }
            else
            {
                return null;
            }
        }

    }
}
