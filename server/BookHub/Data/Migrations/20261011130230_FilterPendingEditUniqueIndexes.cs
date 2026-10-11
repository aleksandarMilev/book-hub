using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class FilterPendingEditUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BookEdits_BookId",
                table: "BookEdits");

            migrationBuilder.DropIndex(
                name: "IX_AuthorEdits_AuthorId",
                table: "AuthorEdits");

            migrationBuilder.CreateIndex(
                name: "IX_BookEdits_BookId",
                table: "BookEdits",
                column: "BookId",
                unique: true,
                filter: "NOT \"IsDeleted\"");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorEdits_AuthorId",
                table: "AuthorEdits",
                column: "AuthorId",
                unique: true,
                filter: "NOT \"IsDeleted\"");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BookEdits_BookId",
                table: "BookEdits");

            migrationBuilder.DropIndex(
                name: "IX_AuthorEdits_AuthorId",
                table: "AuthorEdits");

            migrationBuilder.CreateIndex(
                name: "IX_BookEdits_BookId",
                table: "BookEdits",
                column: "BookId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthorEdits_AuthorId",
                table: "AuthorEdits",
                column: "AuthorId",
                unique: true);
        }
    }
}
