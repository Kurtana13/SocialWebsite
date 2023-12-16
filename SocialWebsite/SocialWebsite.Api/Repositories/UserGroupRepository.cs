using Microsoft.EntityFrameworkCore;
using SocialWebsite.Api.Data;
using SocialWebsite.Api.Repositories.IRepositories;
using SocialWebsite.Models;

namespace SocialWebsite.Api.Repositories
{
    public class UserGroupRepository : GenericRepository<UserGroup>, IUserGroupRepository
    {
        //Without Unit of Work
        public UserGroupRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        //With Unit of Work
        public UserGroupRepository(IUnitOfWork<ApplicationDbContext> unitOfWork)
           : base(unitOfWork)
        {

        }

        public async Task<User> Create(int groupId,User user)
        {
            var result =await _dbSet.FirstOrDefaultAsync(x=>x.GroupId == groupId && x.UserId==user.Id);
            if (result != null)
            {
                return null;
            }
            result = new UserGroup(groupId,user.Id);
            await _dbSet.AddAsync(result);
            return user;
        }
    }
}
