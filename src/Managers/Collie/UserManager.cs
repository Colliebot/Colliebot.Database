using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Colliebot
{
    public class UserManager : DbManager<RootDatabase>
    {
        private readonly DiscordUserManager _discordUsers;

        public UserManager(RootDatabase db, DiscordUserManager discordUsers) 
            : base(db)
        {
            _discordUsers = discordUsers;
        }

        public async Task<DbUser> GetUserAsync(ulong id, params UserInclude[] include)
        {
            var User = await _db.Users.SingleOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

            if (User == null || include.Count() == 0)
                return User;
            if (include.Contains(UserInclude.DiscordUser))
                User.Discord = await _discordUsers.GetUserAsync(User.Id).ConfigureAwait(false);

            return User;
        }

        public async Task<IEnumerable<DbUser>> GetUsersAsync(Expression<Func<DbUser, bool>> expression, int offset = Constants.DefaultPageOffset, int limit = Constants.DefaultPageSize)
            => await _db.Users.Where(expression).Skip(offset).Take(limit).ToArrayAsync().ConfigureAwait(false);

        public async Task<bool> UserExistsAsync(ulong id)
            => await _db.Users.AnyAsync(x => x.Id == id).ConfigureAwait(false);

        public async Task<int> GetUsersCountAsync(Expression<Func<DbUser, bool>> expression)
            => await _db.Users.CountAsync(expression).ConfigureAwait(false);

        public async Task CreateAsync(DbUser User)
        {
            await _db.Users.AddAsync(User);
            await _db.SaveChangesAsync();
        }
    }
}
