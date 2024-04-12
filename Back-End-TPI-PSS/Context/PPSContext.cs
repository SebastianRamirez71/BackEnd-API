using Back_End_TPI_PSS.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Back_End_TPI_PSS.Context
{
    public class PPSContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Colour> Colours { get; set; }
        public DbSet<Size> Sizes { get; set; }

        public PPSContext(DbContextOptions<PPSContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Sizes)
                .WithMany(s => s.Products)
                .UsingEntity(j => j.ToTable("SizesProducts"));

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Colours)
                .WithMany(c => c.Products)
                .UsingEntity(j => j.ToTable("ColoursProducts"));

            modelBuilder.Entity<User>()
                .HasMany(c => c.Orders)
                .WithOne()
                .HasForeignKey(dc => dc.UserId);

            modelBuilder.Entity<OrderLine>()
                .HasOne(ol => ol.Product)
                .WithMany()
                .HasForeignKey(ol => ol.ProductId);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderLines)
                .WithOne()
                .HasForeignKey(ol => ol.OrderId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);
        }
    }
}
