using Cinema.Data;
using Cinema.Repositories.IRepositories;
using Cinema.Repository;
using Microsoft.AspNetCore.Identity;

namespace Cinema.Repositories
{
    public class RoleRepository : Repository<IdentityRole>, IRoleRepository
    {
        private readonly ApplicationDbContext context;

        public RoleRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
