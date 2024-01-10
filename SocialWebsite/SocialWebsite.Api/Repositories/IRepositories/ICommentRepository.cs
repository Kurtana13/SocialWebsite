using SocialWebsite.Models;
using SocialWebsite.Models.ViewModels;

namespace SocialWebsite.Api.Repositories.IRepositories
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        public Task<Comment> Create(int postId, int ownerId, Comment comment);
        public Task<Comment> Create(int postId, int ownerId, CommentViewModel commentViewModel);
    }
}
