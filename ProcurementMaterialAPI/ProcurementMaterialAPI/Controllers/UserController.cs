using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcurementMaterialAPI.Context;
using ProcurementMaterialAPI.DTOs;
using ProcurementMaterialAPI.Enums;
using ProcurementMaterialAPI.ModelDB;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

			string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

			var newUser = new UserModel
			{
				UserName = username,
				UserShortName = userShortName,
				UserRole = userRole,
				PasswordHash = passwordHash
			};

			_context.User.Add(newUser);
			_context.SaveChanges();

			return Ok("User registered successfully.");
		}

		[HttpPost("auth")]
		public ActionResult<string> Auth(UserDTO userDto)
		{
			var user = _context.User.FirstOrDefault(x => x.UserName == userDto.Username);

			if (user == null)
			{
				return NotFound("User not found.");
			}

			// Проверка пароля
			bool isPasswordValid = BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash);
			if (!isPasswordValid)
			{
				return Unauthorized("Invalid password.");
			}

			// Создание JWT токена
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("SECRET_KEY") ?? "YourSuperSecretKey123456789101112131415");
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.UserName),
					new Claim(ClaimTypes.Role, user.UserRole.ToString())
				}),
				Expires = DateTime.UtcNow.AddHours(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			var tokenString = tokenHandler.WriteToken(token);

			// Возвращаем токен клиенту
			return Ok(new { Token = tokenString });
		}
	}
}
