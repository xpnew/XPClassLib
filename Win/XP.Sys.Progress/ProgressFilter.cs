using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Sys.Progress
{
    /// <summary>
    /// 线程过滤器
    /// </summary>
    /// <remarks>
    /// 
    /// 参考：
    /// 
    /// https://docs.microsoft.com/zh-cn/dotnet/api/system.diagnostics.process?view=netframework-4.7.2
    /// 
    /// https://docs.microsoft.com/zh-cn/dotnet/api/system.diagnostics.processmodule?view=netframework-4.7.2
    /// 
    /// </remarks>
    public class ProgressFilter
    {



        /// <summary>
        /// 结果线程
        /// </summary>
        public Process Result { get; set; }



        public Action<string, string> SayLogEvent;


        public List<Process> ResultProcessList { get; set; }



        /// <summary>
        /// 是否发现。
        /// </summary>
        public bool IsFind { get; set; } = false;

        /// <summary>
        /// 全部名称，而不是部分
        /// </summary>
        public bool IsFullNotPart = false;


        /// <summary>
        /// 窗口标题
        /// </summary>
        public string WinTitle { get; set; }


        /// <summary>
        /// 主线程名字
        /// </summary>
        public string MainProcessName { get; set; }

        /// <summary>
        /// 子线程名称
        /// </summary>
        public string SubProcessName { get; set; }




        public async Task FindAsync()
        {
            var ts = Task.Run(() =>
            {

                SayLogEvent("准备扫描线程", "找到的结果");
                var psList = GetProcesses();
                ResultProcessList = psList;
                SayLogEvent("线程扫描结束", "找到的结果" + psList.Count);

                if (psList.Count > 0)
                {
                    IsFind = true;
                    Result = psList.First();
                }

                foreach (var p in psList)
                {
                    //if (FindMainProcess(p))
                    //{
                    //    Result = p;
                    //    IsFind = true;
                    //}
                }

            });

            await ts;


        }





        protected bool FindMainProcess(Process pc)
        {
            var ModelArr = pc.Modules;
            if (null == ModelArr || 0 == ModelArr.Count)
            {
                return false;
            }


            string FilderString = "OpenJDK";

            foreach (ProcessModule mdel in ModelArr)
            {
                string Name = mdel.ModuleName;

                var FileName = mdel.FileName;
                if (0 <= Name.IndexOf(SubProcessName) || 0 <= FileName.IndexOf(SubProcessName))
                {
                    Loger.LogInfo("子线程名称 过滤发现模块！模块名称： " + Name + " ， 文件名： " + FileName);
                    return true;
                }

                if (0 <= Name.IndexOf(FilderString) || 0 <= FileName.IndexOf(FilderString))
                {
                    Loger.LogInfo("模糊过滤发现模块！模块名称： " + Name + " ， 文件名： " + FileName);
                    return true;
                }


            }
            return false;
        }

        protected List<Process> GetProcesses()
        {
            List<Process> Result = new List<Process>();

            // Get all instances of Notepad running on the local computer.
            // This will return an empty array if notepad isn't running.
            //Process[] localByName = Process.GetProcessesByName(MainProcessName);


            //if (null != localByName && 0 < localByName.Length)
            //{
            //    return localByName.ToList();

            //}
            Process[] all = Process.GetProcesses();


            //string AllProcessLogs = JsonHelper.Serialize(all);

            //Loger.Info("全部线程：" + AllProcessLogs);

            //string FilterString = "WerFault";

            //List<string> FilterList = new List<string>() { "WerFault", "Windows 问题报告", "OpenJDK Platform" };

            //string FilderString = "foobar2000";
            if (null != all && 0 < all.Length)
            {

                foreach (var p in all)
                {

                    //研发诊断用的
                    //LogProcess(p);


                    string name = p.ProcessName;

                    var tit = p.MainWindowTitle;
                    //foreach(var filter in FilterList)




                    if (CheckName(name, MainProcessName)   || (!String.IsNullOrEmpty(WinTitle) && CheckName(tit, WinTitle) ))
                    {
                        Loger.Info("发现进程！进程名称： " + name + " ， 进程窗口名称： " + tit);
                        Result.Add(p);
                    }
                }



            }
            else
            {

            }


            return Result;


        }
        

        protected bool CheckName(string self, string filter)
        {
            if (IsFullNotPart)
            {
               return self == filter;
            }
            else
            {
                return self.IndexOf(filter) >= 0;
            }
        }


        protected void LogProcess(Process p)
        {
            string Log = String.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append("\r\n");
            sb.Append("进程名称：");
            sb.Append(p.ProcessName);
            sb.Append(" 进程标题：");
            sb.Append(p.MainWindowTitle);
            sb.Append(" 进程Id ：");
            sb.Append(p.Id);

            try
            {
                sb.Append("\r\n");
                sb.Append("MainModule >>>>");
                sb.Append("\r\n");
                sb.Append("ModuleName： ");
                sb.Append(p.MainModule.ModuleName);

                sb.Append(" FileName ：");
                sb.Append(p.MainModule.FileName);

            }
            catch
            {

            }


            Log = sb.ToString();

            Loger.OriginalInfo(Log);
        }







        protected void SayProcess(Process p)
        {
            string name = p.ProcessName;

            var tit = p.MainWindowTitle;
            Loger.LogInfo("发现进程！进程名称： " + name + " ， 进程窗口名称： " + tit);

            var ModelArr = p.Modules;
            foreach (ProcessModule mdel in ModelArr)
            {
                string ModuleName = mdel.ModuleName;

                var FileName = mdel.FileName;
                if (0 <= ModuleName.IndexOf(SubProcessName) || 0 <= FileName.IndexOf(SubProcessName))
                {
                    Loger.LogInfo("子线程名称 过滤发现模块！模块名称： " + ModuleName + " ， 文件名： " + FileName);

                }

            }
        }






    }
}
