using Microsoft.EntityFrameworkCore;
using SocialWebsite.Api.Repositories;
using SocialWebsite.Models;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SocialWebsite.Api.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public ApplicationDbContext _context;
        protected DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public GenericRepository(IUnitOfWork<ApplicationDbContext> unitOfWork)
            :this(unitOfWork.Context)
        {
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

        public virtual async Task<T> Create(T entity)
        {
            if(entity == null)
            {
                return null;
            }
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task<T> Delete(T entity)
        {
            if(_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
            return entity;
        }

        public virtual async Task<T> DeleteById(object id)
        {
            T ?entityToDelete = await _dbSet.FindAsync(id);
            if(entityToDelete == null) { return null; }
            _dbSet.Remove(entityToDelete);
            return entityToDelete;
        }

        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
