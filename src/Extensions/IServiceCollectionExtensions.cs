using Microsoft.Extensions.DependencyInjection;

namespace Colliebot
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCollieDatabase(this IServiceCollection services)
        {
            services.AddDbContext<RootDatabase>()
                .AddTransient<UserManager>()
                .AddTransient<DiscordUserManager>()
                .AddTransient<DiscordGuildManager>()
                .AddTransient<DiscordGuildUserManager>();
            return services;
        }
    }
}
