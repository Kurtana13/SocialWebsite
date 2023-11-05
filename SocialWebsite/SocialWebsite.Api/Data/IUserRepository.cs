using SocialWebsite.Models;

namespace SocialWebsite.Api.Data
{
    public interface IUserRepository
    {
        public Task<User> GetUserById(int id);
        public Task<IEnumerable<User>> GetUsers();
        public Task<bool> DeleteUser(string userName);
        public Task<bool> UpdateUser(User user);
        public Task<User> CreateUser(User user);
    }
}
