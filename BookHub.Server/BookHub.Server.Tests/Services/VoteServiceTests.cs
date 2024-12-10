namespace BookHub.Server.Tests.Services
{
    using Data;
    using Features.Review.Service;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;
    using FluentAssertions;
    using Features.Review.Data.Models;
    using Features.Book.Data.Models;
    using Features.Identity.Data.Models;

    public class VoteServiceTests
    {
        private readonly BookHubDbContext data;

        private readonly Mock<ICurrentUserService> mockUserService;

        private readonly VoteService voteService;

        public VoteServiceTests()
        {
            var options = new DbContextOptionsBuilder<BookHubDbContext>()
                .UseInMemoryDatabase(databaseName: "VoteServiceInMemoryDatabase")
                .Options;

            this.mockUserService = new Mock<ICurrentUserService>();

            this.data = new BookHubDbContext(options, this.mockUserService.Object);

            this.voteService = new VoteService(this.data, mockUserService.Object);

            Task
                .Run(this.PrepareDbAsync)
                .GetAwaiter()
                .GetResult();
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnNull_WhenVoteAlreadyExists()
        {
            var reviewId = 1;
            var isUpvote = true;
            var userId = "user2Id";

            this.SetGetUserId(userId);

            var result = await this.voteService.CreateAsync(reviewId, isUpvote);
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ShouldRemoveOppositeVote_WhenOppositeVoteExists()
        {
            var reviewId = 1;
            var isUpvote = false;
            var userId = "user2Id";

            this.SetGetUserId(userId);

            var result = await this.voteService.CreateAsync(reviewId, isUpvote);

            result.Should().NotBeNull();

            var existingVote = await this.data
                .Votes
                .FirstOrDefaultAsync(v => v.ReviewId == reviewId && v.CreatorId == userId);

            existingVote!.Should().NotBeNull();
            existingVote!.IsUpvote.Should().Be(isUpvote);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateNewVote_WhenNoVoteExists()
        {
            var reviewId = 1;
            var isUpvote = false;
            var userId = "user1Id";

            this.SetGetUserId(userId);

            var result = await this.voteService.CreateAsync(reviewId, isUpvote);
            result.Should().NotBeNull(); 

            var createdVote = await this.data
                .Votes
                .FirstOrDefaultAsync(v => v.Id == result);

            createdVote!.Should().NotBeNull();
            createdVote!.ReviewId.Should().Be(reviewId);
            createdVote!.IsUpvote.Should().Be(isUpvote);
            createdVote!.CreatorId.Should().Be(userId);
        }

        [Fact]
        public async Task CreateAsync_ShouldNotCreateVote_WhenReviewIdDoesNotExist()
        {
            var invalidReviewId = 123_543_214;
            var isUpvote = false;
            var userId = "user1Id";

            this.SetGetUserId(userId);

            var result = await this.voteService.CreateAsync(invalidReviewId, isUpvote);
            result.Should().NotBeNull(); 

            var createdVote = await this.data
                .Votes
                .FirstOrDefaultAsync(v => v.Id == result);

            createdVote!.Should().NotBeNull();
            createdVote!.ReviewId.Should().Be(invalidReviewId);
        }

        private void SetGetUserId(string userId)
            => this.mockUserService
                .Setup(x => x.GetId())
                .Returns(userId);

        private async Task PrepareDbAsync()
        {
            if (!await this.data.Reviews.AnyAsync())
            {
                var users = new User[]
                {
                    new()
                    {
                        Id = "user1Id",
                        UserName = "user1name",
                        Email = "user1@mail.com"
                    },
                    new()
                    {
                        Id = "user2Id",
                        UserName = "user2name",
                        Email = "user2@mail.com"
                    },
                    new()
                    {
                        Id = "user3Id",
                        UserName = "user3name",
                        Email = "user3@mail.com"
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
                    },
                    new()
                    {
                        Id = 11,
                        Content = "The bond between Sam and Frodo is the heart of this epic journey. Beautifully written.",
                        Rating = 5,
                        CreatorId = "user3Id",
                        CreatedBy = "user3name",
                        BookId = 3
                    },
                    new()
                    {
                        Id = 12,
                        Content = "An epic tale that defines the fantasy genre. Loved every moment of it.",
                        Rating = 5,
                        CreatorId = "user4Id",
                        CreatedBy = "user4name",
                        BookId = 3
                    },
                    new()
                    {
                        Id = 13,
                        Content = "The attention to detail in Middle-earth is staggering. Tolkien is a true genius.",
                        Rating = 5,
                        CreatorId = "user5Id",
                        CreatedBy = "user5name",
                        BookId = 3
                    },
                    new()
                    {
                        Id = 14,
                        Content = "A bit long for my liking, but undeniably one of the greatest stories ever told.",
                        Rating = 4,
                        CreatorId = "user6Id",
                        CreatedBy = "user6name",
                        BookId = 3
                    }
                };

                var votes = new Vote[]
                {
                    new()
                    {
                        Id = 1,
                        IsUpvote = true,
                        ReviewId = 1,
                        CreatorId = "user2Id"
                    },
                    new()
                    {
                        Id = 2,
                        IsUpvote = true,
                        ReviewId = 1,
                        CreatorId = "user3Id"
                    },
                    new()
                    {
                        Id = 3,
                        IsUpvote = true,
                        ReviewId = 1,
                        CreatorId = "user4Id"
                    },
                    new()
                    {
                        Id = 4,
                        IsUpvote = true,
                        ReviewId = 1,
                        CreatorId = "user5Id"
                    },
                    new()
                    {
                        Id = 5,
                        IsUpvote = false,
                        ReviewId = 1,
                        CreatorId = "user6Id"
                    },
                    new()
                    {
                        Id = 6,
                        IsUpvote = true,
                        ReviewId = 2,
                        CreatorId = "user2Id"
                    },
                    new()
                    {
                        Id = 7,
                        IsUpvote = true,
                        ReviewId = 2,
                        CreatorId = "user3Id"
                    },
                };

                this.data.AddRange(users);
                this.data.AddRange(books);
                this.data.AddRange(votes);
                this.data.AddRange(reviews);
                await this.data.SaveChangesAsync();
            }
        }
    }
}
