using SocialWebsite.Models;
using SocialWebsite.Models.ViewModels;

namespace SocialWebsite.Api.Repositories.IRepositories
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        public Task<Post> Create(int userId,Post post);
        public Task<Post> Create(int userId,PostViewModel postViewModel);
        public Task<Post> CreateGroupPost(int groupId,Post post);
        // Gets all comments from the post
        Task<IEnumerable<Comment>> GetAllComment();
    }
}
