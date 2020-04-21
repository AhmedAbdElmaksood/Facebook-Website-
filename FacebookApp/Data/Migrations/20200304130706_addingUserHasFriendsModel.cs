using Microsoft.EntityFrameworkCore.Migrations;

namespace FacebookApp.Data.Migrations
{
    public partial class addingUserHasFriendsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserHasFriendFriendId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserHasFriendUserId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserHasFriends",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    FriendId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHasFriends", x => new { x.FriendId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserHasFriends_AspNetUsers_FriendId",
                        column: x => x.FriendId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserHasFriendFriendId_UserHasFriendUserId",
                table: "AspNetUsers",
                columns: new[] { "UserHasFriendFriendId", "UserHasFriendUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserHasFriends_UserHasFriendFriendId_UserHasFriendUserId",
                table: "AspNetUsers",
                columns: new[] { "UserHasFriendFriendId", "UserHasFriendUserId" },
                principalTable: "UserHasFriends",
                principalColumns: new[] { "FriendId", "UserId" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserHasFriends_UserHasFriendFriendId_UserHasFriendUserId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserHasFriends");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserHasFriendFriendId_UserHasFriendUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserHasFriendFriendId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserHasFriendUserId",
                table: "AspNetUsers");
        }
    }
}
