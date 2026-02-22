using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroManagement.Persistence.Migrations.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectsContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ContextId",
                table: "ProjectsTable",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Contexts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contexts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectsTable_ContextId",
                table: "ProjectsTable",
                column: "ContextId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectsTable_Contexts_ContextId",
                table: "ProjectsTable",
                column: "ContextId",
                principalTable: "Contexts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectsTable_Contexts_ContextId",
                table: "ProjectsTable");

            migrationBuilder.DropTable(
                name: "Contexts");

            migrationBuilder.DropIndex(
                name: "IX_ProjectsTable_ContextId",
                table: "ProjectsTable");

            migrationBuilder.DropColumn(
                name: "ContextId",
                table: "ProjectsTable");
        }
    }
}
