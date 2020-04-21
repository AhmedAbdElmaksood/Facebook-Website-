using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FacebookApp.Data.Migrations
{
    public partial class addingUserCommentsOnPostModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserCommentsOnPosts",
                columns: table => new
                {
                    CommentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    PostId = table.Column<int>(nullable: false),
                    CommentDate = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCommentsOnPosts", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_UserCommentsOnPosts_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCommentsOnPosts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCommentsOnPosts_PostId",
                table: "UserCommentsOnPosts",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCommentsOnPosts_UserId",
                table: "UserCommentsOnPosts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCommentsOnPosts");
        }
    }
}
