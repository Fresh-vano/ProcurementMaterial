using Microsoft.AspNetCore.Mvc;
using ProcurementMaterialAPI.Context;
using ProcurementMaterialAPI.DataServices;
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

		[HttpPost]
		public async Task<ActionResult<List<List<string>>>> UploadFile(IFormFile file)
		{
			if (file == null || file.Length == 0)
			{
				return BadRequest("No file uploaded.");
			}

			if (!Directory.Exists("Temp"))
				Directory.CreateDirectory("Temp");

			string filePath = Path.Combine("Temp", file.FileName);

			// Сохранение файла на сервере
			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			// Обработка файла и чтение данных из Excel
			ImportDataFromExcel excelReader = new ImportDataFromExcel(filePath);
			var result = excelReader.ReadExcelFile(_context);

			System.IO.File.Delete(filePath);

			return Ok(result);
		}
	}
}
