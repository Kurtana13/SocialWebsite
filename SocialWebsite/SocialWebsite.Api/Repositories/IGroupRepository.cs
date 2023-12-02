using SocialWebsite.Api.Repositories;
using SocialWebsite.Models;

namespace SocialWebsite.Api.Data
{
    public interface IGroupRepository : IGenericRepository<Group>
    {
        Task<User> AddUser(User user);
        //Gets all posts from the group
        Task<IEnumerable<Post>> GetAllPosts();
    }
}
