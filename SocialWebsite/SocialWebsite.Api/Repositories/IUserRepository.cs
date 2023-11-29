using SocialWebsite.Api.Repositories;
using SocialWebsite.Models;

namespace SocialWebsite.Api.Data
{
    public interface IUserRepository : IGenericRepository<User>
    {
        new public Task<User> Create(User user);
    }
}
