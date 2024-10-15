using Microsoft.EntityFrameworkCore;
using ProductApi.Data.Models;
using System.Reflection.Metadata;

namespace ProductApi.Data
{
    public class ProductionDbContext : DbContext
    {
        public ProductionDbContext(DbContextOptions<ProductionDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVersion> ProductVersions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductVersions)
                .WithOne(v => v.Product)
                .HasForeignKey(v => v.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .ToTable(tb => tb.HasTrigger("TR_Product_Insert_Update_Delete"));

            modelBuilder.Entity<ProductVersion>()
                .ToTable(tb => tb.HasTrigger("TR_ProductVersion_Insert_Update_Delete"));
        }
    }
}
