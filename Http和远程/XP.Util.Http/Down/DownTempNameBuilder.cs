using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XP.Util.ClassTemplate;

namespace XP.Util.Http.Down
{
    /// <summary>
    /// 下载时 临时文件 文件名生成器 DownTempNameBuilder 的摘要说明
    /// </summary>
    /// <remarks>
    /// 作    者：Administrator
    /// 创建时间：2026/8/22 14:44:13	
    /// 
    /// </remarks>
    public class DownTempNameBuilder
    {

        #region 快速单例模式

        public static readonly DownTempNameBuilder _Instance = new DownTempNameBuilder();

        public static DownTempNameBuilder Self
        {
            get { return _Instance; }
        }
        #endregion
        #region  基础参数和属性


        public long ItemIndex { get; set; } = 0;


        //public List<Timer4IdItem> Items { get; set; } = new List<Timer4IdItem>();

        public readonly static object _Locker = new object();

        #endregion

        #region  构造函数和初始化
        protected DownTempNameBuilder()
        {
            _Init();
        }


        protected void _Init()
        {
            //_InnerNextTimer.Start();
        }



        public List<string> LstName4MS { get; set; } = new List<string>();

        #endregion
        #region  内部处理


        #endregion
        #region  外部控制



        public string GetName(string phyFile, string ext, int mode = 0)
        {
           
           if(1 == mode)
            {
                return GetGuidName(phyFile, ext);
            }


            return GetMsName(phyFile, ext);


        }


        public string GetMsName(string phyDir,string ext)
        {
            string dir =  phyDir;
            if(! dir.EndsWith("\\"))
            {
                dir += "\\";
            }
            if(!ext.StartsWith("."))
            {
                ext = "." + ext;
            }
            string tempName;
            lock (_Locker)
            {
                string rnd = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                tempName = dir + rnd + ext;

                while (LstName4MS.Contains(tempName) || System.IO.File.Exists(tempName))
                {
                    int SleepRnd = GeneralTool.Random(3, 10);
                    Thread.Sleep(SleepRnd);
                    rnd = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                    tempName = dir + rnd + ext;
                }


                LstName4MS.Add(tempName);

            }

            return tempName;
        }
        public string GetGuidName(string phyDir,string ext)
        {
            string dir =  phyDir;
            if(! dir.EndsWith("\\"))
            {
                dir += "\\";
            }
            if(!ext.StartsWith("."))
            {
                ext = "." + ext;
            }
            string tempName;
            lock (_Locker)
            {
                string rnd = Guid.NewGuid().ToString("N");
                tempName = dir + rnd + ext;

                while (LstName4MS.Contains(tempName) || System.IO.File.Exists(tempName))
                {
                    int SleepRnd = GeneralTool.Random(3, 10);
                    Thread.Sleep(SleepRnd);
                    rnd = Guid.NewGuid().ToString("N");
                    tempName = dir + rnd + ext;
                }


                LstName4MS.Add(tempName);

            }

            return tempName;
        }



        #endregion

    }
}
