﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProcurementMaterialAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "User",
                newName: "PasswordHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "User",
                newName: "Password");
        }
    }
}
