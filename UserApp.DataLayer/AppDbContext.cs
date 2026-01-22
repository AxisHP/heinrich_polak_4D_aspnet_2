using Microsoft.EntityFrameworkCore;
using UserApp.DataLayer.Entities;
using Common.Enums;

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
        public DbSet<FavouriteEntity> Favourites { get; set; }
        public DbSet<CartItemEntity> CartItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\UserApp.DataLayer"));
                var dbPath = Path.Combine(basePath, "app_database.db");

                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.PublicId);

            modelBuilder.Entity<ItemEntity>()
                .HasIndex(i => i.PublicId);

            modelBuilder.Entity<ItemEntity>()
                .HasOne(i => i.Category)
                .WithMany(c => c.Items)
                .HasForeignKey(i => i.CategoryId)
                .HasPrincipalKey(c => c.PublicId);

            modelBuilder.Entity<OrderEntity>()
                .HasIndex(o => o.PublicId);

            modelBuilder.Entity<OrderEntity>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserPublicId)
                .HasPrincipalKey(u => u.PublicId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderPublicId)
                .HasPrincipalKey(o => o.PublicId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Item)
                .WithMany()
                .HasForeignKey(oi => oi.ItemPublicId)
                .HasPrincipalKey(i => i.PublicId);

            modelBuilder.Entity<FavouriteEntity>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserPublicId)
                .HasPrincipalKey(u => u.PublicId);

            modelBuilder.Entity<FavouriteEntity>()
                .HasOne(f => f.Item)
                .WithMany()
                .HasForeignKey(f => f.ItemPublicId)
                .HasPrincipalKey(i => i.PublicId);

            modelBuilder.Entity<CartItemEntity>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserPublicId)
                .HasPrincipalKey(u => u.PublicId);

            modelBuilder.Entity<CartItemEntity>()
                .HasOne(c => c.Item)
                .WithMany()
                .HasForeignKey(c => c.ItemPublicId)
                .HasPrincipalKey(i => i.PublicId);

            modelBuilder.Entity<CategoryEntity>()
                .HasIndex(c => c.PublicId);
        }

        public void SeedData()
        {
            if (Users.Any()) return;

            var adminPublicId = Guid.NewGuid();
            var customerPublicId = Guid.NewGuid();

            var adminUser = new UserEntity
            {
                PublicId = adminPublicId,
                Name = "admin",
                LastName = "user",
                Email = "admin@test.com",
                DateOfBirth = new DateOnly(2000, 1, 1),
                PhoneNumber = "1234567890",
                Address = "Stavbarska 36",
                Role = UserRole.Admin,
                PasswordHash = "admin123"
            };

            var customerUser = new UserEntity
            {
                PublicId = customerPublicId,
                Name = "customer",
                LastName = "user",
                Email = "customer@test.com",
                DateOfBirth = new DateOnly(2000, 1, 1),
                PhoneNumber = "0987654321",
                Address = "Stavbarska 38",
                Role = UserRole.Customer,
                PasswordHash = "customer123"
            };

            Users.AddRange(adminUser, customerUser);
            SaveChanges();
        }
    }
}
