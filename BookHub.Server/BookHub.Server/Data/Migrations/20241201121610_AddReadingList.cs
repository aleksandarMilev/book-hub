using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookHub.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReadingList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReadingLists",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingLists", x => new { x.UserId, x.BookId });
                    table.ForeignKey(
                        name: "FK_ReadingLists_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReadingLists_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ReadingLists_BookId",
                table: "ReadingLists",
                column: "BookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReadingLists");

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 11, 30, 19, 54, 59, 977, DateTimeKind.Local).AddTicks(8503));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 11, 30, 19, 54, 59, 977, DateTimeKind.Local).AddTicks(8632));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 11, 30, 19, 54, 59, 977, DateTimeKind.Local).AddTicks(8643));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "6e71d9fd-cb0c-4c18-b4f8-8a09b98e5d2a", "e68f7799-169a-4e99-9261-f0eb6a3b640b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "22d14517-4c8c-46d5-8f9e-b4499c809dbf", "a5fd96f5-4c8e-46a5-bfb8-83e15b4f681e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "436849b5-3eb5-40da-a2e7-26c7d5a7ffb2", "d281b5d6-004c-4444-8673-7558cc795994" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user4Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "73a7b8b9-b407-4fdf-a560-bdd3aead7e0a", "f046f46c-94b3-420c-88e2-b23161a427fd" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user5Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "8c9f3281-db0f-4510-96d6-10d57968d8ea", "14c64b16-3440-4e95-be5a-a3ce681b7046" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user6Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "4618ca90-889c-46b9-b67e-70d98aed533c", "79cac276-bf73-4a7e-ac71-cbee59109869" });
        }
    }
}
