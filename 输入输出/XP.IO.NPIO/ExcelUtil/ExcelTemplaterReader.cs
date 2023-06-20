using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;
using System.IO;

namespace XP.IO.ExcelUtil
{
    public class ExcelTemplaterReader : ExcelReadBase
    {


        public ISheet TmSheet { get; set; }

        /// <summary>
        /// 模板行的位置，0索引
        /// </summary>
        public int TmLineIndex { get; set; }



        public IWorkbook NewWorkBook { get; set; }

        public ExcelTemplaterReader()
            : base()
        {


        }

        public ExcelTemplaterReader(string path)
            : base(path)
        {
        }


        protected override void _Init()
        {
            base._Init();
            NewWorkBook = new XSSFWorkbook();
            TmLineIndex = 1;
        }

        public void GetTmSheet()
        {

            ISheet Result = null;
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
            TmSheet = WorkBook.GetSheetAt(0);

        }

        public void CloneSheet()
        {
            ISheet sheet = NewWorkBook.CreateSheet("newsheet"); //创建一个sheet
            try
            {
                ICellStyle dateStyle = NewWorkBook.CreateCellStyle();
                IDataFormat format = NewWorkBook.CreateDataFormat();
                dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd hh:mm:ss");


                for (int i = 0; i <= TmSheet.LastRowNum; i++)
                {
                    IRow tmDataRow = TmSheet.GetRow(i);
                    IRow row = sheet.CreateRow(i);

                    if (null == tmDataRow)
                    {
                        continue;
                    }
                    for (int j = 0; j <= tmDataRow.LastCellNum; j++)
                    {
                        ICell tmCell = tmDataRow.GetCell(j);
                        ICell cell = row.CreateCell(j);
                        if (null == tmCell)
                        {
                            continue;
                        }
                        ICellStyle style = NewWorkBook.CreateCellStyle();
                        style.CloneStyleFrom(tmCell.CellStyle);
                        //RowStyleList.Add(style);
                        //复制公式
                        cell.CellStyle = style;
                        if (null == tmCell || tmCell.CellType == CellType.Blank || (tmCell.CellType == CellType.String && String.IsNullOrEmpty(tmCell.StringCellValue)))
                        {
                            continue;
                        }

                        //if (tmCell.CellType == CellType.Formula && null != tmCell.CellFormula && tmCell.CellFormula != "")
                        //{
                        //    cell.SetCellFormula(tmCell.CellFormula);
                        //}
                        //else
                        //{
                        //    cell.SetCellValue(tmCell.StringCellValue);

                        //}
                        #region 填充单元格数据
                        switch (tmCell.CellType)
                        {
                            case CellType.Numeric:
                                //NewRow[j] = tmCell.NumericCellValue;
                                //因为不能识别数字还是日期，所以直接输出文本了
                                if (DateUtil.IsCellDateFormatted(tmCell))
                                {
                                    tmCell.CellStyle = dateStyle;
                                }
                                cell.SetCellValue(tmCell.ToString());
                                break;
                            case CellType.Boolean:

                                cell.SetCellValue(tmCell.BooleanCellValue);
                                break;
                            case CellType.Error:
                                cell.SetCellValue(tmCell.ErrorCellValue);
                                break;
                            case CellType.String:
                                cell.SetCellValue(tmCell.StringCellValue);
                                break;
                            case CellType.Formula:
                                // NewRow[ColIndex] = tmCell.CachedFormulaResultType;
                                switch (tmCell.CachedFormulaResultType)
                                {
                                    case CellType.String:
                                        string strFORMULA = tmCell.StringCellValue;
                                        if (strFORMULA != null && strFORMULA.Length > 0)
                                        {
                                            cell.SetCellFormula(tmCell.CellFormula);
                                        }
                                        else
                                        {
                                            //NewRow[j] = null;
                                        }
                                        break;
                                    case CellType.Numeric:
                                        if (DateUtil.IsCellDateFormatted(tmCell))
                                        {
                                            tmCell.CellStyle = dateStyle;
                                            cell.SetCellValue(tmCell.ToString());
                                        }
                                        else
                                        {
                                            cell.SetCellValue(tmCell.ToString());
                                        }
                                        break;
                                    case CellType.Boolean:
                                        cell.SetCellValue(tmCell.BooleanCellValue);
                                        break;
                                    case CellType.Error:
                                        cell.SetCellValue(tmCell.ErrorCellValue);
                                        break;
                                    default:
                                        cell.SetCellValue(tmCell.ToString());

                                        break;
                                }
                                break;
                            default:
                                cell.SetCellValue(tmCell.ToString());

                                break;
                        }
                        #endregion 填充单元格数据
                        //RowFormulaList.Add(cf);

                    }

                    //行结束 

                }
                sheet.ForceFormulaRecalculation = true;

            }
            catch (Exception ex)
            {
                MsgErr("复制模板数据出现异常", ex);
                throw ex;
            }
            finally
            {
            }

        }


        public void CloneSheet(string captionText, bool enableCaption)
        {
            ISheet sheet = NewWorkBook.CreateSheet("newsheet"); //创建一个sheet
            try
            {

                ICellStyle dateStyle = NewWorkBook.CreateCellStyle();
                IDataFormat format = NewWorkBook.CreateDataFormat();
                dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

                //数据行起始位置
                int DataRowStartIndex = 0;
                //标题行起始位置
                int ColumnRowStartIndex = 0;
                int rowIndex = 0;

                //第一行是跨行大标题
                if (enableCaption)
                {
                    IRow tmRow = TmSheet.GetRow(0);
                    ICell tmCell = tmRow.GetCell(0);


                    IRow row = sheet.CreateRow(0);
                    ICell cell = row.CreateCell(0);
                    cell.SetCellValue(captionText);
                    ICellStyle style = NewWorkBook.CreateCellStyle();
                    style.CloneStyleFrom(tmCell.CellStyle);
                    cell.CellStyle = style;


                    sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 5));


                    //因为有跨行标题，所以数据行要下移
                    DataRowStartIndex++;
                    ColumnRowStartIndex++;
                    rowIndex++;
                }

                //添加上列名
                //Excel当中起始行的索引是1，所以这里列标题行要加上1
                IRow TitelRow = sheet.CreateRow(ColumnRowStartIndex);
                IRow tmTitleRow = TmSheet.GetRow(ColumnRowStartIndex);



                for (int j = 0; j < tmTitleRow.Cells.Count; j++)
                {
                    // Range range = xSt.Range[excel.Cells[ColumnRowStartIndex, j + 1], excel.Cells[ColumnRowStartIndex, j + 1]];
                    //range.NumberFormatLocal = "@";

                    ICell cell = TitelRow.CreateCell(j);
                    ICell tmCell = tmTitleRow.GetCell(j);

                    ICellStyle style = NewWorkBook.CreateCellStyle();
                    style.CloneStyleFrom(tmCell.CellStyle);
                    cell.CellStyle = style;
                    cell.SetCellValue(tmCell.StringCellValue);
                }
                ColumnRowStartIndex++;


                //单元格的坐标，起始值为(1,1)，所以，要对DataTable的坐标进行转换
                //单元格的坐标，是Y轴在前
                int CellX, CellY;
                IRow tmDataRow = TmSheet.GetRow(ColumnRowStartIndex);
                List<ICellStyle> RowStyleList = new List<ICellStyle>();
                List<CellFormula> RowFormulaList = new List<CellFormula>();

                for (int j = 0; j < tmDataRow.Cells.Count; j++)
                {
                    ICell tmCell = tmDataRow.GetCell(j);
                    ICellStyle style = NewWorkBook.CreateCellStyle();
                    style.CloneStyleFrom(tmCell.CellStyle);
                    RowStyleList.Add(style);
                    //复制公式

                    CellFormula cf = new CellFormula();
                    if (tmCell.CellType == CellType.Formula && null != tmCell.CellFormula && tmCell.CellFormula != "")
                    {
                        cf.UsedFormula = true;
                        cf.FormulaString = tmCell.CellFormula;
                    }
                    RowFormulaList.Add(cf);

                }


                rowIndex++;
                // 格式化当前sheet，用于数据total计算
                sheet.ForceFormulaRecalculation = true;

            }
            catch (Exception ex)
            {
                MsgErr("复制模板数据出现异常", ex);
                throw ex;
            }
            finally
            {
            }

        }
        public bool WriteTm2File(string filePhysicalPath)
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

        public bool WriteFile(string filePhysicalPath)
        {
            if (null == NewWorkBook || 0 == NewWorkBook.NumberOfSheets)
            {
                MsgErr("生成的新文档为空，或者没有工作表");
                return false;
            }

            try
            {
                using (FileStream file = new FileStream(filePhysicalPath, FileMode.Create))
                {
                    NewWorkBook.Write(file);
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






    }
}
