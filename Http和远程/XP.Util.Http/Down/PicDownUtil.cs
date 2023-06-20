using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XP.Comm.Http.Tasks;

namespace XP.Util.Http.Down
{
    public class PicDownUtil
    {
        private static HttpTaskQueue _Queue;
        public static HttpTaskQueue Queue
        {
            get
            {
                if (null == _Queue)
                {
                    _Queue = new HttpTaskQueue();
                }
                return _Queue;
            }
            set
            {
                _Queue = value;
            }
        }


        public static void Down(string webUrl, string phyPath)
        {
            PicDownTaskItem NewItem = new PicDownTaskItem(webUrl, phyPath);

            _Queue.Add(NewItem);
        }


        public static bool DownTask(string taskName, string root, List<string> urls)
        {
            PicDownTaskQueue _Queue = new PicDownTaskQueue(taskName, root, urls);
            _Queue.Play();
            return false;

        }

        public static bool ParalleDownTask(string taskName, string root, List<string> urls)
        {
            PicDownTaskQueue _Queue = new PicDownTaskQueue(taskName, root, urls);
            _Queue.ParalleRun();
            return false;

        }



        public static void Start()
        {
            _Queue.Play();
        }
    }
}
