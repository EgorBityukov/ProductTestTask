using Microsoft.Data.SqlClient;
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
                .HasIndex(pv => pv.Name)
                .HasDatabaseName("IX_Product_Name");

            modelBuilder.Entity<ProductVersion>()
                .HasIndex(pv => pv.Name)
                .HasDatabaseName("IX_ProductVersion_Name");

            modelBuilder.Entity<ProductVersion>()
                .HasIndex(pv => pv.CreatingDate)
                .HasDatabaseName("IX_ProductVersion_CreatingDate");

            modelBuilder.Entity<ProductVersion>()
                .HasIndex(pv => pv.Width)
                .HasDatabaseName("IX_ProductVersion_Width");

            modelBuilder.Entity<ProductVersion>()
                .HasIndex(pv => pv.Height)
                .HasDatabaseName("IX_ProductVersion_Height");

            modelBuilder.Entity<ProductVersion>()
                .HasIndex(pv => pv.Length)
                .HasDatabaseName("IX_ProductVersion_Length");

            modelBuilder.Entity<Product>()
                .ToTable(tb => tb.HasTrigger("TR_Product_Insert_Update_Delete"));

            modelBuilder.Entity<ProductVersion>()
                .ToTable(tb => tb.HasTrigger("TR_ProductVersion_Insert_Update_Delete"));

            modelBuilder.HasDbFunction(() => GetProductVersions(default, default, default, default)).HasName("GetProductVersions");
        }

        public IQueryable<ProductVersionsDAO> GetProductVersions(string? productName, string? productVersionName, double? minVolume, double? maxVolume) =>
            FromExpression(() => GetProductVersions(productName, productVersionName, minVolume, maxVolume));
    }
}
