using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class FullTextSearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF FULLTEXTSERVICEPROPERTY('IsFullTextInstalled') <> 1
    RETURN;

IF NOT EXISTS (SELECT 1 FROM sys.fulltext_catalogs WHERE name = N'BookHubFT')
BEGIN
    CREATE FULLTEXT CATALOG [BookHubFT] AS DEFAULT;
END;

IF NOT EXISTS (
    SELECT 1
    FROM sys.fulltext_indexes fi
    INNER JOIN sys.objects o ON o.object_id = fi.object_id
    WHERE o.name = N'Profiles' AND SCHEMA_NAME(o.schema_id) = N'dbo'
)
BEGIN
    DECLARE @ProfilesPkIndexName sysname;

    SELECT TOP (1) @ProfilesPkIndexName = i.name
    FROM sys.indexes i
    INNER JOIN sys.objects o ON o.object_id = i.object_id
    WHERE o.name = N'Profiles'
      AND SCHEMA_NAME(o.schema_id) = N'dbo'
      AND i.is_primary_key = 1;

    IF @ProfilesPkIndexName IS NULL
        THROW 50001, 'Cannot create FULLTEXT INDEX: primary key index not found for dbo.Profiles.', 1;

    DECLARE @ProfilesSql nvarchar(max) = N'
    CREATE FULLTEXT INDEX ON [dbo].[Profiles]
    (
        [FirstName],
        [LastName]
    )
    KEY INDEX ' + QUOTENAME(@ProfilesPkIndexName) + N'
    WITH CHANGE_TRACKING = AUTO;';

    EXEC sp_executesql @ProfilesSql;
END;

IF NOT EXISTS (
    SELECT 1
    FROM sys.fulltext_indexes fi
    INNER JOIN sys.objects o ON o.object_id = fi.object_id
    WHERE o.name = N'Genres' AND SCHEMA_NAME(o.schema_id) = N'dbo'
)
BEGIN
    DECLARE @GenresPkIndexName sysname;

    SELECT TOP (1) @GenresPkIndexName = i.name
    FROM sys.indexes i
    INNER JOIN sys.objects o ON o.object_id = i.object_id
    WHERE o.name = N'Genres'
      AND SCHEMA_NAME(o.schema_id) = N'dbo'
      AND i.is_primary_key = 1;

    IF @GenresPkIndexName IS NULL
        THROW 50002, 'Cannot create FULLTEXT INDEX: primary key index not found for dbo.Genres.', 1;

    DECLARE @GenresSql nvarchar(max) = N'
    CREATE FULLTEXT INDEX ON [dbo].[Genres]
    (
        [Name]
    )
    KEY INDEX ' + QUOTENAME(@GenresPkIndexName) + N'
    WITH CHANGE_TRACKING = AUTO;';

    EXEC sp_executesql @GenresSql;
END;

IF NOT EXISTS (
    SELECT 1
    FROM sys.fulltext_indexes fi
    INNER JOIN sys.objects o ON o.object_id = fi.object_id
    WHERE o.name = N'Books' AND SCHEMA_NAME(o.schema_id) = N'dbo'
)
BEGIN
    DECLARE @BooksPkIndexName sysname;

    SELECT TOP (1) @BooksPkIndexName = i.name
    FROM sys.indexes i
    INNER JOIN sys.objects o ON o.object_id = i.object_id
    WHERE o.name = N'Books'
      AND SCHEMA_NAME(o.schema_id) = N'dbo'
      AND i.is_primary_key = 1;

    IF @BooksPkIndexName IS NULL
        THROW 50003, 'Cannot create FULLTEXT INDEX: primary key index not found for dbo.Books.', 1;

    DECLARE @BooksSql nvarchar(max) = N'
    CREATE FULLTEXT INDEX ON [dbo].[Books]
    (
        [Title],
        [ShortDescription]
    )
    KEY INDEX ' + QUOTENAME(@BooksPkIndexName) + N'
    WITH CHANGE_TRACKING = AUTO;';

    EXEC sp_executesql @BooksSql;
END;

IF NOT EXISTS (
    SELECT 1
    FROM sys.fulltext_indexes fi
    INNER JOIN sys.objects o ON o.object_id = fi.object_id
    WHERE o.name = N'Articles' AND SCHEMA_NAME(o.schema_id) = N'dbo'
)
BEGIN
    DECLARE @ArticlesPkIndexName sysname;

    SELECT TOP (1) @ArticlesPkIndexName = i.name
    FROM sys.indexes i
    INNER JOIN sys.objects o ON o.object_id = i.object_id
    WHERE o.name = N'Articles'
      AND SCHEMA_NAME(o.schema_id) = N'dbo'
      AND i.is_primary_key = 1;

    IF @ArticlesPkIndexName IS NULL
        THROW 50004, 'Cannot create FULLTEXT INDEX: primary key index not found for dbo.Articles.', 1;

    DECLARE @ArticlesSql nvarchar(max) = N'
    CREATE FULLTEXT INDEX ON [dbo].[Articles]
    (
        [Title],
        [Introduction]
    )
    KEY INDEX ' + QUOTENAME(@ArticlesPkIndexName) + N'
    WITH CHANGE_TRACKING = AUTO;';

    EXEC sp_executesql @ArticlesSql;
END;

IF NOT EXISTS (
    SELECT 1
    FROM sys.fulltext_indexes fi
    INNER JOIN sys.objects o ON o.object_id = fi.object_id
    WHERE o.name = N'Authors' AND SCHEMA_NAME(o.schema_id) = N'dbo'
)
BEGIN
    DECLARE @AuthorsPkIndexName sysname;

    SELECT TOP (1) @AuthorsPkIndexName = i.name
    FROM sys.indexes i
    INNER JOIN sys.objects o ON o.object_id = i.object_id
    WHERE o.name = N'Authors'
      AND SCHEMA_NAME(o.schema_id) = N'dbo'
      AND i.is_primary_key = 1;

    IF @AuthorsPkIndexName IS NULL
        THROW 50005, 'Cannot create FULLTEXT INDEX: primary key index not found for dbo.Authors.', 1;

    DECLARE @AuthorsSql nvarchar(max) = N'
    CREATE FULLTEXT INDEX ON [dbo].[Authors]
    (
        [Name],
        [PenName]
    )
    KEY INDEX ' + QUOTENAME(@AuthorsPkIndexName) + N'
    WITH CHANGE_TRACKING = AUTO;';

    EXEC sp_executesql @AuthorsSql;
END;

IF NOT EXISTS (
    SELECT 1
    FROM sys.fulltext_indexes fi
    INNER JOIN sys.objects o ON o.object_id = fi.object_id
    WHERE o.name = N'Chats' AND SCHEMA_NAME(o.schema_id) = N'dbo'
)
BEGIN
    DECLARE @ChatsPkIndexName sysname;

    SELECT TOP (1) @ChatsPkIndexName = i.name
    FROM sys.indexes i
    INNER JOIN sys.objects o ON o.object_id = i.object_id
    WHERE o.name = N'Chats'
      AND SCHEMA_NAME(o.schema_id) = N'dbo'
      AND i.is_primary_key = 1;

    IF @ChatsPkIndexName IS NULL
        THROW 50006, 'Cannot create FULLTEXT INDEX: primary key index not found for dbo.Chats.', 1;

    DECLARE @ChatsSql nvarchar(max) = N'
    CREATE FULLTEXT INDEX ON [dbo].[Chats]
    (
        [Name]
    )
    KEY INDEX ' + QUOTENAME(@ChatsPkIndexName) + N'
    WITH CHANGE_TRACKING = AUTO;';

    EXEC sp_executesql @ChatsSql;
END;
", suppressTransaction: true);
        }

        /// <inheritdoc />
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

IF EXISTS (
    SELECT 1
    FROM sys.fulltext_indexes fi
    INNER JOIN sys.objects o ON o.object_id = fi.object_id
    WHERE o.name = N'Genres' AND SCHEMA_NAME(o.schema_id) = N'dbo'
)
BEGIN
    DROP FULLTEXT INDEX ON [dbo].[Genres];
END;

IF EXISTS (
    SELECT 1
    FROM sys.fulltext_indexes fi
    INNER JOIN sys.objects o ON o.object_id = fi.object_id
    WHERE o.name = N'Books' AND SCHEMA_NAME(o.schema_id) = N'dbo'
)
BEGIN
    DROP FULLTEXT INDEX ON [dbo].[Books];
END;

IF EXISTS (
    SELECT 1
    FROM sys.fulltext_indexes fi
    INNER JOIN sys.objects o ON o.object_id = fi.object_id
    WHERE o.name = N'Articles' AND SCHEMA_NAME(o.schema_id) = N'dbo'
)
BEGIN
    DROP FULLTEXT INDEX ON [dbo].[Articles];
END;

IF EXISTS (
    SELECT 1
    FROM sys.fulltext_indexes fi
    INNER JOIN sys.objects o ON o.object_id = fi.object_id
    WHERE o.name = N'Authors' AND SCHEMA_NAME(o.schema_id) = N'dbo'
)
BEGIN
    DROP FULLTEXT INDEX ON [dbo].[Authors];
END;

IF EXISTS (
    SELECT 1
    FROM sys.fulltext_indexes fi
    INNER JOIN sys.objects o ON o.object_id = fi.object_id
    WHERE o.name = N'Chats' AND SCHEMA_NAME(o.schema_id) = N'dbo'
)
BEGIN
    DROP FULLTEXT INDEX ON [dbo].[Chats];
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
}
