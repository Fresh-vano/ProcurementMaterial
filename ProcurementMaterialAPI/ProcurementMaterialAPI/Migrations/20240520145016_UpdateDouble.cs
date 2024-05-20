using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProcurementMaterialAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDouble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dok_SF",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    buisnes_number = table.Column<int>(type: "int", nullable: false),
                    buisnes_consignee = table.Column<int>(type: "int", nullable: false),
                    fact_number = table.Column<int>(type: "int", nullable: false),
                    fact_pos = table.Column<int>(type: "int", nullable: false),
                    material = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    material_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    material_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date_budat = table.Column<DateOnly>(type: "date", nullable: false),
                    material_group = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    material_group_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EI = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    INN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    quan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Direction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    normalization = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dok_SF", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "InformationSystemsMatch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaterialNom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BEI = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupMaterialCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupMaterialName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubGroupMaterialCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubGroupMaterialName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountOutgo = table.Column<int>(type: "int", nullable: false),
                    SumOutgo = table.Column<float>(type: "real", nullable: false),
                    CountEnd = table.Column<int>(type: "int", nullable: false),
                    SumEnd = table.Column<float>(type: "real", nullable: false),
                    DepartmentCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformationSystemsMatch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserShortName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserRole = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dok_SF");

            migrationBuilder.DropTable(
                name: "InformationSystemsMatch");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
