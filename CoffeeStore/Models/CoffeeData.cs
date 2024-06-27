using Microsoft.EntityFrameworkCore;
namespace CoffeeStore.Models
{
    public static class CoffeeData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            CoffeeDbContext context = app.ApplicationServices
            .CreateScope().ServiceProvider.GetRequiredService<CoffeeDbContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if (!context.Products.Any())
            {
                context.SaveChanges();
            }
            if (!context.Customers.Any())
            {
                context.SaveChanges();
            }
            /*if (!context.Orders.Any())
            {
                context.SaveChanges();
            }*/
        }
    }
}
