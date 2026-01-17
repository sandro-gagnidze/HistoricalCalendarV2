using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication6.Migrations
{
    /// <inheritdoc />
    public partial class Localization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "DailyImages");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "DailyImages");

            migrationBuilder.CreateTable(
                name: "DailyImageLocalizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DailyImageId = table.Column<int>(type: "int", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyImageLocalizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyImageLocalizations_DailyImages_DailyImageId",
                        column: x => x.DailyImageId,
                        principalTable: "DailyImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyImageLocalizations_DailyImageId",
                table: "DailyImageLocalizations",
                column: "DailyImageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyImageLocalizations");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DailyImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "DailyImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
