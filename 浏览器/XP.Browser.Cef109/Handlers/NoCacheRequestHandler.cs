using CefSharp;
using CefSharp.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Browser.Cef109.Handlers
{
    /// <summary>
    /// 禁用缓存请求 NoCacheRequestHandler 的摘要说明
    /// </summary>
    /// <remarks>
    /// 作    者：Administrator
    /// 创建时间：2026/9/17 10:27:42	
    /// 
    /// </remarks>
    public class NoCacheRequestHandler : RequestHandler
    {

        /// <summary>
        /// 是否启用禁用缓存功能
        /// </summary>
        protected bool IsCacheDisabled { get; set; } = false;

        /// <summary>
        /// Before：请求发送前，只能拿到 IRequest
        /// </summary>
        protected virtual void OnBeforeRequest(IRequest request)
        {

        }


        /// <summary>
        /// After：请求完成后，能拿到 IRequest + IResponse + 状态 + 字节数
        /// </summary>
        protected virtual void OnAfterRequest(
            string requestUrl,
            IRequest request,
            IResponse response,
            UrlRequestStatus status,
            long receivedContentLength)
        {
        }
        /// <summary>
        /// 重写爷爷层的公开方法，调用受保护的虚方法
        /// 这个方法只负责调用 OnBeforeRequest，不写具体业务逻辑
        /// </summary>
        protected override IResourceRequestHandler GetResourceRequestHandler(
            IWebBrowser chromiumWebBrowser,
            IBrowser browser,
            IFrame frame,
            IRequest request,
            bool isNavigation,
            bool isDownload,
            string requestInitiator,
            ref bool disableDefaultHandling)
        {
            // 调用受保护的虚方法，让子类有机会扩展
            OnBeforeRequest(request);

            if (IsCacheDisabled)
            {
                var headers = request.Headers;
                headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                headers["Pragma"] = "no-cache";
                headers["Expires"] = "0";
                request.Headers = headers;
            }

            // 使用这种方式处理after ，是因为 ResourceRequestHandler 的 OnResourceLoadComplete 方法在请求完成后会被调用
            //这是异步处理的机制决定的
            return new InnerResourceRequestHandler(this, request.Url, request);
        }


        private class InnerResourceRequestHandler : ResourceRequestHandler
        {
            private readonly NoCacheRequestHandler _parent;
            private readonly IRequest _request;
            private readonly string _url;

            public InnerResourceRequestHandler(NoCacheRequestHandler parent,string url, IRequest request)
            {
                _parent = parent;
                _url = url; 
                _request = request;
            }

            protected override void OnResourceLoadComplete(
                IWebBrowser chromiumWebBrowser,
                IBrowser browser,
                IFrame frame,
                IRequest request,
                IResponse response,
                UrlRequestStatus status,
                long receivedContentLength)
            {
                base.OnResourceLoadComplete(
                    chromiumWebBrowser, browser, frame, request,
                    response, status, receivedContentLength);

                _parent.OnAfterRequest(_url, _request, response, status, receivedContentLength);
            }
        }
    }
}
