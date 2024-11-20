namespace BookHub.Server.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class FixReviewTypo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Context",
                table: "Reviews",
                newName: "Content");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "3193aed5-26b6-4aea-94f5-8a107e132f3d", "70f71a7c-d027-40de-9ce0-0089806f771b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "e3a73635-33e6-4185-b6a6-cc767d5637bd", "eea513dd-f7c1-4d67-8c5b-21b1e0534488" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "678441cf-45f3-4a56-b086-873cc4148c70", "b55f0866-a343-4ed4-b793-cbe63310f39a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Reviews",
                newName: "Context");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "1f15752b-9998-4338-b468-e0dabf9456c1", "e417b8b4-6e58-4462-905b-3cfeb7b2fe99" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "4ec21011-be12-43f6-8a7f-5ddfcaa2fb20", "81dfd5a0-0fa0-4466-b125-9d74c9be2b12" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "3ec382d8-bbfe-45e5-b6f7-08619902e880", "c04a8791-72b3-4130-9a3b-e76a95f29276" });
        }
    }
}
