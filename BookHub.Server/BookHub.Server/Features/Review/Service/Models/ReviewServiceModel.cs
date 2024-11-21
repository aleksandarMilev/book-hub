namespace BookHub.Server.Features.Review.Service.Models
{
    public class ReviewServiceModel
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public int Rating { get; set; }

        public int Likes { get; set; }

        public int Dislikes { get; set; }

        public string CreatorId { get; set; } = null!;

        public string CreatedBy { get; set; } = null!;

        public int BookId { get; set; }
    }
}
