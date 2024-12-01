using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookHub.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExpandProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedAuthorsCount",
                table: "Profiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBooksCount",
                table: "Profiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReviewsCount",
                table: "Profiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAuthorsCount",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "CreatedBooksCount",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ReviewsCount",
                table: "Profiles");

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 1, 14, 32, 4, 728, DateTimeKind.Local).AddTicks(1152));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 1, 14, 32, 4, 728, DateTimeKind.Local).AddTicks(1220));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 1, 14, 32, 4, 728, DateTimeKind.Local).AddTicks(1223));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "eecb8012-20f1-4bfa-adf1-460f0fb820aa", "64038dfb-cd28-41b9-b8cb-85a878193bc0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "1678d7b2-a116-4425-9db1-3f3b1a6a007c", "a98c061e-cf2d-49d5-961e-ce36626c478a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "4613c70b-b9dc-4baa-b640-c90e8a4f9b7e", "8d187695-c782-4ac1-ab67-62bd5399a913" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user4Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "85f47670-d274-40d8-885f-a8ae8df07462", "b7145ef4-c8eb-4a69-872c-f08aaf20ffce" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user5Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "ca730cf9-fcab-435e-990c-79b2f4a7d3c6", "6abaa88d-c924-4d7a-908a-e0a0ab0a3e29" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user6Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "1210dc1c-ebad-464b-98a4-f33374fbb1c1", "71d0a102-658a-4f0f-8fe5-84f6a5aa4674" });
        }
    }
}
