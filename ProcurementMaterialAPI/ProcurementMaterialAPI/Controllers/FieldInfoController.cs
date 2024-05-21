using Microsoft.AspNetCore.Mvc;
using ProcurementMaterialAPI.DTOs;
using ProcurementMaterialAPI.ModelDB;
using System.Reflection;
using System.Collections.Generic;

namespace ProcurementMaterialAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class FieldInfoController : ControllerBase
	{
		private static readonly Dictionary<string, string> TranslationDictionary = new Dictionary<string, string>
		{
            // ModelDok_SF translations
            { "buisnes_number", "Бизнес-номер" },
			{ "buisnes_consignee", "Грузополучатель" },
			{ "fact_number", "Номер счета-фактуры" },
			{ "fact_pos", "Номер позиции СФ" },
			{ "material", "Материал" },
			{ "material_name", "Наименование материала" },
			{ "material_type", "Вид материала" },
			{ "date_budat", "Дата проводки" },
			{ "material_group", "Группа материала" },
			{ "material_group_name", "Наименование группы материала" },
			{ "EI", "ЕИ" },
			{ "INN", "Поставщик ИНН" },
			{ "quan", "Количество" },
			{ "cost", "Стоимость" },
			{ "Direction", "Направление" },
			{ "Department", "Отдел" },
			{ "normalization", "Поставщик нормализованный" },
            
            // InformationSystemsMatch translations
            { "MaterialName", "Материал" },
			{ "MaterialNom", "Номенклатура|Наименование" },
			{ "BEI", "Номенклатура|ЕИ" },
			{ "DepartmentName", "Цех" },
			{ "GroupMaterialCode", "Вид материала" },
			{ "GroupMaterialName", "Наимен. ВидаМатер" },
			{ "SubGroupMaterialCode", "Группа материалов" },
			{ "SubGroupMaterialName", "НаименГруппыМтр" },
			{ "CountOutgo", "КолОтпБезВнутр" },
			{ "SumOutgo", "СтоимОтпБезВнутр" },
			{ "CountEnd", "Остаток на конец" },
			{ "SumEnd", "Стоимость на конец" },
			{ "Date", "Дата загрузки" },
			{ "BE", "Тип предприятия" },
			{ "DepartmentCode", "Код Цеха КИС" }
		};

		[HttpGet("info")]
		public ActionResult<List<string>> GetInformationSystemsMatchFields()
		{
			var result = new List<string>();

			var properties = typeof(InformationSystemsMatch).GetProperties(BindingFlags.Public | BindingFlags.Instance);

			foreach (var property in properties)
			{
				if ((property.PropertyType == typeof(int) || property.PropertyType == typeof(float) ||
					property.PropertyType == typeof(double) || property.PropertyType == typeof(DateOnly) ||
					property.PropertyType == typeof(int?) || property.PropertyType == typeof(float?) ||
					property.PropertyType == typeof(double?) || property.PropertyType == typeof(DateOnly?)) && property.Name != "Id")
				{
					result.Add(TranslationDictionary.ContainsKey(property.Name) ? TranslationDictionary[property.Name] : property.Name);
				}
			}

			return Ok(result);
		}

		[HttpGet("sf")]
		public ActionResult<List<string>> GetModelDok_SFFields()
		{
			var result = new List<string>();

			var properties = typeof(ModelDok_SF).GetProperties(BindingFlags.Public | BindingFlags.Instance);

			foreach (var property in properties)
			{
				if ((property.PropertyType == typeof(int) || property.PropertyType == typeof(float) ||
					property.PropertyType == typeof(double) || property.PropertyType == typeof(DateOnly) ||
					property.PropertyType == typeof(int?) || property.PropertyType == typeof(float?) ||
					property.PropertyType == typeof(double?) || property.PropertyType == typeof(DateOnly?)) && property.Name != "id")
				{
					result.Add(TranslationDictionary.ContainsKey(property.Name) ? TranslationDictionary[property.Name] : property.Name);
				}
			}

			return Ok(result);
		}
	}
}
