using Microsoft.EntityFrameworkCore;

namespace WebApp02.Utils
{
    public static class CRUD
    {
        public static void Update<T>(DbSet<T> db, T item) where T : class
        {
            db.Update(item);
        }
        public static void Insert<T>(DbSet<T> db, T item) where T : class
        {
            db.Add(item);
        }
        public static void Delete<T>(DbSet<T> db, T item) where T : class
        {
            db.Remove(item);
        }
    }
}
