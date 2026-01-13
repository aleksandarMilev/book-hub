namespace BookHub.Features.ReadingLists.Data.Models;

using System.ComponentModel.DataAnnotations.Schema;
using BookHub.Data.Models.Base;
using Books.Data.Models;
using Identity.Data.Models;
using Microsoft.EntityFrameworkCore;
using Shared;

[PrimaryKey(
    nameof(UserId),
    nameof(BookId),
    nameof(Status))]
public class ReadingListDbModel : IDeletableEntity
{
    [ForeignKey(nameof(User))]
    public string UserId { get; set; } = null!;

    public UserDbModel User { get; set; } = null!;

    [ForeignKey(nameof(Book))]
    public Guid BookId { get; set; }

    public BookDbModel Book { get; set; } = null!;

    public ReadingListStatus Status { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }
    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public string? DeletedBy { get; set; }
}