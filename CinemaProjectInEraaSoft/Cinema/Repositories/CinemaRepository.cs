using Cinema.Data;
using Cinema.Models;
using Cinema.Repositories.IRepositories;
using Cinema.Repository;

namespace Cinema.Repositories
{
    public class CinemaRepository : Repository<Cinemaa>, ICinemaRepository
    {
        private readonly ApplicationDbContext context;

        public CinemaRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
