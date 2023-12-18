using Microsoft.Identity.Client;
using SocialWebsite.Api.Data;
using SocialWebsite.Api.Repositories.IRepositories;
using SocialWebsite.Models;

namespace SocialWebsite.Api.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        //Without Unit of Work
        public PostRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        //With Unit of Work
        public PostRepository(IUnitOfWork<ApplicationDbContext> unitOfWork)
           : base(unitOfWork)
        {

        }

        public async Task<Post> Create(int userId,Post post)
        {
            post.UserId = userId;
            return await base.Create(post);
        }

        public async Task<Post> CreateGroupPost(int groupId,Post post)
        {
            post.GroupId = groupId;
            return await Create(post);
        }

        public Task<IEnumerable<Comment>> GetAllComment()
        {
            return null;
        }
    }
}
