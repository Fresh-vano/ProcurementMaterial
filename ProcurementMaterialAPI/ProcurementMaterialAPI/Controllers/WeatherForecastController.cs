using Microsoft.AspNetCore.Mvc;
using NPOI.HPSF;

namespace ProcurementMaterialAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly MaterialDbContext _context;

		public WeatherForecastController(MaterialDbContext context)
		{
			_context = context;
		}

		[HttpGet(Name = "GetWeatherForecast")]
		public IEnumerable<WeatherForecast> Get()
		{
			JsonResult json = new JsonResult("List");
			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
				TemperatureC = Random.Shared.Next(-20, 55),
				Summary = Summaries[Random.Shared.Next(Summaries.Length)]
			})
			.ToArray();
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
