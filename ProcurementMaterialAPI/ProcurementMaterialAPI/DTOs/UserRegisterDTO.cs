using ProcurementMaterialAPI.Enums;

namespace ProcurementMaterialAPI.DTOs
{
	public class UserRegisterDTO
	{
		public string Username { get; set; }
		public string UserShortName { get; set; }
		public UserRole UserRole { get; set; }
		public string Password { get; set; }
	}
}
