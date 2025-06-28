using Cinema.Data;
using Cinema.Models;
using Cinema.Repositories.IRepositories;
using Cinema.Repository;

namespace Cinema.Repositories
{
    public class ApplicationUserOTPRepository : Repository<ApplicationUserOTP>, IApplicationUserOTPRepository
    {
        private readonly ApplicationDbContext context;

        public ApplicationUserOTPRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
