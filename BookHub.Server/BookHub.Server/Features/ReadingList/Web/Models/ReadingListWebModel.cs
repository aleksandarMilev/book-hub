namespace BookHub.Server.Features.ReadingList.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ReadingListWebModel
    {
        public int BookId { get; init; }

        [Required]
        public string Status { get; init; } = null!;
    }
}
