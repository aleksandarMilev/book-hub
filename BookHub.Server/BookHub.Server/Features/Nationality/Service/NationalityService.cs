namespace BookHub.Server.Features.Nationality.Service
{
    using Data;
    using Microsoft.EntityFrameworkCore;

    public class NationalityService(BookHubDbContext data) : INationalityService
    {
        private readonly BookHubDbContext data = data;

        public async Task<IEnumerable<string>> GetNamesAsync()
           => await this.data
                .Nationalities
                .Select(n => n.Name)
                .ToListAsync();
    }
}
