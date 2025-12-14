using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hoshiko.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QuizRetryCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RetryCount",
                table: "UserStageProgresses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RetryCount",
                table: "UserStageProgresses");
        }
    }
}
