using SocialWebsite.Api.Repositories;
using SocialWebsite.Models;

namespace SocialWebsite.Api.Data
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        // Gets all comments from the post
        Task<IEnumerable<Comment>> GetAllComment();
    }
}
