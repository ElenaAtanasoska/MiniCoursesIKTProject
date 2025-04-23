using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniCoursesRepository.Migrations
{
    /// <inheritdoc />
    public partial class AddSemesterApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradedFile_AspNetUsers_UserId",
                table: "GradedFile");

            migrationBuilder.DropForeignKey(
                name: "FK_GradedFile_Homework_HomeworkId",
                table: "GradedFile");

            migrationBuilder.DropForeignKey(
                name: "FK_Homework_AspNetUsers_ProfessorId",
                table: "Homework");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubject_AspNetUsers_UserId",
                table: "StudentSubject");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubject_Subject_SubjectId",
                table: "StudentSubject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentSubject",
                table: "StudentSubject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Homework",
                table: "Homework");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GradedFile",
                table: "GradedFile");

            migrationBuilder.RenameTable(
                name: "StudentSubject",
                newName: "StudentSubjects");

            migrationBuilder.RenameTable(
                name: "Homework",
                newName: "Homeworks");

            migrationBuilder.RenameTable(
                name: "GradedFile",
                newName: "GradedFiles");

            migrationBuilder.RenameIndex(
                name: "IX_StudentSubject_UserId",
                table: "StudentSubjects",
                newName: "IX_StudentSubjects_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentSubject_SubjectId",
                table: "StudentSubjects",
                newName: "IX_StudentSubjects_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Homework_ProfessorId",
                table: "Homeworks",
                newName: "IX_Homeworks_ProfessorId");

            migrationBuilder.RenameIndex(
                name: "IX_GradedFile_UserId",
                table: "GradedFiles",
                newName: "IX_GradedFiles_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_GradedFile_HomeworkId",
                table: "GradedFiles",
                newName: "IX_GradedFiles_HomeworkId");

            migrationBuilder.AlterColumn<string>(
                name: "ProfessorId",
                table: "Homeworks",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentSubjects",
                table: "StudentSubjects",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Homeworks",
                table: "Homeworks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GradedFiles",
                table: "GradedFiles",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SemesterApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Semester = table.Column<int>(type: "integer", nullable: false),
                    SemesterType = table.Column<int>(type: "integer", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StudentId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemesterApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SemesterApplications_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SemesterApplications_StudentId",
                table: "SemesterApplications",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_GradedFiles_AspNetUsers_UserId",
                table: "GradedFiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GradedFiles_Homeworks_HomeworkId",
                table: "GradedFiles",
                column: "HomeworkId",
                principalTable: "Homeworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_AspNetUsers_ProfessorId",
                table: "Homeworks",
                column: "ProfessorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubjects_AspNetUsers_UserId",
                table: "StudentSubjects",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubjects_Subject_SubjectId",
                table: "StudentSubjects",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradedFiles_AspNetUsers_UserId",
                table: "GradedFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_GradedFiles_Homeworks_HomeworkId",
                table: "GradedFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_AspNetUsers_ProfessorId",
                table: "Homeworks");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubjects_AspNetUsers_UserId",
                table: "StudentSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubjects_Subject_SubjectId",
                table: "StudentSubjects");

            migrationBuilder.DropTable(
                name: "SemesterApplications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentSubjects",
                table: "StudentSubjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Homeworks",
                table: "Homeworks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GradedFiles",
                table: "GradedFiles");

            migrationBuilder.RenameTable(
                name: "StudentSubjects",
                newName: "StudentSubject");

            migrationBuilder.RenameTable(
                name: "Homeworks",
                newName: "Homework");

            migrationBuilder.RenameTable(
                name: "GradedFiles",
                newName: "GradedFile");

            migrationBuilder.RenameIndex(
                name: "IX_StudentSubjects_UserId",
                table: "StudentSubject",
                newName: "IX_StudentSubject_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentSubjects_SubjectId",
                table: "StudentSubject",
                newName: "IX_StudentSubject_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Homeworks_ProfessorId",
                table: "Homework",
                newName: "IX_Homework_ProfessorId");

            migrationBuilder.RenameIndex(
                name: "IX_GradedFiles_UserId",
                table: "GradedFile",
                newName: "IX_GradedFile_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_GradedFiles_HomeworkId",
                table: "GradedFile",
                newName: "IX_GradedFile_HomeworkId");

            migrationBuilder.AlterColumn<string>(
                name: "ProfessorId",
                table: "Homework",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentSubject",
                table: "StudentSubject",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Homework",
                table: "Homework",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GradedFile",
                table: "GradedFile",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GradedFile_AspNetUsers_UserId",
                table: "GradedFile",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GradedFile_Homework_HomeworkId",
                table: "GradedFile",
                column: "HomeworkId",
                principalTable: "Homework",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Homework_AspNetUsers_ProfessorId",
                table: "Homework",
                column: "ProfessorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubject_AspNetUsers_UserId",
                table: "StudentSubject",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubject_Subject_SubjectId",
                table: "StudentSubject",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
