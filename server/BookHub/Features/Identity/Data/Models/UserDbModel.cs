namespace BookHub.Features.Identity.Data.Models;

using BookHub.Data.Models.Base;
using BookHub.Data.Models.Shared.ChatUser;
using Authors.Data.Models;
using Books.Data.Models;
using Chat.Data.Models;
using ReadingLists.Data.Models;
using Review.Data.Models;
using UserProfile.Data.Models;
using Microsoft.AspNetCore.Identity;

public class UserDbModel :
    IdentityUser,
    IDeletableEntity
{
    public DateTime CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public string? DeletedBy { get; set; }

    public UserProfile? Profile { get; init; }

    public ICollection<VoteDbModel> Votes { get; init; } = new HashSet<VoteDbModel>();

    public ICollection<BookDbModel> Books { get; init; } = new HashSet<BookDbModel>();

    public ICollection<AuthorDbModel> Authors { get; init; } = new HashSet<AuthorDbModel>();

    public ICollection<ReviewDbModel> Reviews { get; init; } = new HashSet<ReviewDbModel>();

    public ICollection<ReadingListDbModel> ReadingLists { get; init; } = new HashSet<ReadingListDbModel>();

    public ICollection<ChatUser> ChatsUsers { get; init; } = new HashSet<ChatUser>();

    public ICollection<ChatDbModel> ChatsCreated { get; init; } = new HashSet<ChatDbModel>();

    public ICollection<ChatMessageDbModel> SentChatMessages { get; init; } = new HashSet<ChatMessageDbModel>();
}
