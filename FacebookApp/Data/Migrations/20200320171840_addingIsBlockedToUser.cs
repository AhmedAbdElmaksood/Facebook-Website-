using Microsoft.EntityFrameworkCore.Migrations;

namespace FacebookApp.Data.Migrations
{
    public partial class addingIsBlockedToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isBlocked",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isBlocked",
                table: "AspNetUsers");
        }
    }
}
