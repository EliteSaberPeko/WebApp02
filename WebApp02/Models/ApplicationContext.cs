using Microsoft.EntityFrameworkCore;

namespace WebApp02.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Autor> Autors { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<PublishingHouse> PublishingHouses { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql(_connectionString);
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Book>().HasCheckConstraint("Price", "Price > 0", c => c.HasName("CK_Book_Price"));
            //modelBuilder.Entity<Book>().HasCheckConstraint("PublicationYear", "PublicationYear > 0 AND PublicationYear <= 9999", c => c.HasName("CK_Book_PublicationYear"));
            base.OnModelCreating(modelBuilder);
        }
    }
}
