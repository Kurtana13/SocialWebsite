﻿using SocialWebsite.Api.Data;
using SocialWebsite.Models;

namespace SocialWebsite.Api.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;
        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<bool> AddPost(Post post)
        {
            return Task.FromResult(true);
        }

        public Task<bool> DeletePost()
        {
            return Task.FromResult(true);
        }

        public Task<bool> AddComment()
        {
            return Task.FromResult(true);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
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