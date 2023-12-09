using SocialWebsite.Models;

namespace SocialWebsite.Api.Repositories.IRepositories
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<Post> CreateGroupPost(int groupId,Post post);
        // Gets all comments from the post
        Task<IEnumerable<Comment>> GetAllComment();
    }
}
