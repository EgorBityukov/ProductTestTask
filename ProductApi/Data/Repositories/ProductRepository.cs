﻿using Microsoft.EntityFrameworkCore;
using ProductApi.Data.Models;

namespace ProductApi.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductionDbContext _context;

        public ProductRepository(ProductionDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductVersionsDAO>> GetProductsAsync(string? name = "", string? versionName = "", double? minVolume = null, double? maxVolume = null)
        {
            var productVersions = await _context.GetProductVersions(name, versionName, minVolume, maxVolume).AsNoTracking().ToListAsync();
            return productVersions;
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _context.Products
                .Include(p => p.ProductVersions)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ID == id);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<bool> isExistAsync(string name)
        {
            return await _context.Products.AnyAsync(p => p.Name == name);
        }

        public async Task<ProductVersion> GetVersionByIdAsync(Guid id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _context.ProductVersions
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ID == id);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {

            }
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(ProductVersion product)
        {
            await _context.ProductVersions.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductVersion product)
        {
            _context.ProductVersions.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProductVersion product)
        {
            _context.ProductVersions.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
