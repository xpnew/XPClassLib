using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XP.Comm.Task;
using XP.Util.ClassTemplate;
using XP.Util.Configs;

namespace XP.Util.Http.Down
{
    /// <summary>
    /// 图片并行下载任务队列
    /// </summary>
    /// <remarks>
    /// 作    者：Administrator
    /// 创建时间：2026/8/22 7:50:17	
    /// 
    /// </remarks>
    public class PiceDownTaskParallelQueue 

    {
        #region 快速单例模式

        public static readonly PiceDownTaskParallelQueue _Instance = new PiceDownTaskParallelQueue();

        public static PiceDownTaskParallelQueue Self
        {
            get { return _Instance; }
        }
        #endregion
        #region  基础参数和属性
        // 1. 定义信号量，最大并发数为 10 (根据你的需求调整这个数字)
        // 建议设为 static 或单例，确保在整个应用生命周期内共享同一个信号量
        private SemaphoreSlim _semaphore ;

        public long ItemIndex { get; set; } = 0;



        //public List<Timer4IdItem> Items { get; set; } = new List<Timer4IdItem>();

        public readonly static object _Locker = new object();

        #endregion

        #region  构造函数和初始化



        protected PiceDownTaskParallelQueue()
        {
            _Init();
        }


        protected void _Init()
        {

            int ParallelMax = 5;

            ParallelMax =  ConfigReader.Self.GetInt("PicDownParallelMax", ParallelMax);
            _semaphore =  new SemaphoreSlim(ParallelMax, ParallelMax);
            //_InnerNextTimer.Start();
        }


        #endregion
        #region  内部处理


        #endregion
        #region  外部控制



        public void ResetSlim(int maxSize)
        {

            if (maxSize > 0)
            {


                _semaphore.Dispose();
                _semaphore = new SemaphoreSlim(maxSize, maxSize);
            }
        }
        #endregion


        /// <summary>
        /// 通过信号量控制的并行任务
        /// </summary>
        /// <returns></returns>

        public async Task ParallelDown(List<ITaskItem> TaskList)
        {

            var tasks = new List<Task>();

            foreach (var item in TaskList)
            {
                // 将每个下载任务包装在一个异步方法中
                //tasks.Add(DownloadWithLimitAsync(item));

                // 核心优化：使用Task.Run主动将任务调度到线程池执行，避免所有任务挤在初始线程中
                // 新增局部变量拷贝，每个循环都会生成独立的 currentItem 副本
                var currentItem = item;
                tasks.Add(Task.Run(async () => await DownloadWithLimitAsync(currentItem)));
            }

            // 2. 等待所有任务完成
            // Task.WhenAll 会阻塞当前异步流程，直到列表中的所有 Task 都结束
            await Task.WhenAll(tasks);

            Console.WriteLine("所有下载任务已执行完毕，继续后续逻辑...");
            x.TimerLog("所有下载任务已执行完毕，继续后续逻辑...");

        }

        private async Task DownloadWithLimitAsync(ITaskItem item)
        {
            var down = item as PicDownTaskItem;
            if (down == null) return;

            // 3. 请求信号量
            // 如果当前已有 10 个任务在运行，第 11 个任务会在这里异步等待，不会阻塞线程池线程
            await _semaphore.WaitAsync();

            try
            {
                Console.WriteLine($"开始下载: {down.PhyFile}, 线程ID: {Environment.CurrentManagedThreadId}");
                x.TimerLog($"开始下载: {down.PhyFile},开始下载: {down.WebUrl}, 线程ID: {Environment.CurrentManagedThreadId}");

                // 4. 执行实际的异步下载操作
                // 注意：这里必须使用真正的异步 API (如 HttpClient.GetAsync)，不要用 .Result 或 .Wait()
                await PerformActualDownloadAsync(down);

                Console.WriteLine($"完成下载: {down.PhyFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"下载失败: {down.PhyFile}, 错误: {ex.Message}");
                x.TimerLog($"下载失败: {down.PhyFile}, 错误: {ex.Message}");
                XP.Loger.Error($"下载失败: {down.PhyFile}, 错误: {ex.Message}", ex);
            }
            finally
            {
                // 5. 【至关重要】释放信号量
                // 无论成功还是失败，都必须释放，否则其他等待的任务会永远卡住（死锁）
                _semaphore.Release();
            }
        }

        // 模拟真实的异步下载方法
        private async Task PerformActualDownloadAsync(PicDownTaskItem item)
        {
            // 模拟网络延迟
            await Task.Delay(20);
            item.Start();
            x.TimerLog($"模拟下载完成: {item.PhyFile}，线程ID: {Environment.CurrentManagedThreadId}");
            // 实际代码示例:
            // using var client = new HttpClient();
            // var bytes = await client.GetByteArrayAsync(item.Url);
            // File.WriteAllBytes(item.PhyFile, bytes);
        }

    }
}
