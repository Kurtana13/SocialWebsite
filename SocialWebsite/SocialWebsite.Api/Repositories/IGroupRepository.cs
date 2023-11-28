using SocialWebsite.Models;

namespace SocialWebsite.Api.Data
{
    public interface IGroupRepository : IDisposable
    {
        public Task<bool> AddUser(User user);
    }
}
