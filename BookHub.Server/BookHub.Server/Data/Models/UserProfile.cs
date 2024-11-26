namespace BookHub.Server.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Base;

    using static Common.Constants.Validation.Profile;

    public class UserProfile : IEntity
    {
        [Key]
        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;

        public User User { get; set; } = null!;
        [Required]
        [MaxLength(NameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(NameMaxLength)]
        public string LastName { get; set; } = null!;

        [Required]
        [MaxLength(UrlMaxLength)]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [MaxLength(PhoneMaxLength)]
        public string PhoneNumber { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        [MaxLength(UrlMaxLength)]
        public string? SocialMediaUrl { get; set; }

        [MaxLength(BiographyMaxLength)]
        public string? Biography { get; set; }

        public bool IsPrivate { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string? ModifiedBy { get; set; }
    }
}
