using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.IO.ExcelUtil
{
    /// <summary>
    /// 原点定位
    /// </summary>
    public struct OriginCell
    {

        //private int x = -1;
        //private int y = -1;
        //private bool has = false;

        private int x, y;
        private bool has;
        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public bool HasFind { get { return has; } set { has = value; } }

        //public OriginCell(bool has)
        //{
        //    X = -1;
        //    Y = -1;
        //    HasFind = has;
        //}
        public OriginCell(int x, int y, bool has)
        {
            this.x = x;
            this.y = y;
            this.has = has;
        }

        public OriginCell(int x, int y) : this(x, y, true) { }

    }


    /// <summary>
    /// 原点单元格查找器
    /// </summary>
    public class OriginCellFinder
    {

        public ISheet Sheet { get; set; }

        public IWorkbook WorkBook { get; set; }

        public List<string> OriginNames { get; set; }

        public OriginCell ResultCell { get; set; }

        public List<OriginCell> ResultList { get; set; }
        public OriginCellFinder()
        {
            ResultCell = new OriginCell();
            OriginNames = new List<string>();
            ResultList = new List<OriginCell>();
        }


        public OriginCellFinder(ISheet sheet)
            : this(sheet, new List<string>())
        {

        }
        public OriginCellFinder(ISheet sheet, string name)
            : this(sheet, new List<string>() { name })
        {
        }

        public OriginCellFinder(ISheet sheet, List<string> names)
            : this()
        {
            this.Sheet = sheet;
            this.WorkBook = sheet.Workbook;
            this.OriginNames = names;
        }

        public OriginCellFinder(IWorkbook wookbook, List<string> names)
        {
            this.WorkBook = wookbook;
            this.Sheet = wookbook.GetSheetAt(0);
            this.OriginNames = names;
        }




        public void Find()
        {
            this.ResultCell = FindInSheet(this.Sheet);
        }

        public OriginCell FindInSheet(ISheet sheet)
        {
            var ResultOrigin = new OriginCell();
            for (int i = 0; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (null == row)
                    continue;

                if (0 == row.Cells.Count)
                    continue;
                //row.Cells.Count 实际使用的数量 
                //row.LastCellNum 最后一个cell的数字
                //if (row.Cells.Count == row.LastCellNum)
                //{
                //    //Console.WriteLine("equre !");
                //}
                for (int j = 0; j < row.LastCellNum; j++)
                {
                    x.Say(String.Format("准备处理数据【{0}】行【{1}】列", i, j));
                    ICell cell = row.GetCell(j);
                    if (IsOriginCell(cell))
                    {
                        ResultOrigin = new OriginCell(j, i);
                        return ResultOrigin;
                    }
                    //if ("品名" == cellString)
                    //{
                    //    ResultOrigin = new OriginCell(j, i);
                    //    return;
                    //}
                }
            }
            return ResultOrigin;
        }

        protected bool IsOriginCell(ICell cell)
        {
            if (null == cell)
                return false;
            string CellString = ExcelCellUtil.GetCellString(cell);
            return IsOriginCell(CellString);
        }
        protected bool IsOriginCell(string cellString)
        {
            if (null == cellString)
            {
                x.Say("没有数据。");
            }
            else
            {
                x.Say("找到数据：" + cellString);
            }
            //没有指定 原点单元格的文本，则任何文字都是原点
            if (null == OriginNames || 0 == OriginNames.Count)
            {
                return true;
            }
            //指定了 原点的文本，则会详细判断
            if (OriginNames.Contains(cellString))
            {
                return true;
            }
            return false;
        }

        public void FindAll()
        {
            for (int i = 0; i < WorkBook.NumberOfSheets; i++)
            {
                var Sheet = WorkBook.GetSheetAt(i);
                var NewSheetOrigin =  FindInSheet(Sheet);
                //if (ResultOrigin.HasFind)
                //{
                //}
                ResultList.Add(NewSheetOrigin);

            }
        }
    }



}
