#nullable disable
namespace BookHub.Server.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class ChatUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Chats",
                type: "nvarchar(2000)",
                maxLength: 2000,
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Chats");

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
        }
    }
}
