using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conduit.Data.Migrations
{
    public partial class SeedingUsersWithProfilePicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "Hala",
                column: "ProfilePicture",
                value: "https://api.realworld.io/images/smiley-cyrus.jpeg");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "sabah",
                column: "ProfilePicture",
                value: "https://api.realworld.io/images/smiley-cyrus.jpeg");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "Shaymaa",
                column: "ProfilePicture",
                value: "https://api.realworld.io/images/smiley-cyrus.jpeg");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "Hala",
                column: "ProfilePicture",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "sabah",
                column: "ProfilePicture",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "Shaymaa",
                column: "ProfilePicture",
                value: null);
        }
    }
}
