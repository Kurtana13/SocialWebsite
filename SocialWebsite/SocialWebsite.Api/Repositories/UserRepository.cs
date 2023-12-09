using Microsoft.EntityFrameworkCore;
using SocialWebsite.Api.Data;
using SocialWebsite.Api.Repositories.IRepositories;
using SocialWebsite.Models;

namespace SocialWebsite.Api.Repositories
{
    public class UserRepository :GenericRepository<User>, IUserRepository
    {
        //Without Unit of Work
        public UserRepository(ApplicationDbContext context)
            :base(context) 
        {

        }

        //With Unit of Work
        public UserRepository(IUnitOfWork<ApplicationDbContext> unitOfWork)
           : base(unitOfWork)
        {

        }

        public async override Task<User> Create(User user)
        {
            if (await _dbSet.Where(x => x.UserName == user.UserName).FirstOrDefaultAsync() != null)
            {
                return null;
            }
            await _dbSet.AddAsync(user);
            return user;
        }
    }
}
