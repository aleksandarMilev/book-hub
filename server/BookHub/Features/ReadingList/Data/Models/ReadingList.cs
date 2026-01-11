namespace BookHub.Features.ReadingList.Data.Models;

using System.ComponentModel.DataAnnotations.Schema;
using Books.Data.Models;
using BookHub.Data.Models.Base;
using Identity.Data.Models;
using Microsoft.EntityFrameworkCore;

[PrimaryKey(
    nameof(UserId),
    nameof(BookId),
    nameof(Status))]
public class ReadingList : IDeletableEntity
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