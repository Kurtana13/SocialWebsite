using SocialWebsite.Models;

namespace SocialWebsite.Api.Data
{
    public interface IUserRepository
    {
        public Task<bool> DeleteUser(string userName);
        public Task<bool> UpdateUser(User user);
        public Task<bool> CreateUser(User user);
    }
}
