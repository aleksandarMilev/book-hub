#nullable disable
namespace BookHub.Server.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class ChatUserHasAccepted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasAccepted",
                table: "ChatsUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 6, 20, 1, 35, 10, DateTimeKind.Local).AddTicks(9478));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 6, 20, 1, 35, 10, DateTimeKind.Local).AddTicks(9549));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 6, 20, 1, 35, 10, DateTimeKind.Local).AddTicks(9555));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 6, 20, 1, 35, 10, DateTimeKind.Local).AddTicks(9561));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 6, 20, 1, 35, 10, DateTimeKind.Local).AddTicks(9567));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 6, 20, 1, 35, 10, DateTimeKind.Local).AddTicks(9573));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 6, 20, 1, 35, 10, DateTimeKind.Local).AddTicks(9587));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 6, 20, 1, 35, 10, DateTimeKind.Local).AddTicks(9594));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 6, 20, 1, 35, 10, DateTimeKind.Local).AddTicks(9614));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 6, 20, 1, 35, 10, DateTimeKind.Local).AddTicks(9620));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 6, 20, 1, 35, 10, DateTimeKind.Local).AddTicks(9629));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "8e3398f2-9070-48e3-a101-9b27e9e7708f", "d5c2383c-7b53-4a5b-972e-77cbdbd36d61" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "f9d37cad-5634-4bd1-80ad-e55737d01910", "07b5dda5-9a93-48d5-8785-0beef716fd26" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "95997f86-e5f3-4e80-9e5a-42f1ec910516", "34e5ffe3-e2e0-4215-a902-1e6ef5a9d852" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user4Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "1775d6a2-2682-4be8-a07e-405f7932a873", "d3a5d956-af44-43b6-aa6f-8ed009bbf64d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user5Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "8e27dbb8-0920-4a9b-83b1-6c2d419f7f0d", "cbbdcede-0a41-42f6-a915-6609d7eb603e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user6Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "2cb949d2-1d38-49db-9e37-ccc44511c8fc", "ac288de7-9172-4a6f-b3a8-6ac087329d16" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasAccepted",
                table: "ChatsUsers");

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
    }
}
