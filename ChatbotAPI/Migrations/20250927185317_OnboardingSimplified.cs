using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatbotAPI.Migrations
{
    /// <inheritdoc />
    public partial class OnboardingSimplified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HelpfulLinks");

            migrationBuilder.DropTable(
                name: "PdfFiles");

            migrationBuilder.DropIndex(
                name: "IX_WidgetSettings_ProjectId",
                table: "WidgetSettings");

            migrationBuilder.DropIndex(
                name: "IX_TrainingData_ProjectId",
                table: "TrainingData");

            migrationBuilder.DropColumn(
                name: "BusinessCategory",
                table: "TrainingData");

            migrationBuilder.DropColumn(
                name: "BusinessName",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "WebsiteUrl",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "BusinessName",
                table: "TrainingData",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebsiteUrl",
                table: "TrainingData",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Projects",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WidgetSettings_ProjectId",
                table: "WidgetSettings",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainingData_ProjectId",
                table: "TrainingData",
                column: "ProjectId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WidgetSettings_ProjectId",
                table: "WidgetSettings");

            migrationBuilder.DropIndex(
                name: "IX_TrainingData_ProjectId",
                table: "TrainingData");

            migrationBuilder.DropColumn(
                name: "BusinessName",
                table: "TrainingData");

            migrationBuilder.DropColumn(
                name: "WebsiteUrl",
                table: "TrainingData");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Projects");

            migrationBuilder.AddColumn<string>(
                name: "BusinessCategory",
                table: "TrainingData",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessName",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WebsiteUrl",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "HelpfulLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainingDataId = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelpfulLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HelpfulLinks_TrainingData_TrainingDataId",
                        column: x => x.TrainingDataId,
                        principalTable: "TrainingData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PdfFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainingDataId = table.Column<int>(type: "int", nullable: false),
                    FileContent = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PdfFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PdfFiles_TrainingData_TrainingDataId",
                        column: x => x.TrainingDataId,
                        principalTable: "TrainingData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WidgetSettings_ProjectId",
                table: "WidgetSettings",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingData_ProjectId",
                table: "TrainingData",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_HelpfulLinks_TrainingDataId",
                table: "HelpfulLinks",
                column: "TrainingDataId");

            migrationBuilder.CreateIndex(
                name: "IX_PdfFiles_TrainingDataId",
                table: "PdfFiles",
                column: "TrainingDataId");
        }
    }
}
