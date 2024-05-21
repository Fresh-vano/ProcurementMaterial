using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProcurementMaterialAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "buisnes_number",
                table: "Dok_SF",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "buisnes_number",
                table: "Dok_SF",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
