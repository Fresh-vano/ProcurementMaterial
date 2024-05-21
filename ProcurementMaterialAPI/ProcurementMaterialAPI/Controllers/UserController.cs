using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcurementMaterialAPI.Context;
using ProcurementMaterialAPI.Enums;
using ProcurementMaterialAPI.ModelDB;

namespace ProcurementMaterialAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		private readonly MaterialDbContext _context;

		public UserController(MaterialDbContext context)
		{
			_context = context;
		}

		[HttpPost("register")]
		public ActionResult<string> Register(string username, string userShortName, UserRole userRole, string password)
		{
			var existingUser = _context.User.FirstOrDefault(x => x.UserName == username);
			if (existingUser != null)
			{
				return BadRequest("User already exists.");
			}

			var newUser = new UserModel
			{
				UserName = username,
				UserShortName = userShortName,
				UserRole = userRole,
				Password = password
			};

			_context.User.Add(newUser);
			_context.SaveChanges();

			return Ok("User registered successfully.");
		}

		[HttpPost("auth")]
		public ActionResult<string> Auth(string username, string password)
		{
			var user = _context.User.FirstOrDefault(x => x.UserName == username);

			if (user == null)
			{
				return NotFound();
			}

			if (password != user.Password)
			{
				return BadRequest();
			}

			return Ok(user.UserRole.ToString().ToLower());
		}
	}
}
