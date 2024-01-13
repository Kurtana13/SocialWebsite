using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SocialWebsite.Api.Repositories.IRepositories;
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
            
            foreach(var includeProperty in includeProporties.Split
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

        public virtual async Task<IEnumerable<T>> Delete(Expression<Func<T, bool>> filter)
        {
            IEnumerable<T> result = await Get(filter);
            foreach(var entity in result)
            {
                await Delete(entity);
            }
            return result;
        }

        public virtual async Task<T> DeleteById(object id)
        {
            T ?entityToDelete = await GetById(id);
            if(entityToDelete == null) { return null; }
            return await Delete(entityToDelete);
        }

        public virtual async Task<T> Put(T entity,T newEntity)
        {
            _dbSet.Attach(entity);
            if(_dbSet.Entry(entity).State == EntityState.Unchanged)
            {
                _dbSet.Entry(entity).CurrentValues.SetValues(newEntity);
            }
            _context.Entry(entity).State = EntityState.Modified;
            return newEntity;
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
