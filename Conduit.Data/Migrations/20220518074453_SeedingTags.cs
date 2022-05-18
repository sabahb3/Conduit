using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conduit.Data.Migrations
{
    public partial class SeedingTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tags",
                column: "Tag",
                value: "Conversation");

            migrationBuilder.InsertData(
                table: "Tags",
                column: "Tag",
                value: "Greetings");

            migrationBuilder.InsertData(
                table: "Tags",
                column: "Tag",
                value: "Welcoming");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Tag",
                keyValue: "Conversation");

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Tag",
                keyValue: "Greetings");

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Tag",
                keyValue: "Welcoming");
        }
    }
}
