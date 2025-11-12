using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hoshiko.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StageSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Text = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    OptionA = table.Column<string>(type: "TEXT", nullable: true),
                    OptionB = table.Column<string>(type: "TEXT", nullable: true),
                    OptionC = table.Column<string>(type: "TEXT", nullable: true),
                    OptionD = table.Column<string>(type: "TEXT", nullable: true),
                    CorrectAnswer = table.Column<string>(type: "TEXT", nullable: true),
                    StageId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StageSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StageSections_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Stages",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[] { 1, "این یک مرحله‌ی نمونه است برای تست سیستم.", "مرحله آزمایشی" });

            migrationBuilder.InsertData(
                table: "StageSections",
                columns: new[] { "Id", "CorrectAnswer", "OptionA", "OptionB", "OptionC", "OptionD", "StageId", "Text", "Type" },
                values: new object[,]
                {
                    { 1, null, null, null, null, null, 1, "در این بخش یاد می‌گیریم چطور مراحل بازی کار می‌کنند.", 0 },
                    { 2, null, null, null, null, null, 1, "هر مرحله دارای چند توضیح و چند سوال است.", 0 },
                    { 3, "B", "دو بخش", "چهار بخش", "یک بخش", "پنج بخش", 1, "چند بخش در هر مرحله وجود دارد؟", 1 },
                    { 4, "C", "به مرحله بعد می‌روی", "بازی ریست می‌شود", "پاسخ خط می‌خورد و ادامه می‌دهی", "هیچ‌چیز", 1, "اگر پاسخ اشتباه بدهی چه می‌شود؟", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_StageSections_StageId",
                table: "StageSections",
                column: "StageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StageSections");

            migrationBuilder.DropTable(
                name: "Stages");
        }
    }
}
