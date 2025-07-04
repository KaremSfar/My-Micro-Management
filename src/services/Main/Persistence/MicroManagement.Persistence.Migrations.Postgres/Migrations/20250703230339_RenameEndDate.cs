using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroManagement.Persistence.Migrations.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class RenameEndDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "TimeSessionsTable",
                newName: "EndTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "TimeSessionsTable",
                newName: "EndDate");
        }
    }
}
