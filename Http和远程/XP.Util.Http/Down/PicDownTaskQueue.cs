using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XP.Comm.Task;

namespace XP.Util.Http.Down
{
    public class PicDownTaskQueue : XP.Comm.Http.Tasks.HttpTaskQueue
    {
        public string Name { get; set; }

        public string PhyRoot { get; set; }

        public string RealDirName { get; set; }

        public Size? MiniSize { get; set; }

        public string SourceUrl { get; set; }

        public PicDownTaskQueue() : base() { }


        public string FullDirName { get; set; }

        public PicDownTaskQueue(string name, string root, List<string> urls)
            : this()
        {
            Name = name;
            PhyRoot = root;
            CheckRoot();
            RealDirName = GetTaskDownDir();
            if (IsCanDo)
            {
                foreach (string url in urls)
                {
                    AddUrl(url);
                }
            }
        }
        public PicDownTaskQueue(string name, string root,Size mini, List<string> urls)
            : this(name,root,urls)
        {
            MiniSize = mini;

        }



        public void ParalleRun()
        {
            ParallelOptions options = new ParallelOptions();
            //指定使用的硬件线程数为10
            options.MaxDegreeOfParallelism = 20;
            ParallelLoopResult result = Parallel.ForEach<ITaskItem>(TaskList, options, (task, state, i) =>
            {
                PicDownTaskItem down = task as PicDownTaskItem;
                Console.WriteLine("迭代次数：{0},{1}", i, down.PhyFile);
                task.Start();
                //if (i > 35)
                //    state.Break();
            });

        }


        /// <summary>
        /// 获取任务实际的目录名
        /// </summary>
        /// <returns></returns>
        protected string GetTaskDownDir()
        {
            if (!IsCanDo)
            {
                return String.Empty;
            }
            //目录名基础，重复的时候上名字后面加括号和数字
            string BaseDirName = String.Empty;
            if (!String.IsNullOrEmpty(Name))
            {
                BaseDirName = Name;
            }
            else
            {
                BaseDirName = "TaskTemp_" + DateTime.Now.ToString("yyyyMMdd");
            }
            int CounterNum = 0;
            //最终结果目录名
            string FinalyName = BaseDirName;
            //完成的目录地址
            FullDirName = PhyRoot + FinalyName;

            while (System.IO.Directory.Exists(FullDirName))
            {
                CounterNum++;
                FinalyName = BaseDirName + "(" + CounterNum + ")";
                FullDirName = PhyRoot + FinalyName;
            }
            System.IO.Directory.CreateDirectory(FullDirName);
            //if (System.IO.Directory.Exists(FullDirName))
            //{
            //    while (true)
            //    {
            //        if (System.IO.File.Exists(FullDirName))
            //        {
            //            CounterNum++;
            //            FinalyName = BaseDirName + "(" + CounterNum + ")";
            //            FullDirName = PhyRoot + FinalyName;

            //        }
            //        else
            //        {
            //            System.IO.Directory.CreateDirectory(FullDirName);
            //            break;
            //        }
            //    }

            //}
            //else
            //{
            //    System.IO.Directory.CreateDirectory(FullDirName);
            //}

            return FinalyName;
        }

        protected void CheckRoot()
        {
            if (System.IO.Directory.Exists(PhyRoot))
            {
                if (!PhyRoot.EndsWith("\\"))
                {
                    PhyRoot += "\\";
                }
                IsCanDo = true;
                return;

            }
            else
            {
                IsCanDo = false;
            }
     

        }

        public void AddUrl(string url)
        {
            PicDownTaskItem NewItem = new PicDownTaskItem(url, PhyRoot + RealDirName + "\\" + BuildFileName(url));
            NewItem.MiniSize = this.MiniSize;
            Add(NewItem);
        }

        private string BuildFileName(string url)
        {
            Uri uri = new Uri(url);
            string path = uri.LocalPath;

            if (String.IsNullOrEmpty(path))
            {
                return BuildRodamFileName();
            }
            if (0 > path.IndexOf("/"))
            {
                return BuildRodamFileName();
            }

            if (path.LastIndexOf("/") == path.Length - 1)
            {
                return BuildRodamFileName();
            }
            string Filename = path.Substring(path.LastIndexOf("/") + 1);
            return Filename;
        }


        private string BuildRodamFileName()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jpg";
        }


    }
}
