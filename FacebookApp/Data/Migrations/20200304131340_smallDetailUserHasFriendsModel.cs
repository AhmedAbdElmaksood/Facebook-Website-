using Microsoft.EntityFrameworkCore.Migrations;

namespace FacebookApp.Data.Migrations
{
    public partial class smallDetailUserHasFriendsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "UserHasFriends",
                newName: "Status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "UserHasFriends",
                newName: "status");
        }
    }
}
