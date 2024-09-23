using Microsoft.AspNetCore.Mvc;

namespace ProcurementMaterialAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class HealthController : ControllerBase
	{
		[HttpGet]
		public ActionResult<string> Get()
		{
			return Ok("Healthy");
		}
	}
}
