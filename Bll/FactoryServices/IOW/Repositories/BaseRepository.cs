using AutoMapper;
using Dal;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Linq.Expressions;

namespace Bll.FactoryServices.UOW.Repositories
{
    public class BaseRepository<T> : IGenericRepository<T> where T : class
    {
        protected ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BaseRepository(ApplicationDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public IEnumerable<T> GetAll() =>
            this._context.Set<T>().ToList();

        public async Task<IEnumerable<T>> GetAllAsync()
        => await this._context.Set<T>().ToListAsync();

        public virtual T Get(int id)
        => this._context.Set<T>().Find(id);

        public virtual async Task<T> GetAsync(int id) =>
            await this._context.Set<T>().FindAsync(id);

        public virtual T Add(T t)
        {
            this._context.Set<T>().Add(t);
            return t;
        }

        public virtual void AddRange(IEnumerable<T> t) =>
            this._context.Set<T>().AddRange(t);

        public virtual void RemoveRange(IEnumerable<T> entity) =>
            this._context.Set<T>().RemoveRange(entity);

        public virtual async Task<T> AddAsyn(T t)
        {
            await _context.Set<T>().AddAsync(t);
            return t;
        }

        public virtual T Find(Expression<Func<T, bool>> match) =>
            this._context.Set<T>().SingleOrDefault(match);

        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> match) =>
            await _context.Set<T>().SingleOrDefaultAsync(match);

        public virtual T FindTracking(Expression<Func<T, bool>> match) =>
            this._context.Set<T>().AsTracking().SingleOrDefault(match);

        public virtual async Task<T> FindTrackingAsync(Expression<Func<T, bool>> match) =>
            await _context.Set<T>().AsTracking().SingleOrDefaultAsync(match);

        public ICollection<T> FindAll(Expression<Func<T, bool>> match) =>
            this._context.Set<T>().Where(match).ToList();

        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match) =>
            await this._context.Set<T>().Where(match).ToListAsync();

        public virtual void Delete(T entity) =>
            this._context.Set<T>().Remove(entity);

        public virtual Task DeleteAsyn(T entity) =>
            Task.Run(() =>
            {
                var result = this._context.Set<T>().Remove(entity);
                return result;
            });

        public virtual T Update(T t, object key)
        {
            if (t == null)
                return null;

            T exist = _context.Set<T>().Find(key);

            if (exist == null) return exist;

            _context.Entry(exist).CurrentValues.SetValues(t);

            return exist;
        }

        public virtual T Update(T t, params Object[] key)
        {
            if (t == null)
                return null;

            T exist = _context.Set<T>().Find(key);

            if (exist == null) return exist;

            _context.Entry(exist).CurrentValues.SetValues(t);

            return exist;
        }

        public virtual async Task<T> UpdateAsyn(T t, object key)
        {
            if (t == null)
                return null;

            T exist = await _context.Set<T>().FindAsync(key);

            if (exist != null)
            {
                _context.Entry(exist).CurrentValues.SetValues(t);
            }

            return exist;
        }

        public int Count() => this._context.Set<T>().Count();

        public async Task<int> CountAsync() => await _context.Set<T>().CountAsync();

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate) => this._context.Set<T>().Where(predicate);

        public virtual async Task<ICollection<T>> FindByAsyn(Expression<Func<T, bool>> predicate)
            => await this._context.Set<T>().Where(predicate).ToListAsync();

        public async Task<PagedResult<TResult>> GetAllAsync<TResult>(
            QueryParameters queryParameters,
            Expression<Func<T, bool>> where = null,
            Expression<Func<T, object>> OrderBy = null
            )
        {
            int totalSize;
            int skipped = (queryParameters.Page - 1) * queryParameters.PageSize;

            IQueryable<T> query = _context.Set<T>().AsQueryable();

            if (where != null)
            {
                query = query.Where(where);
            }

            totalSize = await query.CountAsync();

            //query.AsNoTracking()
            //   // .Where(c => EF.Functions.Like(queryParameters.Search_column, $"%{queryParameters.Search_value}%" ))
            //   .Skip(skipped)
            //   .Take(queryParameters.PageSize);
            //.ProjectTo<TResult>(_mapper.ConfigurationProvider)

            if (OrderBy != null)
            {
                //if (!string.IsNullOrWhiteSpace(queryParameters.OrderBy))
                //{
                //var props = typeof(T).GetProperties();
                //var orderByProperty = props.FirstOrDefault(n => n.GetCustomAttribute<SortableAttribute>()?.OrderBy == queryParameters.OrderBy);

                //var par = props.GetType().GetProperty(queryParameters.OrderBy);
                //query = query.OrderBy(o => orderByProperty.GetValue(o));
                //query = query.OrderByDescending(o => orderByProperty.GetValue(o));

                //if (!queryParameters.Descending)
                //    query = query.OrderBy(OrderBy);
                //else
                //    query = query.OrderByDescending(OrderBy);
            }

            query = query
                .AsNoTracking()
               // .Where(c => EF.Functions.Like(queryParameters.Search_column, $"%{queryParameters.Search_value}%" ))
               .Skip(skipped)
               .Take(queryParameters.PageSize);

            List<T> items = await query.ToListAsync();

            return new PagedResult<TResult>
            {
                Items = _mapper.Map<List<TResult>>(items),
                PageNumber = queryParameters.Page,
                RecordNumber = queryParameters.PageSize,
                TotalCount = totalSize,
                TotalPages = (int)Math.Ceiling(totalSize / (double)queryParameters.PageSize)
            };
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await GetAsync(id);
            return entity != null;
        }

        public IQueryable<T> FindInclude(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] properties)
        {
            IQueryable<T> query = this._context.Set<T>().Where(predicate);

            query = properties.Aggregate(query, (current, property) => current.Include(property));

            return query.AsNoTracking();
        }

        public async Task ExecuteSqlInterpolatedAsync(FormattableString query)
        {
            await this._context.Database.BeginTransactionAsync();
            await this._context.Database.ExecuteSqlInterpolatedAsync(query);
            await this._context.Database.CommitTransactionAsync();
        }

        public void ExecuteSqlRaw(string query, params object[] parameters)
            => this._context.Database.ExecuteSqlRaw(query, parameters);

        public async Task ExecuteSqlRawAsync(string query, params object[] parameters)
            => await this._context.Database.ExecuteSqlRawAsync(query, parameters);
    }
}