using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniCoursesRepository.Migrations
{
    /// <inheritdoc />
    public partial class AddSubjectsToApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SemesterApplicationId",
                table: "StudentSubjects",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentSubjects_SemesterApplicationId",
                table: "StudentSubjects",
                column: "SemesterApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubjects_SemesterApplications_SemesterApplicationId",
                table: "StudentSubjects",
                column: "SemesterApplicationId",
                principalTable: "SemesterApplications",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubjects_SemesterApplications_SemesterApplicationId",
                table: "StudentSubjects");

            migrationBuilder.DropIndex(
                name: "IX_StudentSubjects_SemesterApplicationId",
                table: "StudentSubjects");

            migrationBuilder.DropColumn(
                name: "SemesterApplicationId",
                table: "StudentSubjects");
        }
    }
}
