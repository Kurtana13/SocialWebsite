using SocialWebsite.Api.Data;
using SocialWebsite.Models;
using System.Runtime.CompilerServices;

namespace SocialWebsite.Api.Repositories
{
    public class UnitOfWork<TContext> : IDisposable, IUnitOfWork<TContext> where TContext : ApplicationDbContext
    {
        public TContext Context { get; }

        public UnitOfWork(TContext context)
        {
            Context = context;
        }

        public async Task Save()
        {
            await Context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
