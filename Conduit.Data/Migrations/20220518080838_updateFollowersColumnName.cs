using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conduit.Data.Migrations
{
    public partial class updateFollowersColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Users_FollowerId",
                table: "Followers");

            migrationBuilder.RenameColumn(
                name: "FollowerId",
                table: "Followers",
                newName: "FollowingId");

            migrationBuilder.RenameIndex(
                name: "IX_Followers_FollowerId",
                table: "Followers",
                newName: "IX_Followers_FollowingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Users_FollowingId",
                table: "Followers",
                column: "FollowingId",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Followers_Users_FollowingId",
                table: "Followers");

            migrationBuilder.RenameColumn(
                name: "FollowingId",
                table: "Followers",
                newName: "FollowerId");

            migrationBuilder.RenameIndex(
                name: "IX_Followers_FollowingId",
                table: "Followers",
                newName: "IX_Followers_FollowerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Followers_Users_FollowerId",
                table: "Followers",
                column: "FollowerId",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
