using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcurementMaterialAPI.Context;
using ProcurementMaterialAPI.DataServices;
using ProcurementMaterialAPI.Enums;
using ProcurementMaterialAPI.ModelDB;

namespace ProcurementMaterialAPI.Controllers
{
	[Authorize(Roles = "Report_group")]
	[ApiController]
	[Route("[controller]")]
	public class FileController : ControllerBase
	{

		private readonly MaterialDbContext _context;

		public FileController(MaterialDbContext context)
		{
			_context = context;
		}

		[HttpPost]
		public async Task<ActionResult> UploadFile([FromForm] ICollection<IFormFile> files, string dateString)
		{
			try
			{
				if (files == null || files.Count == 0)
				{
					return BadRequest("No file uploaded.");
				}

				if (!DateOnly.TryParse(dateString, out DateOnly date))
				{
					return BadRequest("Invalid date format.");
				}

				var tempFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
				Directory.CreateDirectory(tempFolder);

				foreach (var file in files)
				{
					if (file == null || file.Length == 0)
					{
						return BadRequest("Empty file.");
					}

					if (!file.ContentType.Equals("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", StringComparison.OrdinalIgnoreCase))
					{
						return BadRequest("Only .xlsx files are allowed.");
					}

					var fileName = Path.GetRandomFileName() + ".xlsx";
					var filePath = Path.Combine(tempFolder, fileName);

					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await file.CopyToAsync(stream);
					}

					try
					{
						var excelReader = new ImportDataFromExcel(_context, filePath);
						var fileType = excelReader.ReadExcelFile(date);
					}
					catch (Exception ex)
					{
						Directory.Delete(tempFolder, true);
						return Problem(ex.Message);
					}
				}

				Directory.Delete(tempFolder, true);

				return Ok("File(s) uploaded and processed successfully.");
			}
			catch (Exception ex)
			{
				return Problem(ex.Message);
			}
		}

		[HttpPost("sf")]
		public async Task<ActionResult> UploadSFFile([FromForm] IFormFileCollection files)
		{
			try
			{
				if (files == null || files.Count == 0)
				{
					return BadRequest("No file uploaded.");
				}

				var tempFolder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
				Directory.CreateDirectory(tempFolder);

				foreach (var file in files)
				{
					if (file == null || file.Length == 0)
					{
						return BadRequest("Empty file.");
					}

					if (!file.ContentType.Equals("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", StringComparison.OrdinalIgnoreCase))
					{
						return BadRequest("Only .xlsx files are allowed.");
					}

					var fileName = Path.GetRandomFileName() + ".xlsx";
					var filePath = Path.Combine(tempFolder, fileName);

					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await file.CopyToAsync(stream);
					}

					try
					{
						var excelReader = new ImportDataFromSF(_context, filePath);
						excelReader.ReadExcelFile(); 
					}
					catch (Exception ex)
					{
						Directory.Delete(tempFolder, true);
						return Problem(ex.Message);
					}
				}

				Directory.Delete(tempFolder, true);

				return Ok("File(s) uploaded and processed successfully.");
			}
			catch (Exception ex)
			{
				return Problem(ex.Message);
			}
		}
	}
}
