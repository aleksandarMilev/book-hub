namespace BookHub.Infrastructure.Services.Result
{
    public class Result
    {
        public Result(bool succeeded)
            => Succeeded = succeeded;

        public Result(string errorMessage)
        {
            Succeeded = false;
            ErrorMessage = errorMessage;
        }

        public bool Succeeded { get; init; }

        public string? ErrorMessage { get; init; }


        public static implicit operator Result(bool succeeded)
            => new(succeeded);

        public static implicit operator Result(string errorMessage)
            => new(errorMessage);
    }
}
