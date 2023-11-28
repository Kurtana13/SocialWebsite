using SocialWebsite.Models;

namespace SocialWebsite.Api.Data
{
    public interface IPostRepository : IDisposable
    {
        public Task<bool> AddPost(Post post);
        public Task<bool> DeletePost();
        public Task<bool> AddComment();
    }
}
