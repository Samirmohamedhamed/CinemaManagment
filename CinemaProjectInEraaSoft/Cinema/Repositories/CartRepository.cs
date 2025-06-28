using Cinema.Data;
using Cinema.Models;
using Cinema.Repositories.IRepositories;
using Cinema.Repository;

namespace Cinema.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext context;

        public CartRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
