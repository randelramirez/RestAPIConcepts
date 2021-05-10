using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RestAPIConcepts.Models;
using System.Linq;

namespace RestAPIConcepts
{
    public static class StartupHelper
    {
        public static void SeeeDataContext(this IApplicationBuilder application)
        {
            using (var serviceScope = application.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DataContext>();

                context.Database.EnsureCreated();

                var databaseSeeder = new DataSeeder(context);
                if (!context.SupplierGuids.Any())
                {
                    // Seed both SuppliersGuid and ProductsGuid 
                    databaseSeeder.SeedGuidEntities();
                }
            }
        }
    }
}
