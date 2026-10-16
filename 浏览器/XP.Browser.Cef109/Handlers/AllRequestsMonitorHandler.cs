using CefSharp;
using CefSharp.Handler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Browser.Cef109.Filters;

namespace XP.Browser.Cef109.Handlers
{
    /// <summary>
    /// 监听所有的页面请求的 AllRequestsMonitorHandler 的摘要说明
    /// </summary>
    /// <remarks>
    /// 作    者：Administrator
    /// 创建时间：2026/9/14 20:55:38	
    /// 
    /// </remarks>
    public class AllRequestsMonitorHandler: NoCacheRequestHandler
    {

        public IRequestFileFilter RequestFileFilter { get; set; }


        public bool EnableTrace { get; set; } = false;


        public AllRequestsMonitorHandler() {
            x.Say("创建了一个AllRequestsMonitorHandler实例。。");
        }
        // protected override IResourceRequestHandler GetResourceRequestHandler(
        //IWebBrowser chromiumWebBrowser,
        //IBrowser browser,
        //IFrame frame,
        //IRequest request,
        //bool isNavigation,
        //bool isDownload,
        //string requestInitiator,
        //ref bool disableDefaultHandling)
        // {
        //     // 获取当前请求的完整 URL 和资源类型
        //     string url = request.Url;
        //     ResourceType resourceType = request.ResourceType;

        //     // 在这里进行过滤和分析。例如，只筛选图片请求：
        //     if (resourceType == ResourceType.Image)
        //     {
        //         System.Diagnostics.Debug.WriteLine($"[捕获到图片请求] URL: {url}");
        //         // 你可以将 url 添加到一个线程安全的集合中供外部使用
        //     }

        //     // 如果还需要分析其他类型的请求（如 XHR 数据接口）：
        //     if (resourceType == ResourceType.Xhr || resourceType == ResourceType.Prefetch)
        //     {
        //         System.Diagnostics.Debug.WriteLine($"[捕获到数据请求] URL: {url}");
        //     }

        //     // 返回 null 表示让浏览器使用默认的处理方式继续加载该资源
        //     // 如果你需要拦截并修改响应内容，则返回自定义的 IResourceRequestHandler 实例
        //     return null;
        // }


        /// <summary>
        /// 只重写受保护的虚方法，不复写爷爷层的公开方法
        /// 先执行父类的默认行为（注入禁用缓存头），再扩展监控逻辑
        /// </summary>
        protected override void OnBeforeRequest(IRequest request)
        {
            // 1. 先调用父类的默认实现（注入禁用缓存头）
            base.OnBeforeRequest(request);

            // 2. 再扩展自己的监控逻辑
            //string url = request.Url;
            //string method = request.Method;
            // 获取当前请求的完整 URL 和资源类型
            string url = request.Url;
            ResourceType resourceType = request.ResourceType;

            RequestFileFilter?.Push(url, resourceType);
            // 在这里进行过滤和分析。例如，只筛选图片请求：
            if (resourceType == ResourceType.Image && EnableTrace)
            {
                System.Diagnostics.Debug.WriteLine($"[捕获到图片请求] URL: {url}");
                // 你可以将 url 添加到一个线程安全的集合中供外部使用
            }

            // 如果还需要分析其他类型的请求（如 XHR 数据接口）：
            if (EnableTrace && (resourceType == ResourceType.Xhr || resourceType == ResourceType.Prefetch))
            {
                System.Diagnostics.Debug.WriteLine($"[捕获到数据请求] URL: {url}");
            }
        }

        protected override void OnAfterRequest(string requestUrl, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
        {
            base.OnAfterRequest( requestUrl,request, response, status, receivedContentLength);

            // 你可以在这里处理请求完成后的逻辑，例如记录响应状态码   
            // 判断 404
            bool isNotFound = response?.StatusCode == 404;

            // 获取文件大小（bytes）
            long sizeBytes = receivedContentLength;

            // 获取 MIME 类型（判断是不是图片）
            string mimeType = response?.MimeType;
            // 最终 URL 从 request 里拿（CefSharp 会在回调中更新为跳转后的地址）
            string finalUrl = requestUrl;

            RequestFileFilter.CheckAndSave(finalUrl, response?.StatusCode ?? 0, sizeBytes, mimeType);
            if (EnableTrace)
            {
                Debug.WriteLine(
                    $"[After] {finalUrl} | " +
                    $"Status: {response?.StatusCode} | " +
                    $"Size: {sizeBytes} bytes | " +
                    $"Type: {mimeType} | " +
                    $"LoadStatus: {status}");
            }

        }
    }
}
