using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Comm.Task
{
    public class BaseTaskQueue : ITaskQueue
    {

        /// <summary>
        /// 已经开始 
        /// </summary>
        public bool HasStart { get; set; }
        /// <summary>
        /// 正在工作
        /// </summary>
        public bool HasWorking { get; set; }
        /// <summary>
        /// 任务可以进行，初始化成功、初始条件具备 
        /// </summary>
        public bool IsCanDo { get; set; } = true;
        /// <summary>
        /// 成功完成（避免Finish 和complate混淆）
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 任务结束，不管成功与否
        /// </summary>
        public bool IsComplate { get; set; }

        /// <summary>
        /// 是否允许并行任务
        /// </summary>
        public bool EnableParallel { get; set; }
        public readonly object ParallelLock = new object();
        /// <summary>
        /// 最大并行数量
        /// </summary>
        public int MaxDegreeOfParallelism { get; set; } = 20;

        public List<ITaskItem> TaskList { get; set; }

        public int Index { get; set; }

        protected ITaskItem Current
        {
            get
            {
                if (null != TaskList && 0 <= Index && Index < TaskList.Count)
                {
                    return TaskList[Index];
                }
                return null;
            }
        }
        public BaseTaskQueue()
        {
            TaskList = new List<ITaskItem>();
            // Current = null;
            Index = -1;
            EnableParallel = false;
        }


        #region 任务流程和控制

        public void Next()
        {
            if (null == TaskList || 0 == TaskList.Count)
            {
                return;
            }
            if (Index >= TaskList.Count)
            {
                if (EnableParallel)
                {

                }
                else
                {
                    Clear();
                }
                IsSuccess = true;
                IsComplate = true;
                return;
            }
            Index++;
            //Current = TaskList[Index];
        }

        public void Pause()
        {
            if (0 <= Index)
            {
                HasWorking = false;
                Current.Pause();
            }

        }

        public void Cancel()
        {
            HasWorking = false;
            throw new NotImplementedException();
        }

        public void Clear()
        {
            TaskList = new List<ITaskItem>();
            Index = -1;
        }


        public void Play()
        {
            HasStart = true;
            HasWorking = true;
            if (!IsCanDo)
            {
                IsComplate = true;
                IsSuccess = false;
                return;
            }

            if (null != TaskList && 0 != TaskList.Count)
            {
                Next();
                while (null != Current)
                {
                    if (EnableParallel)
                    {

                    }
                    else
                    {
                        lock (ParallelLock)
                        {
                            Current.Start();
                        }
                    }
                    Next();
                }

            }
        }
        public async System.Threading.Tasks.Task PlayAsync()
        {
            HasStart = true;
            HasWorking = true;
            if (!IsCanDo)
            {
                IsComplate = true;
                IsSuccess = false;
                return;
            }

            if (null != TaskList && 0 != TaskList.Count)
            {

                if (EnableParallel)
                {
                    await ParalleRun();
                }
                else
                {

                    Next();
                    while (null != Current)
                    {
                        await Current.StartAsync();
                        Next();
                    }
                }
            }
        }


        public async System.Threading.Tasks.Task ParalleRun()
        {
            ParallelOptions options = new ParallelOptions();
            //指定使用的硬件线程数为10
            options.MaxDegreeOfParallelism = MaxDegreeOfParallelism;


            try
            {
                ParallelLoopResult result = Parallel.ForEach<ITaskItem>(TaskList, options, (task, state, i) =>
               {
                   //PicDownTaskItem down = task as PicDownTaskItem;
                   //Console.WriteLine("迭代次数：{0},{1}", i, down.PhyFile);
                   x.Say($"并行 迭代次数：{i}，任务名称 :{task.TaskName}");
                   var t = task.StartAsync();
                   t.Wait();
                   Console.WriteLine($"并行 完成， 迭代次数：{i}，任务名称 :{task.TaskName}");
                   //if (i > 35)
                   //    state.Break();
               });

                //System.Threading.Tasks.Task.WaitAll(result);
                x.Say("并行 任务结束,并行数量：" + MaxDegreeOfParallelism + ", 任务完成结果： " + result.IsCompleted);
            }
            // No exception is expected in this example, but if one is still thrown from a task,
            // it will be wrapped in AggregateException and propagated to the main thread.
            catch (AggregateException e)
            {
                x.Say("Parallel.ForEach has thrown an exception. THIS WAS NOT EXPECTED.\n" + e);
            }





        }
        public async System.Threading.Tasks.Task ListParalle()
        {
            //ParallelOptions options = new ParallelOptions();
            ////指定使用的硬件线程数为10
            //options.MaxDegreeOfParallelism = MaxDegreeOfParallelism;
            //ParallelLoopResult result = Parallel.ForEach<ITaskItem>(TaskList, options, async (task, state, i) =>
            //{
            //    //PicDownTaskItem down = task as PicDownTaskItem;
            //    //Console.WriteLine("迭代次数：{0},{1}", i, down.PhyFile);
            //    x.Say($"并行 迭代次数：{i}，任务名称 :{task.TaskName}");
            //    await task.StartAsync();
            //    //if (i > 35)
            //    //    state.Break();
            //});
            int Index = 1;
            var pq = TaskList.AsParallel().WithDegreeOfParallelism(MaxDegreeOfParallelism);


            pq.ForAll((item) =>
            {

                System.Threading.Interlocked.Increment(ref Index);
                //x.Say($"并行 迭代次数：{Index}，任务名称 :{item.TaskName}");
                var t = item.StartAsync();
                t.Wait();

            });




            x.Say("并行 任务结束,并行数量：" + MaxDegreeOfParallelism);




        }

        #endregion
        public void Add(ITaskItem Item)
        {
            TaskList.Add(Item);
        }

    }
}
