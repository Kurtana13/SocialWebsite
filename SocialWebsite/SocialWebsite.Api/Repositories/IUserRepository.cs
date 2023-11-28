using SocialWebsite.Models;

namespace SocialWebsite.Api.Data
{
    public interface IUserRepository : IDisposable
    {
        public Task<bool> DeleteUser(string userName);
        public Task<bool> UpdateUser(User user);
        public Task<User> CreateUser(User user);
    }
}
