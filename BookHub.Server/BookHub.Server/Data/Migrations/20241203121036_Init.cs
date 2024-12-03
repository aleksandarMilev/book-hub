#nullable disable
#pragma warning disable CA1814 
namespace BookHub.Server.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Introduction = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Views = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nationalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nationalities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ResourceId = table.Column<int>(type: "int", nullable: false),
                    ResourceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SocialMediaUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Biography = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedBooksCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAuthorsCount = table.Column<int>(type: "int", nullable: false),
                    ReviewsCount = table.Column<int>(type: "int", nullable: false),
                    ReadBooksCount = table.Column<int>(type: "int", nullable: false),
                    ToReadBooksCount = table.Column<int>(type: "int", nullable: false),
                    CurrentlyReadingBooksCount = table.Column<int>(type: "int", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Profiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Biography = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    PenName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RatingsCount = table.Column<int>(type: "int", nullable: false),
                    AverageRating = table.Column<double>(type: "float", nullable: false),
                    NationalityId = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    BornAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authors_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Authors_Nationalities_NationalityId",
                        column: x => x.NationalityId,
                        principalTable: "Nationalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LongDescription = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    AverageRating = table.Column<double>(type: "float", nullable: false),
                    RatingsCount = table.Column<int>(type: "int", nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: true),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BooksGenres",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BooksGenres", x => new { x.BookId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_BooksGenres_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BooksGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReadingLists",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingLists", x => new { x.UserId, x.BookId, x.Status });
                    table.ForeignKey(
                        name: "FK_ReadingLists_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReadingLists_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Replies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Context = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Likes = table.Column<int>(type: "int", nullable: false),
                    Dislikes = table.Column<int>(type: "int", nullable: false),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReviewId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Replies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Replies_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Replies_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsUpvote = table.Column<bool>(type: "bit", nullable: false),
                    ReviewId = table.Column<int>(type: "int", nullable: false),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Votes_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Votes_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "Content", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "ImageUrl", "Introduction", "IsDeleted", "ModifiedBy", "ModifiedOn", "Title", "Views" },
                values: new object[,]
                {
                    { 1, "Stephen King's Pet Sematary is not just a horror story; it is a poignant exploration of grief, the inevitability of death, and the dark corners of the human psyche. At its heart, the novel is about Louis Creed, a young father who moves with his wife, Rachel, and their two children to a rural house in Maine. Soon after arriving, they discover a mysterious cemetery near their home, known as the Pet Sematary, where local children bury their beloved pets. What seems like a quaint, albeit eerie, tradition, becomes the setting for a nightmare that challenges Louis' understanding of life, death, and morality. \n\nThe cemetery behind the Creed family’s house is a place that defies the natural order, capable of bringing the dead back to life—albeit in a twisted, unnatural form. The temptation to reverse the inevitable loss of loved ones soon becomes too great to ignore. Louis, who is already grappling with the recent death of his son's cat, Church, learns that there is another cemetery—an even darker place—that has the power to raise humans from the dead. The price of this power, however, is steep, and the consequences far-reaching. When Louis makes the fateful decision to bring Church back from the grave, he begins a journey that will lead him down a path of moral corruption, despair, and irreversible tragedy. \n\nAs the story unfolds, King masterfully weaves themes of family, loss, and the struggles of parenthood. Louis' motivations are rooted in love for his family, particularly for his children, Ellie and Gage. His desire to protect them from the pain of loss becomes an overwhelming force, overshadowing his ability to consider the consequences. His wife Rachel, who harbors her own deep-seated fears about death due to the trauma of losing her sister Zelda to a debilitating illness, is caught in her own web of denial. She represents the fragility of human nature and the lengths people will go to avoid facing their darkest fears. The tension between Louis' desire to preserve life and Rachel’s unwillingness to confront death creates a tragic dynamic that becomes central to the novel’s emotional core. \n\nKing’s portrayal of death is particularly haunting in Pet Sematary. He doesn’t just focus on the physical act of dying; instead, he delves deeply into the psychological, emotional, and spiritual toll death takes on individuals and families. The novel is filled with gut-wrenching scenes that force readers to confront the reality that death is not just the end of a person’s life, but also the end of the relationships, dreams, and futures that are intertwined with them. Louis’ journey is a devastating reflection on the lengths people are willing to go to undo the pain of losing a loved one, even if it means sacrificing their humanity in the process. \n\nIn Pet Sematary, King’s signature blend of supernatural horror and psychological depth is on full display. The power of the cemetery is a metaphor for the human desire to control the uncontrollable—to bend fate to one’s will. The cemetery represents the dangerous temptation of hubris, a theme that has been explored in countless stories across literature and folklore. King takes this familiar theme and brings it to a new level of horror, showing how the attempt to reverse death leads not to salvation, but to destruction. The creatures that return from the cemetery are not the same as they were in life; they are tainted, twisted by the unnatural forces that brought them back. This raises unsettling questions about the nature of life and death—what makes us human, and what happens to us when the natural order is disrupted. \n\nOne of the most terrifying aspects of the novel is its exploration of the idea that some things are better left untouched. The moral lesson of Pet Sematary is a powerful warning about the dangers of trying to undo the irreversible. It serves as a meditation on the importance of accepting loss, moving through grief, and letting go. In trying to alter fate, Louis not only brings tragedy to his family, but also damns himself in the process. His decision is a desperate attempt to hold onto the past, but in doing so, he loses everything that matters in the present. The cost of defying death is the ultimate price: the loss of one’s soul. \n\nPet Sematary stands as one of King’s most deeply emotional works. It is a story that reminds us of the importance of embracing life’s fleeting moments and coming to terms with our own mortality. Though the novel is often categorized as a horror story, its true horror lies not in the supernatural events, but in the internal struggles of the characters—in their desperate attempts to avoid facing the unavoidable. King’s writing is a masterclass in creating tension and unease, building dread not just through external events, but through the complex emotional landscapes of his characters. The novel’s haunting final scenes linger long after the last page is turned, leaving readers to reflect on the true meaning of life, love, and loss.", null, new DateTime(2024, 12, 3, 14, 10, 36, 126, DateTimeKind.Local).AddTicks(1309), null, null, "https://thedailytexan.com/wp-content/uploads/2023/09/pet_sematary_bloodlines_4press-1200x800.jpg", "A tale of love, loss, and the chilling cost of defying death.", false, null, null, "Exploring the Haunting Depths of Pet Sematary", 0 },
                    { 2, "Harry Potter and the Deathly Hallows is not just the final book in J. K. Rowling’s iconic series, but the culmination of a journey that began with a young, orphaned wizard discovering his destiny in a world full of magic, mystery, and danger. The seventh and final installment brings to a close the epic battle between Harry Potter and the dark wizard Lord Voldemort, and it is a story that revolves around themes of love, sacrifice, loyalty, and the ultimate quest for truth. As the story reaches its climax, readers are taken on a journey not only through the magical world but also through the very heart of what it means to face evil, to stand up for what is right, and to embrace the unknown. \n\nIn The Deathly Hallows, Harry, Ron, and Hermione are no longer students at Hogwarts but fugitives on the run, tasked with finding and destroying Voldemort’s Horcruxes—objects that contain pieces of his soul and which must be destroyed in order to defeat him once and for all. The trio embarks on a perilous mission, not only to protect the wizarding world but also to safeguard their loved ones from the growing power of the Dark Lord. Their journey takes them deep into the heart of the magical world, uncovering secrets from the past that could alter the future forever. As they travel from one dangerous encounter to the next, Harry comes face to face with painful truths about his own family, the past, and his role in the final battle. \n\nThe book delves into the personal lives of its beloved characters. Harry's struggle is no longer about simply surviving the trials set before him—he is now the symbol of hope and resistance against the forces of evil. Throughout the story, Harry is confronted with the burden of being ‘the chosen one’—the person who must face Voldemort in a final confrontation. He learns that his greatest strength lies not in his ability to perform magic, but in his unwavering loyalty to his friends and his deep understanding of the importance of love. Ron and Hermione also undergo incredible growth as they wrestle with their own personal fears, relationships, and responsibilities. The trio’s bond strengthens, proving that their friendship is the key to their success. The sacrifices they make along the way are deeply moving and serve as a testament to the enduring power of love and loyalty. \n\nIn the search for the Horcruxes, the trio uncovers a series of interconnected mysteries, including the tale of the Deathly Hallows. The Deathly Hallows are three magical objects that are said to grant their possessor mastery over death itself. The Elder Wand, the Resurrection Stone, and the Invisibility Cloak are legendary artifacts, and their story is deeply tied to the larger battle between Harry and Voldemort. As the Hallows' significance is revealed, Harry finds himself caught between two paths: the desire to pursue the Hallows and the need to destroy Voldemort’s Horcruxes. His eventual understanding of the Hallows helps him realize that true power lies not in conquering death, but in embracing the love, selflessness, and choices that define humanity. \n\nOne of the most significant themes in The Deathly Hallows is the idea of sacrifice. Throughout the novel, the characters are faced with the ultimate test of what they are willing to give up in order to achieve victory. Harry’s willingness to sacrifice himself for the greater good is a powerful moment in the series, a testament to his maturity and understanding of the true cost of war. It is in these moments of self-sacrifice that the characters display their most heroic qualities, showing that heroism is not defined by the glory of battle, but by the quiet acts of bravery in the face of overwhelming odds. \n\nThe final battle at Hogwarts is the climax of the series, and it is a moment of immense emotional and narrative weight. The Death Eaters, led by Voldemort, lay siege to the school, forcing the students, teachers, and members of the Order of the Phoenix to stand and fight. The battle is not just a fight for survival—it is a fight for the future of the wizarding world. As the forces of good prepare for the ultimate confrontation, it becomes clear that the series has always been about more than just defeating evil. It is about fighting for a world where love, loyalty, and hope are the guiding principles. As characters who were once side players rise to take their place in the fight, readers are reminded that courage comes from unexpected places, and that even the smallest act of bravery can change the course of history. \n\nHarry Potter and the Deathly Hallows also explores the theme of legacy—what we leave behind, and how our actions shape the future. The end of the book sees the closure of several long-standing mysteries and storylines, while also providing the series with an optimistic, yet bittersweet conclusion. The future is uncertain, but the characters find peace knowing that they gave everything for a better world. Through the lens of loss, Rowling shows that the end is not necessarily the end, but a new beginning. The epilogue, set 19 years after the final battle, offers readers a glimpse of the next generation and how the lessons learned from the past will shape the future. Harry, Ron, and Hermione have grown, matured, and become parents, passing on their own legacies of courage, kindness, and love to their children. \n\nUltimately, Harry Potter and the Deathly Hallows is a story of hope—hope in the face of fear, in the face of evil, and in the face of the unknown. It is a story about growing up, about making choices, and about the power of love to overcome even the darkest forces. Rowling's storytelling is masterful, blending action, emotion, and depth in a way that resonates with readers of all ages. As the final chapter in the Harry Potter series, The Deathly Hallows delivers a powerful message: no matter how dark the world may seem, love and loyalty will always light the way.", null, new DateTime(2024, 12, 3, 14, 10, 36, 126, DateTimeKind.Local).AddTicks(1352), null, null, "https://choicefineart.com/cdn/shop/products/book-7-harry-potter-and-the-deathly-hallows-311816.jpg?v=1688079541", "The battle against Voldemort reaches its thrilling and emotional finale.", false, null, null, "The Epic Conclusion: Harry Potter and the Deathly Hallows", 0 },
                    { 3, "The Lord of the Rings is an epic fantasy novel by J. R. R. Tolkien, widely regarded as one of the greatest works of literature in the 20th century. The novel follows the journey of a young hobbit, Frodo Baggins, as he is tasked with destroying a powerful and corrupt artifact known as the One Ring, which was created by the Dark Lord Sauron to rule over all of Middle-earth. As the story unfolds, Frodo and his companions embark on an arduous and perilous quest to reach the fires of Mount Doom, where the Ring must be destroyed in order to defeat Sauron and save their world. However, the journey is far from simple—trapped by the corrupting influence of the Ring, the fellowship of travelers must navigate treachery, loss, and immense personal challenges, all while facing the growing darkness of Sauron's forces.\n\nThe story of The Lord of the Rings is one of epic adventure and enduring friendship. At the heart of the novel is the bond between Frodo and his loyal companion, Samwise Gamgee, whose unwavering devotion to Frodo and the quest drives the emotional core of the narrative. Together, along with Aragorn, Legolas, Gimli, and others, Frodo faces seemingly insurmountable odds as they travel through treacherous lands, battling forces of evil that threaten to consume the world. The fellowship's journey represents much more than just the destruction of the Ring—it is a story of personal growth, sacrifice, and the power of hope in the face of overwhelming darkness. \n\nTolkien's mastery of world-building is evident throughout the novel, as he brings to life a richly detailed world called Middle-earth. From the peaceful Shire to the dark, desolate lands of Mordor, every part of Middle-earth is immensely vivid, filled with complex histories, diverse cultures, and fascinating creatures. The novel introduces readers to a pantheon of memorable characters, from the wise and enigmatic Gandalf the wizard to the tragic and conflicted Gollum, whose obsession with the Ring adds an unsettling layer of tension to the story. Each character in the novel has their own role to play, and they all contribute to the overarching theme of the importance of teamwork, loyalty, and friendship in the face of an increasingly dark and divided world. \n\nOne of the central themes of The Lord of the Rings is the idea of power and its ability to corrupt. The One Ring, a seemingly simple object, has the power to control the minds and hearts of those who seek to possess it. As Frodo and his companions travel toward Mount Doom, they encounter individuals and groups who fall under the Ring's corrupting influence, whether they are Sauron's minions or those who believe they can control the Ring for good. The tragic story of Boromir, who succumbs to the Ring's temptation, serves as a powerful reminder of the destructive nature of unchecked ambition and the dangers of trying to wield power without understanding its true cost. In contrast, Frodo's struggle to resist the Ring's temptation demonstrates the power of selflessness and humility, as he refuses to let his own desires define the outcome of the quest. \n\nAnother important theme explored in the novel is the idea of sacrifice. Throughout The Lord of the Rings, characters are called upon to give up something precious for the greater good. Aragorn, the rightful king of Gondor, must step into a leadership role even as he struggles with his own doubts and fears. Frodo, Sam, and their companions make tremendous sacrifices—physically, emotionally, and morally—in order to see the quest through to the end. In particular, the burden of carrying the Ring weighs heavily on Frodo, and it is only through the unwavering loyalty of Sam that Frodo is able to complete the journey. Sam’s sacrifice is profound, as he puts his own desires and happiness aside to support Frodo through the darkest moments of their journey. \n\nThe novel is also deeply concerned with the power of hope and perseverance. Even in the darkest moments, when all seems lost, the characters persist in their quest. The struggle against Sauron and his forces is one of unyielding resistance against evil, and the power of the small, seemingly insignificant individuals in the story—such as Frodo and Sam—reminds readers that even the most humble among us can play a vital role in shaping the future. Tolkien also illustrates the importance of community and solidarity in the face of adversity, as characters from different races and backgrounds come together to fight for a common cause. The diverse Fellowship of the Ring exemplifies the value of cooperation, tolerance, and mutual respect, showing that unity is the key to overcoming even the greatest of challenges. \n\nAs the novel reaches its conclusion, the fate of Middle-earth hangs in the balance. In a climactic showdown at the Black Gate of Mordor, Frodo and Sam's courage and determination lead to the destruction of the Ring and the fall of Sauron. But the victory comes at a great cost. Many lives are lost, and the characters must reconcile the sacrifices they’ve made with the world that they have saved. The closing chapters of The Lord of the Rings are marked by both the triumph of good over evil and the melancholy recognition that the world has forever changed. \n\nIn the end, The Lord of the Rings is a story about the enduring power of friendship, the consequences of wielding great power, and the importance of selflessness, sacrifice, and hope. Tolkien's work transcends the boundaries of fantasy literature, offering a timeless and deeply resonant narrative that continues to captivate readers of all ages. Whether it is the humble courage of Frodo, the noble sacrifice of Aragorn, or the steadfast loyalty of Sam, The Lord of the Rings reminds us that in the face of darkness, there is always a light to guide us—if we are brave enough to seek it.", null, new DateTime(2024, 12, 3, 14, 10, 36, 126, DateTimeKind.Local).AddTicks(1355), null, null, "https://blogger.googleusercontent.com/img/b/R29vZ2xl/AVvXsEibI7_-Az0QVZhhwZO_PcgrNRK7RYnS7JPiddt_LvTC8NTgTzzYcaagGBLR6KtgY1J_VyZzS6HhL7MW9x1h-rioISPanc-daPbdgnZCQQb48PNELDt9gbQlohCJuXGHgritNS_3Ff08oUhs/w1200-h630-p-k-no-nu/acetolkien.jpg", "A journey of courage, friendship, and the fight against overwhelming darkness.", false, null, null, "The Timeless Epic: The Lord of the Rings", 0 },
                    { 4, "The Shining is a psychological horror novel by Stephen King, first published in 1977. The story follows Jack Torrance, an aspiring writer and recovering alcoholic, who takes a job as the winter caretaker of the Overlook Hotel, a remote resort nestled in the mountains of Colorado. Jack moves to the hotel with his wife Wendy and his young son Danny, who possesses a psychic ability called 'the shining.' As the winter weather isolates the family from the outside world, the Overlook's dark, haunted history begins to unravel, and the hotel's malevolent supernatural forces begin to take control of Jack, pushing him toward violence and madness. What begins as a simple family retreat spirals into a nightmare of terror and survival, as Danny and Wendy fight to escape the hotel's deadly grip. \n\nAt the core of The Shining is the gradual descent of Jack Torrance into madness. A complex and deeply flawed character, Jack is haunted by his past mistakes, including his struggles with alcoholism and his abusive relationship with his family. As Jack becomes more influenced by the sinister forces within the hotel, his inner turmoil and his violent tendencies are amplified, and he slowly becomes a threat to his own family. The novel portrays the terrifying effects of isolation, desperation, and personal demons, making Jack's descent into madness both heartbreaking and horrifying. \n\nThe relationship between Jack and his family, particularly his wife Wendy and son Danny, is one of the most important elements of the novel. Wendy is a strong and resourceful character, doing everything she can to protect her son and preserve the family unit in the face of increasing danger. Danny, with his psychic ability, serves as both a victim and a savior, as his 'shining' allows him to sense the evil forces at work within the hotel. Danny's bond with Wendy is central to the emotional core of the novel, as they both struggle to survive against the malevolent force trying to consume them. Their relationship is a powerful representation of love and loyalty in the face of unimaginable horror. \n\nKing's mastery of psychological horror is evident in the novel's chilling atmosphere, which builds steadily throughout the story. The Overlook Hotel, with its vast, empty halls and eerie, oppressive presence, serves as a character in its own right, slowly driving Jack to madness and serving as a dark mirror to the family's fractured relationships. King uses the setting to evoke a constant sense of dread, with the hotel's ghosts and supernatural phenomena representing both the past sins of its former occupants and the growing malevolence that threatens the Torrance family. \n\nOne of the central themes of The Shining is the idea of the supernatural and its influence on human behavior. While the novel is filled with terrifying encounters and ghostly apparitions, the true horror lies in how the hotel’s evil influences the minds of its inhabitants. The Overlook represents the corruption of power, and its ability to manipulate Jack into violence is a reflection of how external forces can prey on internal vulnerabilities. The story also delves into the idea of destiny, as Danny’s 'shining' allows him to foresee the danger that lies ahead, even though he cannot fully comprehend the extent of the evil within the hotel. Danny’s ability to sense the past, present, and future creates a sense of fate and inevitability that permeates the novel. \n\nAnother key theme in The Shining is the effect of isolation on the human psyche. Trapped in a remote location with no contact with the outside world, the Torrance family is forced to confront their personal demons and unravel the mysteries of the Overlook. Jack’s internal struggles, combined with the hotel’s supernatural power, push him further towards violence and madness. The novel explores the ways in which the isolation of the winter season, coupled with the psychological weight of past trauma, creates an environment ripe for horror. \n\nHope and survival are also prominent themes, as Danny and Wendy's perseverance and love for each other become their only means of survival. While Jack succumbs to the hotel's power, Danny and Wendy fight with everything they have to escape the nightmare. Their struggle against the Overlook represents the resilience of the human spirit in the face of overwhelming evil. \n\nAs the novel reaches its climactic moments, the tension reaches its peak. In a final confrontation between father and son, Danny’s psychic abilities and Wendy’s determination lead to the destruction of the malevolent forces within the hotel, but not without significant cost. Jack, overwhelmed by the power of the Overlook, meets his tragic end, and the family must leave the hotel behind, scarred but alive. The conclusion of The Shining leaves readers with a sense of both horror and catharsis, as the characters escape the hotel's grip but are forever haunted by the events they experienced. \n\nIn the end, The Shining is a story about the fragility of the human mind, the destructive nature of addiction, and the dangerous consequences of isolation. Stephen King's novel delves into the darkness of the human soul, showing how the line between reality and madness can blur in the face of intense fear and pressure. It remains one of King’s most haunting and enduring works, cementing his reputation as the master of psychological horror. Whether it's the suffocating atmosphere of the Overlook or the chilling unraveling of Jack Torrance's sanity, The Shining continues to captivate and terrify readers, proving that sometimes the greatest horror lies not in what we see, but in what we cannot escape from within.", null, new DateTime(2024, 12, 3, 14, 10, 36, 126, DateTimeKind.Local).AddTicks(1357), null, null, "https://kcopera.org/wp-content/uploads/2024/03/the-shining-recording-page-banner.jpg", "A chilling exploration of isolation, madness, and the supernatural, The Shining takes readers deep into the heart of terror.", false, null, null, "The Haunting Legacy of The Shining", 0 },
                    { 5, "1984 is a dystopian novel by George Orwell, first published in 1949. The novel is set in a totalitarian society ruled by the Party, a tyrannical government led by the figurehead Big Brother. In this world, individual freedom is suppressed, and citizens are constantly monitored by surveillance, subjected to propaganda, and manipulated into submission. The protagonist, Winston Smith, works for the Party rewriting history in order to align it with the Party’s ever-changing narrative. However, Winston harbors rebellious thoughts, longing for truth and freedom. As Winston embarks on a secret affair with Julia, a fellow Party member, and secretly seeks to resist Big Brother’s rule, he becomes entangled in a dangerous game of deception and surveillance, where the very concept of truth is in constant flux. \n\nAt the heart of 1984 is the theme of oppression and the obliteration of individuality. The Party’s control over every aspect of life is absolute: from the daily routines of citizens to the language they speak (Newspeak), the Party seeks to mold every individual thought and action. Winston’s journey is one of self-awareness, as he questions the reality that has been imposed upon him. Orwell paints a bleak picture of a world where even personal thoughts are not safe from the reach of the Party. The novel explores the horrifying consequences of a society where individuality is eradicated and citizens are reduced to mere cogs in the machinery of an all-powerful state. \n\nThe Party's control is not just through physical surveillance, but also through the manipulation of truth. One of the most striking features of Orwell's novel is the concept of doublethink—the ability to hold two contradictory beliefs at the same time and accept both as true. This is central to the Party’s strategy of controlling thought and creating a reality where nothing is ever truly certain. Winston works at the Ministry of Truth, where his job is to falsify records, erasing any evidence of the Party's lies. This manipulation of history reflects the Party’s need to control not only the present but also the past, erasing any evidence that could challenge its authority. In this world, truth is not based on objective reality but on whatever the Party decrees it to be at any given moment. \n\nOne of the central themes of 1984 is the loss of personal freedom and autonomy. The Party's dominance extends to every facet of life, creating a society where even private thoughts are policed. The concept of ‘thoughtcrime,’ the act of thinking against the Party, is one of the most terrifying elements of the novel. Winston's rebellion against the Party is not just physical but intellectual—he dares to think independently, to question the society he lives in. However, the Party's control is so complete that even Winston’s secret rebellion is eventually quashed, demonstrating the terrifying power of totalitarianism. \n\nOrwell also explores the dangers of surveillance in 1984. The Party’s slogan, ‘Big Brother is watching you,’ is a constant reminder that no one is ever truly alone or free. Telescreens, microphones, and constant surveillance are woven into the fabric of daily life. This omnipresent watchfulness stifles any possibility of rebellion, as the fear of being caught leads to self-censorship. In 1984, privacy is a foreign concept, and the government’s ability to monitor every action and thought creates a society in which no one can trust anyone, not even themselves. \n\nAnother theme in 1984 is the manipulation of language. The Party’s development of Newspeak, a language designed to eliminate words that could foster subversive thoughts, demonstrates how language can shape and control reality. Through Newspeak, the Party seeks to limit the range of thought, making it impossible for people to even think critically about the regime. By controlling language, the Party not only controls communication but also dictates how people are allowed to think, further solidifying its power over every aspect of life. \n\nAt its core, 1984 is a novel about the struggle for truth and freedom in a world where both are systematically destroyed. Winston’s quest for knowledge, individuality, and truth leads him to defy the Party, but in the end, he is broken, and the Party’s grip on reality is solidified. The tragic ending of the novel highlights the total power of the regime and the utter futility of resistance. Winston’s final acceptance of Big Brother is a horrifying testament to the complete domination of the Party over the minds and souls of its citizens. \n\nThe novel’s bleak conclusion leaves readers with a powerful message about the dangers of unchecked power, totalitarianism, and the erosion of personal freedoms. Orwell's vision of the future in 1984 serves as a warning of the potential consequences of oppressive regimes and the fragility of democracy. The novel's exploration of surveillance, mind control, and the manipulation of truth remains strikingly relevant today, as issues of government surveillance, fake news, and the battle for individual freedoms continue to challenge societies worldwide. \n\nIn the end, 1984 is not just a warning about a specific future but an exploration of the enduring struggle between freedom and oppression. Orwell’s chilling narrative of Winston’s life under Big Brother’s watchful eye remains a powerful and thought-provoking work, urging readers to consider the value of truth, individuality, and liberty in the face of ever-growing authoritarianism.", null, new DateTime(2024, 12, 3, 14, 10, 36, 126, DateTimeKind.Local).AddTicks(1359), null, null, "https://s3.amazonaws.com/adg-bucket/1984-george-orwell/3423-medium.jpg", "A chilling exploration of totalitarianism, surveillance, and the loss of individuality, 1984 presents a terrifying vision of the future.", false, null, null, "The Dystopian Reality of 1984", 0 },
                    { 6, "Dracula, written by Bram Stoker and published in 1897, is the quintessential gothic horror novel, blending superstition, desire, and dread into an unforgettable narrative. The story centers around Count Dracula, a centuries-old vampire who embarks on a journey from Transylvania to England, seeking to spread his undead curse. His arrival in England sets off a deadly chain of events, as Dracula begins his relentless pursuit of Mina Harker and her friends, leading to a fierce battle between good and evil. The novel’s form—presented through diary entries, letters, newspaper clippings, and a ship’s log—creates an immersive experience that pulls the reader into the horrifying world of Dracula’s curse. \n\nAt its core, Dracula explores the boundaries between the human world and the supernatural, dissecting themes of death, immortality, and forbidden desire. Count Dracula, an ancient and malevolent force, represents the ultimate fear of death’s corruption and the terrifying power of the undead. With his eerie castle in Transylvania, Dracula lives outside the rules of nature and morality, existing as both a seductive and monstrous figure. His very existence is an affront to the natural order, and throughout the novel, he manipulates and controls those around him to serve his insatiable hunger for blood and power. \n\nDracula’s pursuit of Mina is not just a physical conquest, but a spiritual and psychological one. The vampire’s ability to manipulate minds, corrupt souls, and sow doubt in the hearts of the innocent becomes the primary means by which he attacks. Dracula is a master of fear, slowly draining the vitality and willpower of those around him, especially Mina, who represents purity and virtue. Her transformation from an innocent young woman to a creature tainted by Dracula’s curse serves as a central theme of the novel—one that raises questions about sexual desire, repression, and the dangers of unchecked lust. Dracula’s actions echo the sexual fears of the Victorian era, as he embodies the darker, more forbidden aspects of human nature. His supernatural abilities allow him to seduce and dominate, transforming his victims into mindless, bloodthirsty followers, turning the act of bloodletting into an intimate and terrifying ritual. \n\nThe battle against Dracula is not just a fight against an individual monster, but a reflection of the ongoing struggle between civilization and savagery. The vampire represents the unrestrained forces of nature, while the protagonists—Jonathan Harker, Mina, Professor Van Helsing, Lucy, and others—symbolize the civilized world, with its belief in logic, science, and morality. Throughout the novel, the characters’ reliance on reason and rationality is tested, as Dracula’s supernatural abilities defy all understanding. However, the struggle also reveals the limits of science and the need for faith, love, and bravery in the face of unimaginable evil. As the group comes together to confront Dracula, they form a united front, using their collective strengths to overcome the vampire’s growing power. \n\nThe gothic atmosphere in Dracula is perhaps one of its most compelling features. Stoker’s descriptions of the eerie Transylvanian landscape—dark, foreboding mountains, deep forests, and the imposing Castle Dracula—imbue the novel with an almost tangible sense of dread. The supernatural horror that pervades the narrative is not just confined to Dracula’s castle but extends to England itself, where the vampire’s presence warps reality and disrupts the lives of those he encounters. In London, the fog, the shadows, and the sense of pervasive danger create a setting that feels suffocating, and the characters must constantly struggle against the unseen forces that threaten to engulf them. The tension between the known world and the unknown, represented by Dracula’s powers, creates an atmosphere of claustrophobic terror. \n\nIn addition to the terrifying supernatural elements, Dracula explores the concept of forbidden knowledge and the limits of human understanding. As the protagonists discover the vampire’s nature, they must challenge their own preconceived notions about life, death, and the supernatural. This journey into the unknown mirrors the fear of losing control over one’s own fate and the terrifying realization that knowledge—when it comes to creatures like Dracula—might be too dangerous to comprehend. Van Helsing, as the novel’s intellectual leader, is both a man of science and a believer in the supernatural, and his attempts to rationalize Dracula’s power show the tension between reason and faith. \n\nMina Harker, though a central figure in the novel, represents something more than just a victim or love interest. Her transformation throughout the story reveals the fragility of innocence when confronted with evil. Her ability to resist Dracula’s influence, even as she is drawn closer to him, symbolizes the strength of human will and the power of love and friendship. It is her intellect, compassion, and the support of her companions that ultimately play a pivotal role in Dracula’s downfall. Mina’s character challenges the stereotypical victimized woman in gothic literature, as she not only survives but also becomes one of the strongest and most resilient figures in the fight against Dracula. \n\nThe male characters, especially Jonathan Harker, are tested by their journey into Dracula’s world. Harker, who initially seeks adventure and romance, quickly realizes the terrifying truth about his situation. His journals document not just the physical horror he faces but also the psychological toll of being trapped in Dracula’s lair. His struggle to escape is both a literal and metaphorical journey toward freedom and survival. The camaraderie and bond shared by the group of characters as they work to defeat Dracula is one of the novel’s most powerful elements. The strength of their unity contrasts sharply with Dracula’s isolation, as he is portrayed as a solitary, ancient figure who is ultimately undone by the collective strength of human connection and resolve. \n\nOne of the novel’s most disturbing features is the symbolic power of blood. Dracula’s thirst for blood, and the act of feeding, is a central metaphor for the parasitic nature of evil and the corrupting influence of the vampire. Blood in Dracula represents not only the loss of life but the loss of autonomy and identity. The act of blood-drinking is not just physical; it is an intimate invasion, one that turns the victim into a mere shadow of themselves. The vampire’s bite is symbolic of the breakdown of moral and spiritual boundaries, and Dracula’s quest to transform others into vampires represents the spread of evil and the destruction of what makes someone human. \n\nAt its conclusion, Dracula offers a sobering reflection on the battle between light and dark, life and death. The pursuit of Dracula, while ultimately successful, is fraught with loss, and the victory is bittersweet. The final moments, in which Dracula meets his demise, emphasize that evil, even when defeated, leaves a permanent scar on the world. The legacy of Dracula’s evil lingers in the characters’ lives, reminding them—and readers—that evil never truly disappears. The conclusion reinforces the novel’s enduring message about the fragility of life, the importance of moral courage, and the constant need to confront the darkness that lurks both outside and within. \n\nUltimately, Dracula is more than just a horror novel. It is a reflection on the nature of good and evil, the dangers of unchecked power, and the fear of the unknown. Bram Stoker’s masterpiece has become a timeless work of gothic fiction, influencing countless adaptations and interpretations. Its exploration of desire, fear, and the supernatural continues to resonate with readers today, making Dracula a novel that stands as both a product of its time and an ever-relevant commentary on the darkness that exists within humanity. Its tale of terror, love, and sacrifice remains a chilling reminder of the eternal struggle between the forces of darkness and the light of human resilience.", null, new DateTime(2024, 12, 3, 14, 10, 36, 126, DateTimeKind.Local).AddTicks(1361), null, null, "https://miro.medium.com/v2/resize:fit:1000/1*3US2qS3OLXELxVA0AEVZxQ.jpeg", "A chilling exploration of darkness, fear, and the supernatural, Dracula by Bram Stoker unravels a tale of obsession, seduction, and the eternal battle between life and death.", false, null, null, "The Eternal Horror of Dracula: A Deep Dive into Gothic Terror", 0 },
                    { 7, "First published in 1962, A Clockwork Orange by Anthony Burgess is a provocative and unsettling work that explores the complex intersection of free will, violence, and the role of society in shaping individual behavior. Set in a dystopian future, the novel follows Alex, a young delinquent with a penchant for violence, and his journey from cruelty to a forced form of 'rehabilitation.' Through its unsettling narrative, Burgess invites readers to question the nature of morality and the boundaries of human freedom. \n\nThe novel’s central theme revolves around the concept of free will. Alex, the protagonist, is a violent teenager who delights in criminal activities, yet when he is arrested and subjected to an experimental treatment to eliminate his desire for violence, the novel shifts its focus to a deeper philosophical question: what does it mean to truly be human if free will is taken away? The treatment strips Alex of his autonomy, forcing him to exist as a ‘clockwork orange,’ a machine-like creature devoid of the ability to choose between good and evil. Burgess, through this stark transformation, forces readers to grapple with the ethical question: Is it better to choose evil than to be deprived of the ability to choose at all? \n\nThe novel is known for its distinctive use of Nadsat, a fictional language created by Burgess, which blends Russian slang and English to give the novel a unique and disorienting atmosphere. The language serves as both a tool of alienation and a means of immersing the reader in Alex's world. Nadsat is central to the novel's tone, and its use emphasizes the youthful rebellion of the characters, as well as the gap between generations. It creates a sense of disconnection and unease, reflecting the novel's broader themes of societal breakdown and the alienation of the individual. \n\nViolence is a key element throughout A Clockwork Orange, not only as an action but as a social commentary. Burgess portrays a world in which violence has become normalized, whether through the actions of Alex and his friends or through the oppressive methods of state control. The novel doesn't shy away from the brutality of its characters' actions, including graphic scenes of murder, assault, and psychological torment. These scenes are designed not to shock for shock's sake, but to underline the pervasive violence in the world Burgess creates—a world where violence is both a tool of rebellion and a tool of control. \n\nThe dystopian society in A Clockwork Orange is one where the government attempts to manipulate and control its citizens in the name of societal order. Alex's forced treatment, known as Ludovico's Technique, is an extreme measure of government intervention aimed at rehabilitating criminals through psychological conditioning. However, the treatment raises the question of whether it is ethical to suppress an individual's free will, even if it is in the name of societal good. Through the protagonist’s experiences, Burgess critiques a society where the state has too much power over its citizens, and the individual’s autonomy is sacrificed for the illusion of peace. \n\nAt its heart, A Clockwork Orange is a novel about the tension between human freedom and societal control. The novel examines the implications of taking away an individual's ability to choose, forcing readers to question whether a life without free will is truly worth living. Is it more humane to allow someone to make their own choices, even if they are harmful, or is it better to impose conformity through control? The novel does not offer easy answers but instead leaves readers to grapple with the complex interplay of morality, freedom, and control. \n\nOne of the most compelling aspects of A Clockwork Orange is its exploration of the idea of redemption. Although Alex is subjected to a brutal treatment, the novel ultimately hints at the possibility of his redemption. In the final chapters, Alex begins to express a desire to leave his violent past behind, suggesting that even in a world filled with violence and corruption, there is room for change and growth. The final lines of the novel reflect Alex’s yearning for a better life, a future beyond his criminal past, and his ultimate choice to embrace maturity. \n\nA Clockwork Orange is as much a work of philosophy as it is of literature. It’s a fierce critique of the systems of control and authority that dominate society, but it’s also a profound meditation on the nature of free will and the human condition. Through Alex’s journey, Burgess poses difficult questions about the right to choose and the consequences of living in a world that seeks to eliminate choice. \n\nThe novel’s conclusion, with Alex’s eventual maturation and recognition of the need for change, serves as a reminder of the importance of personal agency. Despite the society around him, Alex realizes that true freedom lies in the ability to choose a different path, a reflection of hope even in the darkest of worlds. In the end, A Clockwork Orange remains a challenging and controversial work, but one that leaves a lasting impact on its readers, compelling them to think critically about violence, free will, and the role of society in shaping human behavior. \n\nA Clockwork Orange continues to resonate today as a powerful exploration of violence, control, and the power of individual choice. Its controversial themes, innovative language, and unforgettable characters have made it a defining work in the dystopian genre, ensuring its place as one of the most influential novels of the 20th century.", null, new DateTime(2024, 12, 3, 14, 10, 36, 126, DateTimeKind.Local).AddTicks(1364), null, null, "https://portsmouthreview.com/wp-content/uploads/2019/01/clockworkorange-1160x774.png", "A dark and thought-provoking exploration of free will, violence, and societal control, A Clockwork Orange by Anthony Burgess challenges perceptions of morality and human nature.", false, null, null, "The Dystopian Horror of A Clockwork Orange: A Deep Dive into Violence and Free Will", 0 },
                    { 8, "First published in 1934, Murder on the Orient Express is one of Agatha Christie’s most celebrated novels. The story unfolds aboard the famous Orient Express train, where an American millionaire, Samuel Ratchett, is found murdered in his compartment. The Belgian detective Hercule Poirot, who is traveling on the same train, is called upon to investigate the crime. As Poirot conducts his investigation, he uncovers a tangled web of lies and hidden secrets, with each passenger harboring a possible motive. \n\nThe novel is a brilliant example of Christie's skill in misdirection and plotting. Through her use of red herrings, multiple perspectives, and a carefully constructed set of clues, Christie keeps the reader guessing until the very end. The structure of the narrative, with Poirot’s methodical examination of the passengers and their testimonies, allows for a deep dive into human psychology and the complexities of moral judgment. \n\nAt its core, Murder on the Orient Express explores themes of justice and revenge. Throughout the story, Poirot is faced with difficult questions about what constitutes true justice and whether revenge is ever justified. As the mystery unravels, Poirot is forced to confront the moral ambiguity of the case, as he discovers that the passengers may not be what they initially appear to be. \n\nThe book's plot twist, revealed in the final pages, is one of the most famous in literary history. Christie’s ability to deceive and surprise her readers with such a shocking conclusion is a testament to her mastery of the genre. The revelation forces readers to reconsider the entire narrative and question the concept of justice and fairness.\n\nIn addition to its plot, Murder on the Orient Express is filled with rich characterizations. Poirot, known for his meticulous nature and sharp intellect, is presented as both a compassionate and calculating figure, contrasting with the colorful array of passengers aboard the train. The novel also features a variety of personalities, from the aristocratic to the common folk, each adding depth to the story and providing potential suspects for Poirot’s investigation.\n\nMurder on the Orient Express has remained a beloved classic for decades, inspiring numerous adaptations in film, television, and theater. Its themes of morality, justice, and revenge continue to resonate with readers, making it an essential work in the detective genre. Agatha Christie’s iconic writing and the unforgettable twists of this tale ensure that it remains a definitive example of the mystery novel.", null, new DateTime(2024, 12, 3, 14, 10, 36, 126, DateTimeKind.Local).AddTicks(1366), null, null, "https://images-na.ssl-images-amazon.com/images/I/81voh8NSRSL._AC_UL210_SR210,210_.jpg", "Agatha Christie’s Murder on the Orient Express is a classic detective novel that remains one of the most iconic works in the mystery genre. With Hercule Poirot at the helm, the story takes readers on a journey through the labyrinth of human motives, justice, and revenge, all set against the opulent backdrop of a luxurious train ride.", false, null, null, "Murder on the Orient Express: A Masterclass in Mystery and Morality", 0 },
                    { 9, "First published in 1937, Of Mice and Men is one of John Steinbeck's most famous works. Set against the backdrop of the Great Depression, the novel follows George Milton and Lennie Small, two migrant workers traveling through California in search of work. Despite their challenging lives, they share a common dream: to one day own a piece of land and live off it in peace.\n\nThe novel explores themes of loneliness, dreams, and human dignity. Steinbeck presents the harshness of the world through the lens of George and Lennie's relationship, highlighting the disparity between their hopes and the grim reality of their lives. Lennie, a large and mentally disabled man, depends on George for guidance and protection. George, in turn, takes on the responsibility of caring for Lennie, making their bond one of both friendship and survival.\n\nThrough a cast of other characters on the ranch, Steinbeck portrays the isolation and yearning for connection felt by many during the Great Depression. Characters like Candy, Crooks, and Curley's wife each grapple with their own desires for companionship and a better life, yet they are trapped by their circumstances.\n\nOf Mice and Men also examines the theme of powerlessness. The characters, particularly George and Lennie, are often powerless to change their fates due to the social and economic conditions of the time. Steinbeck uses their story to comment on the limitations of the American Dream, especially for marginalized individuals.\n\nThe novel’s tragic conclusion, with George making a heartbreaking decision about Lennie’s fate, serves as a powerful commentary on the human condition. It forces readers to confront the sacrifices individuals make for love, loyalty, and friendship, while also questioning the cost of these decisions.\n\nUltimately, Of Mice and Men is a tale about the fragility of human dreams and the deep need for companionship, making it a timeless classic that resonates with readers across generations. Steinbeck’s masterful storytelling and deep empathy for his characters ensure the novel’s place in the canon of American literature.", null, new DateTime(2024, 12, 3, 14, 10, 36, 126, DateTimeKind.Local).AddTicks(1368), null, null, "https://m.media-amazon.com/images/I/81nuvmpbEdL._AC_UF1000,1000_QL80_.jpg", "John Steinbeck's Of Mice and Men is a poignant tale of friendship, dreams, and the harsh realities of the American Depression. It chronicles the lives of two displaced migrant workers, George and Lennie, who dream of a better life, yet struggle against the societal and economic forces that limit their aspirations.", false, null, null, "Of Mice and Men: The Tragic Story of Friendship and Dreams", 0 },
                    { 10, "Published in 1965, Dune is considered one of the greatest works of science fiction ever written. Set on the desert planet of Arrakis, the novel follows Paul Atreides, a young nobleman whose family is entrusted with the stewardship of the planet and its spice production. The spice melange, which is essential for space travel and prolonging life, makes Arrakis the most important planet in the universe. As Paul’s family navigates the dangerous political landscape of the planet, they are drawn into a web of betrayal, rebellion, and prophecy.\n\nAt its core, Dune is a story about power, both its acquisition and its consequences. Paul Atreides is not only struggling with his family's political legacy but also with his own destiny. As he uncovers his latent abilities and learns of the prophecy surrounding him, he must decide what role he will play in the fate of Arrakis and its people.\n\nThe novel is also a profound exploration of ecology and environmentalism. Herbert presents the planet Arrakis as a harsh and unforgiving world, where water is scarce, and the survival of its inhabitants depends on their ability to adapt to the land. Through his vivid descriptions of the desert landscape and the Fremen people who inhabit it, Herbert conveys a deep respect for the delicate balance of ecosystems and the consequences of exploiting natural resources.\n\nOne of the most striking aspects of *Dune* is its intricate world-building. Herbert creates a universe with multiple layers of complexity, from the politics of the noble houses to the religious beliefs of the Fremen to the guild of Navigators who control space travel. The conflict between these factions, each with their own agenda, forms the backdrop of the novel's central drama. Herbert's keen insight into human nature and his ability to weave together these various strands of conflict make *Dune* a deeply engaging and thought-provoking read.\n\nHowever, Dune is not only about power and politics. The novel also examines themes of human evolution, mysticism, and the role of religion in shaping societies. Paul’s journey to embrace his destiny involves both physical and mental transformation, leading to questions about free will, fate, and the responsibilities of leadership.\n\nHerbert’s prose is rich and dense, and while some readers may find the novel’s depth challenging, its rewards are immense. The novel concludes with a climactic and unforgettable moment that sets the stage for the series’ subsequent books.\n\nUltimately, Dune is a tale of survival, leadership, and the burden of destiny. It is a novel that encourages readers to reflect on the power dynamics within our own societies and the ways in which human beings relate to the environment and to each other. Frank Herbert’s Dune continues to inspire readers and adaptations, solidifying its place as a landmark achievement in science fiction literature.", null, new DateTime(2024, 12, 3, 14, 10, 36, 126, DateTimeKind.Local).AddTicks(1370), null, null, "https://i.etsystatic.com/16102212/r/il/2482ff/2771009725/il_570xN.2771009725_chlb.jpg", "Frank Herbert's *Dune* is a monumental science fiction saga that explores the complex interplay of politics, religion, and ecology on the desert planet of Arrakis. The novel delves into the struggle for control of the universe's most valuable substance, the spice melange, while exploring themes of leadership, destiny, and environmentalism.", false, null, null, "Dune: The Epic Tale of Power, Ecology, and Human Destiny", 0 },
                    { 11, "First published in 1979, The Hitchhiker's Guide to the Galaxy is one of the most beloved and influential works of science fiction, known for its unique combination of comedy, satire, and cosmic adventure. The novel introduces readers to Arthur Dent, a regular, unassuming man who finds himself thrust into an absurd journey after Earth is destroyed to make way for a hyperspace bypass. Arthur is rescued by Ford Prefect, a researcher for the titular Guide, who takes him on a space-faring adventure filled with bizarre characters and strange situations.\n\nThe novel is filled with humor that both mocks and celebrates the absurdities of life. Adams' distinctive wit shines through in every chapter, as he presents the reader with outlandish scenarios and deadpan observations. The narrative is packed with tongue-in-cheek moments, from the eccentric Vogon bureaucrats to the two-headed, three-armed Zaphod Beeblebrox. The absurdity is paired with sharp social commentary, as Adams questions human existence, bureaucracy, and the nature of the universe.\n\nAt its core, The Hitchhiker's Guide to the Galaxy is a philosophical exploration of life, the universe, and everything—literally. The novel's famous phrase 'Don't Panic' is a tongue-in-cheek mantra that perfectly encapsulates the book’s view on life’s uncertainties. The story delves into questions about meaning, fate, and chance, all while keeping the reader laughing along the way.\n\nThe impact of The Hitchhiker's Guide to the Galaxy is immense. It has influenced generations of writers, comedians, and filmmakers, becoming a cornerstone of popular culture. Adams' work has been adapted into various formats, including radio shows, television series, stage plays, and a feature film. Despite these adaptations, the original book remains the definitive version of the story, beloved by fans for its ingenious blend of humor, science fiction, and philosophical exploration.\n\nIn conclusion, The Hitchhiker's Guide to the Galaxy is more than just a science fiction novel—it’s a literary treasure that encourages readers to laugh, think, and reflect on the absurdity of life. Its timeless humor and deep questions about existence continue to resonate with readers around the world, making it a must-read for fans of science fiction and comedy alike.", null, new DateTime(2024, 12, 3, 14, 10, 36, 126, DateTimeKind.Local).AddTicks(1373), null, null, "https://cdn2.penguin.com.au/covers/original/9780434023394.jpg", "Douglas Adams' The Hitchhiker's Guide to the Galaxy is a classic blend of science fiction, absurdity, and humor. The story follows Arthur Dent as he is suddenly swept off Earth just before its destruction, leading to a wild and philosophical adventure through space.", false, null, null, "The Hitchhiker's Guide to the Galaxy: A Journey Through Absurdity and Wit", 0 }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "Email", "EmailConfirmed", "IsDeleted", "LockoutEnabled", "LockoutEnd", "ModifiedBy", "ModifiedOn", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "user1Id", 0, "b2103e35-14ab-4151-acfd-a8a91436aad9", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "user1@mail.com", false, false, false, null, null, null, null, null, null, null, false, "566d4322-8b02-4508-9c55-51ca70404efd", false, "user1name" },
                    { "user2Id", 0, "805d4a82-45d3-4f39-8cb9-9ef263e8cc31", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "user2@mail.com", false, false, false, null, null, null, null, null, null, null, false, "b83b1f47-6e04-4dc3-8740-b1830dfa765a", false, "user2name" },
                    { "user3Id", 0, "911aa436-31fc-4653-84e9-09bef7301db7", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "user3@mail.com", false, false, false, null, null, null, null, null, null, null, false, "b90d305f-6f8d-404f-9c7f-9923b56492c5", false, "user3name" },
                    { "user4Id", 0, "842201e0-8ec6-4f9e-aba5-ae4f8b3fa6f8", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "user4@mail.com", false, false, false, null, null, null, null, null, null, null, false, "e67adc78-0a61-478d-b997-43e0c51e9419", false, "user4name" },
                    { "user5Id", 0, "8718889f-78fd-474a-ab5d-7f351c48df4c", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "user5@mail.com", false, false, false, null, null, null, null, null, null, null, false, "2f17dbfa-cfa3-40a9-8aa2-ceb43e06a198", false, "user5name" },
                    { "user6Id", 0, "906098e6-c617-4521-ba80-60a12254d5bf", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "user6@mail.com", false, false, false, null, null, null, null, null, null, null, false, "3edf73a6-0df7-4907-8b1a-ad6081ff7d93", false, "user6name" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "Description", "ImageUrl", "IsDeleted", "ModifiedBy", "ModifiedOn", "Name" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Horror fiction is designed to scare, unsettle, or horrify readers. It explores themes of fear and the unknown, often incorporating supernatural elements like ghosts, monsters, or curses. The genre can also delve into the darker aspects of human psychology, portraying paranoia, obsession, and moral corruption. Subgenres include Gothic horror, psychological horror, and splatterpunk, each offering unique ways to evoke dread. Settings often amplify the tension, ranging from haunted houses to desolate landscapes, while the stories frequently address societal fears and existential questions.", "https://org-dcmp-staticassets.s3.us-east-1.amazonaws.com/posterimages/13453_1.jpg", false, null, null, "Horror" },
                    { 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Science fiction explores futuristic, scientific, and technological themes, challenging readers to consider the possibilities and consequences of innovation. These stories often involve space exploration, artificial intelligence, time travel, or parallel universes. Beyond the speculative elements, science fiction frequently tackles ethical dilemmas, societal transformations, and the human condition. Subgenres include cyberpunk, space opera, and hard science fiction, each offering distinct visions of the future. The genre invites readers to imagine the impact of progress and to ponder humanity’s place in the cosmos.", "https://www.editoreric.com/greatlit/litgraphics/book-spiral-galaxy.jpg", false, null, null, "Science Fiction" },
                    { 3, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Fantasy stories transport readers to magical realms filled with mythical creatures, enchanted objects, and epic quests. These tales often feature battles between good and evil, drawing upon folklore, mythology, and the human imagination. Characters may wield powerful magic or undertake journeys of self-discovery in richly crafted worlds. Subgenres like high fantasy, urban fantasy, and dark fantasy provide diverse settings and tones, appealing to a wide range of readers. Themes of heroism, destiny, and transformation are central to the genre, offering both escape and inspiration.", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT5EcrB6fhai5L3-7Ted6fZgxUjCti0W4avrA&s", false, null, null, "Fantasy" },
                    { 4, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Mystery fiction is a puzzle-driven genre that engages readers with suspense and intrigue. The narrative typically revolves around solving a crime, uncovering hidden truths, or exposing a web of deceit. Protagonists range from amateur sleuths to seasoned detectives, each navigating clues, red herrings, and unexpected twists. Subgenres such as noir, cozy mysteries, and legal thrillers cater to varied tastes. Mystery stories often delve into human motives and societal dynamics, providing a satisfying journey toward uncovering the truth.", "https://celadonbooks.com/wp-content/uploads/2020/03/what-is-a-mystery.jpg", false, null, null, "Mystery" },
                    { 5, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Romance novels celebrate the complexities of love and relationships, weaving stories of passion, connection, and emotional growth. They can be set in diverse contexts, from historical periods to fantastical worlds, and often feature characters overcoming personal or external obstacles to find happiness. Subgenres like contemporary romance, historical romance, and paranormal romance offer unique flavors and settings. The genre emphasizes emotional resonance, with narratives that inspire hope and affirm the power of love.", "https://upload.wikimedia.org/wikipedia/commons/3/36/Hammond-SS10.jpg", false, null, null, "Romance" },
                    { 6, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Thrillers are characterized by their fast-paced, high-stakes plots designed to keep readers on edge. They often involve life-and-death scenarios, sinister conspiracies, or relentless antagonists. The genre thrives on tension and unexpected twists, with protagonists racing against time to prevent disaster. Subgenres like psychological thrillers, spy thrillers, and action thrillers cater to diverse interests. The stories explore themes of survival, justice, and moral ambiguity, delivering an adrenaline-fueled reading experience.", "https://celadonbooks.com/wp-content/uploads/2019/10/what-is-a-thriller-1024x768.jpg", false, null, null, "Thriller" },
                    { 7, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Adventure stories are dynamic tales of action, exploration, and survival. Protagonists often face daunting challenges, traversing uncharted territories or overcoming perilous odds. The genre celebrates courage, resilience, and the human spirit, taking readers on exhilarating journeys. From treasure hunts to epic battles, adventure fiction encompasses diverse settings and narratives. It appeals to those who crave excitement and the thrill of discovery.", "https://thumbs.dreamstime.com/b/open-book-ship-sailing-waves-concept-reading-adventure-literature-generative-ai-270347849.jpg", false, null, null, "Adventure" },
                    { 8, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Historical fiction immerses readers in the past, blending factual events with fictional narratives to create vivid portrayals of bygone eras. These stories illuminate the lives, struggles, and triumphs of people from different times, providing insight into cultural, social, and political contexts. Subgenres include historical romance, historical mysteries, and alternate histories, each offering unique perspectives. The genre enriches our understanding of history while engaging us with compelling characters and plots.", "https://celadonbooks.com/wp-content/uploads/2020/03/Historical-Fiction-scaled.jpg", false, null, null, "Historical fiction" },
                    { 9, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Biographies chronicle the lives of real individuals, offering intimate portraits of their experiences, achievements, and legacies. These works range from comprehensive life stories to focused accounts of specific events or periods. Biographies can inspire, inform, and provide deep insight into historical or contemporary figures. Autobiographies and memoirs, subgenres of biography, allow subjects to share their own narratives, adding personal depth to the genre.", "https://i0.wp.com/uspeakgreek.com/wp-content/uploads/2024/01/biography.webp?fit=780%2C780&ssl=1", false, null, null, "Biography" },
                    { 10, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Self-help books are guides to personal growth, offering practical advice for improving one’s life. Topics range from mental health and relationships to productivity and spiritual fulfillment. The genre emphasizes empowerment, providing readers with strategies and tools for achieving goals and overcoming challenges. Subgenres include motivational literature, mindfulness guides, and career development books, catering to diverse needs and aspirations.", "https://www.wellnessroadpsychology.com/wp-content/uploads/2024/05/Self-Help.jpg", false, null, null, "Self-help" },
                    { 11, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Non-fiction encompasses works rooted in factual information, offering insights into real-world topics. It spans memoirs, investigative journalism, essays, and academic studies, covering subjects like history, science, culture, and politics. The genre educates and engages readers, often challenging perceptions and broadening understanding. Non-fiction can be narrative-driven or expository, appealing to those seeking knowledge or a deeper connection to reality.", "https://pickbestbook.com/wp-content/uploads/2023/06/Nonfiction-Literature-1.png", false, null, null, "Non-fiction" },
                    { 12, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Poetry is a literary form that condenses emotions, thoughts, and imagery into carefully chosen words, often structured with rhythm and meter. It explores universal themes such as love, nature, grief, and introspection, offering readers profound and evocative experiences. From traditional sonnets and haikus to free verse and spoken word, poetry captivates through its ability to articulate the inexpressible, creating deep emotional resonance and intellectual reflection.", "https://assets.ltkcontent.com/images/9037/examples-of-poetry-genres_7abbbb2796.jpg", false, null, null, "Poetry" },
                    { 13, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Drama fiction delves into emotional and relational conflicts, portraying the complexities of human interactions and emotions. It emphasizes character development and nuanced storytelling, often exploring themes of love, betrayal, identity, and societal struggles. Drama offers readers a lens into the intricacies of the human experience, whether through tragic, romantic, or morally ambiguous narratives. Its focus on realism and emotional depth creates stories that resonate deeply with audiences.", "https://basudewacademichub.in/wp-content/uploads/2024/02/drama-literature-solution.png", false, null, null, "Drama" },
                    { 14, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Children's literature is crafted to captivate and inspire young readers with imaginative worlds, moral lessons, and relatable characters. These stories often emphasize themes of curiosity, friendship, and bravery, delivering messages of kindness, resilience, and growth. From whimsical picture books to adventurous chapter books, children's fiction nurtures creativity and fosters a lifelong love of reading, helping young minds explore both real and fantastical realms.", "https://media.vanityfair.com/photos/598888671dc63c45b7b1db6e/master/w_2560%2Cc_limit/MAG-0817-Wild-Things-a.jpg", false, null, null, "Children's" },
                    { 15, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Young Adult (YA) fiction speaks to the unique experiences and challenges of adolescence, addressing themes such as identity, first love, friendship, and coming of age. These stories often feature relatable protagonists navigating personal growth, societal expectations, and emotional upheaval. Subgenres such as fantasy, dystopian, and contemporary YA provide diverse backdrops for these journeys, resonating with readers through authentic and engaging storytelling that reflects their own struggles and triumphs.", "https://m.media-amazon.com/images/I/81xRLF1KCAL._AC_UF1000,1000_QL80_.jpg", false, null, null, "Young Adult" },
                    { 16, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Comedy fiction aims to entertain and delight readers through humor, satire, and absurdity. It uses wit and clever storytelling to highlight human follies, societal quirks, or surreal situations. From lighthearted escapades to biting social commentary, comedy encompasses a range of tones and styles. The genre often brings laughter and joy, offering an escape from the mundane while sometimes delivering thought-provoking messages in the guise of humor.", "https://mandyevebarnett.com/wp-content/uploads/2017/12/humor.jpg?w=640", false, null, null, "Comedy" },
                    { 17, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Graphic novels seamlessly blend visual art and narrative storytelling, using a combination of text and illustrations to convey complex plots and emotions. This versatile format spans a wide array of genres, including superhero tales, memoirs, historical epics, and science fiction. Graphic novels offer an immersive reading experience, appealing to diverse audiences through their ability to convey vivid imagery and intricate storylines that are as impactful as traditional prose.", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSb0THovTlPB_nRl3RY6TsbWD4R2qEC-TQSAg&s", false, null, null, "Graphic Novel" },
                    { 18, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "The 'Other' genre serves as a home for unconventional, experimental, or cross-genre works that defy traditional categorization. This category embraces innovation and diversity, welcoming stories that push the boundaries of storytelling, structure, and style. From hybrid narratives to avant-garde experiments, 'Other' offers a platform for unique voices and creative expressions that don’t fit neatly into predefined genres.", "https://www.98thpercentile.com/hubfs/388x203%20(4).png", false, null, null, "Other" },
                    { 19, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Dystopian fiction paints a grim portrait of societies marred by oppression, inequality, or disaster, often set in a future shaped by catastrophic events or authoritarian regimes. These cautionary tales explore themes like survival, rebellion, and the loss of humanity, serving as critiques of political, social, and environmental trends. Subgenres such as post-apocalyptic and cyber-dystopia examine the fragility of civilization and the consequences of unchecked power or technological overreach.", "https://www.ideology-theory-practice.org/uploads/1/3/5/5/135563566/050_orig.jpg", false, null, null, "Dystopian" },
                    { 20, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Spirituality books delve into the deeper questions of existence, faith, and the human soul, offering insights and practices to nurture inner peace and personal growth. They often explore themes of mindfulness, self-awareness, and connection to a higher power or universal energy. From philosophical reflections to practical guides, these works resonate with readers seekinginspiration, understanding, and spiritual fulfillment across diverse traditions and belief systems.", "https://m.media-amazon.com/images/I/61jxcM3UskL._AC_UF1000,1000_QL80_.jpg", false, null, null, "Spirituality" },
                    { 21, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Crime fiction is centered around the investigation of a crime, often focusing on the detection of criminals or the pursuit of justice. It may include detectives, police officers, or amateur sleuths solving crimes like murder, theft, or corruption. The genre can involve suspense, action, and exploration of moral dilemmas surrounding law and order. Subgenres include hardboiledcrime, cozy mysteries, and police procedurals, all providing different approaches to solving crimes and investigating human behavior.", "https://img.tpt.cloud/nextavenue/uploads/2019/04/Crime-Fiction-Savvy-Sleuths-Over-50_53473532.inside.1200x775.jpg", false, null, null, "Crime" },
                    { 22, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Urban fiction explores life in modern, often gritty urban settings, focusing on the struggles, relationships, and experiences of people in cities. This genre frequently addresses themes like poverty, crime, social injustice, and community dynamics. It can incorporate elements of drama, romance, and even horror, often portraying the challenges of urban life with raw, unflinching realism. Urban fiction is popular in contemporary literature and often includes characters from marginalized communities.", "https://frugalbookstore.net/cdn/shop/collections/Urban-Fiction.png?v=1724599745&width=480", false, null, null, "Urban Fiction" },
                    { 23, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Fairy tale fiction involves magical or fantastical stories often set in a world where magic and mythical creatures exist. These stories typically follow a clear moral arc, with characters who experience trials or transformation before achieving a happy ending. Fairy tales often feature archetypal characters like witches, princes, and princesses, and they explore themes of good vs. evil, justice, and personal growth. Many fairy tales have been passed down through generations, and the genre continues to inspire modern adaptations and retellings.", "https://news.syr.edu/wp-content/uploads/2023/09/enchanting_fairy_tale_woodland_onto_a_castle_an.original-scaled.jpg", false, null, null, "Fairy Tale" },
                    { 24, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Epic fiction is characterized by large-scale, grand narratives often centered around heroic characters or monumental events. Epics typically focus on the struggles and triumphs of protagonists who undergo significant personal or societal change. These stories often span extensive periods of time and encompass entire civilizations, exploring themes like war, leadership, and cultural identity. Classic examples include *The Iliad* and *The Odyssey*, with modern epics continuing to explore the human experience in vast, sweeping terms.", "https://i0.wp.com/joncronshaw.com/wp-content/uploads/2024/01/DALL%C2%B7E-2024-01-17-09.05.10-A-magical-and-enchanting-landscape-for-a-fantasy-blog-post-featuring-an-ancient-castle-perched-on-a-high-cliff-a-vast-mystical-forest-with-towering.png?fit=1200%2C686&ssl=1", false, null, null, "Epic" },
                    { 25, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Political fiction uses stories to explore, criticize, or comment on political systems, ideologies, and power dynamics. These narratives often examine how political structures affect individuals and societies, focusing on themes of corruption, revolution, and social change. Political fiction can include dystopian novels, satires, and thrillers, offering commentary on both contemporary and historical politics. Through these stories, authors challenge readers to think critically about the systems that govern their lives.", "https://markelayat.com/wp-content/uploads/elementor/thumbs/Political-Fiction-ft-image-qwo9yzatn5xk8t34vvqfivz2ed7zuj5lccn9ylm7bc.png", false, null, null, "Political Fiction" },
                    { 26, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Philosophical fiction delves into profound questions about existence, ethics, free will, and the nature of reality. These novels often explore abstract ideas and are driven by deep intellectual themes rather than plot or action. Philosophical fiction may follow characters who engage in critical thinking, self-reflection, or existential crises. These works often question the meaning of life, morality, and consciousness, and they can be a blend of both fiction and philosophy, prompting readers to consider their own beliefs and perspectives.", "https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1546103428i/5297._UX160_.jpg", false, null, null, "Philosophical Fiction" },
                    { 27, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "True crime fiction is based on real-life criminal events, recounting the details of notorious crimes, investigations, and trials. It often focuses on infamous cases, delving into the psychology of criminals, the detectives or journalists who solve the cases, and the social impact of the crime. True crime often incorporates extensive research and interviews, giving readers an inside look at the complexities of real-life crime and law enforcement. These works can be chilling and thought-provoking, blending elements of mystery, drama,and historical non-fiction.", "https://is1-ssl.mzstatic.com/image/thumb/Podcasts221/v4/00/07/67/000767b5-bad1-5d78-db34-373363ec6b3e/mza_8962416523973028402.jpg/1200x1200bf.webp", false, null, null, "True Crime" },
                    { 28, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Satire is a genre that uses humor, irony, and exaggeration to criticize or mock individuals, institutions, or societal norms. It often employs wit and sarcasm to highlight the flaws and absurdities of the subject being criticized, sometimes with the intent of provoking thought or promoting change. Satirical works can cover a wide range of topics, including politics, culture, and human nature, and can be both lighthearted or dark in tone. Famous examples include works like Gulliver's Travels and Catch-22.", "https://photos.demandstudios.com/getty/article/64/32/529801877.jpg", false, null, null, "Satire" },
                    { 29, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Psychological fiction delves into the inner workings of the mind, exploring complex emotional states, mental illness, and the psychological effects of personal trauma, relationships, and societal pressures. These works often focus on character development and the emotional or mental struggles of the protagonists, rather than external events. Psychological fiction can blur the lines between reality and illusion, questioning perceptions and exploring the deeper layers of human consciousness. It often presents challenging and sometimes disturbing narratives about identity and self-perception. Notable examples include The Bell Jar and The Catcher in the Rye.", "https://literaturelegends.com/wp-content/uploads/2023/08/psychological.jpg", false, null, null, "Psychological Fiction" },
                    { 30, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Supernatural fiction explores phenomena beyond the natural world, often incorporating ghosts, spirits, vampires, or otherworldly beings. These works blend elements of horror, fantasy, and the unexplained, and often delve into themes of life after death, paranormal activity, and other mystifying occurrences. The supernatural genre captivates readers with its portrayal of eerie events and the unknown, often blurring the line between reality and the mystical. Examples include works like The Haunting of Hill House and The Turn of the Screw.", "https://fully-booked.ca/wp-content/uploads/2024/02/evolution-of-paranormal-fiction-1024x576.jpg", false, null, null, "Supernatural" },
                    { 31, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Gothic fiction is characterized by its dark, eerie atmosphere, and often involves elements of horror, mystery, and the supernatural. These stories typically feature gloomy, decaying settings such as castles, mansions, or haunted landscapes, and often include tragic or macabre themes. Gothic fiction focuses on emotions like fear, dread, and despair, and explores the darker sides of human nature. Famous examples include works like Wuthering Heights and Frankenstein.", "https://bookstr.com/wp-content/uploads/2022/09/V8mj92.webp", false, null, null, "Gothic Fiction" },
                    { 32, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Magical realism blends elements of magic or the supernatural with a realistic narrative, creating a world where extraordinary events occur within ordinary settings. This genre often explores themes of identity, culture, and human experience, and it is marked by the seamless integration of magical elements into everyday life. Prominent examples include books like One Hundred Years of Solitude and The House of the Spirits.", "https://www.world-defined.com/wp-content/uploads/2024/04/Magic-Realism-Books-978x652-1.webp", false, null, null, "Magical Realism" },
                    { 33, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Dark fantasy combines elements of fantasy with a sense of horror, despair, and the supernatural. These stories often take place in dark, gritty worlds where magic, danger, and moral ambiguity challenge the characters. Dark fantasy blends the fantastical with the disturbing, creating a sense of dread and unease. Examples include books like The Dark Tower series and A Song of Ice and Fire.", "https://miro.medium.com/v2/resize:fit:1024/1*VU5O34UlH-1SXZkEnL0dyg.jpeg", false, null, null, "Dark Fantasy" }
                });

            migrationBuilder.InsertData(
                table: "Nationalities",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "ModifiedBy", "ModifiedOn", "Name" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Afghanistan" },
                    { 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Albania" },
                    { 3, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Algeria" },
                    { 4, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Andorra" },
                    { 5, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Angola" },
                    { 6, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Antigua and Barbuda" },
                    { 7, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Argentina" },
                    { 8, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Armenia" },
                    { 9, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Australia" },
                    { 10, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Austria" },
                    { 11, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Azerbaijan" },
                    { 12, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bahamas" },
                    { 13, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bahrain" },
                    { 14, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bangladesh" },
                    { 15, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Barbados" },
                    { 16, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Belarus" },
                    { 17, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Belgium" },
                    { 18, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Belize" },
                    { 19, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Benin" },
                    { 20, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bhutan" },
                    { 21, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bolivia" },
                    { 22, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bosnia and Herzegovina" },
                    { 23, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Botswana" },
                    { 24, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Brazil" },
                    { 25, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Brunei" },
                    { 26, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bulgaria" },
                    { 27, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Burkina Faso" },
                    { 28, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Burundi" },
                    { 29, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Cabo Verde" },
                    { 30, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Cambodia" },
                    { 31, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Cameroon" },
                    { 32, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Canada" },
                    { 33, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Central African Republic" },
                    { 34, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Chad" },
                    { 35, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Chile" },
                    { 36, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "China" },
                    { 37, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Colombia" },
                    { 38, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Comoros" },
                    { 39, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Costa Rica" },
                    { 40, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Croatia" },
                    { 41, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Cuba" },
                    { 42, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Cyprus" },
                    { 43, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Czech Republic" },
                    { 44, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Denmark" },
                    { 45, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Djibouti" },
                    { 46, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Dominica" },
                    { 47, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Dominican Republic" },
                    { 48, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ecuador" },
                    { 49, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Egypt" },
                    { 50, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "El Salvador" },
                    { 51, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Equatorial Guinea" },
                    { 52, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Eritrea" },
                    { 53, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Estonia" },
                    { 54, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Eswatini" },
                    { 55, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ethiopia" },
                    { 56, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Fiji" },
                    { 57, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Finland" },
                    { 58, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "France" },
                    { 59, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Gabon" },
                    { 60, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Gambia" },
                    { 61, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Georgia" },
                    { 62, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Germany" },
                    { 63, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ghana" },
                    { 64, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Greece" },
                    { 65, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Grenada" },
                    { 66, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Guatemala" },
                    { 67, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Guinea" },
                    { 68, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Guinea-Bissau" },
                    { 69, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Guyana" },
                    { 70, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Haiti" },
                    { 71, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Honduras" },
                    { 72, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Hungary" },
                    { 73, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Iceland" },
                    { 74, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "India" },
                    { 75, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Indonesia" },
                    { 76, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Iran" },
                    { 77, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Iraq" },
                    { 78, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ireland" },
                    { 79, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Israel" },
                    { 80, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Italy" },
                    { 81, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Jamaica" },
                    { 82, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Japan" },
                    { 83, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Jordan" },
                    { 84, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Kazakhstan" },
                    { 85, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Kenya" },
                    { 86, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Kiribati" },
                    { 87, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "North Korea" },
                    { 88, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "South Korea" },
                    { 89, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Kuwait" },
                    { 90, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Kyrgyzstan" },
                    { 91, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Laos" },
                    { 92, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Latvia" },
                    { 93, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Lebanon" },
                    { 94, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Lesotho" },
                    { 95, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Liberia" },
                    { 96, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Libya" },
                    { 97, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Liechtenstein" },
                    { 98, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Lithuania" },
                    { 99, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Luxembourg" },
                    { 100, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Madagascar" },
                    { 101, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Malawi" },
                    { 102, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Malaysia" },
                    { 103, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Maldives" },
                    { 104, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Mali" },
                    { 105, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Malta" },
                    { 106, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Marshall Islands" },
                    { 107, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Mauritania" },
                    { 108, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Mauritius" },
                    { 109, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Mexico" },
                    { 110, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Micronesia" },
                    { 111, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Moldova" },
                    { 112, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Monaco" },
                    { 113, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Mongolia" },
                    { 114, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Montenegro" },
                    { 115, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Morocco" },
                    { 116, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Mozambique" },
                    { 117, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Myanmar" },
                    { 118, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Namibia" },
                    { 119, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Nauru" },
                    { 120, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Nepal" },
                    { 121, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Netherlands" },
                    { 122, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "New Zealand" },
                    { 123, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Nicaragua" },
                    { 124, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Niger" },
                    { 125, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Nigeria" },
                    { 126, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "North Macedonia" },
                    { 127, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Norway" },
                    { 128, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Oman" },
                    { 129, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Pakistan" },
                    { 130, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Palau" },
                    { 131, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Panama" },
                    { 132, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Papua New Guinea" },
                    { 133, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Paraguay" },
                    { 134, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Peru" },
                    { 135, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Philippines" },
                    { 136, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Poland" },
                    { 137, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Portugal" },
                    { 138, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Qatar" },
                    { 139, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Romania" },
                    { 140, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Russia" },
                    { 141, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Rwanda" },
                    { 142, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Saint Kitts and Nevis" },
                    { 143, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Saint Lucia" },
                    { 144, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Saint Vincent and the Grenadines" },
                    { 145, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Samoa" },
                    { 146, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "San Marino" },
                    { 147, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "São Tomé and Príncipe" },
                    { 148, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Saudi Arabia" },
                    { 149, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Senegal" },
                    { 150, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Serbia" },
                    { 151, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Seychelles" },
                    { 152, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sierra Leone" },
                    { 153, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Singapore" },
                    { 154, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Slovakia" },
                    { 155, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Slovenia" },
                    { 156, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Solomon Islands" },
                    { 157, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Somalia" },
                    { 158, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "South Africa" },
                    { 159, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "South Sudan" },
                    { 160, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Spain" },
                    { 161, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sri Lanka" },
                    { 162, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sudan" },
                    { 163, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Suriname" },
                    { 164, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sweden" },
                    { 165, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Switzerland" },
                    { 166, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Syria" },
                    { 167, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Taiwan" },
                    { 168, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Tajikistan" },
                    { 169, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Tanzania" },
                    { 170, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Thailand" },
                    { 171, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Togo" },
                    { 172, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Tonga" },
                    { 173, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Trinidad and Tobago" },
                    { 174, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Tunisia" },
                    { 175, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Turkey" },
                    { 176, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Turkmenistan" },
                    { 177, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Tuvalu" },
                    { 178, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Uganda" },
                    { 179, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ukraine" },
                    { 180, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "United Arab Emirates" },
                    { 181, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "United Kingdom" },
                    { 182, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "United States" },
                    { 183, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Unknown" },
                    { 184, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Uruguay" },
                    { 185, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Uzbekistan" },
                    { 186, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Vanuatu" },
                    { 187, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Vatican City" },
                    { 188, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Venezuela" },
                    { 189, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Vietnam" },
                    { 190, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Yemen" },
                    { 191, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Zambia" },
                    { 192, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Zimbabwe" }
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "AverageRating", "Biography", "BornAt", "CreatedBy", "CreatedOn", "CreatorId", "DeletedBy", "DeletedOn", "DiedAt", "Gender", "ImageUrl", "IsApproved", "IsDeleted", "ModifiedBy", "ModifiedOn", "Name", "NationalityId", "PenName", "RatingsCount" },
                values: new object[,]
                {
                    { 1, 4.7000000000000002, "Stephen Edwin King (born September 21, 1947) is an American author of horror, supernatural fiction, suspense, crime, and fantasy novels. He is often referred to as the 'King of Horror,' but his work spans many genres, earning him a reputation as one of the most prolific and successful authors of our time. King's novels have sold more than 350 million copies, many of which have been adapted into feature films, miniseries, television series, and comic books. Notable works include Carrie, The Shining, IT, Misery, Pet Sematary, and The Dark Tower series. King is renowned for his ability to create compelling, complex characters and for his mastery of building suspenseful, intricately woven narratives. Aside from his novels, he has written nearly 200 short stories, most of which have been compiled into collections such as Night Shift, Skeleton Crew, and Everything's Eventual. He has received numerous awards for his contributions to literature, including the National Book Foundation's Medal for Distinguished Contribution to American Letters in 2003. King often writes under the pen name 'Richard Bachman,' a pseudonym he used to publish early works such as Rage and The Long Walk. He lives in Bangor, Maine, with his wife, fellow novelist Tabitha King, and continues to write and inspire new generations of readers and writers.", new DateTime(1947, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, 0, "https://media.npr.org/assets/img/2020/04/07/stephen-king.by-shane-leonard_wide-c132de683d677c7a6a1e692f86a7e6670baf3338.jpg?s=1100&c=85&f=jpeg", true, false, null, null, "Stephen King", 182, "Richard Bachman", 0 },
                    { 2, 4.75, "Joanne Rowling (born July 31, 1965), known by her pen name J.K. Rowling, is a British author and philanthropist. She is best known for writing the Harry Potter series, a seven-book fantasy saga that has become a global phenomenon. The series has sold over 600 million copies, been translated into 84 languages, and inspired a massive multimedia franchise, including blockbuster films, stage plays, video games, and theme parks. Notable works include Harry Potter and the Philosopher's Stone, Harry Potter and the Deathly Hallows, and The Tales of Beedle the Bard. Rowling's writing is praised for its imaginative world-building, compelling characters, and exploration of themes such as love, loyalty, and the battle between good and evil. After completing the Harry Potter series, Rowling transitioned to writing for adults, debuting with The Casual Vacancy, a contemporary social satire. She also writes crime fiction under the pseudonym Robert Galbraith, authoring the acclaimed Cormoran Strike series. Rowling has received numerous awards and honors for her literary achievements, including the Order of the British Empire (OBE) for services to children’s literature. She is an advocate for various charitable causes and founded the Volant Charitable Trust to combat social inequality. Rowling lives in Scotland with her family and continues to write, inspiring readers of all ages with her imaginative storytelling and philanthropy.", new DateTime(1965, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, 1, "https://stories.jkrowling.com/wp-content/uploads/2021/09/Shot-B-105_V2_CROP-e1630873059779.jpg", true, false, null, null, "Joanne Rowling", 181, "J. K. Rowling", 0 },
                    { 3, 4.6699999999999999, "John Ronald Reuel Tolkien (January 3, 1892 – September 2, 1973) was an English writer, philologist, and academic. He is best known as the author of The Hobbit and The Lord of the Rings, two of the most beloved works in modern fantasy literature. Tolkien's work has had a profound impact on the fantasy genre, establishing many of the conventions and archetypes that define it today. Set in the richly detailed world of Middle-earth, his stories feature intricate mythologies, languages, and histories, reflecting his scholarly expertise in philology and medieval studies. The Hobbit (1937) was a critical and commercial success, leading to the epic sequel The Lord of the Rings trilogy (1954–1955), which has sold over 150 million copies and been adapted into award-winning films by director Peter Jackson. Tolkien also authored The Silmarillion, a collection of myths and legends that expand the lore of Middle-earth. He served as a professor of Anglo-Saxon at the University of Oxford, where he was a member of the literary group The Inklings, alongside C.S. Lewis. Tolkien's contributions to literature earned him global acclaim and a lasting legacy as the 'father of modern fantasy.' He passed away in 1973, but his works continue to captivate readers worldwide.", new DateTime(1892, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new DateTime(1973, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d4/J._R._R._Tolkien%2C_ca._1925.jpg/330px-J._R._R._Tolkien%2C_ca._1925.jpg", true, false, null, null, "John Ronald Reuel Tolkien ", 181, "J.R.R Tolkien", 0 },
                    { 4, 4.5999999999999996, "Ken Elton Kesey (September 17, 1935 – November 10, 2001) was an American novelist, essayist, and countercultural figure. He is best known for his debut novel One Flew Over the Cuckoo's Nest (1962), which explores themes of individuality, authority, and mental health. The novel became an instant classic, earning critical acclaim for its portrayal of life inside a psychiatric institution and inspiring a celebrated 1975 film adaptation starring Jack Nicholson that won five Academy Awards. Kesey was deeply involved in the counterculture movement of the 1960s, becoming a key figure among the Merry Pranksters, a group famous for their cross-country bus trip chronicled in Tom Wolfe's The Electric Kool-Aid Acid Test. His second novel, Sometimes a Great Notion (1964), was praised for its ambitious narrative structure and portrayal of family dynamics. Kesey's work often reflects his fascination with the human condition, rebellion, and the nature of freedom. In addition to his literary career, Kesey experimented with psychedelics, drawing inspiration from his participation in government experiments and his own personal experiences. He remained an influential voice in American literature and culture, inspiring generations of readers and writers to question societal norms and explore new perspectives. Kesey passed away in 2001, leaving behind a legacy of bold storytelling and a spirit of rebellion.", new DateTime(1935, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new DateTime(2001, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "https://upload.wikimedia.org/wikipedia/en/9/9b/Ken_Kesey%2C_American_author%2C_1935-2001.jpg", true, false, null, null, "Ken Kesey", 182, null, 0 },
                    { 5, 4.9000000000000004, "Eric Arthur Blair (June 25, 1903 – January 21, 1950), known by his pen name George Orwell, was an English novelist, essayist, journalist, and critic. Orwell is best known for his works Animal Farm (1945) and Nineteen Eighty-Four (1949), both of which are considered cornerstones of modern English literature and have had a lasting impact on political thought. Animal Farm, an allegorical novella satirizing the Russian Revolution and the rise of Stalinism, has become a standard in literature about totalitarianism and political corruption. Nineteen Eighty-Four, a dystopian novel set in a totalitarian society controlled by Big Brother, has influenced political discourse around surveillance, propaganda, and government control. Orwell's writings often sexplore themes of social injustice, totalitarianism, and the misuse of power. He was an ardent critic of fascism and communism and was deeply involved in political activism, including fighting in the Spanish Civil War, which influenced his strong anti-authoritarian views. Orwell's style is known for its clarity, precision, and biting social commentary, and his work continues to be relevant and influential in discussions on politics, language, and individual rights. Orwell passed away in 1950 at the age of 46, but his work remains widely read and influential, especially in the context of discussions surrounding state power, civil liberties, and the role of the individual in society.", new DateTime(1903, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new DateTime(1950, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "https://hips.hearstapps.com/hmg-prod/images/george-orwell.jpg", true, false, null, null, "George Orwell", 181, "George Orwell", 0 },
                    { 6, 4.5999999999999996, "Abraham 'Bram' Stoker (November 8, 1847 – April 20, 1912) was an Irish author best known for his Gothic horror novel Dracula, which has become a classic of literature and a foundational work in the vampire genre. Born in Dublin, Ireland, Stoker was a writer, theater manager, and journalist. While Dracula is his most famous work, Stoker wrote numerous other novels, short stories, and essays. He was greatly influenced by Gothic fiction and folklore, particularly Eastern European myths about vampires, which he drew upon to create his iconic antagonist, Count Dracula. Stoker's Dracula introduced the figure of the vampire into mainstream literature and has inspired countless adaptations in film, theater, and popular culture. Though Stoker's works initially received mixed critical reception, Dracula gained increasing recognition and has had a lasting impact on the horror genre. Stoker's writing was often characterized by vivid, atmospheric descriptions and psychological complexity, and his portrayal of fear, desire, and the supernatural remains influential today. Stoker passed away in 1912 at the age of 64, but his legacy as the creator of one of literature's most enduring villains continues to captivate readers around the world.", new DateTime(1847, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new DateTime(1912, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/Bram_Stoker_1906.jpg/640px-Bram_Stoker_1906.jpg", true, false, null, null, "Bram Stoker", 78, "Bram Stoker", 0 },
                    { 7, 4.2999999999999998, "John Anthony Burgess Wilson (February 25, 1917 – November 25, 1993) was an English author, composer, and linguist, best known for his dystopian novel A Clockwork Orange. Born in Manchester, England, Burgess worked in a variety of fields before becoming a full-time writer. His work often explores themes of free will, violence, and social control. A Clockwork Orange, published in 1962, became a landmark work in the genre of dystopian fiction, and the controversial novel was later adapted into a famous film by Stanley Kubrick. Burgess was also known for his deep knowledge of language and for his ability to create complex, linguistically rich narratives. Throughout his career, Burgess wrote numerous novels, short stories, plays, and essays, in addition to works of music and translations. His works, while not always critically lauded, gained a loyal following, and he is considered an influential figure in British literature. Burgess continued to write prolifically until his death in 1993 at the age of 76.", new DateTime(1917, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new DateTime(1993, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "https://grahamholderness.com/wp-content/uploads/2019/04/anthony-burgess-at-home-i-009.jpg", true, false, null, null, "Anthony Burgess", 181, "Anthony Burgess", 0 },
                    { 8, 4.4800000000000004, "Agatha Mary Clarissa Christie (September 15, 1890 – January 12, 1976) was an English writer, widely recognized for her detective fiction. She is best known for creating iconic characters such as Hercule Poirot and Miss Marple, who became central figures in numerous detective novels, short stories, and plays. Christie’s writing is characterized by intricate plots, psychological depth, and unexpected twists. Her first novel, The Mysterious Affair at Styles, introduced the world to Hercule Poirot in 1920. Over the following decades, Christie would become the best-selling novelist of all time, with over two billion copies of her books sold worldwide. Her works have been translated into over 100 languages, and her influence on the mystery genre is profound. Christie was appointed a Dame Commander of the Order of the British Empire in 1971. She continued writing until her death in 1976, leaving behind a legacy that includes Murder on the Orient Express, Death on the Nile, and And Then There Were None.", new DateTime(1890, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new DateTime(1976, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "https://upload.wikimedia.org/wikipedia/commons/c/cf/Agatha_Christie.png", true, false, null, null, "Agatha Christie", 181, "Agatha Christie", 0 },
                    { 9, 4.5, "John Ernst Steinbeck Jr. (February 27, 1902 – December 20, 1968) was an American author known for his impactful portrayal of the struggles of the working class and the disenfranchised. His novels, such as 'The Grapes of Wrath', 'Of Mice and Men', and 'East of Eden', often explored themes of poverty, social injustice, and the human condition. Steinbeck won the Nobel Prize for Literature in 1962 for his realistic and imaginative writings, which had a lasting effect on American literature. Born in Salinas, California, Steinbeck's early works depicted life during the Great Depression, while his later novels explored more personal and philosophical themes. He is best remembered for his compassion for ordinary people and his insightful critiques of social systems. His writing style was marked by his use of vivid, poetic descriptions and a deep understanding of his characters' struggles.", new DateTime(1902, 2, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new DateTime(1968, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "https://m.media-amazon.com/images/M/MV5BYzBiNmIxOWYtNTc3NS00OTg4LTgxOGYtOTJmYTk3NzJhYzE1XkEyXkFqcGc@._V1_.jpg", true, false, null, null, "John Steinbeck", 182, "John Steinbeck", 0 },
                    { 10, 4.6699999999999999, "Frank Herbert (October 8, 1920 – February 11, 1986) was an American science fiction author best known for his landmark series Dune. Herbert's work frequently explored themes of politics, religion, and ecology. *Dune*, first published in 1965, remains one of the most influential and best-selling science fiction novels of all time, winning the Hugo and Nebula Awards. Born in Tacoma, Washington, Herbert began his writing career as a journalist, before transitioning to fiction writing. His Dune series, which consists of six novels, delves into the complexities of power, governance, environmentalism, and the human condition. Herbert's distinctive writing style is known for its philosophical depth, intricate world-building, and the exploration of human evolution. His work has had a profound influence on both science fiction and popular culture. Herbert passed away in 1986, but his legacy continues through his novels, which are still widely read and respected today.", new DateTime(1920, 10, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new DateTime(1986, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "https://www.historylink.org/Content/Media/Photos/Large/Frank-Herbert-signing-books-Seattle-December-5-1971.jpg", true, false, null, null, "Frank Herbert", 182, "Frank Herbert", 0 },
                    { 11, 4.5, "Douglas Adams (March 11, 1952 – May 11, 2001) was an English author, best known for his science fiction series The Hitchhiker's Guide to the Galaxy. Adams’ works are renowned for their wit, absurdity, and philosophical depth. Born in Cambridge, England, Adams began his career in comedy writing before transitioning into fiction. The Hitchhiker's Guide to the Galaxy, first published as a radio play in 1978 and later as a novel in 1979, became a cultural phenomenon, blending humor, satire, and sci-fi elements into a beloved classic. The series, which spans multiple books, explores themes of existence, the absurdity of life, and the universe in a way that resonates with both fans of science fiction and general readers. Adams’ distinctive voice and irreverent humor continue to inspire and entertain readers today. He also worked as a scriptwriter and wrote other notable works, including Dirk Gently's Holistic Detective Agency. Adams passed away in 2001 at the age of 49, but his influence on the science fiction genre and popular culture remains enduring.", new DateTime(1952, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, new DateTime(2001, 5, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "https://static.tvtropes.org/pmwiki/pub/images/DouglasAdams_douglasadams_com.jpg", true, false, null, null, "Douglas Adams", 181, "Douglas Adams", 0 }
                });

            migrationBuilder.InsertData(
                table: "Profiles",
                columns: new[] { "UserId", "Biography", "CreatedAuthorsCount", "CreatedBooksCount", "CreatedBy", "CreatedOn", "CurrentlyReadingBooksCount", "DateOfBirth", "FirstName", "ImageUrl", "IsPrivate", "LastName", "ModifiedBy", "ModifiedOn", "PhoneNumber", "ReadBooksCount", "ReviewsCount", "SocialMediaUrl", "ToReadBooksCount" },
                values: new object[,]
                {
                    { "user1Id", "John is a passionate reader and a book reviewer.", 0, 0, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(1990, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "John", "https://www.shareicon.net/data/512x512/2016/05/24/770117_people_512x512.png", false, "Doe", null, null, "+1234567890", 3, 15, "https://twitter.com/johndoe", 2 },
                    { "user2Id", "Alice enjoys exploring fantasy and sci-fi genres.", 0, 0, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(1985, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alice", "https://static.vecteezy.com/system/resources/previews/002/002/257/non_2x/beautiful-woman-avatar-character-icon-free-vector.jpg", false, "Smith", null, null, "+1987654321", 3, 15, "https://facebook.com/alicesmith", 1 },
                    { "user3Id", "Bob is a new reader with a love for thrillers and mysteries.", 0, 0, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2000, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bob", "https://cdn1.iconfinder.com/data/icons/user-pictures/101/malecostume-512.png", false, "Johnson", null, null, "+1122334455", 3, 14, "https://instagram.com/bobjohnson", 1 }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "AverageRating", "CreatedBy", "CreatedOn", "CreatorId", "DeletedBy", "DeletedOn", "ImageUrl", "IsApproved", "IsDeleted", "LongDescription", "ModifiedBy", "ModifiedOn", "PublishedDate", "RatingsCount", "ShortDescription", "Title" },
                values: new object[,]
                {
                    { 1, 1, 4.25, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg/330px-Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg", true, false, "Pet Sematary is a 1983 horror novel by American writer Stephen King. The story revolves around the Creed family who move into a rural home near a pet cemetery that has the power to resurrect the dead. However, the resurrected creatures return with sinister changes. The novel explores themes of grief, mortality, and the consequences of tampering with nature. It was nominated for a World Fantasy Award for Best Novel in 1984. The book was adapted into two films: one in 1989 directed by Mary Lambert and another in 2019 directed by Kevin Kölsch and Dennis Widmyer. In November 2013, PS Publishing released Pet Sematary in a limited 30th-anniversary edition, further solidifying its status as a classic in horror literature.", null, null, new DateTime(1983, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Sometimes dead is better.", "Pet Sematary" },
                    { 2, 2, 4.75, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "https://librerialaberintopr.com/cdn/shop/products/hallows_459x.jpg?v=1616596206", true, false, "Harry Potter and the Deathly Hallows is a fantasy novel written by British author J.K. Rowling. It is the seventh and final book in the Harry Potter series and concludes the epic tale of Harry's battle against Lord Voldemort. The story begins with Harry, Hermione, and Ron embarking on a dangerous quest to locate and destroy Voldemort's Horcruxes, which are key to his immortality. Along the way, they uncover secrets about the Deathly Hallows—three powerful magical objects that could aid them in their fight. The book builds to an intense and emotional climax at the Battle of Hogwarts, where Harry confronts Voldemort for the last time. Released on 21 July 2007, the book became a cultural phenomenon, breaking sales records and receiving critical acclaim for its complex characters, intricate plotting, and resonant themes of sacrifice, friendship, and love.", null, null, new DateTime(2007, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "The last book from the Harry Potter series.", "Harry Potter and the Deathly Hallows" },
                    { 3, 3, 4.6699999999999999, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "https://upload.wikimedia.org/wikipedia/en/e/e9/First_Single_Volume_Edition_of_The_Lord_of_the_Rings.gif", true, false, "The Lord of the Rings is a high-fantasy novel written by J.R.R. Tolkien and set in the fictional world of Middle-earth. The story follows the journey of Frodo Baggins, a humble hobbit who inherits the One Ring—a powerful artifact created by the Dark Lord Sauron to control Middle-earth. Along with a fellowship of companions, Frodo sets out on a perilous mission to destroy the ring in the fires of Mount Doom, the only place where it can be unmade. The narrative interweaves themes of friendship, courage, sacrifice, and the corrupting influence of power. Written in stages between 1937 and 1949, the novel is widely regarded as one of the greatest works of fantasy literature, influencing countless authors and spawning adaptations, including Peter Jackson's acclaimed film trilogy. With over 150 million copies sold, it remains one of the best-selling books of all time, praised for its richly detailed world-building, complex characters, and timeless appeal.", null, null, new DateTime(1954, 7, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "The Lord of the Rings is an epic fantasy novel by the English author J. R. R. Tolkien.", "Lord of the Rings" },
                    { 4, 4, 4.7999999999999998, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1327144697i/3744438.jpg", true, false, "1984, written by George Orwell, is a dystopian novel set in a totalitarian society under the omnipresent surveillance of Big Brother. Published in 1949, the story follows Winston Smith, a low-ranking member of the Party, as he secretly rebels against the oppressive regime. Through his illicit love affair with Julia and his pursuit of forbidden knowledge, Winston challenges the Party's control over truth, history, and individuality. The novel introduces concepts such as 'doublethink,' 'Newspeak,' and 'thoughtcrime,' which have since become part of modern political discourse. Widely regarded as a classic of English literature, 1984 is a chilling exploration of propaganda, censorship, and the erosion of personal freedoms, serving as a cautionary tale for future generations.", null, null, new DateTime(1949, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "A dystopian novel exploring the dangers of totalitarianism.", "1984" },
                    { 5, 5, 4.8300000000000001, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "https://m.media-amazon.com/images/I/61Lpsc7B3jL.jpg", true, false, "One Flew Over the Cuckoo's Nest, a novel by Ken Kesey, takes place in a mental institution and explores themes of individuality, freedom, and rebellion against oppressive systems. The protagonist, Randle P. McMurphy, a charismatic convict, fakes insanity to serve his sentence in a psychiatric hospital instead of prison. He clashes with Nurse Ratched, the authoritarian head nurse, and inspires the other patients to assert their independence. The story, narrated by Chief Bromden, a silent observer and fellow patient, examines the dynamics of power and the human spirit's resilience. Published in 1962, the book was adapted into a 1975 film that won five Academy Awards. It remains a poignant critique of institutional control and a celebration of nonconformity.", null, null, new DateTime(1962, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "A story about individuality and institutional control.", "One Flew Over the Cuckoo's Nest" },
                    { 6, 1, 4.8300000000000001, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "https://m.media-amazon.com/images/I/91U7HNa2NQL._AC_UF1000,1000_QL80_.jpg", true, false, "The Shining, written by Stephen King, is a psychological horror novel set in the remote Overlook Hotel. Jack Torrance, an aspiring writer and recovering alcoholic, takes a job as the hotel's winter caretaker, bringing his wife Wendy and young son Danny with him. Danny possesses 'the shining,' a psychic ability that allows him to see the hotel's horrific past. As winter sets in, the isolation and supernatural forces within the hotel drive Jack into a murderous frenzy, threatening his family. Published in 1977, The Shining explores themes of addiction, domestic violence, and the fragility of sanity. The novel was adapted into a 1980 film by Stanley Kubrick, though it diverged from King's vision. A sequel, Doctor Sleep, was published in 2013, continuing Danny's story.", null, null, new DateTime(1977, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "A chilling tale of isolation and madness.", "The Shining" },
                    { 7, 6, 4.5999999999999996, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "https://m.media-amazon.com/images/I/91wOUFZCE+L._UF1000,1000_QL80_.jpg", true, false, "Dracula, written by Bram Stoker, is a classic Gothic horror novel that tells the story of Count Dracula's attempt to move from Transylvania to England in order to spread the undead curse, and his subsequent battle with a group of people led by the determined Professor Abraham Van Helsing. The novel is written in epistolary form, with the story told through letters, diary entries, newspaper clippings, and a ship's log. At the heart of Dracula is the struggle between good and evil, as the characters fight to destroy the vampire lord who threatens to spread his dark influence. Themes of fear, desire, sexuality, and superstition permeate the novel, along with reflections on Victorian society's attitudes toward these topics. Published in 1897, Dracula has had a profound impact on the vampire genre, inspiring numerous adaptations in film, television, and popular culture. The novel explores the dangers of unchecked power, the mystery of the unknown, and the terror of the supernatural, cementing Count Dracula as one of literature's most famous and enduring villains.", null, null, new DateTime(1897, 5, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "A gothic horror novel about the legendary vampire Count Dracula.", "Dracula" },
                    { 8, 1, 5.0, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1334416842i/830502.jpg", true, false, "Published in 1986, It is one of Stephen King's most iconic novels, a gripping horror story about the terror that haunts the small town of Derry, Maine. The novel follows a group of childhood friends who come together as adults to confront an ancient evil that takes the form of Pennywise, a shape-shifting entity that primarily appears as a killer clown. The novel explores the power of fear, the strength of friendship, and the eternal battle between good and evil. As the friends, who call themselves 'The Losers,' face off against Pennywise, they must confront their own childhood traumas and deepest fears. It is a chilling exploration of memory, courage, and the horrors of both childhood and adulthood. With its mix of supernatural terror and profound human emotion, It has become a cultural touchstone, inspiring a miniseries, films, and countless discussions about its themes of fear, friendship, and the horrors that lie beneath the surface of everyday life. King’s masterful storytelling and vivid portrayal of childhood and fear make It one of the most enduring works of horror fiction in the genre.", null, null, new DateTime(1986, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "A terrifying tale of childhood fears, and the evil that lurks beneath the surface of a small town.", "It" },
                    { 9, 5, 5.0, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "https://www.bookstation.ie/wp-content/uploads/2019/05/9780141036137.jpg", true, false, "First published in 1945, Animal Farm by George Orwell is a powerful political allegory that critiques totalitarian regimes and explores the dangers of power and corruption. The novella is set on a farm where the animals overthrow their human oppressors and establish a new government, only to find that the new leadership, led by the pigs, becomes just as corrupt as the humans they replaced. The story, told through the animals' perspective, mirrors the events leading up to the Russian Revolution and the rise of Stalinism. Orwell's use of anthropomorphic animals to represent different political figures and ideologies makes Animal Farm both accessible and deeply poignant. Its themes of betrayal, exploitation, and the corruption of ideals are timeless, making Animal Farm a critical commentary on power, leadership, and the manipulation of the masses. The novella has had a significant impact on literature and political thought, remaining a key work in the discussion of totalitarianism and social justice.", null, null, new DateTime(1945, 8, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "A satirical allegory of totalitarianism, exploring the rise of power and corruption.", "Animal Farm" },
                    { 10, 7, 4.2999999999999998, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "https://m.media-amazon.com/images/I/61rZCYUYXuL._AC_UF1000,1000_QL80_.jpg", true, false, "First published in 1962, A Clockwork Orange by Anthony Burgess is a controversial and thought-provoking dystopian novel that examines the nature of free will, violence, and the conflict between individuality and societal control. The story is set in a near-future society and follows Alex, a teenage delinquent who leads a gang of criminals. Alex's journey is told in a unique style, using a fictional slang called 'Nadsat.' The novel explores the psychological and moral implications of the state-sponsored efforts to 'reform' Alex, subjecting him to a form of aversion therapy that strips him of his ability to choose between good and evil. Through this exploration, Burgess raises important questions about free will, the ethics of punishment, and the role of the state in shaping individual behavior. A Clockwork Orange is both a disturbing and insightful critique of modern society and its institutions. The novel has become a cultural touchstone, adapted into a film by Stanley Kubrick and continuing to inspire discussions on the nature of freedom, control, and human nature.", null, null, new DateTime(1962, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "A dystopian novel that explores free will, violence, and the consequences of societal control.", "A Clockwork Orange" },
                    { 11, 8, 4.25, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "https://lyceumtheatre.org/wp-content/uploads/2019/09/Murder-on-the-Orient-Express-WebPstr.jpg", true, false, "Murder on the Orient Express, first published in 1934, is one of Agatha Christie’s most famous works, featuring her legendary Belgian detective Hercule Poirot. The story takes place aboard the luxurious train, the Orient Express, where a wealthy American passenger, Samuel Ratchett, is found murdered in his compartment. Poirot, who happens to be traveling on the train, is asked to investigate the crime. As he delves deeper into the case, Poirot uncovers a complex web of lies and hidden motives. The passengers, all seemingly innocent, each have something to hide, and the detective must use his sharp mind to piece together the truth. Christie’s masterful plot, full of twists and red herrings, keeps readers guessing until the very end. The novel explores themes of justice, revenge, and the moral ambiguity of crime, making it a timeless and captivating mystery that has been adapted into numerous films and television series.", null, null, new DateTime(1934, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "A classic Hercule Poirot mystery aboard the luxurious Orient Express, filled with twists and a brilliant solution to a baffling crime.", "Murder on the Orient Express" },
                    { 12, 8, 4.7000000000000002, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "https://upload.wikimedia.org/wikipedia/en/5/57/The_Murder_of_Roger_Ackroyd_First_Edition_Cover_1926.jpg", true, false, "The Murder of Roger Ackroyd, first published in 1926, is one of Agatha Christie's most groundbreaking works, featuring her famous detective Hercule Poirot. The story is set in the quiet village of King's Abbot, where the wealthy Roger Ackroyd is found murdered in his study. The case takes on new complexity when Poirot, who is retired in the village, is drawn into the investigation by Ackroyd's fiancée, Mrs. Ferrars, who has died under mysterious circumstances just days before. As Poirot begins to unravel the case, he discovers that nearly everyone in the village is hiding something, and he must use his unparalleled skills of deduction to piece together the truth. Christie’s brilliant twist ending revolutionized the genre and is still one of the most celebrated and discussed endings in detective fiction. The novel touches on themes of deceit, betrayal, and the nature of truth, making it a timeless classic in the mystery genre.", null, null, new DateTime(1926, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "A groundbreaking Hercule Poirot mystery that reshaped the detective genre with its iconic twist ending.", "The Murder of Roger Ackroyd" },
                    { 13, 9, 4.5, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "https://m.media-amazon.com/images/I/91gmBp2wQNL._AC_UF894,1000_QL80_.jpg", true, false, "East of Eden, published in 1952, is one of John Steinbeck’s most ambitious and famous works. The novel explores the complex relationships between two families, the Trasks and the Hamiltons, in California's Salinas Valley during the early 20th century. Central to the narrative are the themes of good versus evil, inherited sin, and the choices that define our lives. Steinbeck uses the biblical story of Cain and Abel as a backdrop, drawing parallels between the characters' struggles and moral dilemmas. The novel is a sweeping exploration of human nature, as well as the destructive impact of jealousy, desire, and guilt. With its vivid descriptions of the California landscape and rich characterizations, East of Eden is considered one of Steinbeck's greatest works, illustrating his deep understanding of the human condition.", null, null, new DateTime(1952, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "A sprawling, multigenerational story of good and evil, focusing on two families in California's Salinas Valley.", "East of Eden" },
                    { 14, 10, 4.6699999999999999, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "https://www.book.store.bg/dcrimg/340714/dune.jpg", true, false, "Dune, first published in 1965, is Frank Herbert's most iconic and influential novel, often regarded as one of the greatest works of science fiction. The novel is set on the desert planet of Arrakis, also known as Dune, the only source of the most valuable substance in the universe, spice melange. The story follows Paul Atreides, the young heir to House Atreides, as he navigates political intrigue, power struggles, and the harsh desert environment. The novel touches on themes of power, ecology, religion, and the future of humanity. Herbert creates a detailed world filled with complex social, political, and ecological systems that interweave throughout the narrative. *Dune* is renowned for its intricate plotting, philosophical depth, and exploration of human potential. Its impact on both science fiction and modern literature is immeasurable, and it remains a defining work of the genre.", null, null, new DateTime(1965, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "A sweeping science fiction epic set on the desert planet Arrakis, exploring politics, religion, and ecology.", "Dune" },
                    { 15, 11, 4.5, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1404613595i/13.jpg", true, false, "The Hitchhiker's Guide to the Galaxy, first published in 1979, is Douglas Adams' most famous and influential work, blending science fiction, humor, and satire. The story begins with Arthur Dent, an ordinary man, who is suddenly whisked away from Earth just before it is destroyed to make way for an intergalactic freeway. Arthur joins Ford Prefect, an alien researcher for the titular Guide, on a wild journey through space, encountering strange planets, peculiar beings, and the galaxy's most incompetent bureaucracy. The novel explores themes of the absurdity of life, the meaning of existence, and the randomness of the universe, all wrapped in Adams' signature wit and absurdity. Known for its irreverence and humor, *The Hitchhiker's Guide to the Galaxy* has become a cult classic, inspiring numerous adaptations in radio, television, and film. Its influence on science fiction and comedy continues to resonate with readers and fans worldwide.", null, null, new DateTime(1979, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "A comedic science fiction adventure that follows Arthur Dent's absurd journey through space after Earth is destroyed.", "The Hitchhiker's Guide to the Galaxy" }
                });

            migrationBuilder.InsertData(
                table: "BooksGenres",
                columns: new[] { "BookId", "GenreId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 4 },
                    { 1, 6 },
                    { 1, 30 },
                    { 2, 3 },
                    { 2, 7 },
                    { 2, 14 },
                    { 2, 15 },
                    { 2, 32 },
                    { 3, 3 },
                    { 3, 7 },
                    { 3, 24 },
                    { 4, 19 },
                    { 4, 25 },
                    { 4, 28 },
                    { 4, 29 },
                    { 5, 13 },
                    { 5, 19 },
                    { 5, 26 },
                    { 5, 28 },
                    { 5, 29 },
                    { 6, 1 },
                    { 6, 6 },
                    { 6, 29 },
                    { 6, 30 },
                    { 7, 1 },
                    { 7, 6 },
                    { 7, 7 },
                    { 7, 8 },
                    { 7, 30 },
                    { 7, 31 },
                    { 8, 1 },
                    { 8, 3 },
                    { 8, 6 },
                    { 8, 7 },
                    { 9, 19 },
                    { 9, 25 },
                    { 9, 28 },
                    { 9, 29 },
                    { 10, 19 },
                    { 10, 21 },
                    { 10, 26 },
                    { 10, 28 },
                    { 10, 29 },
                    { 11, 4 },
                    { 11, 6 },
                    { 11, 21 },
                    { 12, 4 },
                    { 12, 6 },
                    { 12, 21 },
                    { 13, 8 },
                    { 13, 13 },
                    { 13, 26 },
                    { 13, 28 },
                    { 14, 2 },
                    { 14, 3 },
                    { 14, 7 },
                    { 14, 24 },
                    { 14, 25 }
                });

            migrationBuilder.InsertData(
                table: "ReadingLists",
                columns: new[] { "BookId", "Status", "UserId", "CreatedBy", "CreatedOn", "ModifiedBy", "ModifiedOn" },
                values: new object[,]
                {
                    { 1, 0, "user1Id", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 2, 0, "user1Id", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 3, 0, "user1Id", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 4, 1, "user1Id", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 5, 1, "user1Id", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 6, 2, "user1Id", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 1, 0, "user2Id", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 5, 0, "user2Id", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 6, 0, "user2Id", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 10, 1, "user2Id", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 12, 2, "user2Id", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 4, 1, "user3Id", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 7, 0, "user3Id", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 8, 0, "user3Id", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 10, 0, "user3Id", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 12, 2, "user3Id", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 13, 2, "user3Id", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "BookId", "Content", "CreatedBy", "CreatedOn", "CreatorId", "DeletedBy", "DeletedOn", "IsDeleted", "ModifiedBy", "ModifiedOn", "Rating" },
                values: new object[,]
                {
                    { 1, 1, "A truly chilling tale. King masterfully explores the dark side of human grief and love.", "user1name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1Id", null, null, false, null, null, 5 },
                    { 2, 1, "The book was gripping but felt a bit too disturbing at times for my taste.", "user2name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", null, null, false, null, null, 3 },
                    { 3, 1, "An unforgettable story that haunts you long after you've finished it. Highly recommended!", "user3name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", null, null, false, null, null, 5 },
                    { 4, 1, "The characters were well-developed, but the plot felt predictable toward the end.", "user4name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user4Id", null, null, false, null, null, 4 },
                    { 5, 2, "An incredible conclusion to the series. Every twist and turn kept me on edge.", "user1name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1Id", null, null, false, null, null, 5 },
                    { 6, 2, "The Battle of Hogwarts was epic! A bittersweet yet satisfying ending.", "user2name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", null, null, false, null, null, 5 },
                    { 7, 2, "I expected more from some of the character arcs, but still a solid read.", "user3name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", null, null, false, null, null, 4 },
                    { 8, 2, "Rowling’s world-building continues to amaze, even in the final installment.", "user4name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user4Id", null, null, false, null, null, 5 },
                    { 9, 3, "A timeless masterpiece. Tolkien’s world and characters are unmatched in depth and richness.", "user1name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1Id", null, null, false, null, null, 5 },
                    { 10, 3, "The pacing was slow at times, but the payoff in the end was well worth it.", "user2name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", null, null, false, null, null, 4 },
                    { 11, 3, "The bond between Sam and Frodo is the heart of this epic journey. Beautifully written.", "user3name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", null, null, false, null, null, 5 },
                    { 12, 3, "An epic tale that defines the fantasy genre. Loved every moment of it.", "user4name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user4Id", null, null, false, null, null, 5 },
                    { 13, 3, "The attention to detail in Middle-earth is staggering. Tolkien is a true genius.", "user5name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user5Id", null, null, false, null, null, 5 },
                    { 14, 3, "A bit long for my liking, but undeniably one of the greatest stories ever told.", "user6name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user6Id", null, null, false, null, null, 4 },
                    { 15, 4, "A haunting vision of the future that feels increasingly relevant today.", "user1name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1Id", null, null, false, null, null, 5 },
                    { 16, 4, "An intense and thought-provoking read. Orwell's insights are unparalleled.", "user2name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", null, null, false, null, null, 5 },
                    { 17, 4, "I struggled with some parts, but the message is profoundly impactful.", "user3name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", null, null, false, null, null, 4 },
                    { 18, 4, "A must-read for anyone concerned about privacy and freedom.", "user4name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user4Id", null, null, false, null, null, 5 },
                    { 19, 4, "Chilling and unforgettable. Orwell's world feels disturbingly possible.", "user5name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user5Id", null, null, false, null, null, 5 },
                    { 20, 5, "McMurphy is such a memorable character. This book stays with you.", "user1name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1Id", null, null, false, null, null, 5 },
                    { 21, 5, "Kesey’s writing is poetic and raw. A heartbreaking tale of rebellion.", "user2name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", null, null, false, null, null, 5 },
                    { 22, 5, "I loved how the narrative builds through Chief Bromden's perspective.", "user3name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", null, null, false, null, null, 4 },
                    { 23, 5, "A sobering exploration of power and freedom within a broken system.", "user4name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user4Id", null, null, false, null, null, 5 },
                    { 24, 5, "The tension between McMurphy and Nurse Ratched is electric.", "user5name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user5Id", null, null, false, null, null, 5 },
                    { 25, 5, "A deeply human story that makes you think about society’s flaws.", "user6name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user6Id", null, null, false, null, null, 5 },
                    { 26, 6, "Terrifying and beautifully written. Stephen King is at his best.", "user1name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1Id", null, null, false, null, null, 5 },
                    { 27, 6, "Danny’s ‘shining’ powers add so much depth to this chilling story.", "user2name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", null, null, false, null, null, 5 },
                    { 28, 6, "The Overlook Hotel is as much a character as the Torrance family.", "user3name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", null, null, false, null, null, 5 },
                    { 29, 6, "I couldn’t put it down! The suspense is incredible.", "user4name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user4Id", null, null, false, null, null, 5 },
                    { 30, 6, "Jack’s descent into madness is both horrifying and tragic.", "user5name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user5Id", null, null, false, null, null, 4 },
                    { 31, 6, "King perfectly balances psychological and supernatural horror.", "user6name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user6Id", null, null, false, null, null, 5 },
                    { 32, 7, "A timeless classic that redefined the vampire genre. Dracula is both terrifying and captivating.", "user1name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1Id", null, null, false, null, null, 5 },
                    { 33, 7, "The atmosphere of dread and suspense is palpable. Stoker's writing is chilling and immersive.", "user2name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", null, null, false, null, null, 4 },
                    { 34, 7, "Count Dracula is a villain for the ages. A fascinating tale of good versus evil with unforgettable characters.", "user3name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", null, null, false, null, null, 5 },
                    { 35, 7, "A brilliant mix of Gothic horror and suspense. Stoker’s world-building and vivid imagery are unmatched.", "user4name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user4Id", null, null, false, null, null, 5 },
                    { 36, 7, "A dark, thrilling journey that explores the depths of fear and obsession. Dracula remains one of the greatest horror novels ever written.", "user5name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user5Id", null, null, false, null, null, 4 },
                    { 37, 8, "A terrifying masterpiece that captures the horror of childhood fears and the bond of friendship. Pennywise is unforgettable.", "user1name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1Id", null, null, false, null, null, 5 },
                    { 38, 8, "Stephen King's storytelling is unparalleled. *It* is a terrifying and emotionally resonant journey that stays with you long after finishing.", "user2name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", null, null, false, null, null, 5 },
                    { 39, 8, "The depth of character development and the chilling horror elements make *It* an instant classic. A must-read for any horror fan.", "user3name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", null, null, false, null, null, 5 },
                    { 40, 8, "An epic tale of fear, courage, and friendship. King's ability to balance the supernatural with deeply human emotions is astounding.", "user4name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user4Id", null, null, false, null, null, 5 },
                    { 41, 9, "A powerful allegory that exposes the dangers of totalitarianism. Orwell's sharp critique of power and corruption is as relevant today as ever.", "user1name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1Id", null, null, false, null, null, 5 },
                    { 42, 9, "A brilliantly crafted political satire that uses animals to dissect the flaws of human nature and governance. A must-read for anyone interested in societal dynamics.", "user2name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", null, null, false, null, null, 5 },
                    { 43, 10, "A bold exploration of free will, violence, and societal control. Burgess' unique language and dark humor make this a thought-provoking read.", "user1name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1Id", null, null, false, null, null, 5 },
                    { 44, 10, "A chilling and unsettling look at the consequences of state control over individual freedom. The invented language makes it even more immersive.", "user2name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", null, null, false, null, null, 4 },
                    { 45, 10, "This novel challenges ideas of morality and free will, with a disturbing yet captivating narrative. It's hard to put down once you start.", "user3name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", null, null, false, null, null, 5 },
                    { 46, 10, "A powerful exploration of violence and society, but the language barrier can be a bit hard to get used to at first. Still, it's a masterpiece of dystopian fiction.", "user4name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user4Id", null, null, false, null, null, 4 },
                    { 47, 10, "An unforgettable story that questions the nature of human behavior and control. The narrative is dark and surreal, but it raises important questions about freedom and choice.", "user5name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user5Id", null, null, false, null, null, 5 },
                    { 48, 10, "A fascinating, though disturbing, book that delves into the mind of its violent protagonist. Its style is unique, but not everyone will appreciate it.", "user6name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user6Id", null, null, false, null, null, 3 },
                    { 49, 11, "A masterclass in mystery writing. Agatha Christie's ability to weave a complex plot with unexpected twists is unparalleled. A must-read for mystery lovers!", "user1name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1Id", null, null, false, null, null, 5 },
                    { 50, 11, "A brilliant detective story filled with intrigue and suspense. The ending is truly unexpected, and the characters are well-developed. One of the best Poirot novels.", "user2name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", null, null, false, null, null, 5 },
                    { 51, 11, "While the plot is engaging, I found some of the character motivations a bit too contrived. Still, it's an enjoyable read for fans of classic detective fiction.", "user3name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", null, null, false, null, null, 4 },
                    { 52, 11, "This is a solid mystery, but the conclusion felt a bit rushed. Poirot's reasoning is impeccable, but I was hoping for a more satisfying resolution.", "user4name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user4Id", null, null, false, null, null, 3 },
                    { 53, 12, "A brilliant and mind-bending mystery with one of the best plot twists in literary history. Agatha Christie at her finest!", "user1name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1Id", null, null, false, null, null, 5 },
                    { 54, 12, "An incredibly well-crafted story. The twist was shocking, but the pacing felt slow at times. Still, an iconic piece of detective fiction.", "user2name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", null, null, false, null, null, 4 },
                    { 55, 12, "Christie delivers a thrilling and suspenseful plot that kept me guessing until the very end. The twist was absolutely unexpected, making this a standout in the genre.", "user3name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", null, null, false, null, null, 5 },
                    { 56, 13, "A masterwork of fiction that delves into the depths of human nature. Steinbeck's exploration of good and evil, coupled with his rich characters and vivid settings, makes this novel unforgettable.", "user1name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1Id", null, null, false, null, null, 5 },
                    { 57, 13, "East of Eden is an epic story, but it can feel overwhelming at times. While the themes of sin and redemption are powerful, the pacing can be slow in parts. However, Steinbeck's writing is undeniably brilliant.", "user2name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", null, null, false, null, null, 4 },
                    { 58, 13, "An incredibly complex and thought-provoking novel. The story of the Trask and Hamilton families is both heartbreaking and inspiring. Steinbeck creates unforgettable characters and a gripping narrative.", "user3name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", null, null, false, null, null, 5 },
                    { 59, 13, "While I found the writing to be beautiful, the novel's length and depth made it a challenging read. However, it’s clear why East of Eden is considered one of Steinbeck's masterpieces.", "user4name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user4Id", null, null, false, null, null, 4 },
                    { 60, 14, "A groundbreaking masterpiece in the sci-fi genre. *Dune* is a deeply philosophical and complex novel that explores themes of politics, religion, and ecology. The world-building is extraordinary, and the story's depth will stay with you long after you've finished reading.", "user1name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1Id", null, null, false, null, null, 5 },
                    { 61, 14, "Herbert's *Dune* is an epic tale with intricate world-building and a complex, multi-layered narrative. While it can be slow-paced at times, the book offers a compelling story of power, survival, and human evolution. Definitely a must-read for science fiction enthusiasts.", "user2name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", null, null, false, null, null, 4 },
                    { 62, 14, "A truly immersive experience that challenges readers with its philosophical insights and political commentary. *Dune* is a dense, but rewarding read that covers timeless themes of leadership, environmental stewardship, and human resilience. However, it can be heavy and difficult to follow at times.", "user3name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", null, null, false, null, null, 5 },
                    { 63, 15, "A brilliant mix of wit and sci-fi! Douglas Adams' humor shines through in every page, making this a memorable and laugh-out-loud read. A true classic of the genre.", "user1name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1Id", null, null, false, null, null, 5 },
                    { 64, 15, "An absolutely hilarious and thought-provoking journey through space. The absurdity of the plot paired with sharp social commentary makes it both entertaining and intelligent.", "user2name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", null, null, false, null, null, 4 },
                    { 65, 15, "The Hitchhiker's Guide to the Galaxy is a rollercoaster of absurdity, with quirky characters and laugh-out-loud moments. The satire on life, the universe, and everything is simply genius!", "user3name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", null, null, false, null, null, 5 },
                    { 66, 15, "A fun and wildly entertaining read, though the randomness and eccentricities of the story may be off-putting for some. Still, it's a fantastic book that has earned its place in sci-fi history.", "user4name", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user4Id", null, null, false, null, null, 4 }
                });

            migrationBuilder.InsertData(
                table: "Votes",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "CreatorId", "IsUpvote", "ModifiedBy", "ModifiedOn", "ReviewId" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", true, null, null, 1 },
                    { 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", true, null, null, 1 },
                    { 3, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user4Id", true, null, null, 1 },
                    { 4, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user5Id", true, null, null, 1 },
                    { 5, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user6Id", false, null, null, 1 },
                    { 6, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", true, null, null, 2 },
                    { 7, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", true, null, null, 2 },
                    { 8, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", true, null, null, 15 },
                    { 9, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", true, null, null, 15 },
                    { 10, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user4Id", true, null, null, 15 },
                    { 11, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", true, null, null, 15 },
                    { 12, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", true, null, null, 20 },
                    { 13, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", true, null, null, 20 },
                    { 14, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", true, null, null, 20 },
                    { 15, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", true, null, null, 26 },
                    { 16, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", true, null, null, 26 },
                    { 17, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user4Id", true, null, null, 26 },
                    { 18, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1Id", true, null, null, 32 },
                    { 19, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", true, null, null, 32 },
                    { 20, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", true, null, null, 32 },
                    { 21, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user4Id", true, null, null, 32 },
                    { 22, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user5Id", true, null, null, 32 },
                    { 23, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user6Id", false, null, null, 32 },
                    { 24, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1Id", true, null, null, 37 },
                    { 25, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", true, null, null, 37 },
                    { 26, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", true, null, null, 37 },
                    { 27, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1Id", true, null, null, 41 },
                    { 28, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", true, null, null, 41 },
                    { 29, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", true, null, null, 41 },
                    { 30, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1Id", true, null, null, 43 },
                    { 31, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", true, null, null, 43 },
                    { 32, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", true, null, null, 43 },
                    { 33, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", true, null, null, 49 },
                    { 34, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", true, null, null, 53 },
                    { 35, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", true, null, null, 56 },
                    { 36, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", true, null, null, 60 },
                    { 37, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", true, null, null, 63 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_CreatorId",
                table: "Authors",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_NationalityId",
                table: "Authors",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_CreatorId",
                table: "Books",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_BooksGenres_GenreId",
                table: "BooksGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingLists_BookId",
                table: "ReadingLists",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_CreatorId",
                table: "Replies",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_ReviewId",
                table: "Replies",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_BookId",
                table: "Reviews",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CreatorId",
                table: "Reviews",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_CreatorId",
                table: "Votes",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_ReviewId",
                table: "Votes",
                column: "ReviewId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BooksGenres");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "ReadingLists");

            migrationBuilder.DropTable(
                name: "Replies");

            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Nationalities");
        }
    }
}
