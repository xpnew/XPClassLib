/****************************************************************
 * 
 * 文件名：ExcelProvider.cs
 * 功能：Excel文件处理提供者
 * 说明：兼容Excel97格式和Excel2007格式
 * 创建人：于海峰（xpnew@126.com)
 * 创建日期：2016-07-16
 * 
 * 修改人：于海峰&____
 * 修改日期：
 * 
 * 
 * 
 * 
 * 
 * 
 * *******************************************************/



using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XP.IO.ExcelUtil
{
    /// <summary>
    /// Excel文件处理提供者，目标是兼容excel格式
    /// </summary>
    public class ExcelProvider
    {
        public string Path { get; set; }
        public string FullPath { get; set; }
        public IWorkbook Workbook { get; set; }

        public ExcelResultInfo ResultInfo { get; set; }

        public ExcelProvider(string path)
        {

            Path = path;

            Init();
        }
        protected void Init()
        {
            //ColumnDict = new Dictionary<int, string>();
            ResultInfo = new ExcelResultInfo();
            CreateWorkbook();

        }

        private void CreateWorkbook()
        {
            try
            {

                if (!System.IO.File.Exists(Path))
                {
                    Error(ExcelInfoTypes.FilePathError, "文件不存在。");
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
                        Error(ExcelInfoTypes.FileExtNameError, "非法的扩展名。");
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


        private void Error(ExcelInfoTypes type, string errorMsg)
        {
            Workbook = new HSSFWorkbook();
            ResultInfo.Error(type, errorMsg);
        }

    }
}
