using SocialWebsite.Models;

namespace SocialWebsite.Api.Repositories.IRepositories
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        // Gets all comments from the post
        Task<IEnumerable<Comment>> GetAllComment();
    }
}
