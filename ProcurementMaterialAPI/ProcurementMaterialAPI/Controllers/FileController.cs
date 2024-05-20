using Microsoft.AspNetCore.Mvc;
using ProcurementMaterialAPI.Context;
using ProcurementMaterialAPI.ModelDB;

namespace ProcurementMaterialAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class FileController : ControllerBase
	{

		private readonly MaterialDbContext _context;

		public FileController(MaterialDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public ActionResult<List<InformationSystemsMatch>> Get()
		{
			return _context.InformationSystemsMatch.ToList();
		}

        [HttpPost(Name = "GetData")]
        public ActionResult<List<List<string>>> GetData()
        {
            string filePath = "DataServices/СинТЗ-12.2023.xlsx"; // Укажите правильный путь к вашему Excel файлу
            ImportDataFromExcel excelReader = new ImportDataFromExcel(filePath);
            List<List<string>> excelData = excelReader.ReadExcelFile();
			JsonResult json = new JsonResult(excelData);
            // Возвращаем данные в формате JSON
            return Ok(excelData);
        }
    }
}
