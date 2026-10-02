namespace BookHub.Features.Identity.Data.Models;

using Authors.Data.Models;
using BookHub.Data.Models.Base;
using Books.Data.Models;
using Challenges.Data.Models;
using Microsoft.AspNetCore.Identity;
using ReadingLists.Data.Models;
using Reviews.Data.Models;
using UserProfile.Data.Models;

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

    public ICollection<VoteDbModel> Votes { get; init; }
        = new HashSet<VoteDbModel>();

    public ICollection<BookDbModel> Books { get; init; }
        = new HashSet<BookDbModel>();

    public ICollection<AuthorDbModel> Authors { get; init; }
        = new HashSet<AuthorDbModel>();

    public ICollection<ReviewDbModel> Reviews { get; init; }
        = new HashSet<ReviewDbModel>();

    public ICollection<ReadingListDbModel> ReadingLists { get; init; }
        = new HashSet<ReadingListDbModel>();

    public ICollection<ReadingChallengeDbModel> ReadingChallenges { get; init; }
        = new HashSet<ReadingChallengeDbModel>();

    public ICollection<ReadingCheckInDbModel> ReadingCheckIns { get; init; }
        = new HashSet<ReadingCheckInDbModel>();
}
