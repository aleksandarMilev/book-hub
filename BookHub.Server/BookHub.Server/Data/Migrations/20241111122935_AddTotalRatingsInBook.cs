#nullable disable
namespace BookHub.Server.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddTotalRatingsInBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Books",
                newName: "AverageRating");

            migrationBuilder.AddColumn<int>(
                name: "RatingsCount",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatingsCount",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "AverageRating",
                table: "Books",
                newName: "Rating");
        }
    }
}
