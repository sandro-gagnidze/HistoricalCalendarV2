using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication6.Migrations
{
    /// <inheritdoc />
    public partial class CreateYearInfoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_YearDescriptions",
                table: "YearDescriptions");

            migrationBuilder.RenameTable(
                name: "YearDescriptions",
                newName: "YearInfos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_YearInfos",
                table: "YearInfos",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_YearInfos",
                table: "YearInfos");

            migrationBuilder.RenameTable(
                name: "YearInfos",
                newName: "YearDescriptions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_YearDescriptions",
                table: "YearDescriptions",
                column: "Id");
        }
    }
}
