using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookHub.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProfileMoreColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentlyReadingBooksCount",
                table: "Profiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReadBooksCount",
                table: "Profiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ToReadBooksCount",
                table: "Profiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 2, 13, 36, 43, 236, DateTimeKind.Local).AddTicks(9378));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 2, 13, 36, 43, 236, DateTimeKind.Local).AddTicks(9459));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 2, 13, 36, 43, 236, DateTimeKind.Local).AddTicks(9469));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "53e48d15-2a2c-4fbd-a344-d5d98269a04f", "9964a10f-559f-4cfa-87b9-f22043cd46c4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "187221b3-27f5-4e71-9749-73ee4c24ccff", "ef4db198-1cb7-4e86-abaf-7d90f256c14b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "e6d0d96b-3356-4bd6-81fd-6a52741bc768", "69aaf8fb-b6ae-495d-bb07-bd0f182948a9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user4Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "bf4addbe-a197-4435-9dad-39cae71010fa", "85a5ac16-9b27-47b8-8264-7087b8314c3e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user5Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "753e488b-7393-4a5e-b7cd-b33f6d0f28c0", "32de326f-f25a-4988-b53b-9e2bdcb97d4d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user6Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "f22115a6-d1ca-4364-b2c9-2092d04b19cd", "db182d2c-4294-4ac1-82a4-654f03ed08d9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentlyReadingBooksCount",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ReadBooksCount",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ToReadBooksCount",
                table: "Profiles");

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 1, 18, 9, 13, 643, DateTimeKind.Local).AddTicks(5120));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 1, 18, 9, 13, 643, DateTimeKind.Local).AddTicks(5175));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 1, 18, 9, 13, 643, DateTimeKind.Local).AddTicks(5178));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "6de7d3c9-2a54-480c-8800-798f2d378869", "ce6ad944-9891-4389-a747-2f5dc6e8a9b9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "56a98f52-f7e3-4144-b8c1-630ef5f945d9", "5edf8d96-b6b8-4079-b808-3dcf87cd5c39" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "abddb0ff-d71d-4c46-a762-5381261b043a", "9d8daf5f-41c3-4b8a-8c1d-2e4b205c69d5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user4Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "bbf18926-c336-48e0-a0ab-80de1ed688b1", "02f9e1c6-705e-461f-99ed-e312c55b296b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user5Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "2166dd48-5684-4959-aa90-be1ca2fbdf64", "aebd9c23-f1d3-4ff1-be8c-803811b2263d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user6Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "7f3585d7-a485-4632-9a20-971a421b9593", "c8348e56-a68c-4b41-af46-2502c395e28e" });
        }
    }
}
