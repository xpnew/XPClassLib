using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Comm.Task;

namespace XP.Web.JumonySDK.LinkSearch
{

    /// <summary>
    /// 定义一个Link搜索队列
    /// </summary>
    public class LinkQueue: BaseTaskQueue
    {

        /// <summary>
        /// 需要搜索的关键字
        /// </summary>
        public string SearchKey { get; set; }


        //public List<string> Keys { get; set; }

        /// <summary>
        /// 准备搜索的页面地址、模板
        /// </summary>
        public string PageUrlPattern { get; set; }

        /// <summary>
        /// dom选择器（类似于jQuery）
        /// </summary>
        public string DomSelector { get; set; }


        public int EndPageIndex { get; set; }

        public int StartPageIndex { get; set; } = 1;

        public void Init()
        {
           for(int i  = StartPageIndex; i<= EndPageIndex; i++)
            {
                var task = new LinkTask() {
                    PageUrl = String.Format(PageUrlPattern, i),
                    DomSelector = DomSelector,
                    SearchKey = SearchKey,
                    Queue = this,

                };
                task.TaskName = task.PageUrl;
                this.Add(task);

            }
        }

        public void AddReuslt(LinkResultItem item)
        {
            Result.Add(item);
        }

        public List<LinkResultItem> Result { get; set; } = new List<LinkResultItem>();

        public List<LinkResultItem> GetResult()
        {
            return Result;

        }

        //public async Task PlayAsync()
        //{

        //}
    }
}
