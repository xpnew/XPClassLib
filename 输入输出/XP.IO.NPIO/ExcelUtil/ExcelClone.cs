using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.IO.ExcelUtil
{
    public class ExcelClone
    {

        //public IWorkbook Workbook { get =>_WorkBook; set => _WorkBook = value; }

        /// <summary>
        /// 内部使用的工作薄（excel）
        /// </summary>
        protected IWorkbook _WorkBook;
        private string _FilePath;
        /// <summary>
        /// 行定义（空标题略过）
        /// </summary>
        private Dictionary<int, string> ColumnDict { get; set; }
        /// <summary>
        /// 标题行的长度，实际数据空列跳过，0索引
        /// </summary>
        public int ColumnSize { get; set; }




        /// <summary>
        /// 数据开始的行位置，这里按照Excel习惯计数，从1开始
        /// </summary>
        public int StartRowIndex { get; set; }


        /// <summary>
        /// 需要替换文字内容的最大行数
        /// </summary>
        public int ReplaceCellRowMax { get; set; }

        /// <summary>
        /// 数据列结束的位置
        /// </summary>
        public int DataColumnLast { get; set; }

        /// <summary>
        /// 行的长度，从起点到最后一个数据cell，0索引
        /// </summary>
        public int LineLength { get; set; }

        /// <summary>
        /// 结果信息
        /// </summary>
        public ExcelResultInfo ResultInfo { get; set; }

        /// <summary>
        /// 一个集合，帮助程度定位到数据所在的原点，可省略
        /// </summary>
        public List<string> OriginNames { get; set; }

        /// <summary>
        /// 全部模板列表
        /// </summary>
        public List<ISheet> TmSheets { get; set; } = new List<ISheet>();

        /// <summary>
        /// 当前工作表
        /// </summary>
        public ISheet CurrentSheet { get; set; }

        /// <summary>
        /// 当前使用的模板名称
        /// </summary>
        public string CurrentTmSheetName { get; set; }


        private ICellStyle _BaseCellStyle;

        public ExcelClone(string path)

        {
            _FilePath = path;
            Init();
        }
        protected void Init()
        {
            ColumnDict = new Dictionary<int, string>();
            ResultInfo = new ExcelResultInfo();
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
                _BaseCellStyle = _WorkBook.CreateCellStyle();
                _BaseCellStyle.BorderBottom = BorderStyle.Thin;
                _BaseCellStyle.BorderLeft = BorderStyle.Thin;
                _BaseCellStyle.BorderRight = BorderStyle.Thin;
                _BaseCellStyle.BorderTop = BorderStyle.Thin;


            }
            else
            {
                this.ResultInfo = ep.ResultInfo;
            }
        }


        protected void SetCellBorder(ICell cell)
        {
            var cellStyle = cell.CellStyle;
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.BottomBorderColor = HSSFColor.Black.Index;
            cellStyle.TopBorderColor = HSSFColor.Black.Index;
            cellStyle.LeftBorderColor = HSSFColor.Black.Index;
            cellStyle.RightBorderColor = HSSFColor.Black.Index;
            cell.CellStyle = cellStyle;
        }

        public bool GetTemplate()
        {
            var ep = new ExcelProvider(_FilePath);

            if (ep.ResultInfo.Success)
            {
                this._WorkBook = ep.Workbook;
            }
            else
            {
                this.ResultInfo = ep.ResultInfo;
                return false;
            }
            for (int m = 0; m < this._WorkBook.NumberOfSheets; m++)
            {
                ISheet sheet = this._WorkBook.GetSheetAt(m);
                TmSheets.Add(sheet);
            }

            if (0 == TmSheets.Count)
            {
                ResultInfo.Error(ExcelInfoTypes.WorkbookError, "没有找到数据。");
                return false;
            }
            HideTm();
            return true;
        }


        protected void HideTm()
        {


            for (int i = 0; i < TmSheets.Count; i++)
            {
                _WorkBook.SetSheetHidden(i, SheetState.Hidden);
            }
        }

        /// <summary>
        /// 将一个战士的数据填充到新的sheet
        /// </summary>
        /// <param name="name">新sheet的名称</param>
        /// <param name="dt">相关的数据</param>
        /// <param name="colNames">按照顺序去寻找dt参数里面的列</param>
        /// <param name="replaceDict">表头替换字典</param>
        /// <param name="failure">故障信息</param>
        public void FillFighterDt(string name, DataTable dt, List<string> colNames, Dictionary<string, string> replaceDict, string failure)
        {

            var tm = _WorkBook.GetSheet(CurrentTmSheetName);

            var NewSheet = _WorkBook.CloneSheet(_WorkBook.GetSheetIndex(tm));
            // NewSheet.SheetName = name;
            _WorkBook.SetSheetName(_WorkBook.GetSheetIndex(NewSheet), name);




            if (null != replaceDict)
            {
                Replace(NewSheet, replaceDict);
            }
            int CurrentRowIndex = this.StartRowIndex;

            bool HasStart = false;
            IRow SourceRow = NewSheet.GetRow(CurrentRowIndex);
            bool IsNewLine = false;
            if (!String.IsNullOrEmpty(failure))
            {
                var FailureInfoCell = CellUtil.GetCell(CellUtil.GetRow(3, NewSheet), 6);
                FailureInfoCell.SetCellValue(failure);
            }
            foreach (DataRow row in dt.Rows)
            {
                IRow NewRow = null; ;

                if (HasStart)
                {
                    NewRow = NewSheet.GetRow(CurrentRowIndex);
                    if (null == NewRow)
                    {
                        NewRow = NewSheet.CreateRow(CurrentRowIndex);
                        CopyLineStyle(SourceRow, NewRow);
                    }
                }
                else
                {
                    NewRow = NewSheet.GetRow(CurrentRowIndex);
                    HasStart = true;
                }

                for (int j = 0; j < colNames.Count; j++)
                {
                    string colName = colNames[j];
                    ICell newCell = NewRow.GetCell(j);
                    if (DBNull.Value == row[colNames[j]])
                    {
                        continue;
                    }
                    if (row.IsNull(colName))
                    {
                        continue;
                    }
                    string DbCellValue = row[colName].ToString();

                    newCell.SetCellValue(DbCellValue);
                    newCell.CellStyle.ShrinkToFit = true;
                }

                CurrentRowIndex++;
            }

            // 格式化当前sheet，用于数据total计算
            NewSheet.ForceFormulaRecalculation = true;


        }



        public void Replace(ISheet sheet, Dictionary<string, string> replaceDict)
        {

            for (int i = 0; i < ReplaceCellRowMax; i++)
            {
                var row = sheet.GetRow(i);

                if (null != row)
                {
                    for (int j = 0; j < row.LastCellNum; j++)
                    {
                        ICell cell = row.GetCell(j);
                        if (null == cell || cell.CellType == CellType.Blank || (cell.CellType == CellType.String && String.IsNullOrEmpty(cell.StringCellValue)))
                        {
                            continue;
                        }

                        string str = GetCellString(cell);

                        if (String.IsNullOrEmpty(str))
                        {
                            continue;
                        }
                        var rp = ReplaceCell(str, replaceDict);
                        if (rp.IsExist)
                        {
                            cell.SetCellValue(rp.Output);
                        }
                    }
                }
            }
        }

        protected (bool IsExist, string Output) ReplaceCell(string input, Dictionary<string, string> replaceDict)
        {
            (bool IsExist, string Output) Result = (IsExist: true, Output: input);

            foreach (var kr in replaceDict.Keys)
            {
                if (0 <= Result.Output.IndexOf("{" + kr + "}"))
                {
                    Result.Output = Result.Output.Replace("{" + kr + "}", replaceDict[kr]);
                    Result.IsExist = true;
                }
            }
            return Result;
        }


        protected string GetCellString(ICell cell)
        {
            string Result = null;

            switch (cell.CellType)
            {
                case CellType.Numeric:
                    //NewRow[j] = cell.NumericCellValue;
                    //因为不能识别数字还是日期，所以直接输出文本了
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        return null;
                    }
                    Result = cell.ToString();
                    break;
                case CellType.Boolean:
                    //NewRow[j] = cell.BooleanCellValue;
                    break;
                case CellType.Error:
                    //NewRow[j] = cell.ErrorCellValue;
                    break;
                case CellType.String:
                    Result = cell.StringCellValue;
                    break;
                case CellType.Formula:
                    // NewRow[ColIndex] = cell.CachedFormulaResultType;
                    switch (cell.CachedFormulaResultType)
                    {
                        case CellType.String:
                            string strFORMULA = cell.StringCellValue;
                            if (strFORMULA != null && strFORMULA.Length > 0)
                            {
                                Result = strFORMULA.ToString();
                            }
                            else
                            {
                                Result = null;
                            }
                            break;
                        case CellType.Numeric:

                            break;
                        case CellType.Boolean:

                            break;
                        case CellType.Error:

                            break;
                        default:
                            //NewRow[j] = "";
                            break;
                    }
                    break;
                default:
                    Result = cell.ToString();
                    break;
            }


            return Result;
        }



        public bool Save(string filePhysicalPath)
        {
            try
            {
                using (FileStream file = new FileStream(filePhysicalPath, FileMode.Create))
                {
                    _WorkBook.Write(file);
                    file.Close();
                    file.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                x.SayError("写入文件[" + filePhysicalPath + "]失败，可能是没有权限，具体原因请见异常说明。" + ex.Message);
                return false;
            }

        }

        protected void FillCell(ICell newCell, DataColumn column, string drValue, ICellStyle dateStyle)
        {
            switch (column.DataType.ToString())
            {
                case "System.String"://字符串类型
                    newCell.SetCellValue(drValue);
                    newCell.CellStyle.ShrinkToFit = true;
                    break;
                case "System.DateTime"://日期类型
                    DateTime dateV;
                    DateTime.TryParse(drValue, out dateV);
                    newCell.SetCellValue(dateV);

                    newCell.CellStyle = dateStyle;//格式化显示
                    break;
                case "System.Boolean"://布尔型
                    bool boolV = false;
                    bool.TryParse(drValue, out boolV);
                    newCell.SetCellValue(boolV);
                    break;
                case "System.Int16"://整型
                case "System.Int32":
                case "System.Int64":
                case "System.Byte":
                    int intV = 0;
                    int.TryParse(drValue, out intV);
                    newCell.SetCellValue(intV);
                    break;
                case "System.Decimal"://浮点型
                case "System.Double":
                    double doubV = 0;
                    double.TryParse(drValue, out doubV);
                    newCell.SetCellValue(doubV);
                    break;
                case "System.DBNull"://空值处理
                    newCell.SetCellValue("");
                    break;
                default:
                    newCell.SetCellValue("");
                    break;
            }
        }

        protected void CopyLineStyle(IRow source, IRow target)
        {
            target.Height = source.Height;

            for (int col = 0; col < source.LastCellNum; col++)
            {
                var cellsource = source.GetCell(col);
                var cellInsert = target.CreateCell(col);
                var cellStyle = cellsource.CellStyle;
                //设置单元格样式　　　　
                if (cellStyle != null)
                    cellInsert.CellStyle = cellsource.CellStyle;

            }
        }
    }
}
