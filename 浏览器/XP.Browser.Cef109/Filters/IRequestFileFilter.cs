using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Browser.Cef109.Filters
{
    public interface IRequestFileFilter
    {



         void Push(string url, ResourceType resourceType);


         void CheckAndSave(string url,int statuscode, long receivedContentLength, string mimeType);

    }
}
