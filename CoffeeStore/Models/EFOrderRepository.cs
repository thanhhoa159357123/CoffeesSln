using Microsoft.EntityFrameworkCore;

namespace CoffeeStore.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private CoffeeDbContext context;
        public EFOrderRepository(CoffeeDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Order> Orders => context.Orders
        .Include(o => o.OrderLines)
        .ThenInclude(l => l.Product);
        public void SaveOrder(Order order)
        {
            context.AttachRange(order.OrderLines.Select(l => l.Product));
            if (String.IsNullOrEmpty(order.OrderID))
            {
                context.Orders.Add(order);
            }
            context.SaveChanges();
        }
    }

}
