namespace BookHub.Server.Features.Nationality.Service
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;

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
