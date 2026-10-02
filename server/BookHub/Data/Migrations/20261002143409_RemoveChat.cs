using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // DROP FULLTEXT INDEX can't run inside a transaction. The BookHubFT catalog is kept,
            // because the other full-text indexes still use it.
            migrationBuilder.Sql(@"
IF FULLTEXTSERVICEPROPERTY('IsFullTextInstalled') <> 1
    RETURN;

IF EXISTS (
    SELECT 1
    FROM sys.fulltext_indexes fi
    INNER JOIN sys.objects o ON o.object_id = fi.object_id
    WHERE o.name = N'Chats' AND SCHEMA_NAME(o.schema_id) = N'dbo'
)
BEGIN
    DROP FULLTEXT INDEX ON [dbo].[Chats];
END;
", suppressTransaction: true);

            // Chat invitation notifications (ResourceType = 2) point to a resource type that no longer exists.
            migrationBuilder.Sql("DELETE FROM [Notifications] WHERE [ResourceType] = 2;");

            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "ChatsUsers");

            migrationBuilder.DropTable(
                name: "Chats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Not reversible: the deleted chat data and notifications can't be restored, and this
            // migration will be replaced by a fresh PostgreSQL initial migration. The old schema is
            // preserved under the git tag chat-before-removal.
            throw new NotSupportedException("The RemoveChat migration can't be reverted.");
        }
    }
}
