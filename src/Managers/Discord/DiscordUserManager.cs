using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Colliebot
{
    public class DiscordUserManager : DbManager<RootDatabase>
    {
        private readonly UserManager _users;
        private readonly DiscordGuildManager _discordGuilds;
        private readonly DiscordGuildUserManager _discordGuildUsers;

        public DiscordUserManager(
            RootDatabase db,
            UserManager users,
            DiscordGuildManager discordGuilds,
            DiscordGuildUserManager discordGuildUsers) 
            : base(db)
        {
            _users = users;
            _discordGuilds = discordGuilds;
            _discordGuildUsers = discordGuildUsers;
        }

        public async Task<DbDiscordUser> GetUserAsync(ulong id, params DiscordUserInclude[] include)
        {
            var user = await _db.DiscordUsers.SingleOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

            if (user == null || include.Count() == 0)
                return user;
            if (include.Contains(DiscordUserInclude.User))
                user.User = await _users.GetUserAsync(user.Id).ConfigureAwait(false);
            if (include.Contains(DiscordUserInclude.Guilds))
            {
                var guilds = await _discordGuildUsers.GetGuildUsersAsync(x => x.UserId == user.Id).ConfigureAwait(false);
                user.Guilds = guilds.ToList();
            }
            if (include.Contains(DiscordUserInclude.OwnedGuilds))
            {
                var ownedGuilds = await _discordGuilds.GetGuildsAsync(x => x.OwnerId == user.Id).ConfigureAwait(false);
                user.OwnedGuilds = ownedGuilds.ToList();
            }
            
            return user;
        }

        public async Task<IEnumerable<DbDiscordUser>> GetUsersAsync(Expression<Func<DbDiscordUser, bool>> expression, int offset = Constants.DefaultPageOffset, int limit = Constants.DefaultPageSize)
            => await _db.DiscordUsers.Where(expression).Skip(offset).Take(limit).ToArrayAsync().ConfigureAwait(false);

        public async Task<bool> UserExistsAsync(ulong id)
            => await _db.DiscordUsers.AnyAsync(x => x.Id == id).ConfigureAwait(false);

        public async Task<int> GetUsersCountAsync(Expression<Func<DbDiscordUser, bool>> expression)
            => await _db.DiscordUsers.CountAsync(expression).ConfigureAwait(false);

        public async Task CreateAsync(DbDiscordUser User)
        {
            await _db.DiscordUsers.AddAsync(User);
            await _db.SaveChangesAsync();
        }
    }
}
