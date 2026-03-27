using IdentityService.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Extiensions;

public static class AppExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
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
}