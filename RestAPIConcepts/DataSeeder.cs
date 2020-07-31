using RestAPIConcepts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPIConcepts
{
    public class DataSeeder
    {
        private readonly DataContext context;

        public DataSeeder(DataContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void SeedGuidEntities()
        {
            var suppliers = new List<SupplierGuid>();
            suppliers.Add(new SupplierGuid() { Id = Guid.NewGuid(), Name = "Microsoft", Address = "Seattle" }); // specify id here
            suppliers.Add(new SupplierGuid() { Name = "Apple", Address = "Cupertino" });
            suppliers.ForEach(sg => this.context.SupplierGuids.Add(sg));
            this.context.SaveChanges();

            var products = new List<ProductGuid>();
            
            products.Add(new ProductGuid() 
            { 
                Name = "Surface Book Pro 3", 
                Price = 1400, 
                Description = "Best Notebook PC for Developers", 
                SupplierId = suppliers[0].Id 
            });
            products.Add(new ProductGuid()
            {
                Name = "Windows 10",
                Price = 120,
                Description = "OS from Microsoft",
                SupplierId = suppliers[0].Id
            });
            products.Add(new ProductGuid()
            {
                Name = "Visual Studio 2019",
                Price = 140,
                Description = "Developer Tool",
                SupplierId = suppliers[0].Id
            });

            products.Add(new ProductGuid()
            {
                Name = "MacBook Pro 2020",
                Price = 1300,
                Description = "Apple MacBook Pro",
                SupplierId = suppliers[1].Id
            });
            products.Add(new ProductGuid()
            {
                Name = "iPhone 12",
                Price = 1200,
                Description = "Apple iPhone 12",
                SupplierId = suppliers[1].Id
            });
            products.Add(new ProductGuid()
            {
                Name = "iPad Pro 2020",
                Price = 140,
                Description = "Apple iPad Pro 2020",
                SupplierId = suppliers[0].Id
            });

            products.ForEach(p => this.context.ProductGuids.Add(p));
            this.context.SaveChanges();
        }
    }
}
