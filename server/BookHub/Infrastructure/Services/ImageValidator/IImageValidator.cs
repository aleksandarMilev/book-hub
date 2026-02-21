namespace BookHub.Infrastructure.Services.ImageValidator;

using Result;
using ServiceLifetimes;

public interface IImageValidator : ITransientService
{
    Result ValidateImageFile(IFormFile image);
}
