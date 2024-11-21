namespace BookHub.Server.Infrastructure.Extensions
{
    using System.Text;

    using Data;
    using Data.Models;
    using Features.Authors.Service;
    using Features.Books.Service;
    using Features.Genre.Service;
    using Features.Identity.Service;
    using Features.Nationality.Service;
    using Features.Review.Service;
    using Features.Review.Votes.Service;
    using Features.Search.Service;
    using Filters;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using Services;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
            => services
                .AddDbContext<BookHubDbContext>(options =>
                {
                    options.UseSqlServer(configuration.GetDefaultConnectionString());
                });

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<User, IdentityRole>(opt =>
                {
                    opt.Password.RequireUppercase = false;
                    opt.Password.RequireLowercase = false;
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequireDigit = false;
                })
                .AddEntityFrameworkStores<BookHubDbContext>();

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, AppSettings appSettings)
        {
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services
                .AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt =>
                {
                    opt.RequireHttpsMetadata = false;
                    opt.SaveToken = true;
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            var apiInfo = new OpenApiInfo()
            {
                Title = "My BookHub API",
                Version = "v1"
            };

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", apiInfo));
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
            => services
                .AddScoped<ICurrentUserService, CurrentUserService>()
                .AddTransient<IIdentityService, IdentityService>()
                .AddTransient<IBookService, BookService>()
                .AddTransient<IAuthorService, AuthorService>()
                .AddTransient<IGenreService, GenreService>()
                .AddTransient<INationalityService, NationalityService>()
                .AddTransient<ISearchService, SearchService>()
                .AddTransient<IReviewService, ReviewService>()
                .AddTransient<IVoteService, VoteService>();

        public static IServiceCollection AddApiControllers(this IServiceCollection services)
        {
            services
                .AddControllers(opt =>
                {
                    opt.Filters.Add<ModelOrNotFoundActionFilter>();
                });

            return services;
        }

        public static AppSettings GetAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsConfiguration = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsConfiguration);

            return appSettingsConfiguration
                    .Get<AppSettings>()
                    ?? throw new InvalidOperationException("AppSettings not found.");
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
            => services.AddAutoMapper(typeof(Program).Assembly);
    }
}
