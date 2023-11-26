using Microsoft.EntityFrameworkCore;
using SocialWebsite.Api.Repositories;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SocialWebsite.Api.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public virtual async Task<IEnumerable<T>> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProporties = "")
        {
            IQueryable<T> query = _dbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }
            
            foreach(var includeProperty  in includeProporties.Split
                (new char[] {','},
                StringSplitOptions.RemoveEmptyEntries))
            {
                query.Include(includeProperty);
            }
            
            if(orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public virtual async Task<T> GetById(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual void Insert(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Delete(T entity)
        {
            if(_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }

        public virtual async Task Delete(object id)
        {
            T ?entityToDelete = await _dbSet.FindAsync(id);
            _dbSet.Remove(entityToDelete);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
