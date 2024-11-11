using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookHub.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUnknownNationality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 183,
                column: "Name",
                value: "Unknown");

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 184,
                column: "Name",
                value: "Uruguay");

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 185,
                column: "Name",
                value: "Uzbekistan");

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 186,
                column: "Name",
                value: "Vanuatu");

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 187,
                column: "Name",
                value: "Vatican City");

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 188,
                column: "Name",
                value: "Venezuela");

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 189,
                column: "Name",
                value: "Vietnam");

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 190,
                column: "Name",
                value: "Yemen");

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 191,
                column: "Name",
                value: "Zambia");

            migrationBuilder.InsertData(
                table: "Nationalities",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "ModifiedBy", "ModifiedOn", "Name" },
                values: new object[] { 192, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Zimbabwe" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 192);

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 183,
                column: "Name",
                value: "Uruguay");

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 184,
                column: "Name",
                value: "Uzbekistan");

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 185,
                column: "Name",
                value: "Vanuatu");

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 186,
                column: "Name",
                value: "Vatican City");

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 187,
                column: "Name",
                value: "Venezuela");

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 188,
                column: "Name",
                value: "Vietnam");

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 189,
                column: "Name",
                value: "Yemen");

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 190,
                column: "Name",
                value: "Zambia");

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 191,
                column: "Name",
                value: "Zimbabwe");
        }
    }
}
