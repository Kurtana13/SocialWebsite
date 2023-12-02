using System.Linq.Expressions;

namespace SocialWebsite.Api.Repositories
{
    public interface IGenericRepository<T>: IDisposable where T : class
    {
        public Task<IEnumerable<T>> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProporties = "");
        public Task<T> GetById(object id);
        public Task<T> Create(T entity);
        public Task<T> Delete(T entity);
        public Task<T> DeleteById(object id);
        public Task<T> Update(T entity,T newEntity);
        public Task Save();
    }
}
