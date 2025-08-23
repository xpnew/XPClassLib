using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XP.IO.ExcelUtil
{
    /// <summary>
    /// MultiTableReader
    /// </summary>
    /// <remarks>
    /// 创建日期：2025/8/20 18:30:57
    /// 类名：MultiTableReader
    /// 创建人： xpnew@126.com
    /// </remarks>
    public class MultiTableReader : ExcelReader
    {
        #region <属性>

        public List<TableInfo> ResultItems = new List<TableInfo>();



        public int LineIndex { get; set; } = 0;


        /// <summary>
        /// 结尾空行数
        /// </summary>
        public int EndNullLine { get; set; } = 1;


        #endregion <属性>

        #region <构造方法>

        public MultiTableReader(string path) : base(path)
        {

        }
        #endregion <构造方法>

        #region <内部方法>
        #endregion <内部方法 end>

        protected void _ReadTabels(ISheet sheet)
        {
            var tb = Next(sheet);
           
            while ( null != tb)
            {
                ResultItems.Add(tb);
                tb = Next(sheet);
            }
            x.Say($"程序退出，索引[{LineIndex}]");


        }

        protected TableInfo Next(ISheet sheet)
        {
            int TableNameLine = LineIndex;
            int HeaderLine = LineIndex + 1;
            int ColNameLine = LineIndex + 2;



            TableInfo Result = new TableInfo();

            IRow Row4Tb = sheet.GetRow(TableNameLine);
            IRow Row4Head = sheet.GetRow(HeaderLine);
            IRow Row4Col = sheet.GetRow(ColNameLine);

            if(null ==  Row4Head ||  null == Row4Col)
            {
                return null;
            }

            ICell cell0 = Row4Tb.GetCell(0);
            ICell cell1 = Row4Head.GetCell(0);
            ICell cell2 = Row4Col.GetCell(0);

            if(CheckNullCell(cell0) ||  CheckNullCell(cell1)  && CheckNullCell(cell2))
            {
                return null;
            }

            if (!CheckTableLine(Row4Tb) || !CheckHeadLine(Row4Head))
            {
                return null;
            }
            Result.Name = Row4Tb.GetCell(0).ToString();
            Result.GlobalName = Row4Tb.GetCell(1).ToString();
            int StartIdx = ColNameLine;

            while(null != sheet.GetRow(StartIdx) && !CheckNullCell(sheet.GetRow(StartIdx).GetCell(0)))
            {
                string ColName = sheet.GetRow(StartIdx).GetCell(0).ToString();
                string ColGloble = sheet.GetRow(StartIdx).GetCell(2).ToString();
                ColumnInfo col = new ColumnInfo()
                {
                    Name = ColName,
                    GlobalName = ColGloble,
                };
                Result.Columns.Add(col);
                StartIdx++;
            }
            //if(sheet.IsMergedRegion())

            LineIndex = StartIdx + EndNullLine - 1;

            LineIndex++;

            return Result;

        }


        /// <summary>
        /// 是不是表头行， 第1列、第2列有值，第3列没有值
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        protected bool CheckTableLine( IRow row)
        {
            //x.Say($"第一格类型是：{row.GetCell(0).CellType}，内容是[{row.GetCell(0)}]");
            //x.Say($"第三格类型是：{row.GetCell(2).CellType}，内容是[{row.GetCell(2)}]");

            //有的表 没有中文说明，所以第二格是空的
            //if(CheckNull(row.GetCell(0)) || CheckNull(row.GetCell(1)))
            if (CheckNullCell(row.GetCell(0)))
            {
                return false;
            }

            if (CheckNullCell(row.GetCell(2)) )
            {
                return true;
            }
            return false;
        }

        protected bool CheckHeadLine(IRow row)
        {
            if (CheckNullCell(row.GetCell(0)) || CheckNullCell(row.GetCell(1)) || CheckNullCell(row.GetCell(2)))
            {
                return false;
            }

            //x.Say($"第一格：{row.GetCell(0)}<");
            //x.Say($"第二格：{row.GetCell(1)}<");
            //x.Say($"第三格：{row.GetCell(2)}<");
            if ("字段名称" == row.GetCell(0).ToString().Trim() && "字段描述" == row.GetCell(2).ToString().Trim())
            {
                return true;
            }

            return false;

        }



        #region <外部方法>

        public void GetTables()
        {
            TryWorkbook();
            if (ResultInfo.Success)
            {
                ISheet FirstSheet = _WorkBook.GetSheetAt(0);


                _ReadTabels(FirstSheet);
            }
            else
            {
                return;
            }

        }


        public bool HasMerged()
        {
            TryWorkbook();
            if (ResultInfo.Success)
            {
                ISheet FirstSheet = _WorkBook.GetSheetAt(0);
                for (int i = 0; i < FirstSheet.NumMergedRegions; i++)
                {
                    CellRangeAddress range = FirstSheet.GetMergedRegion(i);
                    x.Say($"合并单元格： x => [{range.FirstColumn}-{range.LastColumn}], y => [{range.FirstRow}-{range.LastRow}]");
                }
                return true;

                }
            else
            {
                return false;
            }


        }


        #endregion <外部方法 end>

        #region <事件>
        #endregion <事件>

    }

    public class TableInfo
    {
        public string Name;
        public string GlobalName;

        public List<ColumnInfo> Columns { get; set; } = new List<ColumnInfo>();

    }

    public class ColumnInfo
    {
        public string Name;
        public string GlobalName;
    }
}
