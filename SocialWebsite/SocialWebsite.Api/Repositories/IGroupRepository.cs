using SocialWebsite.Models;

namespace SocialWebsite.Api.Data
{
    public interface IGroupRepository
    {
        public Task<bool> AddUser(User user);
    }
}
