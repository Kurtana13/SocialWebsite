using System.Linq.Expressions;

namespace SocialWebsite.Api.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<IEnumerable<T>> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProporties = "");
        public Task<T> GetById(object id);
        public Task<T> Create(T entity);
        public void Delete(T entity);
        public Task Delete(object id);
        public void Update(T entity);
    }
}
