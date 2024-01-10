using SocialWebsite.Models;
using SocialWebsite.Models.ViewModels;

namespace SocialWebsite.Api.Repositories.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public Task<User> Create(User user,string password);
        public Task<User> Create(UserViewModel userViewModel);
        public Task<User> GetByUsername(string username);
    }
}
