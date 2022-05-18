using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conduit.Data.Migrations
{
    public partial class SeedingComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "ArticlesId", "Body", "Date", "Username" },
                values: new object[] { 1, 1, "Keep going", new DateTime(2022, 2, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shaymaa" });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "ArticlesId", "Body", "Date", "Username" },
                values: new object[] { 2, 1, "Awesome", new DateTime(2022, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sabah" });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "ArticlesId", "Body", "Date", "Username" },
                values: new object[] { 3, 1, "Keep going", new DateTime(2022, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shaymaa" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
