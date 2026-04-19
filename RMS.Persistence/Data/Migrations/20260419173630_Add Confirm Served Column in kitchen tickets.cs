using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddConfirmServedColumninkitchentickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ConfirmedServed",
                table: "KitchenTickets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmedServed",
                table: "KitchenTickets");
        }
    }
}
