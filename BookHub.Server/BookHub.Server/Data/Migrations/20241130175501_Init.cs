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
                    Introduction = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
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
                    ShortDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LongDescription = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
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
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
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
                    { 1, "Stephen King's *Pet Sematary* offers a chilling narrative about the emotional weight of grief and the lengths people go to in an effort to reverse the irreversible. When Louis Creed and his family move to rural Maine, they discover a mysterious burial ground behind their home that can bring the dead back to life. What begins as an innocent exploration of local lore quickly turns into a nightmare when Louis faces the ultimate temptation: using the cemetery’s power to alter fate itself. The novel dives deep into human frailty, moral dilemmas,and the unintended consequences of playing God. King's skillful writing leaves readers pondering the fine line between love and selfishness, as well as the destructive force of denial.", null, new DateTime(2024, 11, 30, 19, 54, 59, 977, DateTimeKind.Local).AddTicks(8503), null, null, "https://thedailytexan.com/wp-content/uploads/2023/09/pet_sematary_bloodlines_4press-1200x800.jpg", "A tale of love, loss, and the chilling cost of defying death.", false, null, null, "Exploring the Haunting Depths of *Pet Sematary*", 0 },
                    { 2, "In *Harry Potter and the Deathly Hallows*, J.K. Rowling masterfully concludes the saga of the Boy Who Lived. Harry, Ron, and Hermione take center stage as they leave Hogwarts behind and embark on a perilous journey to locate and destroy Voldemort’sHorcruxes. Along the way, they uncover hidden truths about Dumbledore’s past, face betrayal, and reaffirm their unshakable friendship. The novel culminates in the iconic Battle of Hogwarts, where courage and sacrifice define the fates of beloved characters. Packed with heart-pounding action and poignant moments, Rowling’s finale is not just a fight between good and evilbut a meditation on love, legacy, and the choices that shape our lives.", null, new DateTime(2024, 11, 30, 19, 54, 59, 977, DateTimeKind.Local).AddTicks(8632), null, null, "https://choicefineart.com/cdn/shop/products/book-7-harry-potter-and-the-deathly-hallows-311816.jpg?v=1688079541", "The battle against Voldemort reaches its thrilling and emotional finale.", false, null, null, "The Epic Conclusion: *Harry Potter and the Deathly Hallows*", 0 },
                    { 3, "J.R.R. Tolkien’s *The Lord of the Rings* stands as one of the greatest works of fantasy ever written. Spanning three volumes, the story follows Frodo Baggins and the Fellowship as they embark on a dangerous mission to destroy the One Ring, a powerful artifact that threatens to engulf Middle-earth in darkness. Tolkien’s world-building is unparalleled, bringing to life the rich landscapes of Middle-earth, from the idyllic Shire to the fiery Mount Doom. Along the way, characters like Aragorn, Legolas, and Samwise Gamgee demonstrate the values of loyalty, perseverance, and hope. The narrative explores themes of power, corruption, and the resilience of the human spirit, making it a tale that resonates across generations. Whether it’s Gandalf’s wisdom, Frodo’s burden, or the triumph of unity, Tolkien’s masterpiece continues to inspire and enchant readers worldwide.", null, new DateTime(2024, 11, 30, 19, 54, 59, 977, DateTimeKind.Local).AddTicks(8643), null, null, "https://blogger.googleusercontent.com/img/b/R29vZ2xl/AVvXsEibI7_-Az0QVZhhwZO_PcgrNRK7RYnS7JPiddt_LvTC8NTgTzzYcaagGBLR6KtgY1J_VyZzS6HhL7MW9x1h-rioISPanc-daPbdgnZCQQb48PNELDt9gbQlohCJuXGHgritNS_3Ff08oUhs/w1200-h630-p-k-no-nu/acetolkien.jpg", "A journey of courage, friendship, and the fight against overwhelming darkness.", false, null, null, "The Timeless Epic: *The Lord of the Rings*", 0 }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "Email", "EmailConfirmed", "IsDeleted", "LockoutEnabled", "LockoutEnd", "ModifiedBy", "ModifiedOn", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "user1Id", 0, "6e71d9fd-cb0c-4c18-b4f8-8a09b98e5d2a", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "user1@mail.com", false, false, false, null, null, null, null, null, null, null, false, "e68f7799-169a-4e99-9261-f0eb6a3b640b", false, "user1name" },
                    { "user2Id", 0, "22d14517-4c8c-46d5-8f9e-b4499c809dbf", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "user2@mail.com", false, false, false, null, null, null, null, null, null, null, false, "a5fd96f5-4c8e-46a5-bfb8-83e15b4f681e", false, "user2name" },
                    { "user3Id", 0, "436849b5-3eb5-40da-a2e7-26c7d5a7ffb2", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "user3@mail.com", false, false, false, null, null, null, null, null, null, null, false, "d281b5d6-004c-4444-8673-7558cc795994", false, "user3name" },
                    { "user4Id", 0, "73a7b8b9-b407-4fdf-a560-bdd3aead7e0a", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "user4@mail.com", false, false, false, null, null, null, null, null, null, null, false, "f046f46c-94b3-420c-88e2-b23161a427fd", false, "user4name" },
                    { "user5Id", 0, "8c9f3281-db0f-4510-96d6-10d57968d8ea", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "user5@mail.com", false, false, false, null, null, null, null, null, null, null, false, "14c64b16-3440-4e95-be5a-a3ce681b7046", false, "user5name" },
                    { "user6Id", 0, "4618ca90-889c-46b9-b67e-70d98aed533c", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "user6@mail.com", false, false, false, null, null, null, null, null, null, null, false, "79cac276-bf73-4a7e-ac71-cbee59109869", false, "user6name" }
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
                    { 8, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Historical fiction immerses readers in the past, blending factual events with fictional narratives to create vivid portrayals of bygone eras. These stories illuminate the lives, struggles, and triumphs of people from different times, providing insight into cultural, social, and political contexts. Subgenres include historical romance, historical mysteries, and alternate histories, each offering unique perspectives. The genre enriches our understanding of history while engaging us with compelling characters and plots.", "https://celadonbooks.com/wp-content/uploads/2020/03/Historical-Fiction-scaled.jpg", false, null, null, "Historical" },
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
                    { 20, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Spirituality books delve into the deeper questions of existence, faith, and the human soul, offering insights and practices to nurture inner peace and personal growth. They often explore themes of mindfulness, self-awareness, and connection to a higher power or universal energy. From philosophical reflections to practical guides, these works resonate with readers seekinginspiration, understanding, and spiritual fulfillment across diverse traditions and belief systems.", "https://m.media-amazon.com/images/I/61jxcM3UskL._AC_UF1000,1000_QL80_.jpg", false, null, null, "Spirituality" }
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
                    { 1, 4.25, "Stephen Edwin King (born September 21, 1947) is an American author. Widely known for his horror novels, he has been crowned the \"King of Horror\". He has also explored other genres, among them suspense, crime, science-fiction, fantasy and mystery. Though known primarily for his novels, he has written approximately 200 short stories, most of which have been published in collections.", new DateTime(1947, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1Id", null, null, null, 0, "https://media.npr.org/assets/img/2020/04/07/stephen-king.by-shane-leonard_wide-c132de683d677c7a6a1e692f86a7e6670baf3338.jpg?s=1100&c=85&f=jpeg", true, false, null, null, "Stephen King", 182, "Richard Bachman", 0 },
                    { 2, 4.75, "Joanne Rowling known by her pen name J. K. Rowling, is a British author and philanthropist. She is the author of Harry Potter, a seven-volume fantasy novel series published from 1997 to 2007. The series has sold over 600 million copies, been translated into 84 languages, and spawned a global media franchise including films and video games. The Casual Vacancy (2012) was her first novel for adults. She writes Cormoran Strike, an ongoing crime fiction series, under the alias Robert Galbraith.", new DateTime(1965, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", null, null, null, 1, "https://stories.jkrowling.com/wp-content/uploads/2021/09/Shot-B-105_V2_CROP-e1630873059779.jpg", true, false, null, null, "Joanne Rowling", 181, "J. K. Rowling", 0 },
                    { 3, 4.6699999999999999, "John Ronald Reuel Tolkien was an English writer and philologist. He was the author of the high fantasy works The Hobbit and The Lord of the Rings.", new DateTime(1892, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", null, null, new DateTime(1973, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d4/J._R._R._Tolkien%2C_ca._1925.jpg/330px-J._R._R._Tolkien%2C_ca._1925.jpg", true, false, null, null, "John Ronald Reuel Tolkien ", 181, "J.R.R Tolkien", 0 }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "AverageRating", "CreatedBy", "CreatedOn", "CreatorId", "DeletedBy", "DeletedOn", "ImageUrl", "IsApproved", "IsDeleted", "LongDescription", "ModifiedBy", "ModifiedOn", "PublishedDate", "RatingsCount", "ShortDescription", "Title" },
                values: new object[,]
                {
                    { 1, 1, 4.25, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1Id", null, null, "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg/330px-Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg", true, false, "Pet Sematary is a 1983 horror novel by American writer Stephen King. The novel was nominated for a World Fantasy Award for Best Novel in 1984,and adapted into two films: one in 1989 and another in 2019. In November 2013, PS Publishing released Pet Sematary in a limited 30th-anniversary edition.", null, null, new DateTime(1983, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Sometimes dead is better.", "Pet Sematary" },
                    { 2, 2, 4.75, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", null, null, "https://librerialaberintopr.com/cdn/shop/products/hallows_459x.jpg?v=1616596206", true, false, "Harry Potter and the Deathly Hallows is a fantasy novel written by the British author J. K. Rowling. It is the seventh and final novel in the Harry Potter series. It was released on 21 July 2007 in the United Kingdom by Bloomsbury Publishing, in the United States by Scholastic, and in Canada by Raincoast Books. The novel chronicles the events directly following Harry Potter and the Half-Blood Prince (2005) and the final confrontation between the wizards Harry Potter and Lord Voldemort.", null, null, new DateTime(2007, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "The last book from the Harry Potter series.", "Harry Potter and the Deathly Hallows" },
                    { 3, 3, 4.6699999999999999, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", null, null, "https://upload.wikimedia.org/wikipedia/en/e/e9/First_Single_Volume_Edition_of_The_Lord_of_the_Rings.gif", true, false, "Set in Middle-earth, the story began as a sequel to Tolkien's 1937 children's book The Hobbit, but eventually developed into a much larger work. Written in stages between 1937 and 1949, The Lord of the Rings is one of the best-selling books ever written, with over 150 million copies sold.", null, null, new DateTime(1954, 7, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "The Lord of the Rings is an epic fantasy novel by the English author J. R. R. Tolkien.", "Lord of the Rings" }
                });

            migrationBuilder.InsertData(
                table: "BooksGenres",
                columns: new[] { "BookId", "GenreId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 6 },
                    { 2, 3 },
                    { 2, 14 },
                    { 2, 15 },
                    { 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "BookId", "Content", "CreatedBy", "CreatedOn", "CreatorId", "DeletedBy", "DeletedOn", "IsDeleted", "ModifiedBy", "ModifiedOn", "Rating" },
                values: new object[,]
                {
                    { 1, 1, "A truly chilling tale. King masterfully explores the dark side of human grief and love.", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1Id", null, null, false, null, null, 5 },
                    { 2, 1, "The book was gripping but felt a bit too disturbing at times for my taste.", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", null, null, false, null, null, 3 },
                    { 3, 1, "An unforgettable story that haunts you long after you've finished it. Highly recommended!", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", null, null, false, null, null, 5 },
                    { 4, 1, "The characters were well-developed, but the plot felt predictable toward the end.", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user4Id", null, null, false, null, null, 4 },
                    { 5, 2, "An incredible conclusion to the series. Every twist and turn kept me on edge.", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1Id", null, null, false, null, null, 5 },
                    { 6, 2, "The Battle of Hogwarts was epic! A bittersweet yet satisfying ending.", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", null, null, false, null, null, 5 },
                    { 7, 2, "I expected more from some of the character arcs, but still a solid read.", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", null, null, false, null, null, 4 },
                    { 8, 2, "Rowling’s world-building continues to amaze, even in the final installment.", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user4Id", null, null, false, null, null, 5 },
                    { 9, 3, "A timeless masterpiece. Tolkien’s world and characters are unmatched in depth and richness.", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1Id", null, null, false, null, null, 5 },
                    { 10, 3, "The pacing was slow at times, but the payoff in the end was well worth it.", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2Id", null, null, false, null, null, 4 },
                    { 11, 3, "The bond between Sam and Frodo is the heart of this epic journey. Beautifully written.", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", null, null, false, null, null, 5 },
                    { 12, 3, "An epic tale that defines the fantasy genre. Loved every moment of it.", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user4Id", null, null, false, null, null, 5 },
                    { 13, 3, "The attention to detail in Middle-earth is staggering. Tolkien is a true genius.", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user5Id", null, null, false, null, null, 5 },
                    { 14, 3, "A bit long for my liking, but undeniably one of the greatest stories ever told.", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user6Id", null, null, false, null, null, 4 }
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
                    { 7, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3Id", true, null, null, 2 }
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
