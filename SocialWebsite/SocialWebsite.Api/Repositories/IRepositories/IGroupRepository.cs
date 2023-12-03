using SocialWebsite.Models;

namespace SocialWebsite.Api.Repositories.IRepositories
{
    public interface IGroupRepository : IGenericRepository<Group>
    {
        Task<User> AddUser(int groupId, User user);
        //Gets all posts from the group
    }
}
