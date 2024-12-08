namespace BookHub.Server.Features.Authors.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Features.Book.Data.Models;
    using Features.Identity.Data.Models;

    using Server.Data.Models.Base;

    using static Shared.ValidationConstants.Author;

    public class Author : DeletableEntity<int>, IApprovableEntity
    {
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(ImageUrlMaxLength)]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [MaxLength(BiographyMaxLength)]
        public string Biography { get; set; } = null!;

        [MaxLength(PenNameMaxLength)]
        public string? PenName { get; set; }

        public int RatingsCount { get; set; }

        public double AverageRating { get; set; }

        [ForeignKey(nameof(Nationality))]
        public int NationalityId { get; set; }

        public Nationality Nationality { get; set; } = null!;

        public Gender Gender { get; set; }

        public DateTime? BornAt { get; set; }

        public DateTime? DiedAt { get; set; }

        [ForeignKey(nameof(User))]
        public string? CreatorId { get; set; }

        public User? Creator { get; set; }

        public bool IsApproved { get; set; }

        public ICollection<Book> Books { get; } = new HashSet<Book>();
    }
}
