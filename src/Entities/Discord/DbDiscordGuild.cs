using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Colliebot
{
    public class DbDiscordGuild
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public ulong OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime LinkedAt { get; set; }

        // Relationships
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DbDiscordUser Owner { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<DbDiscordGuildUser> Users { get; set; }
        
        internal static void AddModel(ModelBuilder builder)
        {
            builder.Entity<DbDiscordGuild>()
                .HasKey(x => x.Id);
            builder.Entity<DbDiscordGuild>()
                .Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();
            builder.Entity<DbDiscordGuild>()
                .Property(x => x.OwnerId)
                .IsRequired();
            builder.Entity<DbDiscordGuild>()
                .Property(x => x.CreatedAt)
                .IsRequired();
            builder.Entity<DbDiscordGuild>()
                .Property(x => x.UpdatedAt)
                .ValueGeneratedOnUpdate();
            builder.Entity<DbDiscordGuild>()
                .Property(x => x.LinkedAt)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Entity<DbDiscordGuild>()
                .HasOne(x => x.Owner)
                .WithMany(x => x.OwnedGuilds);
            builder.Entity<DbDiscordGuild>()
                .HasMany(x => x.Users)
                .WithOne(x => x.Guild);
        }
    }
}
