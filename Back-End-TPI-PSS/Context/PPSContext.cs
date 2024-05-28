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
        public DbSet<Category> Categories { get; set; }
        public DbSet<StockSize> StockSizes { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Image> Images { get; set; }

        public PPSContext(DbContextOptions<PPSContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Stocks)
                .WithOne()
                .HasForeignKey(s => s.ProductId);


            modelBuilder.Entity<Stock>()
                .HasMany(s => s.StockSizes)
                .WithOne()
                .HasForeignKey(ss => ss.StockId);

            modelBuilder.Entity<Stock>()
                .HasMany(s => s.Images)
                .WithOne()
                .HasForeignKey(i => i.StockId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Categories)
                .WithMany()
                .UsingEntity(j => j.ToTable("CategoriesProducts"));

            modelBuilder.Entity<StockSize>()
                .HasOne(ss => ss.Size)
                .WithMany()
                .HasForeignKey(ss => ss.SizeId);

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
