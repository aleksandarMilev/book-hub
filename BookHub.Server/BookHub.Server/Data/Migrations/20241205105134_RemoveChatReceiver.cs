#nullable disable
namespace BookHub.Server.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class RemoveChatReceiver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_AspNetUsers_ReceiverId",
                table: "ChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_ReceiverId",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "ChatMessages");

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 51, 33, 612, DateTimeKind.Local).AddTicks(9642));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 51, 33, 612, DateTimeKind.Local).AddTicks(9700));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 51, 33, 612, DateTimeKind.Local).AddTicks(9703));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 51, 33, 612, DateTimeKind.Local).AddTicks(9747));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 51, 33, 612, DateTimeKind.Local).AddTicks(9749));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 51, 33, 612, DateTimeKind.Local).AddTicks(9752));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 51, 33, 612, DateTimeKind.Local).AddTicks(9754));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 51, 33, 612, DateTimeKind.Local).AddTicks(9756));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 51, 33, 612, DateTimeKind.Local).AddTicks(9759));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 51, 33, 612, DateTimeKind.Local).AddTicks(9761));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 51, 33, 612, DateTimeKind.Local).AddTicks(9763));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "b7aca636-b3a3-4f73-b910-7864ba1b9c1e", "9554b6cb-d9f9-45ea-a460-28d0a302da42" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "930804e8-3b2a-4573-8b04-78f671faa275", "e9548527-73dd-44cc-a4fb-e37e0572d915" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "25ff4ebf-45de-4024-99d8-b42a4fdc3648", "bff9ac17-25a3-4558-92a9-b80f7c558139" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user4Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "01127c83-9311-480e-8bff-811789d7a909", "c2897af5-082e-4458-b063-fb94f16ac460" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user5Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "63759832-6bab-46c8-b449-2e3ab97810f7", "666b1349-6656-4bc0-9094-c5f4a57d0f74" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user6Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "060db03d-de24-4171-9680-0c39aa850637", "7ae860e1-39da-4f66-ba61-70b07c78ab3d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "ChatMessages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 29, 47, 506, DateTimeKind.Local).AddTicks(4414));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 29, 47, 506, DateTimeKind.Local).AddTicks(4464));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 29, 47, 506, DateTimeKind.Local).AddTicks(4466));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 29, 47, 506, DateTimeKind.Local).AddTicks(4468));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 29, 47, 506, DateTimeKind.Local).AddTicks(4470));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 29, 47, 506, DateTimeKind.Local).AddTicks(4473));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 29, 47, 506, DateTimeKind.Local).AddTicks(4475));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 29, 47, 506, DateTimeKind.Local).AddTicks(4477));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 29, 47, 506, DateTimeKind.Local).AddTicks(4479));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 29, 47, 506, DateTimeKind.Local).AddTicks(4482));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 29, 47, 506, DateTimeKind.Local).AddTicks(4484));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "d481c2d8-3072-4645-825f-8dfa18ad71ce", "73e03980-f3b9-4c4e-ad19-e241819d0973" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "d3af303b-07d7-4dc8-98ea-46aa65606444", "c4aa6c06-90fe-453f-ad51-ea03e6ab48da" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "5ef35365-b926-4956-8a2a-fadf049bc576", "fa407967-ec7f-4bbf-b4f3-07a238e4b1d3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user4Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "fcee0f11-9fc0-4e74-b9ba-6f7d0026b950", "ac1ac5ae-3907-491a-a606-26b9bc02b809" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user5Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "52eb7202-6a6e-416b-88ca-f333333c8419", "715b87bc-aa5c-4dbd-b7f5-5adca61e71dd" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user6Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "d11c76a7-9677-40e0-a74a-5f34a0f62dfc", "f9c1f205-5f77-48a8-8dc8-73bcaacd0ec1" });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ReceiverId",
                table: "ChatMessages",
                column: "ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_AspNetUsers_ReceiverId",
                table: "ChatMessages",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
