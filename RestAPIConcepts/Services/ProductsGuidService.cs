using Microsoft.EntityFrameworkCore;
using RestAPIConcepts.Models;
using RestAPIConcepts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPIConcepts.Services
{
    public class ProductsGuidService
    {
        private readonly DataContext context;

        public ProductsGuidService(DataContext context) =>
            this.context = context ?? throw new ArgumentNullException(nameof(context));


        public async Task<IEnumerable<ProductGuidViewModel>> GetAllAsync() =>
             await this.context.ProductGuids.AsNoTracking().Select(pg =>
                new ProductGuidViewModel()
                {
                    Id = pg.Id,
                    Name = pg.Name,
                    Description = pg.Description,
                    Price = pg.Price,
                    SupplierId = pg.SupplierId
                }).ToListAsync();


        public async Task<IEnumerable<ProductGuidViewModel>> GetAllBySupplierIdAsync(Guid supplierId)
            => await this.context.ProductGuids.AsNoTracking()
                .Where(pg => pg.SupplierId == supplierId)
                .Select(pg =>
                    new ProductGuidViewModel()
                    {
                        Id = pg.Id,
                        Name = pg.Name,
                        Price = pg.Price,
                        Description = pg.Description,
                        SupplierId = pg.SupplierId
                    })
                .ToListAsync();


        public async Task<ProductGuidViewModel> GetAsync(Guid productId)
        {
            var product = await this.context.ProductGuids.AsNoTracking().SingleOrDefaultAsync(pg => pg.Id == productId);
            return new ProductGuidViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                SupplierId = product.SupplierId
            };
        }

        public async Task AddAsync(ProductGuid product)
        {
            await this.context.AddAsync(product);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductGuid product)
        {
            // approach #1
            // This apporoach, updates all properties
            //this.context.Update(supplier);
            //await this.context.SaveChangesAsync();

            // approach #2
            // Updates changed properties only
            //var existing = await this.context.SupplierGuids.FindAsync(supplier.Id);
            //existing.Address = supplier.Address;
            //existing.Name = supplier.Name;
            //await this.context.SaveChangesAsync();

            // approach #3
            // Updates changed properties only (similar to approach #2)
            var exists = await this.context.ProductGuids.FindAsync(product.Id);
            this.context.Entry(exists).CurrentValues.SetValues(product);
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid productId)
        {
            var product = await this.context.ProductGuids.FindAsync(productId);
            this.context.Remove(product);
            await this.context.SaveChangesAsync();
        }

        public async Task<bool> IsExistingAsync(Guid productId)
        {
            var product = await this.context.ProductGuids.FindAsync(productId);
            return product != null;
        }
    }
}
