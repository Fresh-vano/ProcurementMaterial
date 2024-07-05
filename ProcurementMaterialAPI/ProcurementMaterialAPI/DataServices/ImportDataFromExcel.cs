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
	public class ImportDataFromExcel
	{
		private readonly MaterialDbContext _context;
		private readonly string _filePath;
		private readonly ResourceManager _resourceKis;
		private readonly ResourceManager _resourceSap;

		public ImportDataFromExcel(MaterialDbContext context, string filePath)
		{
			_context = context;
			_filePath = filePath;
			_resourceKis = new ResourceManager("ProcurementMaterialAPI.Resources.ResourceKis", Assembly.GetExecutingAssembly());
			_resourceSap = new ResourceManager("ProcurementMaterialAPI.Resources.ResourceSap", Assembly.GetExecutingAssembly());
		}

		public Enums.BE? ReadExcelFile(DateOnly date)
		{
			Enums.BE? fileType = null;

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
				// Определение типа файла по заголовкам
				if (headers.Values.Any(header => !string.IsNullOrEmpty(_resourceKis.GetString(header))))
				{
					fileType = Enums.BE.BUKRS_2000;
				}
				else if (headers.Values.Any(header => !string.IsNullOrEmpty(_resourceSap.GetString(header))))
				{
					fileType = Enums.BE.BUKRS_1300;
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

					var entity = new InformationSystemsMatch();

					for (int colIdx = 1; colIdx < row.LastCellNum; colIdx++)
					{
						ICell cell = row.GetCell(colIdx);
						if (cell == null) continue;

						string header = headers[colIdx];
						string cellValue = cell.ToString();

						string propertyName = null;
						if (fileType == Enums.BE.BUKRS_2000)
						{
							propertyName = _resourceKis.GetString(header);
						}
						else if (fileType == Enums.BE.BUKRS_1300)
						{
							propertyName = _resourceSap.GetString(header);
						}

						if (!string.IsNullOrEmpty(propertyName))
						{
							SetPropertyValue(entity, propertyName, cellValue);
						}
					}

					entity.Date = date;
					entity.BE = fileType;

					bool exists = _context.InformationSystemsMatch
						.Any(e =>
							e.Date == entity.Date &&
							e.BE == entity.BE &&
							e.MaterialName == entity.MaterialName &&
							e.MaterialNom == entity.MaterialNom &&
							e.BEI == entity.BEI &&
							e.DepartmentName == entity.DepartmentName &&
							e.GroupMaterialCode == entity.GroupMaterialCode &&
							e.GroupMaterialName == entity.GroupMaterialName &&
							e.SubGroupMaterialCode == entity.SubGroupMaterialCode &&
							e.SubGroupMaterialName == entity.SubGroupMaterialName &&
							e.CountOutgo == entity.CountOutgo &&
							e.SumOutgo == entity.SumOutgo &&
							e.CountEnd == entity.CountEnd &&
							e.SumEnd == entity.SumEnd &&
							e.DepartmentCode == entity.DepartmentCode
						);

					if (exists)
						_context.InformationSystemsMatch.Add(entity);

					if (rowIdx % 1000 == 0)
						_context.SaveChanges();
				}
			}

			return fileType;
		}

		private void SetPropertyValue(InformationSystemsMatch entity, string propertyName, string value)
		{
			var property = typeof(InformationSystemsMatch).GetProperty(propertyName);
			if (property != null)
			{
				if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
				{
					if (int.TryParse(value, out int intValue))
					{
						property.SetValue(entity, intValue);
					}
				}
				else if (property.PropertyType == typeof(float) || property.PropertyType == typeof(float?))
				{
					if (float.TryParse(value, out float floatValue))
					{
						property.SetValue(entity, floatValue);
					}
				}
				else if (property.PropertyType == typeof(double) || property.PropertyType == typeof(double?))
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