namespace BookHub.Server.Features.Authors.Service
{
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Server.Data;

    public class NationalityService(BookHubDbContext data) : INationalityService
    {
        private readonly BookHubDbContext data = data;

        public async Task<IEnumerable<NationalityServiceModel>> NamesAsync()
           => await this.data
                .Nationalities
                .Select(n => new NationalityServiceModel()
                {
                    Id = n.Id,
                    Name = n.Name
                })
                .ToListAsync();
    }
}
