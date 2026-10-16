using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Browser.Cef109.Filters
{
    /// <summary>
    /// BaseFilter 的摘要说明
    /// </summary>
    /// <remarks>
    /// 作    者：Administrator
    /// 创建时间：2026/9/14 14:05:34	
    /// 
    /// </remarks>
    public class BaseFilter : IRequestFileFilter
    {

        public List<FilterResultItem> ResultItems { get; set; } = new List<FilterResultItem>();

        public void CheckAndSave(string url, int statuscode, long receivedContentLength, string mimeType)
        {
           var ExistingItem = ResultItems.FirstOrDefault(item => item.Url == url);
            if (ExistingItem != null)
            {
                ExistingItem.StatusCode = statuscode;
                ExistingItem.ReceivedContentLength = receivedContentLength;
                ExistingItem.MimeType = mimeType;
                OnFilterResultItemUpdated(ExistingItem);
            }


        }

        public void Push(string url, ResourceType resourceType)
        {
           
            var NewItem = new FilterResultItem
            {
                Url = url,
                ResourceType = resourceType
            };

            ResultItems.Add(NewItem);
            OnFilterResultItemAdded(NewItem);
        }


        protected virtual void OnFilterResultItemAdded(FilterResultItem item)
        {
            // 可以在这里触发事件或执行其他操作
        }

        protected virtual void OnFilterResultItemUpdated(FilterResultItem item)
        {
            // 可以在这里触发事件或执行其他操作
        }
    }
}
