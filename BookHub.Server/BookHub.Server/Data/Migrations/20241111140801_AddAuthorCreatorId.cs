#nullable disable
namespace BookHub.Server.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddAuthorCreatorId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_AspNetUsers_UserId",
                table: "Authors");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Authors",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Authors_UserId",
                table: "Authors",
                newName: "IX_Authors_CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_AspNetUsers_CreatorId",
                table: "Authors",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_AspNetUsers_CreatorId",
                table: "Authors");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Authors",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Authors_CreatorId",
                table: "Authors",
                newName: "IX_Authors_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_AspNetUsers_UserId",
                table: "Authors",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
