using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Browser.Cef109.Filters
{
    /// <summary>
    /// PicFilter 的摘要说明
    /// </summary>
    /// <remarks>
    /// 作    者：Administrator
    /// 创建时间：2026/9/14 14:05:50	
    /// 
    /// </remarks>
    public class PicFilter: BaseFilter
    {



        public long MinSize { get; set; } = 0;  


        public long MaxSize { get; set; } = long.MaxValue;


        override protected void OnFilterResultItemUpdated(FilterResultItem item)
        {
            base.OnFilterResultItemUpdated(item);
            if (item.IsImage() && item.ReceivedContentLength >= MinSize && item.ReceivedContentLength <= MaxSize)
            {
                item.IsHit = true;
            }
            else
            {
                item.IsHit = false;
            }
        }

        public List<FilterResultItem> ReFilterPic(int size)
        {
            return ResultItems.Where(o => o.IsImage() && o.ReceivedContentLength >= size).ToList();
        }


    }
}
