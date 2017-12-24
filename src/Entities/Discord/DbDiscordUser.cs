using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Colliebot
{
    public class DbDiscordUser
    {
        public ulong Id { get; set; }
        public ulong UserId { get; set; }
        public string Name { get; set; }
        public ushort Discriminator { get; set; }
        public string IconUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime LinkedAt { get; set; }

        // Relationships
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DbUser User { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<DbDiscordGuildUser> Guilds { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<DbDiscordGuild> OwnedGuilds { get; set; }

        internal static void AddModel(ModelBuilder builder)
        {
            builder.Entity<DbDiscordUser>()
                .HasKey(x => x.Id);
            builder.Entity<DbDiscordUser>()
                .Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();
            builder.Entity<DbDiscordUser>()
                .Property(x => x.Discriminator)
                .HasMaxLength(4)
                .IsRequired();
            builder.Entity<DbDiscordUser>()
                .Property(x => x.CreatedAt)
                .IsRequired();
            builder.Entity<DbDiscordUser>()
                .Property(x => x.UpdatedAt)
                .ValueGeneratedOnUpdate();
            builder.Entity<DbDiscordUser>()
                .Property(x => x.LinkedAt)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Entity<DbDiscordUser>()
                .HasOne(x => x.User)
                .WithOne(x => x.Discord);
            builder.Entity<DbDiscordUser>()
                .HasMany(x => x.Guilds)
                .WithOne(x => x.User);
            builder.Entity<DbDiscordUser>()
                .HasMany(x => x.OwnedGuilds)
                .WithOne(x => x.Owner);
        }
    }
}
