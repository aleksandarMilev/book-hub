namespace BookHub.Data.Migrations;

using Microsoft.EntityFrameworkCore.Migrations;

public partial class FullTextSearch : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
IF FULLTEXTSERVICEPROPERTY('IsFullTextInstalled') <> 1
    RETURN;

IF NOT EXISTS (SELECT 1 FROM sys.fulltext_catalogs WHERE name = N'BookHubFT')
BEGIN
    CREATE FULLTEXT CATALOG [BookHubFT] AS DEFAULT;
END;

IF EXISTS (
    SELECT 1
    FROM sys.fulltext_indexes fi
    INNER JOIN sys.objects o ON o.object_id = fi.object_id
    WHERE o.name = N'Profiles' AND SCHEMA_NAME(o.schema_id) = N'dbo'
)
    RETURN;

DECLARE @PkIndexName sysname;

SELECT TOP (1) @PkIndexName = i.name
FROM sys.indexes i
INNER JOIN sys.objects o ON o.object_id = i.object_id
WHERE o.name = N'Profiles'
  AND SCHEMA_NAME(o.schema_id) = N'dbo'
  AND i.is_primary_key = 1;

IF @PkIndexName IS NULL
    THROW 50001, 'Cannot create FULLTEXT INDEX: primary key index not found for dbo.Profiles.', 1;

DECLARE @Sql nvarchar(max) = N'
CREATE FULLTEXT INDEX ON [dbo].[Profiles]
(
    [FirstName],
    [LastName]
)
KEY INDEX ' + QUOTENAME(@PkIndexName) + N'
WITH CHANGE_TRACKING = AUTO;';

EXEC sp_executesql @Sql;
", suppressTransaction: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
IF FULLTEXTSERVICEPROPERTY('IsFullTextInstalled') <> 1
    RETURN;

IF EXISTS (
    SELECT 1
    FROM sys.fulltext_indexes fi
    INNER JOIN sys.objects o ON o.object_id = fi.object_id
    WHERE o.name = N'Profiles' AND SCHEMA_NAME(o.schema_id) = N'dbo'
)
BEGIN
    DROP FULLTEXT INDEX ON [dbo].[Profiles];
END;

IF EXISTS (SELECT 1 FROM sys.fulltext_catalogs WHERE name = N'BookHubFT')
AND NOT EXISTS (
    SELECT 1
    FROM sys.fulltext_indexes fi
    INNER JOIN sys.fulltext_catalogs fc ON fc.fulltext_catalog_id = fi.fulltext_catalog_id
    WHERE fc.name = N'BookHubFT'
)
BEGIN
    DROP FULLTEXT CATALOG [BookHubFT];
END;
", suppressTransaction: true);
    }
}
