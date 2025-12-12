namespace BookHub.Features.Identity.Data.Models;

using BookHub.Data.Models.Base;
using BookHub.Data.Models.Shared.ChatUser;
using Features.Authors.Data.Models;
using Features.Book.Data.Models;
using Features.Chat.Data.Models;
using Features.ReadingList.Data.Models;
using Features.Review.Data.Models;
using Features.UserProfile.Data.Models;
using Microsoft.AspNetCore.Identity;

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

    public ICollection<VoteDbModel> Votes { get; set; } = new HashSet<VoteDbModel>();

    public ICollection<BookDbModel> Books { get; } = new HashSet<BookDbModel>();

    public ICollection<AuthorDbModel> Authors { get; } = new HashSet<AuthorDbModel>();

    public ICollection<ReviewDbModel> Reviews { get; } = new HashSet<ReviewDbModel>();

    public ICollection<ReadingList> ReadingLists { get; } = new HashSet<ReadingList>();

    public ICollection<ChatUser> ChatsUsers { get; set; } = new HashSet<ChatUser>();

    public ICollection<ChatDbModel> ChatsCreated { get; set; } = new HashSet<ChatDbModel>();

    public ICollection<ChatMessageDbModel> SentChatMessages { get; } = new HashSet<ChatMessageDbModel>();
}
