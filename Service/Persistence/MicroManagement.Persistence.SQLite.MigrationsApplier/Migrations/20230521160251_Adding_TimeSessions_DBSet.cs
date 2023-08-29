using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroManagement.Persistence.SQLite.MigrationsApplier.Migrations
{
    /// <inheritdoc />
    public partial class Adding_TimeSessions_DBSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectEntityTimeSessionEntity_TimeSessionEntity_TimeSessionsId",
                table: "ProjectEntityTimeSessionEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeSessionEntity",
                table: "TimeSessionEntity");

            migrationBuilder.RenameTable(
                name: "TimeSessionEntity",
                newName: "TimeSessionsTable");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeSessionsTable",
                table: "TimeSessionsTable",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectEntityTimeSessionEntity_TimeSessionsTable_TimeSessionsId",
                table: "ProjectEntityTimeSessionEntity",
                column: "TimeSessionsId",
                principalTable: "TimeSessionsTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectEntityTimeSessionEntity_TimeSessionsTable_TimeSessionsId",
                table: "ProjectEntityTimeSessionEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeSessionsTable",
                table: "TimeSessionsTable");

            migrationBuilder.RenameTable(
                name: "TimeSessionsTable",
                newName: "TimeSessionEntity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeSessionEntity",
                table: "TimeSessionEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectEntityTimeSessionEntity_TimeSessionEntity_TimeSessionsId",
                table: "ProjectEntityTimeSessionEntity",
                column: "TimeSessionsId",
                principalTable: "TimeSessionEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
