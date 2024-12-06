#nullable disable
namespace BookHub.Server.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class ChatCreator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Chats",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 27, 35, 243, DateTimeKind.Local).AddTicks(5114));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 27, 35, 243, DateTimeKind.Local).AddTicks(5171));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 27, 35, 243, DateTimeKind.Local).AddTicks(5174));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 27, 35, 243, DateTimeKind.Local).AddTicks(5176));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 27, 35, 243, DateTimeKind.Local).AddTicks(5178));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 27, 35, 243, DateTimeKind.Local).AddTicks(5181));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 27, 35, 243, DateTimeKind.Local).AddTicks(5183));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 27, 35, 243, DateTimeKind.Local).AddTicks(5185));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 27, 35, 243, DateTimeKind.Local).AddTicks(5188));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 27, 35, 243, DateTimeKind.Local).AddTicks(5190));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 27, 35, 243, DateTimeKind.Local).AddTicks(5192));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "2e98cda0-831a-47c0-804a-2039a30deaf0", "3c2e4f71-bd51-4fbd-8223-3371f94d63c5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "3e5f08fd-b44f-4df1-8623-0ddc1511a4bb", "0c775815-7870-4c90-878f-e7e5e6d8ee34" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "68dedaa8-7f90-441b-8e74-23124299cb4e", "1d496f00-c49a-4e29-a9eb-e2b560a3b157" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user4Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "774b96f5-4a86-4f11-93a6-20767db31c52", "39f45b34-0d38-4ec0-bdcb-7c224691df67" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user5Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "992f9768-2f7f-4f20-a08e-19dd9f53cb15", "5c4dabca-b02d-4356-8238-b2c6ab8ba74b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user6Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "717cf623-30bd-4d11-9d49-f30a3e70c7ba", "e388d3b8-1e97-48e7-9528-8fed5ddba870" });

            migrationBuilder.CreateIndex(
                name: "IX_Chats_CreatorId",
                table: "Chats",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_AspNetUsers_CreatorId",
                table: "Chats",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_AspNetUsers_CreatorId",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_CreatorId",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Chats");

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 10, 32, 230, DateTimeKind.Local).AddTicks(4435));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 10, 32, 230, DateTimeKind.Local).AddTicks(4488));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 10, 32, 230, DateTimeKind.Local).AddTicks(4490));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 10, 32, 230, DateTimeKind.Local).AddTicks(4492));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 10, 32, 230, DateTimeKind.Local).AddTicks(4495));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 10, 32, 230, DateTimeKind.Local).AddTicks(4497));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 10, 32, 230, DateTimeKind.Local).AddTicks(4500));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 10, 32, 230, DateTimeKind.Local).AddTicks(4502));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 10, 32, 230, DateTimeKind.Local).AddTicks(4505));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 10, 32, 230, DateTimeKind.Local).AddTicks(4508));

            migrationBuilder.UpdateData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedOn",
                value: new DateTime(2024, 12, 5, 12, 10, 32, 230, DateTimeKind.Local).AddTicks(4510));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "370567a4-64e9-4b1c-bbf9-ad55883600ac", "6390dacd-e274-45cb-b796-5d77969b5c54" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "436fc23c-0325-415f-9385-bab7bce93409", "be332103-e648-4ece-bb03-46826584c1c4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "00f1b7e4-d77b-4da6-a93b-ebb7ad9acdbc", "02d3dfe5-7d21-49ad-9879-e6030d7d5718" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user4Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "118f30fe-b1d6-468f-8331-9802d911f3e8", "7de4a73e-335b-4967-8855-9dee418e872f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user5Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "4b7b87dd-91e4-471c-a021-42de03d55ef2", "5aa1e056-06c6-40a8-8b37-57a8a46dc9a2" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user6Id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "3de0d311-c0a8-4b36-b48d-b14dd7d106a5", "1c893bfb-48eb-41dd-8f32-29c79f872aac" });
        }
    }
}
