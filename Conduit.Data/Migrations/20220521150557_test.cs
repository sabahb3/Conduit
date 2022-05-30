using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conduit.Data.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Followers",
                columns: new[] { "FollowingId", "Username" },
                values: new object[] { "Hala", "Shaymaa" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Followers",
                keyColumns: new[] { "FollowingId", "Username" },
                keyValues: new object[] { "Hala", "Shaymaa" });
        }
    }
}
