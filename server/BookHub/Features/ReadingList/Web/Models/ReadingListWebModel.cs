namespace BookHub.Features.ReadingList.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ReadingListWebModel
    {
        public Guid BookId { get; init; }

        [Required]
        public string Status { get; init; } = null!;
    }
}
