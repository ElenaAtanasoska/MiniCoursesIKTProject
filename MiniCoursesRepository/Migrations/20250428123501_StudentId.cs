using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniCoursesRepository.Migrations
{
    /// <inheritdoc />
    public partial class StudentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "StudentSubjects",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "StudentSubjects");
        }
    }
}
