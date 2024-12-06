namespace BookHub.Server.Features.Chat.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class AddUserToChatWebModel
    {
        [Required]
        public string UserId { get; set; } = null!;
    }
}
