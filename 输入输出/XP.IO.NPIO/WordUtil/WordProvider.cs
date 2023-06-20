using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.IO.WordUtil
{
    /// <summary>
    /// Word文件处理提供者，目标是兼容 Word格式
    /// </summary>
    public class WordProvider
    {


        public string Path { get; set; }
        public string FullPath { get; set; }
        public IWorkbook Workbook { get; set; }

        public WordResultInfo ResultInfo { get; set; }

        public WordProvider(string path)
        {

            Path = path;

            Init();
        }
        protected void Init()
        {
            //ColumnDict = new Dictionary<int, string>();
            ResultInfo = new WordResultInfo();
            CreateWorkbook();

        }

        private void CreateWorkbook()
        {
            try
            {

                if (!System.IO.File.Exists(Path))
                {
                    Error(WordErrorDef.FilePathError, "文件不存在。");
                    return;
                }

                using (var fs = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    if (Path.Contains(".xlsx")) // 2007版本
                        Workbook = new XSSFWorkbook(fs);
                    else if (Path.Contains(".xls")) // 2003版本
                        Workbook = new HSSFWorkbook(fs);
                    else
                    {
                        // throw new Exception("excel 文件扩展名错误");
                        Error(WordErrorDef.FileExtNameError, "非法的扩展名。");
                    }
                }

            }
            catch (Exception ex)
            {
                if (0 <= ex.Message.IndexOf("未能加载文件或程序集"))
                {
                    if (0 <= ex.Message.IndexOf("ICSharpCode.SharpZipLib"))
                    {
                        Error(ExcelInfoTypes.ComponentErrReferenceSharpZip, ex.Message);
                        return;
                    }
                    Error(ExcelInfoTypes.ComponentErrReference, ex.Message);
                    return;
                }

                Error(ExcelInfoTypes.FormatCannotRead, "文件格式不对。");
            }
        }


        private void Error(WordErrorDef type, string errorMsg)
        {
            Workbook = new HSSFWorkbook();
            ResultInfo.Error(type, errorMsg);
        }
    }
}
