using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingWebsiteBack.Migrations
{
    /// <inheritdoc />
    public partial class AddLectureAndQuizTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lecture_Courses_CourseId",
                table: "Lecture");

            migrationBuilder.DropForeignKey(
                name: "FK_Quiz_Courses_CourseId",
                table: "Quiz");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempt_Quiz_QuizId",
                table: "QuizAttempt");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempt_Quiz_QuizId1",
                table: "QuizAttempt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Quiz",
                table: "Quiz");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lecture",
                table: "Lecture");

            migrationBuilder.RenameTable(
                name: "Quiz",
                newName: "Quizzes");

            migrationBuilder.RenameTable(
                name: "Lecture",
                newName: "Lectures");

            migrationBuilder.RenameIndex(
                name: "IX_Quiz_CourseId",
                table: "Quizzes",
                newName: "IX_Quizzes_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Lecture_CourseId",
                table: "Lectures",
                newName: "IX_Lectures_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quizzes",
                table: "Quizzes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lectures",
                table: "Lectures",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_Courses_CourseId",
                table: "Lectures",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempt_Quizzes_QuizId",
                table: "QuizAttempt",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempt_Quizzes_QuizId1",
                table: "QuizAttempt",
                column: "QuizId1",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Courses_CourseId",
                table: "Quizzes",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_Courses_CourseId",
                table: "Lectures");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempt_Quizzes_QuizId",
                table: "QuizAttempt");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempt_Quizzes_QuizId1",
                table: "QuizAttempt");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Courses_CourseId",
                table: "Quizzes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Quizzes",
                table: "Quizzes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lectures",
                table: "Lectures");

            migrationBuilder.RenameTable(
                name: "Quizzes",
                newName: "Quiz");

            migrationBuilder.RenameTable(
                name: "Lectures",
                newName: "Lecture");

            migrationBuilder.RenameIndex(
                name: "IX_Quizzes_CourseId",
                table: "Quiz",
                newName: "IX_Quiz_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Lectures_CourseId",
                table: "Lecture",
                newName: "IX_Lecture_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quiz",
                table: "Quiz",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lecture",
                table: "Lecture",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lecture_Courses_CourseId",
                table: "Lecture",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quiz_Courses_CourseId",
                table: "Quiz",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempt_Quiz_QuizId",
                table: "QuizAttempt",
                column: "QuizId",
                principalTable: "Quiz",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempt_Quiz_QuizId1",
                table: "QuizAttempt",
                column: "QuizId1",
                principalTable: "Quiz",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
