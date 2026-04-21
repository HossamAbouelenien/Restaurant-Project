using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProviderEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetPasswordCodeExpiry",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ResetPasswordCodeHash",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "UserProviders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Provider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProviders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProviders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProviders_Provider_ProviderId",
                table: "UserProviders",
                columns: new[] { "Provider", "ProviderId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProviders_UserId",
                table: "UserProviders",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProviders");

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetPasswordCodeExpiry",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResetPasswordCodeHash",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
