using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace XP.IO.ExcelUtil
{
    /// <summary>
    /// Excel文件读取器
    /// </summary>
    public class ExcelReader
    {

        /// <summary>
        /// 数据的起始单元格
        /// </summary>
        private int _StartX = 0;
        private int _StartY = 0;

        /// <summary>
        /// 行定义（空标题略过）
        /// </summary>
        private Dictionary<int, string> ColumnDict { get; set; }

        /// <summary>
        /// 标题行的长度，实际数据空列跳过，0索引
        /// </summary>
        public int ColumnSize { get; set; }

        /// <summary>
        /// 行的长度，从起点到最后一个数据cell，0索引
        /// </summary>
        public int LineLength { get; set; }

        /// <summary>
        /// 最后一个数据Cell的值，NPOI提供，可能包括空列
        /// </summary>
        public int MaxCellNum { get; set; }

        /// <summary>
        /// 原点（需要废弃）
        /// </summary>
        public OriginCell Origin { get; set; }

        /// <summary>
        /// 一个集合，帮助程度定位到数据所在的原点，可省略
        /// </summary>
        public List<string> OriginNames { get; set; }

        /// <summary>
        /// 结果信息
        /// </summary>
        public ExcelResultInfo ResultInfo { get; set; }

        /// <summary>
        /// 内部使用的工作薄（excel）
        /// </summary>
        private IWorkbook _WorkBook;

        public List<DataTable> ResultTables { get; set; }

        private string _FilePath;

        public ExcelReader()
        {

            Init();
        }

        public ExcelReader(string path)
            : this()
        {
            _FilePath = path;

        }


        protected void Init()
        {
            ColumnDict = new Dictionary<int, string>();
            ResultInfo = new ExcelResultInfo();
            ResultTables = new List<DataTable>();
            OriginNames = new List<string>();
        }


        /// <summary>
        /// 尝试打开Excel文件，并且获取工作薄
        /// </summary>
        protected void TryWorkbook()
        {
            var ep = new ExcelProvider(_FilePath);

            if (ep.ResultInfo.Success)
            {
                this._WorkBook = ep.Workbook;
            }
            else
            {
                this.ResultInfo = ep.ResultInfo;
            }
        }

        public void GetData()
        {
            if (ResultInfo.Success)
            {
                if (null == _WorkBook)
                {
                    TryWorkbook();
                    if (!ResultInfo.Success)
                    {
                        return;
                    }
                }
            }
            else
            {
                return;
            }

            for (int m = 0; m < this._WorkBook.NumberOfSheets; m++)
            {
                ISheet sheet = this._WorkBook.GetSheetAt(m);
                DataTable NewDt = new DataTable();

                FillDt(sheet, NewDt);

                if (0 == NewDt.Columns.Count)
                {
                    return;
                }
                if (0 < NewDt.Rows.Count)
                {
                    ResultTables.Add(NewDt);
                }
            }

            if (0 == ResultTables.Count)
            {
                ResultInfo.Error(ExcelInfoTypes.WorkbookError, "没有找到数据。");
            }
        }


        public DataTable ToDataTable()
        {
            DataTable Result = new DataTable();
            if (ResultInfo.Success)
            {
                if (null == _WorkBook)
                {
                    TryWorkbook();
                    if (!ResultInfo.Success)
                    {
                        return Result;
                    }
                }
            }
            else
            {
                return Result;
            }
            FillDt(_WorkBook, Result);
            return Result;
        }

        /// <summary>
        /// 使用工作薄填充一个空白的DataTable
        /// </summary>
        /// <param name="book"></param>
        /// <param name="dt"></param>
        private void FillDt(IWorkbook book, DataTable dt)
        {
            ISheet FirstSheet = book.GetSheetAt(0);
            if (null == FirstSheet)
                return;

            dt.TableName = FirstSheet.SheetName;
            IDataFormat format = book.CreateDataFormat();
            ICellStyle dateStyle = book.CreateCellStyle();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd hh:mm:ss");

            FindOrigin(book);


            if (!Origin.HasFind)
            {
                return;
            }
            //数据行起始位置
            int DataRowStartIndex = 0;
            //标题行起始位置
            int ColumnRowStartIndex = 0;
            //列数
            int ColumnsLength;
            //行数
            int CellRowsLength;


            //Excel当中起始行的索引是1，所以这里列标题行要加上1
            IRow tmTitleRow = FirstSheet.GetRow(Origin.Y);
            if (Origin.Y > 0)
            {
                ICell cell = FirstSheet.GetRow(Origin.Y - 1).GetCell(Origin.X);
                if (null != cell)
                {
                    string CellString = GetCellString(cell);
                    if (null == CellString)
                    {
                        dt.TableName = CellString;
                    }
                }
            }
            #region 处理标题
            ColumnRowStartIndex = Origin.Y;
            //添加上列名
            //ColumnsLength = tmTitleRow.Cells.Count;
            for (int j = Origin.X; j < tmTitleRow.LastCellNum; j++)
            {
                ICell cell = tmTitleRow.GetCell(j);
                string CellString = GetCellString(cell);
                if (null == CellString)
                {
                    x.Say("没有数据。");
                    x.Say(String.Format("准备处理数据【{0}】行【{1}】列", Origin.Y, j));
                    Error(ExcelInfoTypes.ColumnError, "空的标题行", j, Origin.Y);
                }
                dt.Columns.Add(new DataColumn(cell.StringCellValue));

            }
            #endregion


            #region 处理数据
            DataRowStartIndex = Origin.Y + 1;

            for (int i = DataRowStartIndex; i <= FirstSheet.LastRowNum; i++)
            {
                IRow Row = FirstSheet.GetRow(i);
                DataRow NewRow = dt.NewRow();
                if (null == Row)
                {
                    dt.Rows.Add(NewRow);
                    continue;
                }
                for (int j = Origin.X; j < Row.LastCellNum; j++)
                {
                    ICell cell = Row.GetCell(j);
                    if (null == cell || cell.CellType == CellType.Blank || (cell.CellType == CellType.String && String.IsNullOrEmpty(cell.StringCellValue)))
                    {
                        continue;
                    }

                    switch (cell.CellType)
                    {
                        case CellType.Numeric:
                            //NewRow[j] = cell.NumericCellValue;
                            //因为不能识别数字还是日期，所以直接输出文本了
                            if (DateUtil.IsCellDateFormatted(cell))
                            {
                                cell.CellStyle = dateStyle;
                            }
                            NewRow[j] = cell.ToString();
                            break;
                        case CellType.Boolean:
                            NewRow[j] = cell.BooleanCellValue;
                            break;
                        case CellType.Error:
                            NewRow[j] = cell.ErrorCellValue;
                            break;
                        case CellType.String:
                            NewRow[j] = cell.StringCellValue;
                            break;
                        default:
                            NewRow[j] = cell.ToString();
                            break;
                    }
                }
                dt.Rows.Add(NewRow);
            }
            #endregion

        }
        private void FillDt(ISheet currentSheet, DataTable dt)
        {
            if (null == currentSheet)
                return;

            var book = currentSheet.Workbook;
            dt.TableName = currentSheet.SheetName;

            IDataFormat format = book.CreateDataFormat();
            ICellStyle dateStyle = book.CreateCellStyle();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd hh:mm:ss");

            FindOrigin(currentSheet);


            if (!Origin.HasFind)
            {
                return;
            }
            //数据行起始位置
            int DataRowStartIndex = 0;
            //标题行起始位置
            int ColumnRowStartIndex = 0;
            //列数
            int ColumnsLength;
            //行数
            int CellRowsLength;


            //Excel当中起始行的索引是1，所以这里列标题行要加上1
            IRow tmTitleRow = currentSheet.GetRow(Origin.Y);
            if (Origin.Y > 0)
            {
                IRow CaptionRow = currentSheet.GetRow(Origin.Y - 1);
                ICell cell;
                string CellString = null;
                if (null != CaptionRow)
                {
                    cell = currentSheet.GetRow(Origin.Y - 1).GetCell(Origin.X);
                    if (null != cell)
                    {
                        CellString = GetCellString(cell);
                    }
                }
                if (null == CellString)
                {
                    dt.TableName = CellString;
                }
            }
            #region 处理标题
            ColumnRowStartIndex = Origin.Y;
            //添加上列名
            //ColumnsLength = tmTitleRow.Cells.Count;
            for (int j = Origin.X; j < tmTitleRow.LastCellNum; j++)
            {
                ICell cell = tmTitleRow.GetCell(j);
                string CellString = GetCellString(cell);
                if (null == CellString)
                {
                    x.Say(String.Format("空的标题行【{0}】行【{1}】列", Origin.Y, j));
                    Error(ExcelInfoTypes.ColumnError, "空的标题行", j, Origin.Y);
                }
                dt.Columns.Add(new DataColumn(cell.StringCellValue));

            }
            #endregion


            #region 处理数据
            DataRowStartIndex = Origin.Y + 1;

            for (int i = DataRowStartIndex; i <= currentSheet.LastRowNum; i++)
            {
                IRow Row = currentSheet.GetRow(i);
                DataRow NewRow = dt.NewRow();
                if (null == Row)
                {
                    dt.Rows.Add(NewRow);
                    continue;
                }
                for (int j = Origin.X; j < Row.LastCellNum; j++)
                {
                    ICell cell = Row.GetCell(j);
                    if (null == cell || cell.CellType == CellType.Blank || (cell.CellType == CellType.String && String.IsNullOrEmpty(cell.StringCellValue)))
                    {
                        continue;
                    }


                    int ColIndex = j - Origin.X;
                    switch (cell.CellType)
                    {
                        case CellType.Numeric:
                            //NewRow[j] = cell.NumericCellValue;
                            //因为不能识别数字还是日期，所以直接输出文本了
                            if (DateUtil.IsCellDateFormatted(cell))
                            {
                                cell.CellStyle = dateStyle;
                            }
                            NewRow[ColIndex] = cell.ToString();
                            break;
                        case CellType.Boolean:
                            NewRow[ColIndex] = cell.BooleanCellValue;
                            break;
                        case CellType.Error:
                            NewRow[ColIndex] = cell.ErrorCellValue;
                            break;
                        case CellType.String:
                            NewRow[ColIndex] = cell.StringCellValue;
                            break;
                        default:
                            NewRow[ColIndex] = cell.ToString();
                            break;
                    }
                }
                dt.Rows.Add(NewRow);
            }
            #endregion

        }

        /// <summary>
        /// 查找原点
        /// </summary>
        /// <param name="book"></param>
        private void FindOrigin(IWorkbook book)
        {
            if (0 == book.NumberOfSheets)
            {
                Origin = new OriginCell();
                return;
            }
            var Finder = new OriginCellFinder(book, this.OriginNames);

            Finder.FindAll();
            Origin = Finder.ResultList[0];

            if (null != Finder)
            {
                return;
            }


            #region   old code......
            Origin = new OriginCell();

            ISheet sheet = book.GetSheetAt(0);

            for (int i = 0; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (null == row)
                    continue;

                if (0 == row.Cells.Count)
                    continue;

                if (row.Cells.Count == row.LastCellNum)
                {

                    Console.WriteLine("equre !");
                }

                for (int j = 0; j < row.LastCellNum; j++)
                {
                    //ICell cell = row.Cells[j];
                    ICell cell = row.GetCell(j);
                    string CellString = GetCellString(cell);
                    x.Say(String.Format("准备处理数据【{0}】行【{1}】列", i, j));
                    if (null == CellString)
                    {
                        x.Say("没有数据。");
                    }
                    else
                    {
                        x.Say("找到数据：" + CellString);
                    }
                    if ("品名" == CellString)
                    {
                        Origin = new OriginCell(j, i);
                        return;
                    }
                }

            }
            #endregion

        }
        /// <summary>
        /// 查找原点
        /// </summary>
        /// <param name="book"></param>
        private void FindOrigin(ISheet sheet)
        {

            var Finder = new OriginCellFinder(sheet, this.OriginNames);

            Finder.Find();
            Origin = Finder.ResultCell;

            if (null != Finder)
            {
                return;
            }

            #region   old code......

            Origin = new OriginCell();


            for (int i = 0; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (null == row)
                    continue;

                if (0 == row.Cells.Count)
                    continue;

                if (row.Cells.Count == row.LastCellNum)
                {

                    Console.WriteLine("equre !");
                }

                for (int j = 0; j < row.LastCellNum; j++)
                {
                    //ICell cell = row.Cells[j];
                    ICell cell = row.GetCell(j);
                    string CellString = GetCellString(cell);
                    x.Say(String.Format("准备处理数据【{0}】行【{1}】列", i, j));
                    if (null == CellString)
                    {
                        x.Say("没有数据。");
                    }
                    else
                    {
                        x.Say("找到数据：" + CellString);
                    }
                    if ("品名" == CellString)
                    {
                        Origin = new OriginCell(j, i);
                        return;
                    }
                }

            }
            #endregion

        }

        public void FindMaxCellNum()
        {
            int CurrentRowLength = -1;

            for (int m = 0; m < this._WorkBook.NumberOfSheets; m++)
            {
                ISheet sheet = this._WorkBook.GetSheetAt(m);

                for (int i = 0; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (null == row)
                        continue;

                    if (0 == row.Cells.Count)
                        continue;

                    if (row.Cells.Count == row.LastCellNum)
                    {

                        x.Say("这是一个满行,行数：" + i);
                    }
                    else
                    {
                        x.Say(String.Format("这是一个满行,行数：{0}。有数据的格数【{1}】，总格数【{2}】。", i, row.Cells.Count, row.LastCellNum));
                        CurrentRowLength = row.LastCellNum;
                        if (CurrentRowLength > MaxCellNum)
                        {

                            x.Say(String.Format("行内格数变化了：{0}。前面的格数【{1}】，将变换格数【{2}】。", i, CurrentRowLength, CurrentRowLength));
                            MaxCellNum = CurrentRowLength;
                        }

                    }


                }


            }
        }

        private string GetCellString(ICell cell)
        {
            if (null == cell || cell.CellType == CellType.Blank || (cell.CellType == CellType.String && String.IsNullOrEmpty(cell.StringCellValue)))
            {
                return null;
            }
            return cell.ToString();
        }

        private void Error(ExcelInfoTypes type, string errorMsg)
        {

            ResultInfo.Success = false;
            ResultInfo.ErrorList.Add(new ExcelCellInfo(type, errorMsg));
        }


        private void Error(ExcelInfoTypes type, string errorMsg, int x, int y)
        {

            ResultInfo.Success = false;
            var ErrorInfo = new ExcelCellInfo(type, errorMsg);
            ErrorInfo.RowNum = y;
            ErrorInfo.CellNum = x + 1;
            ResultInfo.ErrorList.Add(ErrorInfo);
        }

    }
}
