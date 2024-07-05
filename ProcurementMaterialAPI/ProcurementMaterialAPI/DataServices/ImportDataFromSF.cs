using System.Reflection;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Resources;
using ProcurementMaterialAPI.ModelDB;
using ProcurementMaterialAPI.Context;
using System.Collections;
using Microsoft.EntityFrameworkCore;

namespace ProcurementMaterialAPI.DataServices
{
	public class ImportDataFromSF
	{
		private readonly MaterialDbContext _context;
		private readonly string _filePath;
		private readonly ResourceManager _resourceSapSF;

		public ImportDataFromSF(MaterialDbContext context, string filePath)
		{
			_context = context;
			_filePath = filePath;
			_resourceSapSF = new ResourceManager("ProcurementMaterialAPI.Resources.ResourceSapSF", Assembly.GetExecutingAssembly());
		}

        public void ReadExcelFile()
        {

            using (FileStream fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(fs);
                ISheet sheet = workbook.GetSheetAt(0);

                // Чтение заголовков
                IRow headerRow = sheet.GetRow(0);
                var headers = new Dictionary<int, string>();
                for (int colIdx = 0; colIdx < headerRow.LastCellNum; colIdx++)
                {
                    ICell cell = headerRow.GetCell(colIdx);
                    if (cell != null)
                    {
                        headers[colIdx] = cell.ToString();
                    }
                }

                // Чтение данных
                for (int rowIdx = 1; rowIdx <= sheet.LastRowNum; rowIdx++)
                {
                    IRow row = sheet.GetRow(rowIdx);
                    if (row == null) continue;

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
                    if (isEmptyRow) continue;

                    var entity = new ModelDok_SF();

                    for (int colIdx = 0; colIdx < row.LastCellNum; colIdx++)
                    {
                        ICell cell = row.GetCell(colIdx);
                        if (cell == null) continue;

                        string header = headers[colIdx];
                        string cellValue = cell.ToString();

                        string propertyName = _resourceSapSF.GetString(header);
                        
                        if (!string.IsNullOrEmpty(propertyName))
                        {
                            SetPropertyValue(entity, propertyName, cellValue);
                        }
                    }

					bool exists = _context.Dok_SF
						.Any(e =>
							e.buisnes_number == entity.buisnes_number &&
							e.buisnes_consignee == entity.buisnes_consignee &&
							e.fact_number == entity.fact_number &&
							e.fact_pos == entity.fact_pos &&
							e.material == entity.material &&
							e.material_name == entity.material_name &&
							e.material_type == entity.material_type &&
							e.date_budat == entity.date_budat &&
							e.material_group == entity.material_group &&
							e.material_group_name == entity.material_group_name &&
							e.EI == entity.EI &&
							e.INN == entity.INN &&
							e.quan == entity.quan &&
							e.cost == entity.cost &&
							e.Direction == entity.Direction &&
							e.Department == entity.Department &&
							e.normalization == entity.normalization
						);

					if (exists)
						_context.Dok_SF.Add(entity);

                    if (rowIdx % 1000 == 0)
                        _context.SaveChanges();
                }
				_context.SaveChanges();
			}
        }

        private void SetPropertyValue(ModelDok_SF entity, string propertyName, string value)
		{
			var property = typeof(ModelDok_SF).GetProperty(propertyName);
			if (property != null)
			{
				if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
				{
					if (int.TryParse(value, out int intValue))
					{
						property.SetValue(entity, intValue);
					}
				}
				else if (property.PropertyType == typeof(float))
				{
					if (float.TryParse(value, out float floatValue))
					{
						property.SetValue(entity, floatValue);
					}
				}
				else if (property.PropertyType == typeof(DateOnly))
				{
					if (DateOnly.TryParse(value, out DateOnly dateValue))
					{
						property.SetValue(entity, dateValue);
					}
				}
				else if (property.PropertyType == typeof(double))
				{
					if (double.TryParse(value, out double doubleValue))
					{
						property.SetValue(entity, doubleValue);
					}
				}
				else
				{
					property.SetValue(entity, value);
				}
			}
		}
	}
}