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

        //public RootDatabase()  // Migrations Constructor
        //{
        //    _config = new ConfigurationBuilder()
        //        .SetBasePath(AppContext.BaseDirectory)
        //        .AddJsonFile("_configuration.json", optional: true)
        //        .Build();
        //}

        public RootDatabase(IConfigurationRoot config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseInMemoryDatabase("testing");

            //string connection = $"Server={_config["postgre:server"]};" +
            //    $"Port={_config["postgre:port"]};" +
            //    $"User ID={_config["postgre:user"]};" +
            //    $"Password={_config["postgre:password"]};" +
            //    $"Database=test;";
            //builder.UseNpgsql(connection);
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            DbDiscordGuild.AddModel(builder);
            DbDiscordGuildUser.AddModel(builder);
            DbDiscordGuild.AddModel(builder);
        }
    }
}
