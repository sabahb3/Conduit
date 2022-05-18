using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conduit.Data.Migrations
{
    public partial class SeedingUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Username", "Bio", "Email", "Password" },
                values: new object[] { "Hala", null, "Hala@gmail.com", "0" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Username", "Bio", "Email", "Password" },
                values: new object[] { "sabah", null, "sabahBaara4@gmail.com", "4050" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Username", "Bio", "Email", "Password" },
                values: new object[] { "Shaymaa", null, "shaymaaAshqar@gmail.com", "1234" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "Hala");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "sabah");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "Shaymaa");
        }
    }
}
