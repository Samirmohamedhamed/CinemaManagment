using Cinema.Data;
using Cinema.Models;
using Cinema.Repositories.IRepositories;
using Cinema.Repository;

namespace Cinema.Repositories
{
    public class OrderItemsRepository : Repository<OrderItems>, IOrderItemsRepository
    {
        private readonly ApplicationDbContext context;

        public OrderItemsRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
