using Microsoft.EntityFrameworkCore;
using StudyHub.API.Data;
using StudyHub.API.Interfaces.Accounts;
using StudyHub.API.Services.Accounts;

namespace StudyHub.API.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, string? connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Connection string 'StudyHubAPIContext' not found.");

            return services.AddDbContext<StudyHubAPIContext>(options =>
                options.UseSqlServer(connectionString));
        }

        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IUserService, UserService>();
        }
    }
}
