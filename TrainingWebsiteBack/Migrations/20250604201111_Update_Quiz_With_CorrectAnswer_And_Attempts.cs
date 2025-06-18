using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingWebsiteBack.Migrations
{
    /// <inheritdoc />
    public partial class Update_Quiz_With_CorrectAnswer_And_Attempts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempt_Quizzes_QuizId1",
                table: "QuizAttempt");

            migrationBuilder.DropIndex(
                name: "IX_QuizAttempt_QuizId",
                table: "QuizAttempt");

            migrationBuilder.DropIndex(
                name: "IX_QuizAttempt_QuizId1",
                table: "QuizAttempt");

            migrationBuilder.DropColumn(
                name: "QuizId1",
                table: "QuizAttempt");

            migrationBuilder.DropColumn(
                name: "QuizId2",
                table: "QuizAttempt");

            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswer",
                table: "Quizzes",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempt_QuizId",
                table: "QuizAttempt",
                column: "QuizId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_QuizAttempt_QuizId",
                table: "QuizAttempt");

            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "Quizzes");

            migrationBuilder.AddColumn<int>(
                name: "QuizId1",
                table: "QuizAttempt",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuizId2",
                table: "QuizAttempt",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempt_QuizId",
                table: "QuizAttempt",
                column: "QuizId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempt_QuizId1",
                table: "QuizAttempt",
                column: "QuizId1");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempt_Quizzes_QuizId1",
                table: "QuizAttempt",
                column: "QuizId1",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
