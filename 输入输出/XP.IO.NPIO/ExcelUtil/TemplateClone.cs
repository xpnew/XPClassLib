using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.IO.ExcelUtil
{
    public class TemplateClone : ExcelReadBase
    {

        public ISheet TmSheet { get; set; }

        /// <summary>
        /// 填充区的首行，0索引
        /// </summary>
        public int FillRangeStart { get; set; }

        /// <summary>
        /// 填充区的尾行，包含最后一行
        /// </summary>
        public int FillRangeEnd { get; set; }


        public bool HasTmReady { get; set; }


        /// <summary>
        /// 是否带页脚
        /// </summary>
        public bool HasPageFoot { get; set; }

        /// <summary>
        /// 最后一列包含备注（留空）
        /// </summary>
        public bool HasLastRemarkColumn { get; set; }



        public List<CaptionFormatLine> CaptionFormat { get; set; }

        private IDataFormat _CellFormating;
        public TemplateClone()
            : base()
        {


        }

        public TemplateClone(string path)
            : base(path)
        {
        }


        protected override void _Init()
        {
            base._Init();
            FillRangeStart = 1;
            FillRangeEnd = 1;
            CaptionFormat = new List<CaptionFormatLine>();
            HasTmReady = false;
            HasPageFoot = true;
            HasLastRemarkColumn = true;
        }
        /// <summary>
        /// 初始化模板
        /// </summary>
        protected void _InitTm()
        {
            if (HasTmReady)
            {
                return;
            }
            MsgLog("准备打开文件" + PhysicalFilePath);
            if (ResultInfo.Success)
            {
                if (null == WorkBook)
                {
                    TryWorkbook();
                    if (!ResultInfo.Success)
                    {
                        MsgErr("打开文件失败");

                        return;
                    }
                }
            }
            else
            {
                MsgErr("文件打开失败，跳出");
                return;
            }

            if (0 == WorkBook.NumberOfSheets)
            {
                MsgErr("没有工作表sheet，跳出");
                return;
            }
            HasTmReady = true;
        }
        //获取模板单元格
        public void GetTmSheet()
        {
            _InitTm();
            if (HasTmReady)
                TmSheet = WorkBook.GetSheetAt(0);

        }

        public void SetRange(int start, int end)
        {
            if (end < start)
            {
                MsgErr("指定填充区间出错，结束位置小于起始位置");
                return;
            }

            this.FillRangeStart = start;
            this.FillRangeEnd = end;

        }
        public void SetRange(string rangeSetString)
        {
            if (String.IsNullOrEmpty(rangeSetString))
            {
                MsgErr("指定填充区间出错，字符串是空的");
                return;
            }
            var arr = rangeSetString.Split(new char[] { ',', '|' });
            if (2 > arr.Length)
            {
                MsgErr("指定填充区间出错，字符串格式不对，不能解析成数组");
                return;
            }
            int start = int.Parse(arr[0]);
            int end = int.Parse(arr[1]);
            SetRange(start, end);
        }

        /// <summary>
        /// 填充数据，并且指表头的格式化
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="captionFormat"></param>
        public void FillData(DataTable dt, List<CaptionFormatLine> captionFormat)
        {
            this.CaptionFormat = captionFormat;

            FillData(dt);

            FormatCaption(TmSheet);
        }


        public void FormatCaption(ISheet sheet)
        {
            var MyRow = sheet.GetRow(0);
            var myCell = MyRow.GetCell(0);

            if (NPOI.SS.UserModel.CellType.String == myCell.CellType)
            {
                if (null != myCell.RichStringCellValue)
                {
                    var RichString = (XSSFRichTextString)myCell.RichStringCellValue;
                    var mms = RichString.GetCTRst();

                    if (0 < mms.r.Count)
                    {
                        for (int i = 0; i < CaptionFormat.Count && i < mms.r.Count; i++)
                        {
                            var Part = mms.r[i];
                            var Format = CaptionFormat[i];
                            if (null == Format.Items || 0 == Format.Items.Count)
                            {
                                continue;
                            }
                            foreach (var f in Format.Items)
                            {
                                Part.t = Part.t.Replace(f.Tm, f.Str);
                            }
                        }
                    }
                    else if (0 == mms.r.Count && !String.IsNullOrEmpty(RichString.String))
                    {
                        var LineArr = RichString.String.Split('\n');
                        string NewMergeString = "";
                        List<string> NewLineArr = new List<string>();
                        for (int i = 0; i < LineArr.Length; i++)
                        {
                            var line = LineArr[i];
                            var Format = CaptionFormat[i];
                            if (null == Format.Items || 0 == Format.Items.Count)
                            {
                                NewLineArr.Add(line);
                                continue;
                            }
                            string LineResult = line;
                            foreach (var f in Format.Items)
                            {
                                LineResult = LineResult.Replace(f.Tm, f.Str);
                            }
                            NewLineArr.Add(LineResult);
                        }
                        NewMergeString = String.Join("\n", NewLineArr);
                        myCell.SetCellValue(NewMergeString);
                    }

                }
            }
        }
        public void FillData(DataTable dt)
        {
            GetTmSheet();

            int NewCount = dt.Rows.Count;
            if (0 == NewCount)
            {
                MsgErr("0行数据，空格表格不处理。");
                return;
            }



            ResetSheet(NewCount);
            FillRange(dt, TmSheet, FillRangeStart);
        }

        public void FillData(DataTable dt, ISheet sheet)
        {
            GetTmSheet();

            int NewCount = dt.Rows.Count;
            if (0 == NewCount)
            {
                MsgErr("0行数据，空格表格不处理。");
                return;
            }



            ResetSheet(NewCount);
            FillRange(dt, TmSheet, FillRangeStart);
        }


        private void FillRange(DataTable dt, ISheet sheet, int startIndex)
        {
            int LineSize = sheet.GetRow(FillRangeStart - 1).LastCellNum - 1;
            if (HasLastRemarkColumn)
            {
                LineSize--;
            }
            int DataColSize = dt.Columns.Count;
            LineSize = LineSize > DataColSize ? LineSize : DataColSize;
            int DbLineSize = dt.Columns.Count;

            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                IRow ExcelRow = sheet.GetRow(i + FillRangeStart);
                var FirstCell = ExcelRow.GetCell(0);
                FirstCell.SetCellValue(i + 1);
                for (int j = 0; j < LineSize; j++)
                {
                    if (LineSize < j)
                        continue;

                    ICell ec = ExcelRow.GetCell(j + 1);



                    if (j >= DbLineSize)
                    {
                        ec.SetCellValue("");
                        continue;
                    }



                    if (DBNull.Value == row[j])
                        continue;
                    if (typeof(DateTime) == row[j].GetType())
                    {
                        ec.SetCellValue(((DateTime)row[j]).ToString("yyyy-MM-dd"));

                    }
                    else if (typeof(int) == row[j].GetType() || typeof(decimal) == row[j].GetType())
                    {
                        double value = Convert.ToDouble(row[j].ToString());
                        ec.SetCellValue(value);
                    }
                    else if (typeof(double) == row[j].GetType())
                    {
                        double value = (double)row[j];
                        ec.SetCellValue(value);
                    }
                    else
                    {
                        ec.SetCellValue(row[j].ToString());

                    }
                }

                i++;
            }

            sheet.ForceFormulaRecalculation = true;

        }


        public void ResetSheet(int dataLineCount)
        {
            int DefaultDataLineCount = FillRangeEnd - FillRangeStart + 1;
            int NewLineCount = dataLineCount - DefaultDataLineCount;

            if (0 >= NewLineCount)
            {
                MsgLog("数据不多，可以不延伸表格长度。");
                return;
            }

            int NewLineIndex = FillRangeEnd + 1;

            var TemplateLine = TmSheet.GetRow(FillRangeStart);

            InertNewLines(TmSheet, NewLineIndex, NewLineCount, TemplateLine);


        }




        public void InertNewLines(ISheet sheet, int lineIndex, int newLineCount, IRow tmRow)
        {
            //有页脚的情况，平移页脚 
            if (HasPageFoot && lineIndex < sheet.LastRowNum)
            {
                sheet.ShiftRows
 (
 lineIndex,                                 //--开始行
 sheet.LastRowNum,//sheet    .LastRowNum-1,                            //--结束行
 newLineCount,                             //--移动大小(行数)--往下移动
 true,                                   //是否复制行高
 true                                  //是否重置行高
 );



            }

            for (int i = lineIndex; i < lineIndex + newLineCount; i++)
            {
                IRow targetRow = null;
                ICell sourceCell = null;
                ICell targetCell = null;

                targetRow = sheet.CreateRow(i);

                for (int m = tmRow.FirstCellNum; m < tmRow.LastCellNum; m++)
                {
                    sourceCell = tmRow.GetCell(m);
                    if (sourceCell == null)
                        continue;
                    targetCell = targetRow.CreateCell(m);
                    targetCell.CellComment = targetCell.CellComment;
                    //targetCell.set = sourceCell.CellStyle.e;
                    targetCell.CellStyle = sourceCell.CellStyle;
                    targetCell.SetCellType(sourceCell.CellType);
                }
            }
        }


        /// <summary>
        /// 将结果写入的磁盘文件
        /// </summary>
        /// <param name="filePhysicalPath"></param>
        /// <returns></returns>
        public bool WriteFile(string filePhysicalPath)
        {
            if (null == WorkBook || 0 == WorkBook.NumberOfSheets)
            {
                MsgErr("生成的新文档为空，或者没有工作表");
                return false;
            }

            try
            {
                using (FileStream file = new FileStream(filePhysicalPath, FileMode.Create))
                {
                    WorkBook.Write(file);
                    file.Close();
                    file.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                MsgErr("写入文件[" + filePhysicalPath + "]失败，可能是没有权限，具体原因请见异常说明。" + ex.Message);
                return false;
            }

        }

        public MemoryStream WriteMemory()
        {

            MemoryStream filestream = new MemoryStream(); //内存文件流(应该可以写成普通的文件流)
            WorkBook.Write(filestream); //把文件读到内存流里面
            return filestream;

        }


        public short GetCellFormat(Type dbType)
        {

            //整数 常规
            if (dbType == typeof(int))
                return 0;
            //数值
            else if (dbType == typeof(decimal))
                return _CellFormating.GetFormat("¥#,##0");
            else if (dbType == typeof(DateTime))
                return _CellFormating.GetFormat("yyyy年m月d日");
            //字符串
            else
                return -1;

        }

        #region 单元测试和研究


        #region  克隆工作表



        public void CloneSheet()
        {
            var NewSheet = WorkBook.CloneSheet(0);

            ///WorkBook.a

        }

        #endregion


        #region 研究行移动的代码

        public void MoveLineTest()
        {
            int MoveRangeStart = 36;
            int MoveRangeEnd = TmSheet.LastRowNum;
            //MoveRangeEnd = 5;
            int MoveStepLine = 10;



            TmSheet.ShiftRows(
         MoveRangeStart,                                 //--开始行
        MoveRangeEnd,                            //--结束行
         MoveStepLine,                             //--移动大小(行数)--往下移动
         true,                                   //是否复制行高
         true                                  //是否重置行高
                                               //true                                    //是否移动批注

         );


            for (int i = MoveRangeStart; i < MoveStepLine; i++)
            {
                //HSSFRow targetRow = null;
                //HSSFCell sourceCell = null;
                //HSSFCell targetCell = null;

                //var targetRow = TmSheet.CreateRow(i + 1);
                //var targetCell = targetRow.CreateCell(0);
                //targetCell.SetCellValue("aaaa");


            }

        }




        public void Move()
        {
            IRow tmLine = TmSheet.GetRow(5);
            MyInsertRow(TmSheet, 36, 5, tmLine);
        }

        /// <summary>
        /// 添加插入一定的行，直接从网上找到的程序，用来研究和测试
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="插入行"></param>
        /// <param name="插入行总数"></param>
        /// <param name="源格式行"></param>
        private void MyInsertRow(ISheet sheet, int 插入行, int 插入行总数, IRow 源格式行)
        {
            #region 批量移动行
            sheet.ShiftRows
                (
                插入行,                                 //--开始行
                sheet.LastRowNum,//sheet    .LastRowNum-1,                            //--结束行
                插入行总数,                             //--移动大小(行数)--往下移动
                true,                                   //是否复制行高
                false                                  //是否重置行高
                                                       //,true                                    //是否移动批注
                );
            //sheet.ShiftRows(36, sheet.LastRowNum, 5, true, true);



            #endregion

            #region 对批量移动后空出的空行插，创建相应的行，并以插入行的上一行为格式源(即：插入行-1的那一行)
            for (int i = 插入行; i < 插入行 + 插入行总数; i++)
            {
                IRow targetRow = null;
                ICell sourceCell = null;
                ICell targetCell = null;

                targetRow = sheet.CreateRow(i);

                for (int m = 源格式行.FirstCellNum; m < 源格式行.LastCellNum; m++)
                {
                    sourceCell = 源格式行.GetCell(m);
                    if (sourceCell == null)
                        continue;
                    targetCell = targetRow.CreateCell(m);
                    targetCell.CellComment = targetCell.CellComment;
                    //targetCell.set = sourceCell.CellStyle.e;
                    targetCell.CellStyle = sourceCell.CellStyle;
                    targetCell.SetCellType(sourceCell.CellType);
                }
                //CopyRow(sourceRow, targetRow);

                //Util.CopyRow(sheet, sourceRow, targetRow);
            }


            #endregion
        }


        #endregion  行移动

        #endregion 单元测试



        static void SetValueAndFormat(IWorkbook workbook, ICell cell, int value, short formatId)
        {
            cell.SetCellValue(value);
            ICellStyle cellStyle = workbook.CreateCellStyle();
            cellStyle.DataFormat = formatId;
            cell.CellStyle = cellStyle;
        }

        static void SetValueAndFormat(IWorkbook workbook, ICell cell, double value, short formatId)
        {
            cell.SetCellValue(value);
            ICellStyle cellStyle = workbook.CreateCellStyle();
            cellStyle.DataFormat = formatId;
            cell.CellStyle = cellStyle;
        }
        static void SetValueAndFormat(IWorkbook workbook, ICell cell, DateTime value, short formatId)
        {
            //set value for the cell
            if (value != null)
                cell.SetCellValue(value);

            ICellStyle cellStyle = workbook.CreateCellStyle();
            cellStyle.DataFormat = formatId;
            cell.CellStyle = cellStyle;
        }
    }


    public class CellSetter<T>
    {

        public T Value { get; set; }


        public short FormatId { get; set; }

    }

    public class CaptionFormatLine
    {
        public List<CaptionFormatItem> Items { get; set; }
    }

    /// <summary>
    /// 表头格式单项（一行可以用多个）
    /// </summary>
    public class CaptionFormatItem
    {
        public string Tm { get; set; }
        public string Str { get; set; }

    }

    public class CaptionFormatPart
    {
        public int Index { get; set; }

    }
}
