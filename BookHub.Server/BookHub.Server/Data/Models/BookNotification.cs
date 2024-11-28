namespace BookHub.Server.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Base;

    public class BookNotification : Notification
    {
        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }

        public Book Book { get; set; } = null!;
    }
}
