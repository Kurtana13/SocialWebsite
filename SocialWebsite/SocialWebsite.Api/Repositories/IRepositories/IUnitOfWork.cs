using Microsoft.EntityFrameworkCore;
using SocialWebsite.Api.Data;

namespace SocialWebsite.Api.Repositories.IRepositories
{
    public interface IUnitOfWork<out TContext> : IDisposable where TContext : ApplicationDbContext
    {
        TContext Context { get; }
        Task Save();
    }
}
