namespace BookHub
{
    public class ApplicationSettings
    {
        public ApplicationSettings()
            => this.Secret = default!;
            
        public string Secret { get; init; } = null!;
    }
}
