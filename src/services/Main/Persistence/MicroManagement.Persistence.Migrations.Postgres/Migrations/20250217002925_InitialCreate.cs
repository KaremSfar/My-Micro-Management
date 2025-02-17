using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroManagement.Persistence.Migrations.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectsTable",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectsTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeSessionsTable",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSessionsTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectEntityTimeSessionEntity",
                columns: table => new
                {
                    ProjectsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TimeSessionsId = table.Column<Guid>(type: "uuid", nullable: false)
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
                        name: "FK_ProjectEntityTimeSessionEntity_TimeSessionsTable_TimeSessio~",
                        column: x => x.TimeSessionsId,
                        principalTable: "TimeSessionsTable",
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
                name: "ProjectsTable");

            migrationBuilder.DropTable(
                name: "TimeSessionsTable");
        }
    }
}
