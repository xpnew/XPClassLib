using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io; // 确保已引用 AngleSharp.Io 命名空间
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using XP.Comm.Task;


namespace XP.Web.AngleSharpSDK.PicDown
{
    /// <summary>
    /// PicDownTask 的摘要说明
    /// </summary>
    /// <remarks>
    /// 作    者：Administrator
    /// 创建时间：2026/8/22 12:39:26	
    /// 
    /// </remarks>
    public class PicDownTask : BaseTaskItem
    {

        public string PageTitle { get; set; }
        public string PageUrl { get; set; }
        public string SelectorRule { get; set; }


        public List<string> DownUrlList { get; set; } = new List<string>();







        public override async Task WorkAsync()
        {
            await base.WorkAsync();

            await LoadAndParse();
            //this.Queue = null;
        }
        async Task LoadAndParse()
        {

            var imgs = await Find(SelectorRule);
            foreach (var elm in imgs)
            {
                var src = elm.GetAttribute("src");
                if (null == src)
                {
                    src = elm.GetAttribute("ess-data");
                }

                if (null != src)
                {
                    DownUrlList.Add(src);
                }
            }

            NoticeStr = $"一共获得图片数量 [ {DownUrlList.Count}  ]";

        }

        public async Task TestAsync()
        {

            var t = Task.Run(async () =>
            {


                //var config = Configuration.Default.WithDefaultLoader();
                //var context = BrowsingContext.New(config);
                //var document = await context.OpenAsync(PageUrl);

                //var HtmlTitle = document.QuerySelector("head title").TextContent;
                ////var cellSelector = "tr.vevent td:nth-child(3)";
                //var cells = document.QuerySelectorAll(SelectorRule);
                ////var titles = cells.Select(m => m.TextContent);

                //var urls = cells.Select(m => m.GetAttribute("src") ?? m.GetAttribute("href")).Where(m => !string.IsNullOrEmpty(m)).ToList();

                var imgs = await Find(SelectorRule);
                foreach (var elm in imgs)
                {
                    x.Say("准备查找图片地址");
                    var src = elm.GetAttribute("src");
                    if (null == src)
                    {
                        src = elm.GetAttribute("ess-data");
                    }

                    if (null != src)
                    {

                        x.Say("找到图片地址 ^^^^  " + src);
                        DownUrlList.Add(src);
                    }
                }

                NoticeStr = $"一共获得图片数量 [ {DownUrlList.Count}  ]";
            });

            await t;

        }
        public async Task<string> TestSelector(string pattern)
        {

            var els = await Find(pattern);



            return String.Join("", els);


        }


        protected async Task<IHtmlCollection<IElement>> Find(string pattern)
        {
            // 1. 确保 PageUrl 有效
            if (string.IsNullOrWhiteSpace(PageUrl))
            {
                throw new InvalidOperationException("目标 URL 不能为空");
            }

            // 2. 确保配置正确初始化
            //var config = Configuration.Default.WithDefaultLoader();

            // 创建配置时，显式指定使用 HttpClientRequester
            var loaderOptions = new AngleSharp.Io.LoaderOptions
            {
                IsResourceLoadingEnabled = true,
                Filter = request => true // 或自定义过滤规则
            };
            var config = Configuration.Default
                .WithDefaultLoader(loaderOptions);

            if (config == null)
            {
                throw new InvalidOperationException("AngleSharp 配置初始化失败");
            }
            // 修正后代码：
            if (!config.Services.Any(s => s is IRequester))
            {
                throw new InvalidOperationException("AngleSharp网络请求服务未正确注册，可能是依赖包缺失");
            }
            //// 替换掉原来的config空判断，直接校验内部请求服务是否正常注册
            //if (!config.Services.Has<AngleSharp.Network.IHttpRequester>())
            //{
            //    throw new InvalidOperationException("AngleSharp网络请求服务未正确注册，可能是依赖包缺失");
            //}
            // 3. 创建上下文
            var context = BrowsingContext.New(config);
            if (context == null)
            {
                throw new InvalidOperationException("浏览器上下文创建失败");
            }
            XP.Loger.Debug($"AngleSharp上下文创建成功，准备加载页面：{PageUrl}");
            XP.Loger.Error($"AngleSharp上下文创建成功，准备加载页面：{PageUrl}");

            // 移除原来的context空校验，新增超时控制，避免异步上下文空转
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            try
            {
                // 传入校验通过的Uri实例+超时令牌，从根源避免内部空引用
                var doc = await context.OpenAsync(PageUrl, cts.Token);
                // 后续处理逻辑
                var HtmlTitle = doc.QuerySelector("head title").TextContent;

                if (this.PageTitle == null)
                {
                    PageTitle = HtmlTitle;
                }
                //var cellSelector = "tr.vevent td:nth-child(3)";
                var cells = doc.QuerySelectorAll(pattern);

                return cells;
            }
            catch (NullReferenceException nrex)
            {
                XP.Loger.Error("AngleSharp OpenAsync触发空引用", nrex);
                // 专门捕获该空引用异常，输出堆栈定位真实内部空值点
                throw new InvalidOperationException($"OpenAsync触发空引用，堆栈信息：{nrex.StackTrace}", nrex);
            }
            catch (Exception ex)
            {
                XP.Loger.Error("AngleSharp OpenAsync触发空引用", ex);
                Console.WriteLine($"加载页面失败: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"内部异常: {ex.InnerException.Message}");
                }
                throw;
            }



            //var config = Configuration.Default.WithDefaultLoader();
            //var context = BrowsingContext.New(config);
            //var document = await context.OpenAsync(PageUrl);

            //var HtmlTitle = document.QuerySelector("head title").TextContent;

            //if(this.PageTitle == null)
            //{
            //    PageTitle = HtmlTitle;
            //}
            ////var cellSelector = "tr.vevent td:nth-child(3)";
            //var cells = document.QuerySelectorAll(pattern);

            //return cells;
        }

    }
}
