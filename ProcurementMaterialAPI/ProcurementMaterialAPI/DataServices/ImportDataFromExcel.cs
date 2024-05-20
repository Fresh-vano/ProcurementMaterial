using System;
using System.Collections.Generic;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel; // Используется для .xlsx файлов
// using NPOI.HSSF.UserModel; // Используется для .xls файлов

public class ImportDataFromExcel
{
    private readonly string _filePath;

    public ImportDataFromExcel(string filePath)
    {
        _filePath = filePath;
    }

    public List<List<string>> ReadExcelFile()
    {
        var data = new List<List<string>>();

        using (FileStream fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
        {
            IWorkbook workbook = new XSSFWorkbook(fs); // для .xlsx
            // IWorkbook workbook = new HSSFWorkbook(fs); // для .xls

            ISheet sheet = workbook.GetSheetAt(0);

            for (int rowIdx = 0; rowIdx <= sheet.LastRowNum; rowIdx++)
            {
                IRow row = sheet.GetRow(rowIdx);

                bool isEmptyRow = true;
                for (int colIdx = 0; colIdx < row.LastCellNum; colIdx++)
                {
                    ICell cell = row.GetCell(colIdx);
                    if (cell != null && cell.CellType != CellType.Blank)
                    {
                        isEmptyRow = false;
                        break;
                    }
                }
                if (isEmptyRow) break;

                var rowData = new List<string>();

                for (int colIdx = 0; colIdx < row.LastCellNum; colIdx++)
                {
                    ICell cell = row.GetCell(colIdx);
                    if (cell != null)
                    {
                        rowData.Add(cell.ToString());
                    }
                    else
                    {
                        rowData.Add(string.Empty);
                    }
                }

                data.Add(rowData);
            }
        }

        return data;
    }


}
