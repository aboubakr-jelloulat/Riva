using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Riva.API.Data.Context;
using Riva.API.Data.Repository.IRepository;
using System.Linq;
using System.Linq.Expressions;

namespace Riva.API.Data.Repository.Repositories
{

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        private DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }


        public void Add(T entity) => dbSet.Add(entity);

        public async Task AddAsync(T entity) => await dbSet.AddAsync(entity);


        public T? Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query = tracked ? dbSet : dbSet.AsNoTracking();

            query = query.Where(filter);
            query = ApplyIncludes(query, includeProperties);

            return query.FirstOrDefault();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query = tracked ? dbSet : dbSet.AsNoTracking();

            query = query.Where(filter);
            query = ApplyIncludes(query, includeProperties);

            return await query.FirstOrDefaultAsync();
        }



        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter is not null)
                query = query.Where(filter);

            query = ApplyIncludes(query, includeProperties);

            return query.ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter is not null)
                query = query.Where(filter);

            query = ApplyIncludes(query, includeProperties);

            return await query.ToListAsync();
        }



        public void Remove(T entity) => dbSet.Remove(entity);
        public void RemoveRange(IEnumerable<T> entities) => dbSet.RemoveRange(entities);


        public async Task RemoveRangeAsync(IEnumerable<T> entities) => await Task.Run(() => dbSet.RemoveRange(entities));


        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            return await dbSet.AnyAsync(filter);
        }


        private static IQueryable<T> ApplyIncludes(IQueryable<T> query, string? includeProperties)
        {
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp.Trim());
                }
            }
            return query;
        }
    }
}
