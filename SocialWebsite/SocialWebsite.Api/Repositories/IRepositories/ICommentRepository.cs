using SocialWebsite.Models;

namespace SocialWebsite.Api.Repositories.IRepositories
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<Comment> Create(int postId, int ownerId, Comment comment);
    }
}
