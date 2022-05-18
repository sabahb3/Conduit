using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conduit.Data.Migrations
{
    public partial class SeedingFollowers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Followers",
                columns: new[] { "FollowingId", "Username" },
                values: new object[] { "Hala", "Sabah" });

            migrationBuilder.InsertData(
                table: "Followers",
                columns: new[] { "FollowingId", "Username" },
                values: new object[] { "Shaymaa", "Sabah" });

            migrationBuilder.InsertData(
                table: "Followers",
                columns: new[] { "FollowingId", "Username" },
                values: new object[] { "Sabah", "Shaymaa" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Followers",
                keyColumns: new[] { "FollowingId", "Username" },
                keyValues: new object[] { "Hala", "Sabah" });

            migrationBuilder.DeleteData(
                table: "Followers",
                keyColumns: new[] { "FollowingId", "Username" },
                keyValues: new object[] { "Shaymaa", "Sabah" });

            migrationBuilder.DeleteData(
                table: "Followers",
                keyColumns: new[] { "FollowingId", "Username" },
                keyValues: new object[] { "Sabah", "Shaymaa" });
        }
    }
}
