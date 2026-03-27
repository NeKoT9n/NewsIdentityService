using IdentityService.Application.Abstraction;
using IdentityService.Application.Options;
using IdentityService.Application.Services;
using IdentityService.Application.Services.JWT;
using IdentityService.Application.Services.Password;
using IdentityService.DataAccess;
using IdentityService.Mappers;
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
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
        
            return services;
        }

        public IServiceCollection RegisterOptions(IConfiguration configuration)
        {
            services.Configure<JwtOptions>(
                configuration.GetSection(nameof(JwtOptions)));

            return services;
        }

        public IServiceCollection AddMappers()
        {
            services.AddSingleton<RegistrationMapper>();
            services.AddSingleton<LoginMapper>();
            
            return services;
        }
    }
}