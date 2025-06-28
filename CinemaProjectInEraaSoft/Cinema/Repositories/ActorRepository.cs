using Cinema.Data;
using Cinema.Models;
using Cinema.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cinema.Repository
{
    public class ActorRepository : Repository<Actor>, IActorRepository
    {
        private readonly ApplicationDbContext context;

        public ActorRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
