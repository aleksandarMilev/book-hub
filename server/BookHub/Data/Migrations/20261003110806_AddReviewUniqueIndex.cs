using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_CreatorId",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CreatorId_BookId",
                table: "Reviews",
                columns: new[] { "CreatorId", "BookId" },
                unique: true,
                filter: "NOT \"IsDeleted\"");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_CreatorId_BookId",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CreatorId",
                table: "Reviews",
                column: "CreatorId");
        }
    }
}
