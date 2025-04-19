using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Util.Threadings
{
    public class AsyncTask
    {
        /// <summary>
        /// 开启后台异步
        /// </summary>
        /// <param name="a"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static Task BuildBGAsync(Action a, TaskCreationOptions option = TaskCreationOptions.HideScheduler)
        {
            return Task.Factory.StartNew(a, option);
        }
        /// <summary>
        /// 开启后台异步
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="function"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static Task<TResult> BuildBGAsync<TResult>(Func<TResult> function, TaskCreationOptions option = TaskCreationOptions.HideScheduler)
        {
            return Task.Factory.StartNew(function, option);
        }


        /// <summary>
        /// 同步方法当中调取异步方法，并且等待异步的返回结果
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="function"></param>
        /// <returns></returns>
        public static TResult GetAsyncResult<TResult>(Task<TResult> function)
        {
            return Task.Factory.StartNew(() => function).Unwrap().GetAwaiter().GetResult();
        }

        public static void WaitAsync(Task task)
        {
            Task.WaitAll(task);
        }
        //public static void WaitAsync(this Task task)
        //{
        //    if (task == null)
        //    {
        //        return;
        //    }
        //    Task.Factory.StartNew(() => task).Unwrap().GetAwaiter().GetResult();
        //}
        //public static void WaitAsync<T>(this Task<T> task)
        //{
        //    if (task == null)
        //    {
        //        return;
        //    }
        //    Task.Factory.StartNew(() => task).Unwrap().GetAwaiter().GetResult();
        //}
    }
}
