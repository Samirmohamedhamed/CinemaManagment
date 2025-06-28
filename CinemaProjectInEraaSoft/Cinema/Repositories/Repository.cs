using Cinema.Data;
using Cinema.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using Cinema.Repositories.IRepositories;

namespace Cinema.Repository
{
    public class Repository<T>:IRepository<T>  where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            
        }
        public async Task<bool> CreateAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"{ex.Message}");
                return false;
            }
        }
        public bool UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"{ex.Message}");
                return false;
            }
        }
        public bool Remove(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"{ex.Message}");
                return false;
            }
        }
        public async Task<bool> CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"{ex.Message}");
                return false;
            }
        }
        public T? GetFirstOrDefault(
   Expression<Func<T, bool>>? expression = null,
   Expression<Func<T, object>>[]? Includes = null,
   bool traked = true)
        {
            IQueryable<T> entities = _dbSet;

            if (expression != null)
            {
                entities = entities.Where(expression);
            }

            if (Includes != null)
            {
                foreach (var include in Includes)
                {
                    entities = entities.Include(include);
                }
            }

            if (!traked)
            {
                entities = entities.AsNoTracking();
            }

            return entities.FirstOrDefault();
        }

        public T? GetOne(Expression<Func<T, bool>>? expression = null, Expression<Func<T, Object>>?[] Includes = null, bool traked = true)
        {
            var entity = Get(expression, Includes, traked).FirstOrDefault();
            ;
            return entity;
        }
        public IEnumerable<T> Get(Expression<Func<T, bool>>? expression = null, Expression<Func<T, Object>>?[] Includes = null, bool traked = true)
        {
            IQueryable<T> entities = _dbSet;
            if (expression is not null)
            {
                entities = entities.Where(expression);
            }
            if (Includes is not null)
            {
                foreach (var item in Includes)
                {
                    entities = entities.Include(item);
                }
            }
            if (!traked)
            {
                entities = entities.AsNoTracking();
            }
            return entities;

        }
    }
}

