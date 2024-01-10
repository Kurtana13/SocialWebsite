using Microsoft.EntityFrameworkCore;
using SocialWebsite.Api.Data;
using SocialWebsite.Api.Repositories.IRepositories;
using SocialWebsite.Models;
using SocialWebsite.Models.ViewModels;

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

        public async override Task<Group> Create(Group group)
        {
            if (await _dbSet.Where(x => x.Name == group.Name).FirstOrDefaultAsync() != null)
            {
                return null;
            }
            return await base.Create(group);
        }

        public async Task<Group> Create(GroupViewModel groupViewModel)
        {
            return await Create(new Group(groupViewModel));
        }
    }
}
