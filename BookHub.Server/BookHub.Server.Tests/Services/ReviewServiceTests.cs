namespace BookHub.Server.Tests.Services
{
    using AutoMapper;
    using Data;
    using Features.Authors.Data.Models;
    using Features.Book.Data.Models;
    using Features.Review.Data.Models;
    using Features.Review.Mapper;
    using Features.Review.Service;
    using Features.Review.Service.Models;
    using Features.UserProfile.Service;
    using FluentAssertions;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class ReviewServiceTests
    {
        private readonly BookHubDbContext data;

        private readonly Mock<ICurrentUserService> mockUserService;
        private readonly Mock<IProfileService> mockProfileService;
        private readonly IMapper mapper;

        private readonly ReviewService reviewService;

        public ReviewServiceTests()
        {
            var options = new DbContextOptionsBuilder<BookHubDbContext>()
                .UseInMemoryDatabase(databaseName: "ReviewServiceInMemoryDatabase")
                .Options;

            this.mockUserService = new Mock<ICurrentUserService>();

            this.mockUserService
                .Setup(x => x.GetUsername())
                .Returns("current-user-username");

            this.mockUserService
                .Setup(x => x.GetId())
                .Returns("current-user-id");

            this.mockUserService
                .Setup(x => x.IsAdmin())
                .Returns(false);

            this.data = new BookHubDbContext(options, this.mockUserService.Object);

            this.mockProfileService = new Mock<IProfileService>();
            this.mapper = new MapperConfiguration(cfg => cfg.AddProfile(new ReviewMapper())).CreateMapper();

            this.reviewService = new ReviewService(
                this.data,
                this.mockUserService.Object,
                this.mockProfileService.Object,
                this.mapper
            );

            Task
                .Run(this.PrepareDbAsync)
                .GetAwaiter()
                .GetResult();
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_IfUserAlreadyReviewedTheBook()
        {
            var bookId = 1;
            var userId = "current-user-id";
            var username = "current-user-username";

            var existingReview = new Review()
            {
                BookId = bookId,
                CreatorId = userId,
                Content = "Existing review content.",
                Rating = 4,
            };

            this.data.Add(existingReview);
            await this.data.SaveChangesAsync();

            var newReviewModel = new CreateReviewServiceModel()
            {
                BookId = bookId,
                Content = "New review content.",
                Rating = 5,
            };

            var createAsync = async () =>
            {
                await this.reviewService.CreateAsync(newReviewModel);
            };

            await createAsync
                .Should()
                .ThrowAsync<ReviewDuplicatedException>()
                .WithMessage($"{username} has already reviewed book with Id: {bookId}!");
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateReview_WhenConditionsAreMet()
        {
            foreach (var r in this.data.Reviews)
            {
                if (r.CreatorId == "current-user-id")
                {
                    this.data.Remove(r);
                }
            }

            await this.data.SaveChangesAsync();

            var bookId = 1;
            var userId = "current-user-id";

            var newReviewModel = new CreateReviewServiceModel()
            {
                BookId = bookId,
                Content = "Great book!",
                Rating = 5,
            };

            var id = await this.reviewService.CreateAsync(newReviewModel);

            var review = await this.data.Reviews.FindAsync(id);
            review.Should().NotBeNull();
            review!.Content.Should().Be("Great book!");
            review.Rating.Should().Be(5);
            review.CreatorId.Should().Be(userId);
            review.BookId.Should().Be(bookId);
        }

        [Fact]
        public async Task CreateAsync_ShouldUpdateBookAndAuthorRating_WhenNewReviewIsAdded()
        {
            var bookId = 1;
            var authorId = 1;

            var prevBookRatingsCount = await this.data
                .Books
                .Where(b => b.Id == bookId)
                .Select(b => b.RatingsCount)
                .FirstOrDefaultAsync();

            var prevAuthorRatingsCount = await this.data
                .Authors
                .Where(a => a.Id == authorId)
                .Select(a => a.RatingsCount)
                .FirstOrDefaultAsync();

            var reviewModel = new CreateReviewServiceModel()
            {
                BookId = bookId,
                Content = "Fantastic read!",
                Rating = 5,
            };

            await this.reviewService.CreateAsync(reviewModel);

            var book = await this.data.Books.FindAsync(bookId);
            book.Should().NotBeNull();
            book!.RatingsCount.Should().Be(prevBookRatingsCount + 1);

            var author = await this.data.Authors.FindAsync(authorId);
            author.Should().NotBeNull();
            author!.RatingsCount.Should().Be(prevAuthorRatingsCount +  1);
        }

        [Fact]
        public async Task EditAsync_ShouldReturnErrorMessage_WhenIdIsInvalid()
        {
            foreach (var r in this.data.Reviews)
            {
                if (r.CreatorId == "current-user-id")
                {
                    this.data.Remove(r);
                }
            }

            await this.data.SaveChangesAsync();

            var bookId = 1;

            var reviewModel = new CreateReviewServiceModel()
            {
                BookId = bookId,
                Content = "Fantastic read!",
                Rating = 5,
            };

            _ = await this.reviewService.CreateAsync(reviewModel);

            var updatedRating = 4;

            var updatedReviewModel = new CreateReviewServiceModel()
            {
                BookId = bookId,
                Content = "Edited",
                Rating = updatedRating,
            };

            var invalidId = 412_263_892;
            var result = await this.reviewService.EditAsync(invalidId, updatedReviewModel);
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be($"Review with Id: {invalidId} was not found!");
        }

        [Fact]
        public async Task EditAsync_ShouldReturnErrorMessage_WhenIdUserIsNotReviewCreator_AndAlso_IsNotAdmin()
        {
            foreach (var r in this.data.Reviews)
            {
                if (r.CreatorId == "current-user-id")
                {
                    this.data.Remove(r);
                }
            }

            await this.data.SaveChangesAsync();

            var bookId = 1;

            var reviewModel = new CreateReviewServiceModel()
            {
                BookId = bookId,
                Content = "Fantastic read!",
                Rating = 5,
            };

            var reviewId = await this.reviewService.CreateAsync(reviewModel);
            var review = await this.data.Reviews.FindAsync(reviewId);
            review!.CreatorId = "fake-creator-id";
            await this.data.SaveChangesAsync();

            var updatedRating = 4;

            var updatedReviewModel = new CreateReviewServiceModel()
            {
                BookId = bookId,
                Content = "Edited",
                Rating = updatedRating,
            };

            var result = await this.reviewService.EditAsync(reviewId, updatedReviewModel);
            result.Succeeded.Should().BeFalse();
            result
                .ErrorMessage
                .Should()
                .Be($"current-user-username can not modify Review with Id: {reviewId}!");
        }

        [Fact]
        public async Task EditAsync_ShouldUpdateReview_WhenConditionsAreMet_AndCurrentUserIsTheCreator()
        {
            foreach (var r in this.data.Reviews)
            {
                if (r.CreatorId == "current-user-id")
                {
                    this.data.Remove(r);
                }
            }

            await this.data.SaveChangesAsync();

            var bookId = 1;

            var reviewModel = new CreateReviewServiceModel()
            {
                BookId = bookId,
                Content = "Fantastic read!",
                Rating = 5,
            };

            var reviewId = await this.reviewService.CreateAsync(reviewModel);

            var updatedRating = 4;

            var updatedReviewModel = new CreateReviewServiceModel()
            {
                BookId = bookId,
                Content = "Edited",
                Rating = updatedRating,
            };

            var result = await this.reviewService.EditAsync(reviewId, updatedReviewModel);
            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();

            var review = await this.data.Reviews.FindAsync(reviewId);
            review.Should().NotBeNull();
            review!.Content.Should().Be("Edited");
            review.Rating.Should().Be(updatedRating);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnErrorMessage_WhenIdIsInvalid()
        {
            foreach (var r in this.data.Reviews)
            {
                if (r.CreatorId == "current-user-id")
                {
                    this.data.Remove(r);
                }
            }

            await this.data.SaveChangesAsync();

            var bookId = 1;

            var reviewModel = new CreateReviewServiceModel()
            {
                BookId = bookId,
                Content = "Fantastic read!",
                Rating = 5,
            };

            _ = await this.reviewService.CreateAsync(reviewModel);

            var invalidId = 412_263_892;
            var result = await this.reviewService.DeleteAsync(invalidId);
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be($"Review with Id: {invalidId} was not found!");
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnErrorMessage_WhenIdUserIsNotReviewCreator_AndAlso_IsNotAdmin()
        {
            foreach (var r in this.data.Reviews)
            {
                if (r.CreatorId == "current-user-id")
                {
                    this.data.Remove(r);
                }
            }

            await this.data.SaveChangesAsync();

            var bookId = 1;

            var reviewModel = new CreateReviewServiceModel()
            {
                BookId = bookId,
                Content = "Fantastic read!",
                Rating = 5,
            };

            var reviewId = await this.reviewService.CreateAsync(reviewModel);
            var review = await this.data.Reviews.FindAsync(reviewId);
            review!.CreatorId = "fake-creator-id";
            await this.data.SaveChangesAsync();

            var result = await this.reviewService.DeleteAsync(reviewId);
            result.Succeeded.Should().BeFalse();
            result
                .ErrorMessage
                .Should()
                .Be($"current-user-username can not modify Review with Id: {reviewId}!");
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteReview_WhenConditionsAreMet_AndCurrentUserIsTheCreator()
        {
            foreach (var r in this.data.Reviews)
            {
                if (r.CreatorId == "current-user-id")
                {
                    this.data.Remove(r);
                }
            }

            await this.data.SaveChangesAsync();

            var bookId = 1;

            var reviewModel = new CreateReviewServiceModel()
            {
                BookId = bookId,
                Content = "Fantastic read!",
                Rating = 5,
            };

            var reviewId = await this.reviewService.CreateAsync(reviewModel);

            var result = await this.reviewService.DeleteAsync(reviewId);
            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();

            var review = await this.data
                .Reviews
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(r => r.Id == reviewId);

            review!.IsDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnErrorMessage_WhenIdUserIsNotReviewCreator_ButIsAdmin()
        {
            this.SetIsAdmin(true);

            foreach (var r in this.data.Reviews)
            {
                if (r.CreatorId == "current-user-id")
                {
                    this.data.Remove(r);
                }
            }

            await this.data.SaveChangesAsync();

            var bookId = 1;

            var reviewModel = new CreateReviewServiceModel()
            {
                BookId = bookId,
                Content = "Fantastic read!",
                Rating = 5,
            };

            var reviewId = await this.reviewService.CreateAsync(reviewModel);
            var review = await this.data.Reviews.FindAsync(reviewId);
            review!.CreatorId = "fake-creator-id";
            await this.data.SaveChangesAsync();

            var result = await this.reviewService.DeleteAsync(reviewId);
            result.Succeeded.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();

            var reviewDeleted = await this.data
                .Reviews
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(r => r.Id == reviewId);

            reviewDeleted!.IsDeleted.Should().BeTrue();

            this.SetIsAdmin(false);
        }

        private void SetIsAdmin(bool isAdmin)
            => this.mockUserService
                .Setup(x => x.IsAdmin())
                .Returns(isAdmin);

        private async Task PrepareDbAsync()
        {
            var reviews = new Review[]
            {
                new()
                {
                    Id = 1,
                    Content = "A truly chilling tale. King masterfully explores the dark side of human grief and love.",
                    Rating = 5,
                    CreatorId = "user1Id",
                    CreatedBy = "user1name",
                    BookId = 1
                },
                new()
                {
                    Id = 2,
                    Content = "The book was gripping but felt a bit too disturbing at times for my taste.",
                    Rating = 3,
                    CreatorId = "user2Id",
                    CreatedBy = "user2name",
                    BookId = 1
                },
                new()
                {
                    Id = 3,
                    Content = "An unforgettable story that haunts you long after you've finished it. Highly recommended!",
                    Rating = 5,
                    CreatorId = "user3Id",
                    CreatedBy = "user3name",
                    BookId = 1
                },
                new()
                {
                    Id = 4,
                    Content = "The characters were well-developed, but the plot felt predictable toward the end.",
                    Rating = 4,
                    CreatorId = "user4Id",
                    CreatedBy = "user4name",
                    BookId = 1
                },
                new()
                {
                    Id = 5,
                    Content = "An incredible conclusion to the series. Every twist and turn kept me on edge.",
                    Rating = 5,
                    CreatorId = "user1Id",
                    CreatedBy = "user1name",
                    BookId = 2
                },
                new()
                {
                    Id = 6,
                    Content = "The Battle of Hogwarts was epic! A bittersweet yet satisfying ending.",
                    Rating = 5,
                    CreatorId = "user2Id",
                    CreatedBy = "user2name",
                    BookId = 2
                },
                new()
                {
                    Id = 7,
                    Content = "I expected more from some of the character arcs, but still a solid read.",
                    Rating = 4,
                    CreatorId = "user3Id",
                    CreatedBy = "user3name",
                    BookId = 2
                },
                new()
                {
                    Id = 8,
                    Content = "Rowling’s world-building continues to amaze, even in the final installment.",
                    Rating = 5,
                    CreatorId = "user4Id",
                    CreatedBy = "user4name",
                    BookId = 2
                },
                new()
                {
                    Id = 9,
                    Content = "A timeless masterpiece. Tolkien’s world and characters are unmatched in depth and richness.",
                    Rating = 5,
                    CreatorId = "user1Id",
                    CreatedBy = "user1name",
                    BookId = 3
                },
                new()
                {
                    Id = 10,
                    Content = "The pacing was slow at times, but the payoff in the end was well worth it.",
                    Rating = 4,
                    CreatorId = "user2Id",
                    CreatedBy = "user2name",
                    BookId = 3
                }
            };

            var books = new Book[]
            {
                new()
                {
                    Id = 1,
                    Title = "Pet Sematary",
                    ShortDescription = "Sometimes dead is better.",
                    LongDescription =
                        "Pet Sematary is a 1983 horror novel by American writer Stephen King. " +
                        "The story revolves around the Creed family who move into a rural home near a pet cemetery that has " +
                        "the power to resurrect the dead. However, the resurrected creatures return with sinister changes. " +
                        "The novel explores themes of grief, mortality, and the consequences of tampering with nature. " +
                        "It was nominated for a World Fantasy Award for Best Novel in 1984. " +
                        "The book was adapted into two films: one in 1989 directed by Mary Lambert and another in 2019 directed by Kevin Kölsch and " +
                        "Dennis Widmyer. In November 2013, PS Publishing released Pet Sematary in a limited 30th-anniversary edition, " +
                        "further solidifying its status as a classic in horror literature.",
                    AverageRating = 4.25,
                    RatingsCount = 4,
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg/330px-Pet_Sematary_%281983%29_front_cover%2C_first_edition.jpg",
                    AuthorId = 1,
                    PublishedDate = new DateTime(1983, 11, 4),
                    IsApproved = true
                },
                new()
                {
                    Id = 2,
                    Title = "Harry Potter and the Deathly Hallows",
                    ShortDescription = "The last book from the Harry Potter series.",
                    LongDescription =
                        "Harry Potter and the Deathly Hallows is a fantasy novel written by British author J.K. Rowling. " +
                        "It is the seventh and final book in the Harry Potter series and concludes the epic tale of Harry's battle against Lord Voldemort. " +
                        "The story begins with Harry, Hermione, and Ron embarking on a dangerous quest to locate and destroy Voldemort's Horcruxes, " +
                        "which are key to his immortality. Along the way, they uncover secrets about the Deathly Hallows—three powerful magical objects " +
                        "that could aid them in their fight. The book builds to an intense and emotional climax at the Battle of Hogwarts, " +
                        "where Harry confronts Voldemort for the last time. Released on 21 July 2007, the book became a cultural phenomenon, " +
                        "breaking sales records and receiving critical acclaim for its complex characters, intricate plotting, and resonant themes " +
                        "of sacrifice, friendship, and love.",
                    AverageRating = 4.75,
                    RatingsCount = 4,
                    ImageUrl = "https://librerialaberintopr.com/cdn/shop/products/hallows_459x.jpg?v=1616596206",
                    AuthorId = 2,
                    PublishedDate = new DateTime(2007, 07, 21),
                    IsApproved = true
                },
                new()
                {
                    Id = 3,
                    Title = "Lord of the Rings",
                    ShortDescription = "The Lord of the Rings is an epic fantasy novel by the English author J. R. R. Tolkien.",
                    LongDescription =
                        "The Lord of the Rings is a high-fantasy novel written by J.R.R. Tolkien and set in the fictional world of Middle-earth. " +
                        "The story follows the journey of Frodo Baggins, a humble hobbit who inherits the One Ring—a powerful artifact created by the Dark Lord Sauron " +
                        "to control Middle-earth. Along with a fellowship of companions, Frodo sets out on a perilous mission to destroy the ring in the fires of Mount Doom, " +
                        "the only place where it can be unmade. The narrative interweaves themes of friendship, courage, sacrifice, and the corrupting influence of power. " +
                        "Written in stages between 1937 and 1949, the novel is widely regarded as one of the greatest works of fantasy literature, " +
                        "influencing countless authors and spawning adaptations, including Peter Jackson's acclaimed film trilogy. " +
                        "With over 150 million copies sold, it remains one of the best-selling books of all time, praised for its richly detailed world-building, " +
                        "complex characters, and timeless appeal.",
                    AverageRating = 4.67,
                    RatingsCount = 6,
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/en/e/e9/First_Single_Volume_Edition_of_The_Lord_of_the_Rings.gif",
                    AuthorId = 3,
                    PublishedDate = new DateTime(1954, 07, 29),
                    IsApproved = true
                }
            };

            var authors = new Author[]
            {
                new()
                {
                    Id = 1,
                    Name = "Stephen King",
                    ImageUrl = "https://media.npr.org/assets/img/2020/04/07/stephen-king.by-shane-leonard_wide-c132de683d677c7a6a1e692f86a7e6670baf3338.jpg?s=1100&c=85&f=jpeg",
                    Biography =
                        "Stephen Edwin King (born September 21, 1947) is an American author of horror, supernatural fiction, suspense, crime, " +
                        "and fantasy novels. He is often referred to as the 'King of Horror,' but his work spans many genres, earning him a reputation as"
                    ,
                    NationalityId = 182,
                    Gender = Gender.Male,
                    BornAt = new DateTime(1947, 09, 21),
                    IsApproved = true,
                },
                new()
                {
                    Id = 2,
                    Name = "Joanne Rowling",
                    ImageUrl = "https://stories.jkrowling.com/wp-content/uploads/2021/09/Shot-B-105_V2_CROP-e1630873059779.jpg",
                    Biography =
                        "Joanne Rowling (born July 31, 1965), known by her pen name J.K. Rowling, is a British author and philanthropist. " +
                        "She is best known for writing the Harry Potter series, a seven-book fantasy saga that has become a global phenomenon."
                    ,
                    PenName = "J. K. Rowling",
                    AverageRating = 4.75,
                    NationalityId = 181,
                    Gender = Gender.Female,
                    BornAt = new DateTime(1965, 07, 31),
                    IsApproved = true
                },
                new()
                {
                    Id = 3,
                    Name = "John Ronald Reuel Tolkien ",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d4/J._R._R._Tolkien%2C_ca._1925.jpg/330px-J._R._R._Tolkien%2C_ca._1925.jpg",
                    Biography =
                        "John Ronald Reuel Tolkien (January 3, 1892 – September 2, 1973) was an English writer, philologist, and academic. He is best " +
                        "lasting legacy as the 'father of modern fantasy.' He passed away in 1973, but his works continue to captivate readers worldwide."
                    ,
                    PenName = "J.R.R Tolkien",
                    AverageRating = 4.67,
                    NationalityId = 181,
                    Gender = Gender.Male,
                    BornAt = new DateTime(1892, 01, 03),
                    DiedAt = new DateTime(1973, 09, 02),
                    IsApproved = true
                }
            };

            if (!await this.data.Reviews.AnyAsync())
            {
                this.data.AddRange(reviews);
            }

            if (!await this.data.Books.AnyAsync())
            {
                this.data.AddRange(books);
            }

            if (!await this.data.Authors.AnyAsync())
            {
                this.data.AddRange(authors);
            }

            await this.data.SaveChangesAsync();
        }
    }
}
