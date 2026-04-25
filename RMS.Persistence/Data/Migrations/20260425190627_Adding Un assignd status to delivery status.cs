using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingUnassigndstatustodeliverystatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DeliveryStatus",
                table: "Deliveries",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "UnAssigned",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValue: "Assigned");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DeliveryStatus",
                table: "Deliveries",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "Assigned",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldDefaultValue: "UnAssigned");
        }
    }
}
