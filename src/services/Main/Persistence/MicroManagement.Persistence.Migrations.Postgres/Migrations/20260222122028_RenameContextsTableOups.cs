using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroManagement.Persistence.Migrations.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class RenameContextsTableOups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectsTable_Contexts_ContextId",
                table: "ProjectsTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contexts",
                table: "Contexts");

            migrationBuilder.RenameTable(
                name: "Contexts",
                newName: "ContextsTable");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContextsTable",
                table: "ContextsTable",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectsTable_ContextsTable_ContextId",
                table: "ProjectsTable",
                column: "ContextId",
                principalTable: "ContextsTable",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectsTable_ContextsTable_ContextId",
                table: "ProjectsTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContextsTable",
                table: "ContextsTable");

            migrationBuilder.RenameTable(
                name: "ContextsTable",
                newName: "Contexts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contexts",
                table: "Contexts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectsTable_Contexts_ContextId",
                table: "ProjectsTable",
                column: "ContextId",
                principalTable: "Contexts",
                principalColumn: "Id");
        }
    }
}
