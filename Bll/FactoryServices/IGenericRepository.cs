using Models;
using System.Linq.Expressions;

namespace Bll.FactoryServices
{
    public interface IGenericRepository<T> where T : class
    {
        T Add(T t);

        void AddRange(IEnumerable<T> t);

        void RemoveRange(IEnumerable<T> entity);

        Task<T> AddAsyn(T t);

        int Count();

        Task<int> CountAsync();

        void Delete(T entity);

        T Find(Expression<Func<T, bool>> match);

        ICollection<T> FindAll(Expression<Func<T, bool>> match);

        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);

        Task<T> FindAsync(Expression<Func<T, bool>> match);

        T FindTracking(Expression<Func<T, bool>> match);

        Task<T> FindTrackingAsync(Expression<Func<T, bool>> match);

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        Task<ICollection<T>> FindByAsyn(Expression<Func<T, bool>> predicate);

        T Get(int id);

        IEnumerable<T> GetAll();

        Task<IEnumerable<T>> GetAllAsync();

        Task<PagedResult<TResult>> GetAllAsync<TResult>(
            QueryParameters queryParameters,
            Expression<Func<T, bool>> filter = null,
            Expression<Func<T, object>> OrderBy = null);

        Task<T> GetAsync(int id);

        T Update(T t, object key);

        T Update(T t, params Object[] key);

        Task<T> UpdateAsyn(T t, object key);

        Task<bool> Exists(int id);

        IQueryable<T> FindInclude(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] properties);

        Task ExecuteSqlInterpolatedAsync(FormattableString query);

        void ExecuteSqlRaw(string query, params object[] parameters);

        Task ExecuteSqlRawAsync(string query, params object[] parameters);
    }
}