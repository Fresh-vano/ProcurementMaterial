using System.Reflection;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Resources;
using ProcurementMaterialAPI.ModelDB;
using ProcurementMaterialAPI.Context;
using System.Collections;

namespace ProcurementMaterialAPI.DataServices
{
	public class ImportDataFromExcel
	{
		private readonly string _filePath;
		private readonly ResourceManager _resourceKis;
		private readonly ResourceManager _resourceSap;

		public ImportDataFromExcel(string filePath)
		{
			_filePath = filePath;
			_resourceKis = new ResourceManager("ProcurementMaterialAPI.Resources.ResourceKis", Assembly.GetExecutingAssembly());
			_resourceSap = new ResourceManager("ProcurementMaterialAPI.Resources.ResourceSap", Assembly.GetExecutingAssembly());
		}

		public (string fileType, List<InformationSystemsMatch> data) ReadExcelFile(MaterialDbContext context)
		{
			var data = new List<InformationSystemsMatch>();
			string fileType = "Unknown";

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
					fileType = "KIS";
				}
				else if (headers.Values.Any(header => !string.IsNullOrEmpty(_resourceSap.GetString(header))))
				{
					fileType = "SAP";
				}

				// Чтение данных
				for (int rowIdx = 1; rowIdx <= sheet.LastRowNum; rowIdx++)
				{
					IRow row = sheet.GetRow(rowIdx);
					if (row == null) continue;

					var entity = new InformationSystemsMatch();

					for (int colIdx = 1; colIdx < row.LastCellNum; colIdx++)
					{
						ICell cell = row.GetCell(colIdx);
						if (cell == null) continue;

						string header = headers[colIdx];
						string cellValue = cell.ToString();

						string propertyName = null;
						if (fileType == "KIS")
						{
							propertyName = _resourceKis.GetString(header);
						}
						else if (fileType == "SAP")
						{
							propertyName = _resourceSap.GetString(header);
						}

						if (!string.IsNullOrEmpty(propertyName))
						{
							SetPropertyValue(entity, propertyName, cellValue);
						}
					}

					context.InformationSystemsMatch.Add(entity);
					context.SaveChanges();
					data.Add(entity);
				}
			}

			return (fileType, data);
		}

		private void SetPropertyValue(InformationSystemsMatch entity, string propertyName, string value)
		{
			var property = typeof(InformationSystemsMatch).GetProperty(propertyName);
			if (property != null)
			{
				if (property.PropertyType == typeof(int))
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