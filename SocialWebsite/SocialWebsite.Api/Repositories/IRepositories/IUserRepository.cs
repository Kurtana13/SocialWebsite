using SocialWebsite.Models;

namespace SocialWebsite.Api.Repositories.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        new public Task<User> Create(User user);
    }
}
