using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Colliebot
{
    public class DiscordGuildManager : DbManager<RootDatabase>
    {
        public DiscordGuildManager(RootDatabase db) : base(db) { }

        public async Task<DbDiscordGuild> GetGuildAsync(ulong id, params DiscordGuildInclude[] include)
        {
            var guild = await _db.DiscordGuilds.SingleOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

            if (guild == null || include.Count() == 0)
                return guild;
            if (include.Contains(DiscordGuildInclude.Owner))
                guild.Owner = await _db.DiscordUsers.SingleOrDefaultAsync(x => x.Id == guild.OwnerId).ConfigureAwait(false);
            if (include.Contains(DiscordGuildInclude.Users))
            {
                var guildUsers = await _db.DiscordGuildUsers.Where(x => x.GuildId == guild.Id).ToArrayAsync().ConfigureAwait(false);
                guild.Users = guildUsers.Paginate().ToList();
            }
            
            return guild;
        }

        public async Task<IEnumerable<DbDiscordGuild>> GetGuildsAsync(Expression<Func<DbDiscordGuild, bool>> expression, int offset = Constants.DefaultPageOffset, int limit = Constants.DefaultPageSize)
            => await _db.DiscordGuilds.Where(expression).Skip(offset).Take(limit).ToArrayAsync().ConfigureAwait(false);

        public async Task<bool> GuildExistsAsync(ulong id)
            => await _db.DiscordGuilds.AnyAsync(x => x.Id == id).ConfigureAwait(false);

        public async Task<int> GetGuildsCountAsync(Expression<Func<DbDiscordGuild, bool>> expression)
            => await _db.DiscordGuilds.CountAsync(expression).ConfigureAwait(false);

        public async Task CreateAsync(DbDiscordGuild Guild)
        {
            await _db.DiscordGuilds.AddAsync(Guild);
            await _db.SaveChangesAsync();
        }
    }
}
