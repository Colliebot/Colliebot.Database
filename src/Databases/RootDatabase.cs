using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Colliebot
{
    public class RootDatabase : DbContext
    {
        private readonly IConfigurationRoot _config;

        // Collie
        public DbSet<DbUser> Users { get; private set; }

        // Discord
        public DbSet<DbDiscordGuild> DiscordGuilds { get; private set; }
        public DbSet<DbDiscordGuildUser> DiscordGuildUsers { get; private set; }
        public DbSet<DbDiscordUser> DiscordUsers { get; private set; }

        public RootDatabase()  // Migrations Constructor
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("_configuration.json")
                .Build();
        }

        public RootDatabase(IConfigurationRoot config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            string connection = $"Server={_config["postgre:server"]};" +
                $"Port={_config["postgre:port"]};" +
                $"Guild ID={_config["postgre:Guild"]};" +
                $"Password={_config["postgre:password"]};" +
#if RELEASE
                $"Database=root;";
#elif DEBUG
                $"Database=test;";
#endif
            builder.UseNpgsql(connection);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            DbDiscordGuild.AddModel(builder);
            DbDiscordGuildUser.AddModel(builder);
            DbDiscordGuild.AddModel(builder);
        }
    }
}
