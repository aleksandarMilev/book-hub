#nullable disable
namespace BookHub.Server.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class AddReceiverNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "BookNotifications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "BookNotifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BookNotifications",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "AuthorNotifications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "AuthorNotifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AuthorNotifications",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "992a07c8-e255-458a-8d23-61ef584cb976", "f4483721-3643-48c4-9f22-521c0f798dcd" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "bf308f21-c1f7-40db-bc52-d303b6e24b97", "f9d1ad07-40e7-4c05-91a1-b747e5da49d4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "6caed014-2aac-4a90-ae03-135903ee383d", "6695269e-71d2-4144-8494-bfd0323f9b4f" });

            migrationBuilder.CreateIndex(
                name: "IX_BookNotifications_UserId",
                table: "BookNotifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorNotifications_UserId",
                table: "AuthorNotifications",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorNotifications_AspNetUsers_UserId",
                table: "AuthorNotifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookNotifications_AspNetUsers_UserId",
                table: "BookNotifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorNotifications_AspNetUsers_UserId",
                table: "AuthorNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_BookNotifications_AspNetUsers_UserId",
                table: "BookNotifications");

            migrationBuilder.DropIndex(
                name: "IX_BookNotifications_UserId",
                table: "BookNotifications");

            migrationBuilder.DropIndex(
                name: "IX_AuthorNotifications_UserId",
                table: "AuthorNotifications");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "BookNotifications");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BookNotifications");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "AuthorNotifications");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AuthorNotifications");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "BookNotifications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "AuthorNotifications",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "e5da35ab-e19e-4625-8115-248558c7011b", "3c2dc24a-d0d3-4861-b8d1-ddf77a12794b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "f267cb1b-5a93-4b61-a97e-a42be69934b4", "3edb0c6e-e5a2-4372-8a20-59984cba77bf" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "c2496e52-e8ba-4c01-ac59-66523ca2b0e9", "d8764dff-1b4e-4195-8581-2d84d51ae430" });
        }
    }
}
