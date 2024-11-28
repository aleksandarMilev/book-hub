namespace BookHub.Server.Data.Models.Base
{
    using System.ComponentModel.DataAnnotations;

    using static Common.Constants.Validation.Notification;

    public abstract class Notification : DeletableEntity<int>
    {
        [Required]
        [MaxLength(MessageMaxLength)]
        public string Message { get; set; } = null!;

        public bool IsRead { get; set; }
    }
}
