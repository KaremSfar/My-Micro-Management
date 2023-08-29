using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroManagement.Persistence.SQLite.MigrationsApplier.Migrations
{
    /// <inheritdoc />
    public partial class UserIDToEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "TimeSessionsTable",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "NOCASE");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ProjectsTable",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "NOCASE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TimeSessionsTable");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProjectsTable");
        }
    }
}
