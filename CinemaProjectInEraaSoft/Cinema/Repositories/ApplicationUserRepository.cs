using Cinema.Data;
using Cinema.Models;
using Cinema.Repositories.IRepositories;
using Cinema.Repository;

namespace Cinema.Repositories
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext context;

        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
