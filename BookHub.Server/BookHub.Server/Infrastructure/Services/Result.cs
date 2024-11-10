namespace BookHub.Server.Infrastructure.Services
{
    public class Result
    {
        public Result(bool succeeded)
            => this.Succeeded = succeeded;

        public Result(string errorMessage)
        {
            this.Succeeded = false;
            this.ErrorMessage = errorMessage;
        }

        public bool Succeeded { get; init; }

        public string? ErrorMessage { get; init; }


        public static implicit operator Result(bool succeeded)
            => new(succeeded);

        public static implicit operator Result(string errorMessage)
            => new(errorMessage);
    }
}
