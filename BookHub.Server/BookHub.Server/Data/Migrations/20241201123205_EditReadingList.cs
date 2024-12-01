using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookHub.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditReadingList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ReadingLists",
                table: "ReadingLists");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReadingLists",
                table: "ReadingLists",
                columns: new[] { "UserId", "BookId", "Status" });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ReadingLists",
                table: "ReadingLists");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReadingLists",
                table: "ReadingLists",
                columns: new[] { "UserId", "BookId" });

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 1, 14, 16, 9, 837, DateTimeKind.Local).AddTicks(293));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 1, 14, 16, 9, 837, DateTimeKind.Local).AddTicks(353));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 1, 14, 16, 9, 837, DateTimeKind.Local).AddTicks(356));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "da84699c-e519-4e10-9d3d-0d6e24394eb6", "2b5b7bc2-1fc1-474b-93b9-1b57dc1c88c2" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "da5fe7b0-71a6-458d-a260-ca5b2be5d05b", "64f6ce7e-0062-44f1-9357-23f14b3c722d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "a6efa988-b46d-4bf2-a8a9-4bcab0b68f61", "300cb41d-46b6-4e42-a38b-8141ee834735" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user4Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "b2a07e8e-b4e2-476e-b2f7-9e9f1d100303", "796e21ef-6394-4eb4-a460-c5ee26f33271" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user5Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "2fecc576-7213-45ed-8f93-fc749c91c967", "c4a897cb-8f04-4adb-93c8-ceccb1e1e8e3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user6Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "79c054d2-41a5-4ee3-a951-e4ec117aeaed", "02146d76-9f86-41e7-917c-f3974b945545" });
        }
    }
}
