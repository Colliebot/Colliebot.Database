using Microsoft.EntityFrameworkCore;

namespace Colliebot
{
    public class DbDiscordGuildUser
    {
        public ulong Id { get; set; }
        public ulong GuildId { get; set; }
        public ulong UserId { get; set; }
        public string Nickname { get; set; } = null;
        public int Hierarchy { get; set; }
        public ulong PermissionsValue { get; set; }
        public string HexColorCode { get; set; } = null;

        // Relationships
        public DbDiscordGuild Guild { get; set; }
        public DbDiscordUser User { get; set; }

        internal static void AddModel(ModelBuilder builder)
        {
            builder.Entity<DbDiscordGuildUser>()
                .HasKey(x => new { x.GuildId, x.UserId });
            builder.Entity<DbDiscordGuildUser>()
                .Property(x => x.Nickname)
                .HasMaxLength(100);
            builder.Entity<DbDiscordGuildUser>()
                .Property(x => x.HexColorCode)
                .HasMaxLength(7);

            builder.Entity<DbDiscordGuildUser>()
                .HasOne(x => x.Guild)
                .WithMany(x => x.Users);
            builder.Entity<DbDiscordGuildUser>()
                .HasOne(x => x.User)
                .WithMany(x => x.Guilds);
        }
    }
}
