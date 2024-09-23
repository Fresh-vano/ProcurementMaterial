using Microsoft.EntityFrameworkCore;
using ProcurementMaterialAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProcurementMaterialAPI.ModelDB
{
	public class UserModel
	{
		[Key]
		public string UserName { get; set; }

		public string UserShortName { get; set; }

		/// <summary>
		/// Role (1 - manager, 2 - purchaser, 3 - report_group)
		/// </summary>
		public UserRole UserRole { get; set; }

		public string PasswordHash { get; set; } 
	}
}
