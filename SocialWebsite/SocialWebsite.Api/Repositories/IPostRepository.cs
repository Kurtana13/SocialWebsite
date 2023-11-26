namespace SocialWebsite.Api.Data
{
    public interface IPostRepository
    {
        public Task<bool> AddPost();
        public Task<bool> DeletePost();
        public Task<bool> AddComment();
    }
}
