using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conduit.Data.Migrations
{
    public partial class SeedingArticleTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ArticlesTags",
                columns: new[] { "ArticleId", "Tag" },
                values: new object[] { 1, "Greetings" });

            migrationBuilder.InsertData(
                table: "ArticlesTags",
                columns: new[] { "ArticleId", "Tag" },
                values: new object[] { 1, "Welcoming" });

            migrationBuilder.InsertData(
                table: "ArticlesTags",
                columns: new[] { "ArticleId", "Tag" },
                values: new object[] { 3, "Greetings" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ArticlesTags",
                keyColumns: new[] { "ArticleId", "Tag" },
                keyValues: new object[] { 1, "Greetings" });

            migrationBuilder.DeleteData(
                table: "ArticlesTags",
                keyColumns: new[] { "ArticleId", "Tag" },
                keyValues: new object[] { 1, "Welcoming" });

            migrationBuilder.DeleteData(
                table: "ArticlesTags",
                keyColumns: new[] { "ArticleId", "Tag" },
                keyValues: new object[] { 3, "Greetings" });
        }
    }
}
