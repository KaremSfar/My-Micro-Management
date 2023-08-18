using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroManagement.Persistence.SQLite.MigrationsApplier.Migrations
{
    /// <inheritdoc />
    public partial class Adding_TimeSessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeSessionEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSessionEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectEntityTimeSessionEntity",
                columns: table => new
                {
                    ProjectsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TimeSessionsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectEntityTimeSessionEntity", x => new { x.ProjectsId, x.TimeSessionsId });
                    table.ForeignKey(
                        name: "FK_ProjectEntityTimeSessionEntity_ProjectsTable_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "ProjectsTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectEntityTimeSessionEntity_TimeSessionEntity_TimeSessionsId",
                        column: x => x.TimeSessionsId,
                        principalTable: "TimeSessionEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectEntityTimeSessionEntity_TimeSessionsId",
                table: "ProjectEntityTimeSessionEntity",
                column: "TimeSessionsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectEntityTimeSessionEntity");

            migrationBuilder.DropTable(
                name: "TimeSessionEntity");
        }
    }
}
