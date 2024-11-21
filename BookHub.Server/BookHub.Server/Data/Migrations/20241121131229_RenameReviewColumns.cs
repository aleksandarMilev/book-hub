#nullable disable
namespace BookHub.Server.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;
    /// <inheritdoc />
    public partial class RenameReviewColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Likes",
                table: "Reviews",
                newName: "Upvotes");

            migrationBuilder.RenameColumn(
                name: "Dislikes",
                table: "Reviews",
                newName: "Downvotes");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "21bb355b-6537-4301-a541-bab385e4399a", "36550afc-f5bb-4f62-b5be-bfc6f6b0b7af" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "ff31e405-1abd-4eb7-8dd2-81c8045f845b", "97af7585-22c9-46d1-a039-96080469bc29" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "ee8b3a49-5246-4239-813d-b31b477c13c5", "db20d909-4307-41d4-87bd-f06d1575d148" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Upvotes",
                table: "Reviews",
                newName: "Likes");

            migrationBuilder.RenameColumn(
                name: "Downvotes",
                table: "Reviews",
                newName: "Dislikes");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "da0479aa-7be8-48ec-a212-3a5322442938", "d72bffd0-c99c-4920-9db4-f90586f3f3cf" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "b1ba3a64-1df8-4748-a6ad-747848900fdd", "906f1e55-093a-4f1d-8acd-5af84154f38a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "0bce96d8-d7c7-4e40-8bb2-3be60c702565", "9c9306fd-6d26-4b51-9161-b4118a033586" });
        }
    }
}
