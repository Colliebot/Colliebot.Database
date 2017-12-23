﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Colliebot
{
    public class DiscordGuildUserManager : DbManager<RootDatabase>
    {
        private readonly DiscordUserManager _discordUsers;
        private readonly DiscordGuildManager _discordGuilds;

        public DiscordGuildUserManager(
            RootDatabase db,
            DiscordUserManager discordUsers,
            DiscordGuildManager discordGuilds)
            : base(db)
        {
            _discordUsers = discordUsers;
            _discordGuilds = discordGuilds;
        }

        public async Task<DbDiscordGuildUser> GetGuildUserAsync(ulong id, params DiscordGuildUserInclude[] include)
        {
            var guildUser = await _db.DiscordGuildUsers.SingleOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

            if (guildUser == null || include.Count() == 0)
                return guildUser;
            if (include.Contains(DiscordGuildUserInclude.Guild))
                guildUser.Guild = await _discordGuilds.GetGuildAsync(guildUser.GuildId).ConfigureAwait(false);
            if (include.Contains(DiscordGuildUserInclude.User))
                guildUser.User = await _discordUsers.GetUserAsync(guildUser.UserId).ConfigureAwait(false);

            return guildUser;
        }

        public async Task<IEnumerable<DbDiscordGuildUser>> GetGuildUsersAsync(Expression<Func<DbDiscordGuildUser, bool>> expression, int offset = Constants.DefaultPageOffset, int limit = Constants.DefaultPageSize)
            => await _db.DiscordGuildUsers.Where(expression).Skip(offset).Take(limit).ToArrayAsync().ConfigureAwait(false);

        public async Task<bool> GuildUserExistsAsync(ulong id)
            => await _db.DiscordGuildUsers.AnyAsync(x => x.Id == id).ConfigureAwait(false);

        public async Task<int> GetGuildUsersCountAsync(Expression<Func<DbDiscordGuildUser, bool>> expression)
            => await _db.DiscordGuildUsers.CountAsync(expression).ConfigureAwait(false);

        public async Task CreateAsync(DbDiscordGuildUser GuildUser)
        {
            await _db.DiscordGuildUsers.AddAsync(GuildUser);
            await _db.SaveChangesAsync();
        }
    }
}
