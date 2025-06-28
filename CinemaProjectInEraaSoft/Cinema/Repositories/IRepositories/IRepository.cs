using Cinema.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace Cinema.Repositories.IRepositories
{
    public interface IRepository <T> where T : class
    {
       Task<bool> CreateAsync(T entity);

        bool UpdateAsync(T entity);

        public bool Remove(T entity);

       Task<bool> CommitAsync();
        public T? GetFirstOrDefault(
Expression<Func<T, bool>>? expression = null,
Expression<Func<T, object>>[]? Includes = null,
bool traked = true);
        public T? GetOne(Expression<Func<T, bool>>? expression = null, Expression<Func<T, Object>>?[] Includes = null, bool traked = true);

        public IEnumerable<T> Get(Expression<Func<T, bool>>? expression = null, Expression<Func<T, Object>>?[] Includes = null, bool traked = true);
       
    }
}

