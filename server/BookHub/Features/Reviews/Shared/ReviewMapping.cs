namespace BookHub.Features.Reviews.Shared;

using Data.Models;
using Web.Models;
using Service.Models;

public static class ReviewMapping
{
    public static CreateReviewServiceModel ToServiceModel(
        this CreateReviewWebModel webModel)
        => new()
        {
            Content = webModel.Content,
            Rating = webModel.Rating,
            BookId = webModel.BookId
        };

    public static ReviewDbModel ToDbModel(
        this CreateReviewServiceModel serviceModel)
        => new()
        {
            Content = serviceModel.Content,
            Rating = serviceModel.Rating,
            BookId = serviceModel.BookId
        };

    public static void UpdateDbModel(
        this CreateReviewServiceModel serviceModel,
        ReviewDbModel dbModel)
    {
        dbModel.Rating = serviceModel.Rating;
        dbModel.Content = serviceModel.Content;
    }

    public static IQueryable<ReviewServiceModel> ToServiceModels(
        this IQueryable<ReviewDbModel> dbModels)
        => dbModels.Select(r => new ReviewServiceModel
        {
            Id = r.Id,
            Content = r.Content,
            Rating = r.Rating,
            BookId = r.BookId,
            Upvotes = r.Votes.Count(v => v.IsUpvote),
            Downvotes = r.Votes.Count(v => !v.IsUpvote),
            ModifiedOn = r.ModifiedOn == null ? null : r.ModifiedOn.ToString(),
            CreatedOn = r.CreatedOn.ToString(),
            CreatorId = r.CreatorId,
            CreatedBy = r.CreatedBy
        });

    public static ReviewServiceModel ToServiceModel(
        this ReviewDbModel dbModel)
        => new()
        {
            Id = dbModel.Id,
            Content = dbModel.Content,
            Rating = dbModel.Rating,
            BookId = dbModel.BookId,
            Upvotes = dbModel.Votes.Count(v => v.IsUpvote),
            Downvotes = dbModel.Votes.Count(v => !v.IsUpvote),
            CreatedOn = dbModel.CreatedOn.ToString("O"),
            ModifiedOn = dbModel.ModifiedOn?.ToString("O"),
            CreatorId = dbModel.CreatorId,
            CreatedBy = dbModel.CreatedBy
        };
}
