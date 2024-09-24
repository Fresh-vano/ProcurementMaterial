using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProcurementMaterialAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminUser : Migration
    {
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			string userName = "istia";

			string passwordHashString = "$2a$11$5rITyxSFKTxNTXJ5qy5GLe2p8CioIqv6iK.VtcjH10ArzrxVrK6OK";

			migrationBuilder.Sql($@"
            INSERT INTO [dbo].[User] (UserName, UserShortName, PasswordHash, UserRole)
            VALUES ('{userName}', '{userName}', '{passwordHashString}', 4)
        ");
		}
			
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql($@"DELETE FROM Users WHERE UserName = 'istia'");
		}
	}
}
