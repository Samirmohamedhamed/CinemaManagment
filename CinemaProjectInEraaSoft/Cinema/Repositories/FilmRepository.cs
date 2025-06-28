using Cinema.Data;
using Cinema.Models;
using Cinema.Repositories.IRepositories;
using Cinema.Repository;

namespace Cinema.Repositories
{
    public class FilmRepository : Repository<Film>, IFilmRepository
    {
        private readonly ApplicationDbContext context;

        public FilmRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
