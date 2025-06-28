using Cinema.Data;
using Cinema.Models;
using Cinema.Repositories.IRepositories;
using Cinema.Repository;

namespace Cinema.Repositories
{
    public class FavoriteRepository : Repository<Favorite>, IFavoriteRepository
    {
        private readonly ApplicationDbContext context;

        public FavoriteRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
