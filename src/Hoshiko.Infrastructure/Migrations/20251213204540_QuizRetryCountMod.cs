using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hoshiko.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QuizRetryCountMod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastRetryAt",
                table: "UserStageProgresses",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastRetryAt",
                table: "UserStageProgresses");
        }
    }
}
