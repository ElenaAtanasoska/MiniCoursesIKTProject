using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniCoursesRepository.Migrations
{
    /// <inheritdoc />
    public partial class AddRequiredVariablesForGrades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradedFiles_Homeworks_HomeworkId",
                table: "GradedFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_Subject_SubjectId",
                table: "Homeworks");

            migrationBuilder.RenameColumn(
                name: "HomeworkId",
                table: "GradedFiles",
                newName: "HomeWorkId");

            migrationBuilder.RenameIndex(
                name: "IX_GradedFiles_HomeworkId",
                table: "GradedFiles",
                newName: "IX_GradedFiles_HomeWorkId");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubjectId",
                table: "Homeworks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GradedFiles_Homeworks_HomeWorkId",
                table: "GradedFiles",
                column: "HomeWorkId",
                principalTable: "Homeworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_Subject_SubjectId",
                table: "Homeworks",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradedFiles_Homeworks_HomeWorkId",
                table: "GradedFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_Subject_SubjectId",
                table: "Homeworks");

            migrationBuilder.RenameColumn(
                name: "HomeWorkId",
                table: "GradedFiles",
                newName: "HomeworkId");

            migrationBuilder.RenameIndex(
                name: "IX_GradedFiles_HomeWorkId",
                table: "GradedFiles",
                newName: "IX_GradedFiles_HomeworkId");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubjectId",
                table: "Homeworks",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_GradedFiles_Homeworks_HomeworkId",
                table: "GradedFiles",
                column: "HomeworkId",
                principalTable: "Homeworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_Subject_SubjectId",
                table: "Homeworks",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id");
        }
    }
}
