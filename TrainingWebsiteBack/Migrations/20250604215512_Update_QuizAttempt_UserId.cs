using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingWebsiteBack.Migrations
{
    /// <inheritdoc />
    public partial class Update_QuizAttempt_UserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "QuizAttempt",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempt_UserId",
                table: "QuizAttempt",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempt_Users_UserId",
                table: "QuizAttempt",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempt_Users_UserId",
                table: "QuizAttempt");

            migrationBuilder.DropIndex(
                name: "IX_QuizAttempt_UserId",
                table: "QuizAttempt");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "QuizAttempt");
        }
    }
}
