using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatbotAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddChatHeaderTextToWidgetSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChatHeaderText",
                table: "WidgetSettings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatHeaderText",
                table: "WidgetSettings");
        }
    }
}
