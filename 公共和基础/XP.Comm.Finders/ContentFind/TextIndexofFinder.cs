using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.Comm.Finders.ContentFind
{

    /// <summary>
    /// 使用IndexOf查找文件内容
    /// </summary>
    public class TextIndexofFinder
    {

        /// <summary>
        /// 需要查找到的文本模式
        /// </summary>
        public string FindPattern { get; set; }
        /// <summary>
        /// 准备替换的文本模式
        /// </summary>
        public string RepalcePattern { get; set; }

        public FileInfo File { get; set; }

        public string FullFileName { get; set; }
        /// <summary>
        /// 前导字符的长度-最大值 
        /// </summary>
        public int PrexTextSizeMax = 7;
        /// <summary>
        /// 后续字符的长度-最大值 
        /// </summary>
        public int SuffTextSizeMax = 7;


        public TextFileFoundResult Result { get; set; }

        public TextIndexofFinder()
        {
            _Init();
        }

        public TextIndexofFinder(string path) : this()
        {
            if (!System.IO.File.Exists(path))
            {
                return;
            }
            this.FullFileName = path;
            this.File = new FileInfo(path);
            _InitFile(File);
        }

        public TextIndexofFinder(FileInfo f) : this()
        {
            this.File = f;
            this.FullFileName = f.FullName;
            _InitFile(f);
        }


        protected virtual void _Init()
        {
            if (null == Result)
                Result = new TextFileFoundResult() {IsFind = false};

        }

        protected void _InitFile(FileInfo f)
        {
            Result = new TextFileFoundResult()
            {
                FindLines = new List<TextLineFoundResult>(),
                FileName = f.Name,
                PathName = f.Directory.FullName,
                FullName = f.FullName,
                IsFind =false,
                TotalFoundLine = 0,
            };
        }
        public void StartFind()
        {
            if (null == File)
            {
                return;
            }

            var f = File;

            int LineNumber = 1;
            using (StreamReader sr = f.OpenText())
            {
                string nextLine;
                while ((nextLine = sr.ReadLine()) != null)//循环处理每一行
                {
                    //Console.WriteLine(nextLine);
                    LineNumber++;
                    if (0 == nextLine.Length)
                    {
                        if (0 == FindPattern.Length)
                        {
                            //查找穿行，暂时没用。。。
                            TextLineFoundResult line = new TextLineFoundResult();
                            line.LineNumber = LineNumber;
                            line.CharNumber = 0;
                            line.LineText = nextLine;
                        }
                        continue;
                    }
                    int FirstCharIndex = nextLine.IndexOf(FindPattern);
                    if (0 <= FirstCharIndex)
                    {
                        int NextCharIndex = FirstCharIndex;
                        string TailText = nextLine;
                        while (0 <= NextCharIndex)
                        {
                            TextLineFoundResult line = new TextLineFoundResult();
                            line.LineNumber = LineNumber;
                            line.CharNumber = NextCharIndex + 1;
                            line.LineText = nextLine;
                            TailText = TailText.Substring(NextCharIndex + FindPattern.Length + 1);

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
                }
            }
        }

    }
}
