using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingWebsiteBack.Migrations
{
    /// <inheritdoc />
    public partial class AddQuizAttemptRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Attemp",
                table: "QuizAttempt",
                newName: "Attempt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Attempt",
                table: "QuizAttempt",
                newName: "Attemp");
        }
    }
}
