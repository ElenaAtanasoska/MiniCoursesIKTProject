using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniCoursesRepository.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatorInHomework : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradedFiles_Homeworks_HomeWorkId",
                table: "GradedFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_AspNetUsers_ProfessorId",
                table: "Homeworks");

            migrationBuilder.DropIndex(
                name: "IX_Homeworks_ProfessorId",
                table: "Homeworks");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "Homeworks");

            migrationBuilder.DropColumn(
                name: "ProfessorId",
                table: "Homeworks");

            migrationBuilder.DropColumn(
                name: "File",
                table: "GradedFiles");

            migrationBuilder.RenameColumn(
                name: "HomeWorkId",
                table: "GradedFiles",
                newName: "HomeworkId");

            migrationBuilder.RenameIndex(
                name: "IX_GradedFiles_HomeWorkId",
                table: "GradedFiles",
                newName: "IX_GradedFiles_HomeworkId");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Homeworks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Grade",
                table: "GradedFiles",
                type: "integer",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "GradedFiles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "GradedFiles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_CreatedById",
                table: "Homeworks",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_GradedFiles_Homeworks_HomeworkId",
                table: "GradedFiles",
                column: "HomeworkId",
                principalTable: "Homeworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_AspNetUsers_CreatedById",
                table: "Homeworks",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradedFiles_Homeworks_HomeworkId",
                table: "GradedFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_AspNetUsers_CreatedById",
                table: "Homeworks");

            migrationBuilder.DropIndex(
                name: "IX_Homeworks_CreatedById",
                table: "Homeworks");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Homeworks");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "GradedFiles");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "GradedFiles");

            migrationBuilder.RenameColumn(
                name: "HomeworkId",
                table: "GradedFiles",
                newName: "HomeWorkId");

            migrationBuilder.RenameIndex(
                name: "IX_GradedFiles_HomeworkId",
                table: "GradedFiles",
                newName: "IX_GradedFiles_HomeWorkId");

            migrationBuilder.AddColumn<double>(
                name: "Grade",
                table: "Homeworks",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ProfessorId",
                table: "Homeworks",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Grade",
                table: "GradedFiles",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "GradedFiles",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_ProfessorId",
                table: "Homeworks",
                column: "ProfessorId");

            migrationBuilder.AddForeignKey(
                name: "FK_GradedFiles_Homeworks_HomeWorkId",
                table: "GradedFiles",
                column: "HomeWorkId",
                principalTable: "Homeworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_AspNetUsers_ProfessorId",
                table: "Homeworks",
                column: "ProfessorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
