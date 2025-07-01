using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Comm.Finders
{
    public class TextFileFinder
    {
        /// <summary>
        /// 文件中间缓存，通常是过滤了扩展名之后的结果
        /// </summary>
        public List<FileInfo> Files { get; set; }

        /// <summary>
        /// 前导字符的长度-最大值 
        /// </summary>
        public int PrexTextSizeMax = 7;
        /// <summary>
        /// 后续字符的长度-最大值 
        /// </summary>
        public int SuffTextSizeMax = 7;

        /// <summary>
        /// 查找的原点文件夹路径
        /// </summary>
        public string OriginDirPath { get; set; }
        /// <summary>
        /// 需要跳过的子文件夹
        /// </summary>
        public List<string> SubDirSkipList { get; set; }


        /// <summary>
        /// 需要查找到的文本模式
        /// </summary>
        public string FindPattern { get; set; }
        /// <summary>
        /// 准备替换的文本模式
        /// </summary>
        public string RepalcePattern { get; set; }

        /// <summary>
        /// 扩展名列表 
        /// </summary>

        public List<string> ExtnameList { get; set; }


        public  bool HasPause { get; set; }
        public bool HasCannel { get; set; }



        /// <summary>
        /// （是）需要处理子目录
        /// </summary>
        public bool NeedSubDir { get; set; }

        public List<TextFileFoundResult> FileResult { get; set; }


        private int _NotSetMax = -1;
        public int Max { get; set; } = Constant.NotSetMaxInt;
     

        public TextFileFinder()
        {
            _Init();

        }

        protected virtual void _Init()
        {
            NeedSubDir = true;
            FileResult = new List<TextFileFoundResult>();
            HasCannel = false;
            HasPause = false;
        }

        public void StartFind()
        {

            _InitFiles();

            if (String.IsNullOrEmpty(FindPattern))
            {
                return;
            }

            FindFiles();



        }


        protected virtual void FindFiles()
        {
            int Index = 0;

            foreach (var f in Files)
            {
                if (Index >=  Max)
                {
                    return;                    
                }
                if (HasCannel)
                {
                    return ;
                }

                _FindLineByIndex(f);
                _FindByRegex(f);

                Index++;
            }


        }

        /// <summary>
        /// 通过Text.IndexOf查找行
        /// </summary>
        protected virtual void _FindLineByIndex(FileInfo f)
        {

            int LineNumber = 1;
            using (StreamReader sr = f.OpenText())
            {

                TextFileFoundResult Result = new TextFileFoundResult()
                {
                    FindLines = new List<TextLineFoundResult>(),
                    FileName = f.Name,
                    PathName = f.Directory.FullName,
                    FullName = f.FullName
                };
                string nextLine;
                while ((nextLine = sr.ReadLine()) != null)//循环处理每一行
                {

                    //Console.WriteLine(nextLine);
                    LineNumber++;
                    int FirstCharIndex = nextLine.IndexOf(FindPattern);
                    if (0 <= FirstCharIndex)
                    {
                        int NextCharIndex = FirstCharIndex;
                        string TailText = nextLine; //一行之内余下的字符串
                        while (0 <= NextCharIndex)
                        {
                            TextLineFoundResult line = new TextLineFoundResult();
                            line.LineNumber = LineNumber;
                            line.CharNumber = NextCharIndex + 1;
                            line.LineText = TailText;
                            int NextStart = NextCharIndex + FindPattern.Length + 1;
                            if (NextStart >= TailText.Length)
                            {
                                //break;
                                NextStart = TailText.Length;
                            }
                            TailText = TailText.Substring(NextStart);

                            int PrexIndex = NextCharIndex - PrexTextSizeMax;
                            int PrexSize = PrexTextSizeMax;
                            int SuffSize = SuffTextSizeMax;


                            if (0 > PrexIndex)
                            {
                                PrexIndex = 0;
                                PrexSize = NextCharIndex;
                            }
                            if (SuffSize > TailText.Length)
                            {
                                SuffSize = TailText.Length;
                            }
                            line.FindText = FindPattern;
                            line.ReplaceText = RepalcePattern;

                            line.PrefText = nextLine.Substring(PrexIndex, PrexSize);
                            line.SuffText = TailText.Substring(0, SuffSize);


                            NextCharIndex = TailText.IndexOf(FindPattern);

                            Result.FindLines.Add(line);
                        }
                    }
                }

                if (Result.FindLines.Count > 0)
                {

                    Result.TotalFoundLine = Result.FindLines.Count;
                    Result.IsFind = true;
                
                    FileResult.Add(Result);
                }

            }



        }
        /// <summary>
        /// 通过正则表达式查找
        /// </summary>
        protected virtual void _FindByRegex(FileInfo f)
        {



        }


        protected virtual void _InitFiles()
        {
            Files = new List<FileInfo>();
            if (!System.IO.Directory.Exists(OriginDirPath))
            {
                return;
            }
            DirectoryInfo d = new DirectoryInfo(OriginDirPath);
            Files = GetFiles(d);
            
        }
        /// <summary>获得目录里面所有的文件,开启NeedSubDir可以向下递归子目录</summary>
        /// <param name="d">目录对象</param>
        /// <returns></returns>
        protected List<FileInfo> GetFiles(DirectoryInfo d)
        {
            List<FileInfo> FileList = new List<FileInfo>();
            if (HasCannel)
            {
                return FileList;
            }
            if (null != SubDirSkipList)
            {
                if (SubDirSkipList.Contains(d.FullName))
                {
                    return FileList;
                }
            }
            FileInfo[] fis = null;

            try
            {
                fis = d.GetFiles();
            }
            catch (Exception e)
            {
                return FileList;
            }
            if (null == ExtnameList || 0 == ExtnameList.Count)
            {
                FileList.AddRange(fis.ToList());
            }
            else
            {
                foreach (var f in fis)
                {
                    string name = f.FullName;
                    string ext = System.IO.Path.GetExtension(name);
                    if (ExtnameList.Contains(ext))
                    {
                        FileList.Add(f);
                    }
                }
            }
            if (NeedSubDir)
            {
                foreach (DirectoryInfo subdir in d.GetDirectories())
                {
                    FileList.AddRange(GetFiles(subdir));
                }
            }
            return FileList;        
        }


    }
}
