using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XP.IO.ExcelUtil
{
    public class ExcelCellUtil
    {

        public static string GetCellString(ICell cell)
        {
            if (null == cell || cell.CellType == CellType.Blank || (cell.CellType == CellType.String && String.IsNullOrEmpty(cell.StringCellValue)))
            {
                return null;
            }
            return cell.ToString();
        }
    }
}
