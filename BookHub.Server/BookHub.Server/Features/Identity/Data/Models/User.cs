namespace BookHub.Server.Features.Identity.Data.Models
{
    using Features.Authors.Data.Models;
    using Features.Book.Data.Models;
    using Features.Chat.Data.Models;
    using Features.ReadingList.Data.Models;
    using Features.Review.Data.Models;
    using Features.UserProfile.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Server.Data.Models.Base;
    using Server.Data.Models.Shared.ChatUser;

    public class User : IdentityUser, IDeletableEntity
    {
        public DateTime CreatedOn { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string? DeletedBy { get; set; }

        public UserProfile? Profile { get; set; }

        public ICollection<Vote> Votes { get; set; } = new HashSet<Vote>();

        public ICollection<Book> Books { get; } = new HashSet<Book>();

        public ICollection<Author> Authors { get; } = new HashSet<Author>();

        public ICollection<Review> Reviews { get; } = new HashSet<Review>();

        public ICollection<ReadingList> ReadingLists { get; } = new HashSet<ReadingList>();

        public ICollection<ChatUser> ChatsUsers { get; set; } = new HashSet<ChatUser>();

        public ICollection<Chat> ChatsCreated { get; set; } = new HashSet<Chat>();

        public ICollection<ChatMessage> SentChatMessages { get; } = new HashSet<ChatMessage>();
    }
}
