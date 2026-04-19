using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleColumnToNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Notifications");
        }
    }
}
