using Microsoft.Extensions.DependencyInjection;

namespace Colliebot
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCollieDatabase(this IServiceCollection services)
        {
            services.AddDbContext<RootDatabase>()
                .AddSingleton<UserManager>()
                .AddSingleton<DiscordUserManager>()
                .AddSingleton<DiscordGuildManager>()
                .AddSingleton<DiscordGuildUserManager>();
            return services;
        }
    }
}
