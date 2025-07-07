using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroManagement.Persistence.Migrations.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class TimeSessionUniqueProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "TimeSessionsTable",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TimeSessionsTable_ProjectId",
                table: "TimeSessionsTable",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSessionsTable_ProjectsTable_ProjectId",
                table: "TimeSessionsTable",
                column: "ProjectId",
                principalTable: "ProjectsTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.Sql(@"
                UPDATE ""TimeSessionsTable"" ts
                SET ""ProjectId"" = sub.""ProjectsId""
                FROM (
                    SELECT DISTINCT ON (""TimeSessionsId"") ""TimeSessionsId"", ""ProjectsId""
                    FROM ""ProjectEntityTimeSessionEntity""
                    ORDER BY ""TimeSessionsId"", ""ProjectsId""
                ) sub
                WHERE ts.""Id"" = sub.""TimeSessionsId"";
            ");

            migrationBuilder.DropTable(
                name: "ProjectEntityTimeSessionEntity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSessionsTable_ProjectsTable_ProjectId",
                table: "TimeSessionsTable");

            migrationBuilder.DropIndex(
                name: "IX_TimeSessionsTable_ProjectId",
                table: "TimeSessionsTable");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "TimeSessionsTable");

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
    }
}
