using IdentityService.Application.Abstraction;
using IdentityService.Application.Services;
using IdentityService.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Extiensions;

public static class AppExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddDatabase(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(nameof(IdentityDbContext));

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseNpgsql(connectionString, npgsqlOptions => 
                {
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorCodesToAdd: null);
                }));

            return services;
        }

        public IServiceCollection RegisterServices()
        {
            services.AddScoped<IUserService, UserService>();
        
            return services;
        }
    }
}