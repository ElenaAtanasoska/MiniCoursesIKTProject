using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniCoursesRepository.Migrations
{
    /// <inheritdoc />
    public partial class fixsubjectmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                table: "Homeworks",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_SubjectId",
                table: "Homeworks",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_Subject_SubjectId",
                table: "Homeworks",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_Subject_SubjectId",
                table: "Homeworks");

            migrationBuilder.DropIndex(
                name: "IX_Homeworks_SubjectId",
                table: "Homeworks");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Homeworks");
        }
    }
}
