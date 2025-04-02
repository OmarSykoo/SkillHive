using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateduserunverified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_emailVerificationTokens_Users_UserId",
                table: "emailVerificationTokens");

            migrationBuilder.CreateTable(
                name: "unverifiedUsers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    location_state = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    location_city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    location_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Balance = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_unverifiedUsers", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_unverifiedUsers_Email",
                table: "unverifiedUsers",
                column: "Email");

            migrationBuilder.AddForeignKey(
                name: "FK_emailVerificationTokens_unverifiedUsers_UserId",
                table: "emailVerificationTokens",
                column: "UserId",
                principalTable: "unverifiedUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_emailVerificationTokens_unverifiedUsers_UserId",
                table: "emailVerificationTokens");

            migrationBuilder.DropTable(
                name: "unverifiedUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_emailVerificationTokens_Users_UserId",
                table: "emailVerificationTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
