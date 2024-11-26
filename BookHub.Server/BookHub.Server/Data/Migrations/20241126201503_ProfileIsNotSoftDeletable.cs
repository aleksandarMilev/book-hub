#nullable disable
namespace BookHub.Server.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ProfileIsNotSoftDeletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Profiles");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "9e13de49-1a2f-47e4-ae1e-a882bcfd8094", "6a3b158e-b5db-4d88-8cdd-71db739e1ca7" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "c5ae4362-56f5-4a2a-b2f3-9eb7c43a185d", "29957544-a1ba-4890-931f-eac94a102da7" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "152b1d13-7031-4eaa-bfe3-592014891d40", "8c8c908c-e9e2-4860-9644-008c06a76111" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Profiles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Profiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "7257c00c-f39f-4448-af7d-55fea6fef40c", "1fb79b45-2954-4547-8d3d-db0812a78432" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "34d9aa51-0245-400a-8e5d-a7ed37c27742", "39108844-7841-4860-8b61-dec13daa9bac" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "0208b21a-9f6e-40c3-9d1e-2b20f5396019", "6e807e76-56be-4c4e-87d2-7c731a6a77f3" });
        }
    }
}
