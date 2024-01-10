using SocialWebsite.Models;
using SocialWebsite.Models.ViewModels;

namespace SocialWebsite.Api.Repositories.IRepositories
{
    public interface IGroupRepository : IGenericRepository<Group>
    {
        Task<Group> Create(GroupViewModel groupViewModel);
    }
}
