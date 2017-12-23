using Microsoft.EntityFrameworkCore;

namespace Colliebot
{
    public abstract class DbManager<T> where T : DbContext
    {
        protected readonly T _db;

        public DbManager(T db)
        {
            _db = db;
        }
    }
}
