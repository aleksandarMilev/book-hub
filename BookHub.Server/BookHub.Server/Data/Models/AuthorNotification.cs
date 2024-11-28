namespace BookHub.Server.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Base;

    public class AuthorNotification : Notification
    {
        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }

        public Author Author { get; set; } = null!;
    }
}
