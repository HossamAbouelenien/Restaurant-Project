using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyDeliveryAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryAddress",
                table: "Deliveries");

            migrationBuilder.AddColumn<int>(
                name: "BuildingNumber",
                table: "Deliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Deliveries",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Deliveries",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecialMark",
                table: "Deliveries",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Deliveries",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuildingNumber",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "SpecialMark",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Deliveries");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryAddress",
                table: "Deliveries",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");
        }
    }
}
