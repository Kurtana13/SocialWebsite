using SocialWebsite.Models;

namespace SocialWebsite.Api.Repositories.IRepositories
{
    public interface IUserGroupRepository : IGenericRepository<UserGroup>
    {
        public Task<User> Create(int groupId, User user);
    }
}
