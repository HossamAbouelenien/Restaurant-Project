using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class ArabicNameAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ArabicMessage",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArabicTitle",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArabicName",
                table: "MenuItems",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArabicName",
                table: "Ingredients",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArabicName",
                table: "Categories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ArabicName",
                table: "Branches",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArabicMessage",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ArabicTitle",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ArabicName",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "ArabicName",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "ArabicName",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ArabicName",
                table: "Branches");
        }
    }
}
