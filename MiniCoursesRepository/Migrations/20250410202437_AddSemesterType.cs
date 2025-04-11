using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniCoursesRepository.Migrations
{
    /// <inheritdoc />
    public partial class AddSemesterType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Subject",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SemesterType",
                table: "Subject",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Subject",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Subject");

            migrationBuilder.DropColumn(
                name: "SemesterType",
                table: "Subject");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Subject");
        }
    }
}
