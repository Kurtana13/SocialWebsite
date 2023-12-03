using SocialWebsite.Api.Data;
using SocialWebsite.Api.Repositories.IRepositories;
using SocialWebsite.Models;

namespace SocialWebsite.Api.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        //Without Unit of Work
        public CommentRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        //With Unit of Work
        public CommentRepository(IUnitOfWork<ApplicationDbContext> unitOfWork)
           : base(unitOfWork)
        {

        }
    }
}
