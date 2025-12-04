using Microsoft.EntityFrameworkCore;
using UserApp.DataLayer.Entities;

namespace UserApp.DataLayer
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<BookEntity> Books { get; set; }
        public DbSet<ItemEntity> Items { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=app.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.Id);

            modelBuilder.Entity<ItemEntity>()
                .HasIndex(i => i.Id);
            
            modelBuilder.Entity<OrderEntity>()
                .HasIndex(o => o.Id);
        }
    }
}
