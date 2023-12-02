﻿using SocialWebsite.Api.Data;
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

        public Task<IEnumerable<Comment>> GetAllComment()
        {
            return null;
        }
    }
}
