using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Eval;
using NPOI.SS.UserModel;
using NPOI.SS.Formula.Eval;
using NPOI.SS.UserModel;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using NPOI.SS.Util;
namespace XP.IO.ExcelUtil
{
    /// <summary>
    /// Excel写入器（Excel 97 xls格式 ）
    /// </summary>
    public class ExcelWriter
    {
        /// <summary>
        /// 允许标题栏
        /// </summary>
        public bool EnableCaption { get; set; }


        /// <summary>
        /// 标题栏文字
        /// </summary>
        public string CaptionText { get; set; }

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

        /// <summary>
        /// Excel文件的物理路径
        /// </summary>
        public string FilePhysicalPath { get; set; }
        /// <summary>
        /// 使用的工作薄
        /// </summary>
        public IWorkbook Workbook { get; set; }

        /// <summary>
        /// 表格对就的标题国际化参照字典
        /// </summary>
        public Dictionary<string, List<string>> TableColumnsGlobal { get; set; }

        /// <summary>
        /// 输入的表格数据
        /// </summary>
        private List<DataTable> InputData { get; set; }

        /// <summary>
        /// 当前工作表的索引位置，主要是面向一个数据行数超过64000的情况，分成多个sheet处理
        /// </summary>
        private int _CurrentSheetIndex = 0;


        public ExcelWriter()
        {

        }


        public ExcelWriter(string cap, DataSet data)
        {
            _Init();
            if (null != data)
            {
                foreach (DataTable dt in data.Tables)
                {
                    InputData.Add(dt);
                }
            }
            if (null == cap || 0 == cap.Length)
            {
                CaptionText = null;
                EnableCaption = false;
                return;
            }
            CaptionText = cap;

            EnableCaption = true;
            _InitWorkbook();

        }

        public ExcelWriter(string cap, DataTable dt)
        {
            _Init();
            if (null != dt)
            {
                InputData.Add(dt);
            }
            if (null == cap || 0 == cap.Length)
            {
                CaptionText = null;
                EnableCaption = false;
                return;
            }
            CaptionText = cap;

            EnableCaption = true;
            _InitWorkbook();
        }

        public ExcelWriter(DataTable dt):this(null, dt)
        {
        }


        protected void _Init()
        {
            CaptionText = null;
            EnableCaption = false;
            InputData = new List<DataTable>();
            TableColumnsGlobal = new Dictionary<string, List<string>>();
        }


        private void _InitWorkbook()
        {

            Workbook = new HSSFWorkbook(); //新建一个xls文件

            foreach (var dt in InputData)
            {
                var TableName = dt.TableName;
                List<string> cols;
                if (TableColumnsGlobal.ContainsKey(TableName))
                {
                    cols = TableColumnsGlobal[TableName];
                }
                else
                {
                    cols = FindTableColumnname(dt);
                }
                AddSheet(Workbook, cols, dt, null, false);
            }

        }

        /// <summary>
        /// 创建工作薄
        /// </summary>
        public void BuildWorkbook()
        {
            _InitWorkbook();

        }




        /// <summary>
        /// 从List(Array)创建工作薄
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="captionText"></param>
        /// <param name="enableCaption"></param>
        /// <returns></returns>
        public static IWorkbook ListToWorkbook(List<string> columns, DataTable inputDataTable, string captionText, bool enableCaption)
        {

            //List<string> ColumnsNameList;



            HSSFWorkbook hssfworkbook = new HSSFWorkbook(); //新建一个xls文件
            //hssfworkbook = HSSFWorkbook.Create(hssfworkbook);
            //当前工作表的位置
            int CurrentSheetIndex = 0;
            //所有的工作表数量，用来循环和切割工作表
            //切割工作表由SheetCounter* CurrentSheetIndex+ CurrentSheet实现
            int SheetCounter = 0;
            //每个工作表的最大条数
            int SheetRowMax = 65000;
            //最后一个工作表的条数
            int LastSheetRows = SheetRowMax;
            //当前工作表的条数
            int CurrentSheetRows = SheetRowMax;

            //数据总条数
            int AllRowsCounter = inputDataTable.Rows.Count;
            //数据表当中的位置
            int DtRowsIndex = 0;
            string SheetName = "newsheet";

            if (String.IsNullOrEmpty(inputDataTable.TableName))
            {
                SheetName = inputDataTable.TableName;
            }

            if (0 == AllRowsCounter)
            {
                return hssfworkbook;
            }

            //整除的结果
            SheetCounter = AllRowsCounter / SheetRowMax;
            //取余
            LastSheetRows = AllRowsCounter % SheetRowMax;
            if (0 != LastSheetRows)
            {
                SheetCounter++;
            }

            List<ISheet> SheetList = new List<ISheet>();
            for (int i = 0; i < SheetCounter; i++)
            {
                ISheet NewSheet = hssfworkbook.CreateSheet(SheetName + (i + 1));
                SheetList.Add(NewSheet);
            }


            ICellStyle dateStyle = hssfworkbook.CreateCellStyle();
            IDataFormat format = hssfworkbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");


            try
            {
                for (CurrentSheetIndex = 0; CurrentSheetIndex < SheetCounter; CurrentSheetIndex++)
                {
                    // CurrentSheetIndex = i;
                    ISheet sheet = SheetList[CurrentSheetIndex];

                    if (CurrentSheetIndex == SheetCounter - 1)
                    {
                        CurrentSheetRows = LastSheetRows;
                    }
                    else
                    {
                        CurrentSheetRows = SheetRowMax;
                    }


                    //数据行起始位置
                    int DataRowStartIndex = 0;
                    //标题行起始位置
                    int ColumnRowStartIndex = 0;
                    //当前数据行的位置
                    int CurrentRowIndex = 0;
                    //列数
                    int ColumnsLength = columns.Count;

                    CurrentRowIndex = CreateColumns(columns, sheet, captionText, enableCaption);

                    //第一行是跨行大标题
                    if (enableCaption)
                    {
                        DataRowStartIndex++;
                        //ColumnRowStartIndex++;
                        //CurrentRowIndex++;
                    }




                    //单元格的坐标，起始值为(1,1)，所以，要对DataTable的坐标进行转换
                    //单元格的坐标，是Y轴在前
                    int CellX, CellY;
                    for (int i = 0; i < CurrentSheetRows; i++)
                    {
                        IRow row = sheet.CreateRow(CurrentRowIndex);

                        DtRowsIndex = i + CurrentSheetIndex * SheetRowMax;


                        DataRow CurrentRow = inputDataTable.Rows[DtRowsIndex];
                        //DataRow dRow = dt.Rows[DtRowsIndex];

                        for (int j = 0; j < columns.Count; j++)
                        {

                            ICell newCell = row.CreateCell(j);
                            //object PropertyValue = Copyer.GetValue(CurrentLineObject, columns[j]);
                            //if (null == PropertyValue)
                            //    continue;
                            //string drValue = PropertyValue.ToString();
                            if (DBNull.Value == CurrentRow[columns[j]])
                            {
                                continue;
                            }
                            string DbCellValue = CurrentRow[columns[j]].ToString();


                            newCell.SetCellValue(DbCellValue);
                            newCell.CellStyle.ShrinkToFit = true;

                        }
                        CurrentRowIndex++;
                    }

                    // 格式化当前sheet，用于数据total计算
                    sheet.ForceFormulaRecalculation = true;
                }
                return hssfworkbook;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }

        }

        public static void AddSheet(IWorkbook workbook, List<string> columns, DataTable inputDataTable, string captionText, bool enableCaption)
        {
            //hssfworkbook = HSSFWorkbook.Create(hssfworkbook);
            //当前工作表的位置
            int CurrentSheetIndex = 0;
            //所有的工作表数量，用来循环和切割工作表
            //切割工作表由SheetCounter* CurrentSheetIndex+ CurrentSheet实现
            int SheetCounter = 0;
            //每个工作表的最大条数
            int SheetRowMax = 65000;
            //最后一个工作表的条数
            int LastSheetRows = SheetRowMax;
            //当前工作表的条数
            int CurrentSheetRows = SheetRowMax;

            //数据总条数
            int AllRowsCounter = inputDataTable.Rows.Count;
            //数据表当中的位置
            int DtRowsIndex = 0;
            string SheetName = "newsheet";

            if (String.IsNullOrEmpty(inputDataTable.TableName))
            {
                SheetName = inputDataTable.TableName;
            }

            if (0 == AllRowsCounter)
            {
                return ;
            }

            //整除的结果
            SheetCounter = AllRowsCounter / SheetRowMax;
            //取余
            LastSheetRows = AllRowsCounter % SheetRowMax;
            if (0 != LastSheetRows)
            {
                SheetCounter++;
            }

            List<ISheet> SheetList = new List<ISheet>();
            for (int i = 0; i < SheetCounter; i++)
            {
                ISheet NewSheet = workbook.CreateSheet(SheetName + (i + 1));
                SheetList.Add(NewSheet);
            }


            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");


            try
            {
                for (CurrentSheetIndex = 0; CurrentSheetIndex < SheetCounter; CurrentSheetIndex++)
                {
                    // CurrentSheetIndex = i;
                    ISheet sheet = SheetList[CurrentSheetIndex];

                    if (CurrentSheetIndex == SheetCounter - 1)
                    {
                        CurrentSheetRows = LastSheetRows;
                    }
                    else
                    {
                        CurrentSheetRows = SheetRowMax;
                    }


                    //数据行起始位置
                    int DataRowStartIndex = 0;
                    //标题行起始位置
                    int ColumnRowStartIndex = 0;
                    //当前数据行的位置
                    int CurrentRowIndex = 0;
                    //列数
                    int ColumnsLength = columns.Count;

                    CurrentRowIndex = CreateColumns(columns, sheet, captionText, enableCaption);

                    //第一行是跨行大标题
                    if (enableCaption)
                    {
                        DataRowStartIndex++;
                        //ColumnRowStartIndex++;
                        //CurrentRowIndex++;
                    }




                    //单元格的坐标，起始值为(1,1)，所以，要对DataTable的坐标进行转换
                    //单元格的坐标，是Y轴在前
                    int CellX, CellY;
                    for (int i = 0; i < CurrentSheetRows; i++)
                    {
                        IRow row = sheet.CreateRow(CurrentRowIndex);

                        DtRowsIndex = i + CurrentSheetIndex * SheetRowMax;


                        DataRow CurrentRow = inputDataTable.Rows[DtRowsIndex];
                        //DataRow dRow = dt.Rows[DtRowsIndex];

                        for (int j = 0; j < columns.Count; j++)
                        {

                            ICell newCell = row.CreateCell(j);
                            //object PropertyValue = Copyer.GetValue(CurrentLineObject, columns[j]);
                            //if (null == PropertyValue)
                            //    continue;
                            //string drValue = PropertyValue.ToString();
                            if (DBNull.Value == CurrentRow[columns[j]])
                            {
                                continue;
                            }
                            if(CurrentRow.IsNull(j)){
                                continue;
                            }
                            string DbCellValue = CurrentRow[columns[j]].ToString();


                            newCell.SetCellValue(DbCellValue);
                            newCell.CellStyle.ShrinkToFit = true;

                        }
                        CurrentRowIndex++;
                    }

                    // 格式化当前sheet，用于数据total计算
                    sheet.ForceFormulaRecalculation = true;
                }
             
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
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



        /// <summary>
        /// 创建标题（允许可选的跨行表头）
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sheet"></param>
        /// <param name="captionText"></param>
        /// <param name="enableCaption"></param>
        /// <returns>当前行的索引（从0开始）</returns>
        public static int CreateColumns(List<string> columns, ISheet sheet, string captionText, bool enableCaption)
        {
            //标题行起始位置
            int ColumnRowStartIndex = 0;

            //第一行是跨行大标题
            if (enableCaption)
            {
                IRow row = sheet.CreateRow(0);
                ICell cell = row.CreateCell(0);
                cell.SetCellValue(captionText);
                //列数
                int ColumnsLength = columns.Count;


                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, ColumnsLength - 1));
                ColumnRowStartIndex++;
            }
            //添加上列名
            //Excel当中起始行的索引是1，所以这里列标题行要加上1
            IRow TitelRow = sheet.CreateRow(ColumnRowStartIndex);
            //文字
            for (int j = 0; j < columns.Count; j++)
            {
                ICell cell = TitelRow.CreateCell(j);
                sheet.SetColumnWidth(j, columns[j].Length * 256);
                cell.SetCellValue(columns[j]);
            }

            ColumnRowStartIndex++;
            return ColumnRowStartIndex;
        }


        /// <summary>
        /// 如果没有提供国际化的标题行，通过这里的实现从原来的表格当中提取
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<string> FindTableColumnname(DataTable dt){
            var Result = new List<string>();

            foreach (DataColumn col in dt.Columns)
            {
                Result.Add(col.ColumnName);
            }
            return Result;
        }

        public MemoryStream GetStream()
        {
            MemoryStream filestream = new MemoryStream(); //内存文件流(应该可以写成普通的文件流)
            Workbook.Write(filestream); //把文件读到内存流里面
            return filestream;
        }

        public void Write2Stream(Stream inputStream)
        {

        }

        public bool WriteToFile(string filePhysicalPath)
        {
            try
            {
                using (FileStream file = new FileStream(filePhysicalPath, FileMode.Create))
                {
                    Workbook.Write(file);
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



    }
}
