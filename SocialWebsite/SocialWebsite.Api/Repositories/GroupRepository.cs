using Microsoft.EntityFrameworkCore;
using SocialWebsite.Api.Data;
using SocialWebsite.Api.Repositories.IRepositories;
using SocialWebsite.Models;

namespace SocialWebsite.Api.Repositories
{
    public class GroupRepository : GenericRepository<Group>, IGroupRepository
    {
        //Without Unit of Work
        public GroupRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        //With Unit of Work
        public GroupRepository(IUnitOfWork<ApplicationDbContext> unitOfWork)
           : base(unitOfWork)
        {

        }

        public async Task<Group> Create(User user,Group group)
        {
            if (await _dbSet.Where(x => x.Name == group.Name).FirstOrDefaultAsync() != null)
            {
                return null;
            }
            _dbSet.Add(group);
            return group;
        }
    }
}
