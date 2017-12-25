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
        public UserManager(RootDatabase db) : base(db) { }

        public async Task<DbUser> GetAsync(ulong id, params UserInclude[] include)
        {
            var user = await _db.Users.SingleOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

            if (user == null || include.Count() == 0)
                return user;
            if (include.Contains(UserInclude.DiscordUser))
                user.Discord = await _db.DiscordUsers.SingleOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

            return user;
        }

        public async Task<DateTime?> GetLastUpdatedAsync(ulong id)
        {
            var user = await _db.Users.SingleOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            return user.UpdatedAt;
        }

        public async Task<IEnumerable<DbUser>> GetUsersAsync(Expression<Func<DbUser, bool>> expression, int offset = Constants.DefaultPageOffset, int limit = Constants.DefaultPageSize)
            => await _db.Users.Where(expression).Skip(offset).Take(limit).ToArrayAsync().ConfigureAwait(false);

        public async Task<bool> ExistsAsync(ulong id)
            => await _db.Users.AnyAsync(x => x.Id == id).ConfigureAwait(false);

        public async Task<int> CountAsync(Expression<Func<DbUser, bool>> expression)
            => await _db.Users.CountAsync(expression).ConfigureAwait(false);

        public async Task CreateAsync(DbUser User)
        {
            await _db.Users.AddAsync(User);
            await _db.SaveChangesAsync();
        }
    }
}
