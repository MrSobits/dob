using OfficeOpenXml;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Bars.GkhExcel.Impl
{
    public class GkhOdsProvider : IGkhExcelProvider
    {
        ExcelPackage ExcelPackage;

        #region Open/Close

        public void Open(Stream stream)
        {
            ExcelPackage = new ExcelPackage(stream);
        }

        public void SaveAs(int workbook, Stream outputStream)
        {
            ExcelPackage.SaveAs(outputStream)
        }

        public void Dispose()
        {
            ExcelPackage.Dispose();
        }
        #endregion

        #region Worksheet

        public int GetWorkSheetsCount(int indexWorkbook)
        {
            return ExcelPackage.Workbook.Worksheets.Count;
        }

        #endregion

        #region Rows

        public GkhExcelCell[] GetRow(int indexWorkbook, int indexWorksheets, int indexRow)
        {
            throw new System.NotImplementedException();
        }

        public List<GkhExcelCell[]> GetRows(int indexWorkbook, int indexWorksheet, WorksheetVisibility worksheetVisibility = WorksheetVisibility.Visible)
        {
            throw new System.NotImplementedException();
        }

        public int GetRowsCount(int indexWorkbook, int indexWorksheets)
        {
            var myWorksheet = ExcelPackage.Workbook.Worksheets[indexWorksheets];
            return myWorksheet.Dimension.End.Row;
        }

        #endregion

        #region Cells

        public GkhExcelCell GetCell(int indexWorkbook, int indexWorksheet, int indexRow, int indexCell)
        {
            return new GkhExcelCell(ExcelPackage.Workbook.Worksheets[indexWorksheet].Cells[indexRow, indexCell]);
        }

        #endregion

        public void AddFormula(int workbook, int worksheet, string range, string formula)
        {
            ExcelPackage.Workbook.Worksheets[worksheet].;
        }

        public void AddTableBorder(int workbook, int worksheet, string range)
        {
            throw new System.NotImplementedException();
        }

        public void ImportDataTable(int workbook, int worksheet, int firstRow, int firstColumn, DataTable data)
        {
            throw new System.NotImplementedException();
        }

        public bool IsEmpty(int workbook, int worksheet)
        {
            throw new System.NotImplementedException();
        }

        public void ProtectSheet(int workbook, int worksheet, string password)
        {
            throw new System.NotImplementedException();
        }

        public void UnlockCell(int workbook, int worksheet, int row, int cell)
        {
            throw new System.NotImplementedException();
        }

        public void UnlockCellRange(int workbook, int worksheet, string range)
        {
            throw new System.NotImplementedException();
        }

        public void UseVersionXlsx()
        {

        }
    }
}
