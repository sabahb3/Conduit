using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conduit.Data.Migrations
{
    public partial class SeedingArticles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "Body", "Date", "Description", "Title", "Username" },
                values: new object[] { 1, "We often start an English conversation with a simple “hello.”\r\n                         You may see someone you know, make eye contact with a stranger, \r\n                         or start a phone conversation with this simple greeting. \r\n                         You may be asking yourself: “What should I say instead of “hello?", new DateTime(2022, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "How to say hello", "Hello, and welcome", "Hala" });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "Body", "Date", "Description", "Title", "Username" },
                values: new object[] { 2, "When people start off an English conversation with “How are you?” \r\n                        they usually don’t expect you to go into much detail. \r\n                        Think of the “How are you?” question as a simple way to get the conversation going.", new DateTime(2022, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "What do people really mean when they ask “How are you?", "How are you", "Hala" });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "Body", "Date", "Description", "Title", "Username" },
                values: new object[] { 3, "In some situations it’s good to just repeat the same greeting \r\n                        back to your conversation partner.When you are meeting someone for the first time, \r\n                        it is considered polite to engage in this way.", new DateTime(2022, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Want to be polite? Be a mirror.", "Be polite", "Sabah" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
