using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Browser.Cef109.Filters
{
    /// <summary>
    /// 过滤器结果项 FilterResultItem 的摘要说明
    /// </summary>
    /// <remarks>
    /// 作    者：Administrator
    /// 创建时间：2026/9/20 12:10:27	
    /// 
    /// </remarks>
    public class FilterResultItem
    {
        public string Url { get; set; }

        public string Name { get; set; }

        public string MimeType { get; set; }

        public int StatusCode { get; set; } 
        public string StatusMessage { get; set; }

        public long ReceivedContentLength { get; set; }

        public ResourceType ResourceType { get; set; }

        public bool IsSuccessStatusCode => StatusCode >= 200 && StatusCode < 300;   

        public override string ToString()
        {
            return $"Url: {Url}, Name: {Name}, MimeType: {MimeType}, StatusCode: {StatusCode}, StatusMessage: {StatusMessage}, ReceivedContentLength: {ReceivedContentLength}, ResourceType: {ResourceType}";
        }   
        public bool IsImage()
        {
            return ResourceType == ResourceType.Image || (MimeType != null && MimeType.StartsWith("image/", StringComparison.OrdinalIgnoreCase));
        }   

        public bool IsHit { get; set; } = false;

    }
}
