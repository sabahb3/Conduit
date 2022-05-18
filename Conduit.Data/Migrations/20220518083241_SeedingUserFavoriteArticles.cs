using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conduit.Data.Migrations
{
    public partial class SeedingUserFavoriteArticles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UsersFavoriteArticles",
                columns: new[] { "ArticleId", "Username" },
                values: new object[] { 3, "Hala" });

            migrationBuilder.InsertData(
                table: "UsersFavoriteArticles",
                columns: new[] { "ArticleId", "Username" },
                values: new object[] { 1, "Sabah" });

            migrationBuilder.InsertData(
                table: "UsersFavoriteArticles",
                columns: new[] { "ArticleId", "Username" },
                values: new object[] { 1, "Shaymaa" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UsersFavoriteArticles",
                keyColumns: new[] { "ArticleId", "Username" },
                keyValues: new object[] { 3, "Hala" });

            migrationBuilder.DeleteData(
                table: "UsersFavoriteArticles",
                keyColumns: new[] { "ArticleId", "Username" },
                keyValues: new object[] { 1, "Sabah" });

            migrationBuilder.DeleteData(
                table: "UsersFavoriteArticles",
                keyColumns: new[] { "ArticleId", "Username" },
                keyValues: new object[] { 1, "Shaymaa" });
        }
    }
}
