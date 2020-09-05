using Microsoft.EntityFrameworkCore;
using RestAPIConcepts.Models;
using RestAPIConcepts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPIConcepts.Services
{
    public class SuppliersGuidService
    {
        private readonly DataContext context;

        public SuppliersGuidService(DataContext context) =>
            this.context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<IEnumerable<SupplierGuidViewModel>> GetAllAsync(string nameFilter = null, bool includeProducts = true)
        {
            IEnumerable<SupplierGuidViewModel> supplers;

            if (includeProducts)
            {
                var query = this.context.SupplierGuids.AsQueryable();

                if (!string.IsNullOrEmpty(nameFilter))
                {
                    query = query.Where(s => s.Name.Contains(nameFilter));
                }

                supplers = await query.Include(sg => sg.Products)
                    .AsNoTracking()
                    .Select(sg => new SupplierGuidViewModel()
                    {
                        Id = sg.Id,
                        Name = sg.Name,
                        Address = sg.Address,
                        Products = sg.Products.Select(pg =>
                            new ProductGuidViewModel()
                            {
                                Id = pg.Id,
                                Name = pg.Name,
                                Description = pg.Description,
                                Price = pg.Price,
                                SupplierId = sg.Id
                            }).ToList()
                    }).ToListAsync();
            }
            else
            {
                var query = this.context.SupplierGuids.AsQueryable();
                if (!string.IsNullOrEmpty(nameFilter))
                {
                    query = query.Where(s => s.Name.Contains(nameFilter));
                }

                supplers = await query.AsNoTracking()
                    .Select(sg => new SupplierGuidViewModel()
                    {
                        Id = sg.Id,
                        Name = sg.Name,
                        Address = sg.Address
                    })
                    .ToListAsync();
            }
            return supplers;
        }

        public async Task<SupplierGuidViewModel> GetAsync(Guid supplierId, bool includeProducts = true)
        {
            if (includeProducts)
            {
                var supplier = await this.context.SupplierGuids.Include(sg => sg.Products).AsNoTracking().FirstAsync(sg => sg.Id == supplierId);

                return new SupplierGuidViewModel()
                {
                    Id = supplier.Id,
                    Name = supplier.Name,
                    Address = supplier.Address,
                    Products = supplier.Products.Select(pg =>
                        new ProductGuidViewModel()
                        {
                            Id = pg.Id,
                            Name = pg.Name,
                            Description = pg.Description,
                            Price = pg.Price,
                            SupplierId = pg.SupplierId
                        }).ToList()
                };
            }
            else
            {
                var supplier = await this.context.SupplierGuids.FindAsync(supplierId);

                return new SupplierGuidViewModel()
                {
                    Id = supplier.Id,
                    Name = supplier.Name,
                    Address = supplier.Address
                };
            }
        }

        public async Task<IEnumerable<SupplierGuidViewModel>> GetAsync(IEnumerable<Guid> supplierIds, bool includeProducts = true)
        {
            if (includeProducts)
            {
                var suppliers = await this.context.SupplierGuids.Where(s => supplierIds.Contains(s.Id))
                    .Include(sg => sg.Products)
                    .AsNoTracking()
                    .Select(sg => new SupplierGuidViewModel()
                    {
                        Id = sg.Id,
                        Name = sg.Name,
                        Address = sg.Address,
                        Products = sg.Products.Select(pg =>
                            new ProductGuidViewModel()
                            {
                                Id = pg.Id,
                                Name = pg.Name,
                                Description = pg.Description,
                                Price = pg.Price,
                                SupplierId = sg.Id
                            }).ToList()
                    }).ToListAsync();
                return suppliers;
            }
            else
            {
                var suppliers = await this.context.SupplierGuids
                    .Where(s => supplierIds.Contains(s.Id))
                    .AsNoTracking()
                    .Select(sg => new SupplierGuidViewModel()
                    {
                        Id = sg.Id,
                        Name = sg.Name,
                        Address = sg.Address
                    })
                    .ToListAsync();
                    return suppliers;
            }
        }

        public async Task AddAsync(SupplierGuid supplier)
        {
            await this.context.AddAsync(supplier);
            await this.context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SupplierGuid supplier)
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
            var exists = await this.context.SupplierGuids.FindAsync(supplier.Id);
            this.context.Entry(exists).CurrentValues.SetValues(supplier);
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteAsync(SupplierGuid supplier)
        {
            this.context.Remove(supplier);
            await this.context.SaveChangesAsync();
        }

        public async Task<bool> IsExistingAsync(Guid supplierId)
        {
            var supplier = await this.context.SupplierGuids.FindAsync(supplierId);
            return supplier != null;
        }
    }
}
