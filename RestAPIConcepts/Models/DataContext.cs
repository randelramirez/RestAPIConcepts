using Microsoft.EntityFrameworkCore;

namespace RestAPIConcepts.Models
{
    public class DataContext : DbContext
    {
        public DbSet<SupplierGuid> SupplierGuids { get; set; }

        public DbSet<ProductGuid> ProductGuids { get; set; }

        public DbSet<SupplierInt> SupplierInts { get; set; }


        public DbSet<ProductInt> ProductInts { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SupplierGuid>().HasKey(sg => sg.Id);

            modelBuilder.Entity<ProductGuid>().HasKey(pg => pg.Id);

            modelBuilder.Entity<SupplierGuid>().HasMany(sg => sg.Products)
                .WithOne(pg => pg.Supplier).HasForeignKey(pg => pg.SupplierId);

            modelBuilder.Entity<SupplierInt>().HasMany(si => si.Products)
               .WithOne(pi => pi.Supplier).HasForeignKey(pi => pi.SupplierId);

        }
    }
}
